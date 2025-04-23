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
        //查詢畫面(首頁)
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

        //新增
        public IActionResult Create()
        {
            var viewModel = new BookCreateViewModel
            {
                BoughtDate = DateTime.Today.AddYears(-25), // 預設今天 - 25 年
                BookClasses = _context.BookClass
                    .Select(c => new SelectListItem
                    {
                        Value = c.Book_Class_Id,
                        Text
                        = c.Book_Class_Name
                    }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(BookCreateViewModel model)
        {
         
            //if (!ModelState.IsValid)
            //{
            //    TempData["ErrorMessage"] = "請確認所有欄位皆已正確填寫！";
            //    model.BookClasses = _context.BookClass
            //        .Select(c => new SelectListItem
            //        {
            //            Value = c.Book_Class_Id,
            //            Text = c.Book_Class_Name
            //        }).ToList();

            //    return View(model);
            //}

            var entity = new BookData
            {
                Book_Name = model.BookName,
                Book_Author = model.Author,
                Book_Publisher = model.Publisher,
                Book_Note = model.Description,
                Book_Bought_Date = model.BoughtDate,
                Book_Class_Id = model.BookClassId,
                Book_Amount = 1,
                Book_Status = "A", // 預設可借閱
                Book_Keeper ="",
                Create_Date = DateTime.Now,
                Create_User = "1588", // 或從登入者取值
                Modify_Date = DateTime.Now,
                Modify_User= "1588"
            };

            _context.BookData.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
