namespace GoodNewsTask.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int SelectedPositiveLevel { get; set; }
        public bool IsBlocked { get; set; }
    }
}
