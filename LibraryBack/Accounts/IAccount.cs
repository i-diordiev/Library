namespace LibraryBack.Accounts
{
    /// <summary>
    /// So, it is interface. It has some abstract methods. They must be realised in inheritors....idk what else to write here...
    /// </summary>
    public interface IAccount
    {
        public void Create();

        public void Delete();
        
        public void LogIn();

        public void LogOut();
    }
}