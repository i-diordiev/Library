using LibraryBack.Exceptions;

namespace LibraryBack.Accounts
{
    /// <summary>
    /// Admin account. Can add and remove books in library.
    /// </summary>
    public class LibrarianAccount: Account
    {
        private string _password;

        protected internal override event AccountEventDelegate LoggedIn;  // log in event
        
        protected internal override event AccountEventDelegate LoggedOut;  // log out event

        public LibrarianAccount(int userId, string password) : base(userId)
        {
            _password = password;
        }  // constructor, calling base constructor
        
        // methods for calling events
        public void LogIn(string password)
        {
            if (password == _password)
                LoggedIn?.Invoke(this, new AccountEventArgs("You've logged in admin account, ID: " + Id, Id));
            else
                throw new WrongPasswordException();
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