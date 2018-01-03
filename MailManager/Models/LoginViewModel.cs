namespace MailManager.Models
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);
    }
}