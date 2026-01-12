namespace Administrator.ViewModel
{
    public class AccountViewModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool is_delete { get; set; }
        public string profilePhoto { get; set; }
        public string roleGroupId { get; set; }
        public List<string> roles { get; set; }
    }

    public class AccountApiResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<AccountViewModel> data { get; set; }
        public int pages { get; set; }
    }
    public class UpdateUserRoleRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserRole { get; set; }
    }
    public class DeleteAccountRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
    public class OtpViewModel
    {
        public string Otp { get; set; }
    }
    public class ResendOtpViewModel
    {
        public string Email { get; set; }
    }


}
