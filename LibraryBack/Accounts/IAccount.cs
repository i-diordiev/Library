namespace LibraryBack.Accounts
{
    /// <summary>
    /// Interface of any account. Log in and log out must be realised.
    /// </summary>
    public interface IAccount
    {
        public void LogIn();

        public void LogOut();
    }
}