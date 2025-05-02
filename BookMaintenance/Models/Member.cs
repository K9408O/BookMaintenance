namespace BookMaintenance.Models
{
    public class Member
    {
        public string User_Id { get; set; }
        public string? User_Cname { get; set; }
        public string? User_Ename { get; set; }
        public DateTime? Create_Date { get; set; }
        public string? Create_User { get; set; }
        public DateTime? Modify_Date { get; set; }
        public string? Modify_User { get; set; }


    }
}
