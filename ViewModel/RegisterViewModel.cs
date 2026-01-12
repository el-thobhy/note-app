namespace Administrator.ViewModel
{
    public class RegisterViewModel
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
    }

}
