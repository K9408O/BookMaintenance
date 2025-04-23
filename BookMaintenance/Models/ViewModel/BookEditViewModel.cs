using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookMaintenance.Models.ViewModel
{
    public class BookEditViewModel
    {
        public int Book_Id { get; set; }

        [Required]
        public string Book_Name { get; set; }
        [Required]
        public string Book_Author { get; set; }
        [Required]
        public string Book_Publisher { get; set; }
        [Required]
        public string Book_Note { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Book_Bought_Date { get; set; }

        [Required]
        public string Book_Class_Id { get; set; }
        public List<SelectListItem> BookClasses { get; set; }

        public string Book_Status { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        public string Book_Keeper { get; set; }
        public List<SelectListItem> Borrowers { get; set; }

        public bool IsEdit { get; set; } // 決定是否顯示借閱狀態欄位

    }
}
