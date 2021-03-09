using System;
using System.ComponentModel.DataAnnotations;

namespace Chinook.Model.Entities
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }

    public class BaseInfoModel : BaseModel
    {
        public DateTime? UpdateDate { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
