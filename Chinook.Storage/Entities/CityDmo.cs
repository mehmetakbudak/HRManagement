namespace Chinook.Storage.Entities
{
    public class CityDmo : BaseModel
    {
        public int ProvinceId { get; set; }
        public ProvinceDmo Province{ get; set; }
        public string Name { get; set; }
    }
}
