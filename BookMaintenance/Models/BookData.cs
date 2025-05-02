namespace BookMaintenance.Models
{
    public class BookData
    {
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public string Book_Class_Id { get; set; }
        public string? Book_Author { get; set; }
        public DateTime? Book_Bought_Date { get; set; }
        public string? Book_Publisher { get; set; }
        public string? Book_Note { get; set; }
        public string Book_Status { get; set; }
        public string? Book_Keeper { get; set; }
        public int? Book_Amount { get; set; }
        public DateTime? Create_Date { get; set; }
        public string? Create_User { get; set; }
        public DateTime? Modify_Date { get; set; }
        public string? Modify_User { get; set; }

    }
}
