namespace BookMaintenance.Models.ViewModel
{
    public class BookDetailViewModel
    {
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public string? Book_Author { get; set; }
        public string? Book_Publisher { get; set; }
        public string? Book_Note { get; set; }
        public DateTime? Book_Bought_Date { get; set; }
        public string Book_Class_Id { get; set; }
        public string Book_Status { get; set; }
        public string? Book_Keeper { get; set; }

        // 顯示用
        public string Book_Status_Name { get; set; }
        public string Book_Keeper_Name { get; set; }
    }
}
