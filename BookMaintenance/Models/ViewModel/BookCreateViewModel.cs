using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookMaintenance.Models.ViewModel
{
    public class BookCreateViewModel
    {
        [Required(ErrorMessage = "書名為必填")]
        [StringLength(100)]
        public string BookName { get; set; }
        [Required(ErrorMessage = "作者為必填")]
        [StringLength(100)]
        public string Author { get; set; }
        [Required(ErrorMessage = "出版社為必填")]
        [StringLength(100)]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "內容簡介為必填")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "購書日期為必填")]
        [DataType(DataType.Date)]
        [Display(Name = "購書日期")]
        public DateTime BoughtDate { get; set; }

        [Required(ErrorMessage = "請選擇圖書類別")]
        public string BookClassId { get; set; }

        public List<SelectListItem> BookClasses { get; set; }
    }
}
