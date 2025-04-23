using System.ComponentModel.DataAnnotations;

namespace BookMaintenance.Models
{
    public class BookCode
    {
        [Key]
        public string Code_Id { get; set; }
        public string Code_Type { get; set; }
        public string Code_Type_Desc { get; set; }
        public string Code_Name { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_User { get; set; }
        public DateTime? Modify_Date { get; set; }
        public string Modify_User { get; set; }
    }
}
