namespace BookMaintenance.Models.ViewModel
{
    public class BookListItemViewModel
    {
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public string Book_Class_Id { get; set; }
        public DateTime? Book_Bought_Date { get; set; }

        public string Book_Status { get; set; } // 原始代碼
        public string? Book_Status_Name { get; set; } // JOIN 出來的中文

        public string? Book_Keeper { get; set; } // 原始 ID
        public string? Book_Keeper_Name { get; set; } // JOIN 出來的人名
    }
}
