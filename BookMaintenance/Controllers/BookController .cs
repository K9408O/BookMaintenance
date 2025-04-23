using BookMaintenance.Models;
using BookMaintenance.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookMaintenance.Controllers
{
    public class BookController : Controller
    {
        private readonly BookDbContext _context;

        public BookController(BookDbContext context)
        {
            _context = context;
        }

        public IActionResult BookList()
        {
            var books = _context.BookData.ToList();
            return View(books);
        }

        public IActionResult Index(string bookName, string bookClassId, string borrowerId, string bookStatus)
        {
            var query = _context.BookData.AsQueryable();

            if (!string.IsNullOrEmpty(bookName))
                query = query.Where(b => b.Book_Name.Contains(bookName));

            if (!string.IsNullOrEmpty(bookClassId))
                query = query.Where(b => b.Book_Class_Id == bookClassId);

            if (!string.IsNullOrEmpty(borrowerId))
                query = query.Where(b => b.Book_Keeper == borrowerId);

            if (!string.IsNullOrEmpty(bookStatus))
                query = query.Where(b => b.Book_Status == bookStatus);

            var viewModel = new BookQueryViewModel
            {
                BookClasses = _context.BookClass
                    .Select(c => new SelectListItem { Value = c.Book_Class_Id, Text = c.Book_Class_Name })
                    .ToList(),
                Borrowers = _context.Member
                    .Select(m => new SelectListItem { Value = m.User_Id, Text = m.User_Ename })
                    .ToList(),
                Statuses = _context.BookCode
                    .Where(c => c.Code_Type == "BOOK_STATUS")
                    .Select(c => new SelectListItem { Value = c.Code_Id, Text = c.Code_Name })
                    .ToList(),
                BookData = query.OrderByDescending(b => b.Book_Bought_Date).ToList()
            };

            return View(viewModel);
        }
    }
}
