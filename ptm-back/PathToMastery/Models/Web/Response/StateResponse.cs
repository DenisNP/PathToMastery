namespace PathToMastery.Models.Web.Response
{
    public class StateResponse
    {
        public State.State State { get; private set; }

        public StateResponse(State.State state)
        {
            State = state;
        }
    }
}