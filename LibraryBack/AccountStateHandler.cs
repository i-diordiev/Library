namespace LibraryBack
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs args);
        
    public class AccountEventArgs
    {
        public string Message { get; private set; }
        
        public int UserID { get; private set; }

        public AccountEventArgs(string message, int userId)
        {
            Message = message;
            UserID = userId;
        }
    }
}