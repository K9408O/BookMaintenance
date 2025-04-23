using System.ComponentModel.DataAnnotations;

namespace BookMaintenance.Models
{
    public class SpanTable
    {
        [Key]
        public int Identity_Filed { get; set; }
        public string Span_Year { get; set; }
        public string Span_Start { get; set; }
        public string Span_End { get; set; }
        public string Note { get; set; }
        public DateTime? Cre_Date { get; set; }
        public string Cre_Usr { get; set; }
        public DateTime? Mod_Date { get; set; }
        public string Mod_Usr { get; set; }
    }
}
