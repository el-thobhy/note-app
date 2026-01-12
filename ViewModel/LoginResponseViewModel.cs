namespace Administrator.ViewModel
{
    public class LoginResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserData Data { get; set; }
        public int Pages { get; set; }
    }

    public class LoginRequestViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Is_Delete { get; set; }
        public string Otp { get; set; }
        public string ProfilePhoto { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }

}
