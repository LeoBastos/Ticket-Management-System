namespace Ryze.System.Domain.Interfaces.Accounts
{
    public class RegisterResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
