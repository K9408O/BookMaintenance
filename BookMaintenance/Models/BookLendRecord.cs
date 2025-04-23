using System.ComponentModel.DataAnnotations;

namespace BookMaintenance.Models
{
    public class BookLendRecord
    {
        [Key]
        public int Identity_Filed { get; set; }
        public int Book_Id { get; set; }
        public string Keeper_Id { get; set; }
        public DateTime Lend_Date { get; set; }
        public DateTime? Cre_Date { get; set; }
        public string Cre_Usr { get; set; }
        public DateTime? Mod_Date { get; set; }
        public string Mod_Usr { get; set; }
    }
}
