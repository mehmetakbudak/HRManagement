namespace Chinook.Storage.Entities
{
    public class City : BaseModel
    {
        public int ProvinceId { get; set; }
        public Province Province{ get; set; }
        public string Name { get; set; }
    }
}
