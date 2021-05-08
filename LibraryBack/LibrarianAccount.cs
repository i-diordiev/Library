namespace LibraryBack
{
    public class LibrarianAccount : Account
    {
        protected internal override event AccountStateHandler Created;
        
        protected internal override event AccountStateHandler Deleted;
        
        protected internal override event AccountStateHandler LoggedIn;
        
        protected internal override event AccountStateHandler LoggedOut;

        public LibrarianAccount(int userId, int amount): base(userId, amount) {}
        
        public override void Create()
        {
            Created?.Invoke(this, new AccountEventArgs("You've successfully created ADMIN account, your ID - " + Id, Id));
        }
        
        public override void Delete()
        {
            Deleted?.Invoke(this, new AccountEventArgs("You've successfully deleted ADMIN account, your ID - " + Id, Id));
        }
        
        public override void LogIn()
        {
            LoggedIn?.Invoke(this, new AccountEventArgs("You logged in, ADMIN account, ID - " + Id, Id));
        }

        public override void LogOut()
        {
            LoggedOut?.Invoke(this, new AccountEventArgs("You logged off, ADMIN account, ID - " + Id, Id));
        }

        public void AddBook(Library lib, string name, string author, string theme, int quantity)
        {
            lib.AddBook(name, author, theme, quantity);
        }

        public void RemoveBook(Library lib, int bookId)
        {
            lib.RemoveBook(bookId);
        }
    }
}