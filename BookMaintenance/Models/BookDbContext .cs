using Microsoft.EntityFrameworkCore;

namespace BookMaintenance.Models
{
    public class BookDbContext:DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        public DbSet<BookData> BookData { get; set; }
        public DbSet<BookClass> BookClass { get; set; }
        public DbSet<BookCode> BookCode { get; set; }
        public DbSet<BookLendRecord> BookLendRecord { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<SpanTable> SpanTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookData>().ToTable("BOOK_DATA");
            modelBuilder.Entity<BookData>().HasKey(b => b.Book_Id);
            modelBuilder.Entity<BookData>().Property(b => b.Book_Id).HasColumnName("BOOK_ID");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Name).HasColumnName("BOOK_NAME");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Class_Id).HasColumnName("BOOK_CLASS_ID");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Author).HasColumnName("BOOK_AUTHOR");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Bought_Date).HasColumnName("BOOK_BOUGHT_DATE");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Publisher).HasColumnName("BOOK_PUBLISHER");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Note).HasColumnName("BOOK_NOTE");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Status).HasColumnName("BOOK_STATUS");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Keeper).HasColumnName("BOOK_KEEPER");
            modelBuilder.Entity<BookData>().Property(b => b.Book_Amount).HasColumnName("BOOK_AMOUNT");
            modelBuilder.Entity<BookData>().Property(b => b.Create_Date).HasColumnName("CREATE_DATE");
            modelBuilder.Entity<BookData>().Property(b => b.Create_User).HasColumnName("CREATE_USER");
            modelBuilder.Entity<BookData>().Property(b => b.Modify_Date).HasColumnName("MODIFY_DATE");
            modelBuilder.Entity<BookData>().Property(b => b.Modify_User).HasColumnName("MODIFY_USER");
            modelBuilder.Entity<BookClass>().ToTable("BOOK_CLASS");
            modelBuilder.Entity<BookClass>().HasKey(x => x.Book_Class_Id);
            modelBuilder.Entity<BookData>().ToTable("BOOK_DATA");
            modelBuilder.Entity<BookClass>().ToTable("BOOK_CLASS");
            modelBuilder.Entity<BookCode>().ToTable("BOOK_CODE");
            modelBuilder.Entity<BookLendRecord>().ToTable("BOOK_LEND_RECORD");
            modelBuilder.Entity<Member>().ToTable("MEMBER_M");
            modelBuilder.Entity<SpanTable>().ToTable("SPAN_TABLE");
            modelBuilder.Entity<Member>().ToTable("MEMBER_M");
            modelBuilder.Entity<Member>().HasKey(x => x.User_Id);
        }

    }
}
