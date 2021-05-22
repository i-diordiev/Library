namespace LibraryBack
{
    /// <summary>
    /// used to bind front and back
    /// </summary>
    public delegate void AccountEventDelegate(object sender, AccountEventArgs args);
        
    public class AccountEventArgs
    {
        public string Message { get; private set; }
        
        public int UserId { get; private set; }

        public AccountEventArgs(string message, int userId)
        {
            Message = message;
            UserId = userId;
        }
    }
}