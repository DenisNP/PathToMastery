namespace PathToMastery.Models.Web.Request
{
    public class CreateRequest : BaseRequest
    {
        public int Id { get; set; }
        public int Color { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Notify { get; set; }
        public int[] Days { get; set; }
    }
}