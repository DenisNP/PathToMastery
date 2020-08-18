using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using PathToMastery.Models;
using PathToMastery.Services.Abstract;

namespace PathToMastery.Services
{
    public class MongoService : IDbService
    {
        private readonly ILogger<MongoService> _logger;
        private IMongoDatabase _db;
        private Func<Type, string> _getCollectionName;
        
        public MongoService(ILogger<MongoService> logger)
        {
            _logger = logger;
        }
        
        public void Init(string dbName, Func<Type, string> typeToCollection)
        {
            _getCollectionName = typeToCollection;
            
            var connectionUrl = Environment.GetEnvironmentVariable("MONGO_URL") ?? $"mongodb://localhost:27017/{dbName}";
            var client = new MongoClient(connectionUrl);
            _db = client.GetDatabase(dbName);

            var isMongoLive = _db.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(5000);
            if (isMongoLive)
            {
                _logger.LogInformation($"MongoDB connected: {_db.DatabaseNamespace.DatabaseName}");
            }
            else
            {
                _logger.LogError("Cannot reach MongoDB server");
                throw new IOException("Cannot reach MongoDB server");
            }
        }

        public T ById<T>(string id, bool allowNull = true, string? collection = null) where T : IIdentity
        {
            var col = GetCollection<T>(collection);
            var obj = col.AsQueryable().FirstOrDefault(x => x.Id == id);
            if (obj == null && !allowNull)
            {
                throw new KeyNotFoundException(
                    $"Object with Id = {id} not found in collection: {col.CollectionNamespace.CollectionName}"
                );
            }

            return obj;
        }

        public IQueryable<T> Collection<T>(string? name = null)
        {
            return GetCollection<T>(name).AsQueryable();
        }

        public void Update<T>(T document, string? collection = null) where T : IIdentity
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, document.Id);
            GetCollection<T>(collection).ReplaceOne(filter, document, new ReplaceOptions {IsUpsert = true});
        }

        public void UpdateAsync<T>(T document, string? collection = null) where T : IIdentity
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, document.Id);
            GetCollection<T>(collection).ReplaceOneAsync(filter, document, new ReplaceOptions {IsUpsert = true});
        }

        public void PushAsync<TDocument, TItem>(
            string docId,
            Expression<Func<TDocument, IEnumerable<TItem>>> expression,
            TItem value, 
            string? collection = null
        ) where TDocument : IIdentity
        {
            var update = Builders<TDocument>.Update.Push(expression, value);
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, docId);
            GetCollection<TDocument>(collection).FindOneAndUpdateAsync(filter, update);
        }

        public void DeleteAsync<T>(string id, string? collection = null) where T : IIdentity
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, id);
            GetCollection<T>(collection).DeleteOneAsync(filter);
        }
        
        private IMongoCollection<T> GetCollection<T>(string? name = null)
        {
            return _db.GetCollection<T>(name ?? _getCollectionName(typeof(T)));
        }
    }
}