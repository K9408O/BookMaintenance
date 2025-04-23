using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookMaintenance.Models.ViewModel
{
    public class BookQueryViewModel
    {
        public string BookName { get; set; }
        public string BookClassId { get; set; }
        public string BorrowerId { get; set; }
        public string BookStatus { get; set; }

        public List<SelectListItem> BookClasses { get; set; }
        public List<SelectListItem> Borrowers { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        public List<BookListItemViewModel> BookData { get; set; }
    }
}
