using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PathToMastery.Models.Web.Request;
using PathToMastery.Models.Web.Response;
using PathToMastery.Services;
using PathToMastery.Services.Abstract;

namespace PathToMastery.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly ISocialService _socialService;
        private readonly ConcurrencyService _concurrencyService;
        private readonly PathService _pathService;
        private readonly ILogger<MainController> _logger;

        public MainController(
            ISocialService socialService,
            ConcurrencyService concurrencyService,
            PathService pathService,
            ILogger<MainController> logger
        )
        {
            _socialService = socialService;
            _concurrencyService = concurrencyService;
            _pathService = pathService;
            _logger = logger;
        }
        
        [HttpGet("/test")]
        public ContentResult Test()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<h1>It works!</h1>"
            };
        }

        [HttpPost("/init")]
        public Task Init()
        {
            return HandleRequest<InitRequest, StateResponse>(
                req => new StateResponse(_pathService.LoadUser(req.UserId, req.Offset))
            );
        }

        [HttpPost("/create")]
        public Task Create()
        {
            return HandleRequest<CreateRequest, StateResponse>(
                req =>
                    new StateResponse(
                        _pathService.CreateEditPath(req.UserId, req.Id, req.Name, req.Icon, req.Color, req.Days, req.Notify, req.Offset)
                    )
            );
        }
        
        [HttpPost("/delete")]
        public Task Delete()
        {
            return HandleRequest<IdRequest, StateResponse>(
                req => new StateResponse(_pathService.DeletePath(req.UserId, req.Id, req.Offset))
            );
        }
        
        [HttpPost("/done")]
        public Task Done()
        {
            return HandleRequest<IdRequest, StateResponse>(
                req => new StateResponse(_pathService.SetDone(req.UserId, req.Id, req.Offset))
            );
        }
        
        private Task HandleRequest<TRequest, TResponse>(Func<TRequest, TResponse> handler, bool limitRatio = false) where TRequest : BaseRequest
        {
            // setup response
            Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            
            // load request body
            using var reader = new StreamReader(Request.Body);
            var body = reader.ReadToEnd();
            
            _logger.LogInformation($"REQUEST {typeof(TRequest)}:\n{body}");
            var request = JsonConvert.DeserializeObject<TRequest>(body, Utils.ConverterSettings);

            // check sign
            if (request == null || !_socialService.IsSignValid(request.UserId, request.Params, request.Sign))
            {
                _logger.LogWarning("Signature calculation failed");
                Response.StatusCode = 400;
                return Response.WriteAsync(new ErrorResponse("Signature calculation failed").ToString());
            }
            
            // check requests ratio
            if (limitRatio)
            {
                if (!_concurrencyService.CheckAddRequest(request.UserId))
                {
                    _logger.LogWarning("Too many requests");
                    Response.StatusCode = 400;
                    return Response.WriteAsync(
                        new ErrorResponse("Вы вызываете запросы слишком часто, попробуйте через несколько секунд")
                            .ToString()
                    );
                }
            }
            
            // handle request
            try
            {
                var response = handler(request);
                var stringResponse = JsonConvert.SerializeObject(response, Utils.ConverterSettings);

                _logger.LogInformation($"RESPONSE:\n{stringResponse.SafeSubstring(300)}");
                return Response.WriteAsync(stringResponse);
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#endif
                _logger.LogWarning(e.Message);
                Response.StatusCode = 400;
                return Response.WriteAsync(new ErrorResponse(e.Message).ToString());
            }
        }
    }
}