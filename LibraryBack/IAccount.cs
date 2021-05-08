namespace LibraryBack
{
    public interface IAccount
    {
        public void LogIn();

        public void LogOut();

        public void TakeBook(Library lib,int bookId);

        public void ReturnBook(Library lib,int bookId);
    }
}