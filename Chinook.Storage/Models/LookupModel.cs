namespace Chinook.Storage.Models
{
    public class LookupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LookupDataModel
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }

    public class BlogCategoryLookupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
