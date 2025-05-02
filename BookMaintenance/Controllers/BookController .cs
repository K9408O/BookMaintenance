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
            var query = from book in _context.BookData
                        join member in _context.Member on book.Book_Keeper equals member.User_Id into m
                        from member in m.DefaultIfEmpty()
                        join code in _context.BookCode.Where(c => c.Code_Type == "BOOK_STATUS")
                            on book.Book_Status equals code.Code_Id into c
                        from code in c.DefaultIfEmpty()
                        select new BookListItemViewModel
                        {
                            Book_Id = book.Book_Id,
                            Book_Name = book.Book_Name ?? "",
                            Book_Class_Id = book.Book_Class_Id ?? "",
                            Book_Bought_Date = book.Book_Bought_Date,
                            Book_Status = book.Book_Status ?? "",
                            Book_Status_Name = code != null ? code.Code_Name ?? "" : "",
                            Book_Keeper = book.Book_Keeper ?? "",
                            Book_Keeper_Name = member != null ? member.User_Ename ?? "" : ""
                        };

            if (!string.IsNullOrEmpty(bookName))
                query = query.Where(b => (b.Book_Name ?? "").Contains(bookName));

            if (!string.IsNullOrEmpty(bookClassId))
                query = query.Where(b => (b.Book_Class_Id ?? "") == bookClassId);

            if (!string.IsNullOrEmpty(borrowerId))
                query = query.Where(b => (b.Book_Keeper ?? "") == borrowerId);

            if (!string.IsNullOrEmpty(bookStatus))
                query = query.Where(b => (b.Book_Status ?? "") == bookStatus);


            var viewModel = new BookQueryViewModel
            {
                BookClasses = _context.BookClass
                    .Select(c => new SelectListItem
                    {
                        Value = c.Book_Class_Id ?? "",
                        Text = c.Book_Class_Name ?? "(未命名分類)"
                    })
                    .ToList(),

                Borrowers = _context.Member
                    .Select(m => new SelectListItem
                    {
                        Value = m.User_Id ?? "",
                        Text = m.User_Ename ?? "(未知姓名)"
                    })
                    .ToList(),

                 Statuses = _context.BookCode
                    .Where(c => c.Code_Type == "BOOK_STATUS")
                    .Select(c => new SelectListItem
                    {
                        Value = c.Code_Id ?? "",
                        Text = c.Code_Name ?? "(未知狀態)"
                    })
                    .ToList(),
                BookData = query.OrderByDescending(b => b.Book_Bought_Date).ToList(),

                BookName = bookName,
                BookClassId = bookClassId,
                BookStatus = bookStatus,
                BorrowerId = borrowerId
            };

            return View(viewModel);
        }

        //新增
        public IActionResult Create()
        {
            var viewModel = new BookCreateViewModel
            {
                BoughtDate = DateTime.Today.AddYears(-5), // 預設今天 - 5 年
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
                Book_Keeper = "",
                Create_Date = DateTime.Now,
                Create_User = "admin", // 或從登入者取值
                Modify_Date = DateTime.Now,
                Modify_User = "admin"
            };

            _context.BookData.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        //修改 Edit(GET)

        public IActionResult Edit(int id)
        {
            var book = _context.BookData.FirstOrDefault(b => b.Book_Id == id);
            if (book == null) return NotFound();

            var vm = new BookEditViewModel
            {
                Book_Id = book.Book_Id,
                Book_Name = book.Book_Name,
                Book_Author = book.Book_Author,
                Book_Publisher = book.Book_Publisher,
                Book_Note = book.Book_Note,
                Book_Bought_Date = book.Book_Bought_Date,
                Book_Class_Id = book.Book_Class_Id,
                Book_Status = book.Book_Status,
                Book_Keeper = book.Book_Keeper,

                BookClasses = _context.BookClass.Select(c => new SelectListItem
                {
                    Value = c.Book_Class_Id,
                    Text = c.Book_Class_Name
                }).ToList(),

                Statuses = _context.BookCode
                    .Where(c => c.Code_Type == "BOOK_STATUS")
                    .Select(c => new SelectListItem
                    {
                        Value = c.Code_Id,
                        Text = c.Code_Name
                    }).ToList(),

                Borrowers = _context.Member.Select(m => new SelectListItem
                {
                    Value = m.User_Id,
                    Text = m.User_Ename + "-" + m.User_Cname
                }).ToList(),

                IsEdit = true
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(BookEditViewModel model)
        {

            var entity = _context.BookData.FirstOrDefault(b => b.Book_Id == model.Book_Id);
            if (entity == null) return NotFound();

            entity.Book_Name = model.Book_Name;
            entity.Book_Author = model.Book_Author;
            entity.Book_Publisher = model.Book_Publisher;
            entity.Book_Note = model.Book_Note;
            entity.Book_Bought_Date = model.Book_Bought_Date;
            entity.Book_Class_Id = model.Book_Class_Id;
            entity.Book_Status = model.Book_Status;
            entity.Book_Keeper = (model.Book_Status == "A" || model.Book_Status == "U") ? "" : model.Book_Keeper;
            entity.Modify_Date = DateTime.Now;
            entity.Modify_User = "admin"; // 或登入使用者

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        //刪除
        public IActionResult Delete(int id)
        {
            var book = _context.BookData.FirstOrDefault(b => b.Book_Id == id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "找不到書籍資料，無法刪除。";
                return RedirectBackToCaller(id);
            }

            // 判斷是否不可刪除
            if (book.Book_Status == "B" || book.Book_Status == "C")
            {
                TempData["ErrorMessage"] = "此書籍目前狀態為『已借出』或『已借出(未領)』，無法刪除。";
                return RedirectBackToCaller(id);
            }

            _context.BookData.Remove(book);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "刪除成功！";
            return RedirectToAction("Index");
        }
        private BookEditViewModel GetEditViewModel(int id)
        {
            var book = _context.BookData.FirstOrDefault(b => b.Book_Id == id);
            if (book == null) return null;

            return new BookEditViewModel
            {
                Book_Id = book.Book_Id,
                Book_Name = book.Book_Name,
                Book_Author = book.Book_Author,
                Book_Publisher = book.Book_Publisher,
                Book_Note = book.Book_Note,
                Book_Bought_Date = book.Book_Bought_Date,
                Book_Class_Id = book.Book_Class_Id,
                Book_Status = book.Book_Status,
                Book_Keeper = book.Book_Keeper,
                BookClasses = _context.BookClass.Select(c => new SelectListItem
                {
                    Value = c.Book_Class_Id,
                    Text = c.Book_Class_Name
                }).ToList(),
                Statuses = _context.BookCode
                    .Where(c => c.Code_Type == "BOOK_STATUS")
                    .Select(c => new SelectListItem
                    {
                        Value = c.Code_Id,
                        Text = c.Code_Name
                    }).ToList(),
                Borrowers = _context.Member.Select(m => new SelectListItem
                {
                    Value = m.User_Id,
                    Text = m.User_Ename + "-" + m.User_Cname
                }).ToList(),
                IsEdit = true
            };
        }

        private IActionResult RedirectBackToCaller(int id)
        {
            var referer = Request.Headers["Referer"].ToString();

            if (referer.Contains("/Book/Edit"))
            {
                return View("Edit", GetEditViewModel(id));
            }
            return RedirectToAction("Index");
        }

        //明細detail
        public IActionResult Detail(int id)
        {
            var model = GetEditViewModel(id);
            if (model == null)
                return NotFound();

            model.IsEdit = false; // ✅ 標記為唯讀狀態（如果你想延用）
            return View(model);   // 使用 Views/Book/Detail.cshtml
        }

        public IActionResult History(int id)
        {
            var records = (from record in _context.BookLendRecord
                           join member in _context.Member on record.Keeper_Id equals member.User_Id
                           where record.Book_Id == id
                           orderby record.Lend_Date descending
                           select new BookLendHistoryViewModel
                           {
                               LendDate = record.Lend_Date,
                               KeeperId = member.User_Id,
                               UserEname = member.User_Ename,
                               UserCname = member.User_Cname
                           }).ToList();

            ViewBag.BookName = _context.BookData.FirstOrDefault(b => b.Book_Id == id)?.Book_Name;
            return View(records);
        }

        public IActionResult SortPartial(string sortOrder)
        {
            var query = from book in _context.BookData
                        join keeper in _context.Member on book.Book_Keeper equals keeper.User_Id into keepers
                        from keeper in keepers.DefaultIfEmpty()

                        join code in _context.BookCode.Where(c => c.Code_Type == "BOOK_STATUS")
                            on book.Book_Status equals code.Code_Id into codes
                        from code in codes.DefaultIfEmpty()

                        select new BookListItemViewModel
                        {
                            Book_Id = book.Book_Id,
                            Book_Name = book.Book_Name,
                            Book_Bought_Date = book.Book_Bought_Date,
                            Book_Class_Id = book.Book_Class_Id,
                            Book_Status = book.Book_Status,
                            Book_Status_Name = code.Code_Name ?? "",
                            Book_Keeper = book.Book_Keeper,
                            Book_Keeper_Name = keeper.User_Ename ?? ""
                        };

            // 🔃 排序
            query = sortOrder switch
            {
                "name" => query.OrderBy(b => b.Book_Name),
                "borrower" => query.OrderBy(b => b.Book_Keeper_Name),
                _ => query.OrderBy(b => b.Book_Name)
            };

            var viewModel = new BookQueryViewModel
            {
                BookData = query.ToList()
            };

            return PartialView("_BookTablePartial", viewModel);


        }
    }
}
