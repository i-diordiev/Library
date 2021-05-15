namespace LibraryBack
{
    /// <summary>
    /// Admin account. Can add and remove books.
    /// </summary>
    public class LibrarianAccount : Account
    {
        protected internal override event AccountEventDelegate Created;  // account creation event
        
        protected internal override event AccountEventDelegate Deleted;  // account deletion event
        
        protected internal override event AccountEventDelegate LoggedIn;  // log in event
        
        protected internal override event AccountEventDelegate LoggedOut;  // log out event

        public LibrarianAccount(int userId, int amount): base(userId, amount) {}  // constructor, calling base constructor
        
        // methods for calling events
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

        /// <summary>
        /// Add new book to library
        /// </summary>
        public void AddBook(Library lib, string name, string author, string theme, int quantity)
        {
            lib.AddBook(name, author, theme, quantity);
        }

        /// <summary>
        /// Delete book from library
        /// </summary>
        public void RemoveBook(Library lib, int bookId)
        {
            lib.RemoveBook(bookId);
        }
    }
}