namespace Chinook.Storage.Models
{
    public class FilterModel
    {   
        public FilterModel()
        {
            First = 0;
            Rows = 5;
        }
        public int First { get; set; }
        public int Rows { get; set; }
    }
}