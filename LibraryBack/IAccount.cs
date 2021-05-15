namespace LibraryBack
{
    /// <summary>
    /// So, it is interface. It has some abstract methods. They must be realised in inheritors....idk what else to write here...
    /// </summary>
    public interface IAccount
    {
        public void LogIn();

        public void LogOut();

        public void TakeBook(Library lib,int bookId);

        public void ReturnBook(Library lib,int bookId);
    }
}