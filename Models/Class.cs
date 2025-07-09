using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    [Table("classes")]
    public class Class
    {
        [Column("class_id")]
        public int ClassId { get; set; }

        [Column("class_name")]
        public string ClassName { get; set; }

        [Column("description")]
        public string? Description { get; set; }
    }
}
