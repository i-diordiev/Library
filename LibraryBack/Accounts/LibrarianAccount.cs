using LibraryBack.Exceptions;

namespace LibraryBack.Accounts
{
    /// <summary>
    /// Admin account. Can add and remove books in library.
    /// </summary>
    public class LibrarianAccount : Account
    {
        protected internal override event AccountEventDelegate Created;  // account creation event
        
        protected internal override event AccountEventDelegate Deleted;  // account deletion event
        
        protected internal override event AccountEventDelegate LoggedIn;  // log in event
        
        protected internal override event AccountEventDelegate LoggedOut;  // log out event
        
        public LibrarianAccount(int userId): base(userId) {}  // constructor, calling base constructor
        
        // methods for calling events
        public override void Create()
        {
            Created?.Invoke(this, new AccountEventArgs("You've successfully created admin account, ID: " + Id, Id));
        }
        
        public override void Delete()
        {
            Deleted?.Invoke(this, new AccountEventArgs("You've successfully deleted admin account, ID: " + Id, Id));
        }
        
        public override void LogIn()
        {
            LoggedIn?.Invoke(this, new AccountEventArgs("You've logged in admin account, ID: " + Id, Id));
        }

        public override void LogOut()
        {
            LoggedOut?.Invoke(this, new AccountEventArgs("You've logged off admin account, ID: " + Id, Id));
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