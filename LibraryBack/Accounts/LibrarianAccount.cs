using LibraryBack.Exceptions;

namespace LibraryBack.Accounts
{
    /// <summary>
    /// Admin account. Can add/remove, take/return books.
    /// </summary>
    public class LibrarianAccount : Account
    {
        protected internal override event AccountEventDelegate Created;  // account creation event
        
        protected internal override event AccountEventDelegate Deleted;  // account deletion event
        
        protected internal override event AccountEventDelegate LoggedIn;  // log in event
        
        protected internal override event AccountEventDelegate LoggedOut;  // log out event
        
        protected internal event AccountEventDelegate TakenBook;  // event, book taken from library
        
        protected internal event AccountEventDelegate ReturnedBook;  // event, book returned to library

        public LibrarianAccount(int userId, int amount): base(userId, amount) {}  // constructor, calling base constructor
        
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
        /// User takes book from library
        /// </summary>
        public void TakeBook(Library lib, int bookId)
        {
            if (Available > 0)  // check for possibility of taking book
            {
                // search for the same book in user's book
                bool isMyBook = false;
                for (int i = 0; i < MyBooks.Count; i++)
                {
                    if (MyBooks[i].Id == bookId)
                        isMyBook = true;
                }

                // if book isn't already taken, take it
                if (!isMyBook)
                {
                    Book newBook = lib.GiveBook(bookId);
                    TakenBook?.Invoke(this,
                            new AccountEventArgs("You've taken book #" + bookId + ", your account ID: " + Id, Id));
                    MyBooks.Add(newBook);
                    Available--;
                }
                else
                    throw new BookAlreadyTakenException();
            }
            else
                throw new BookLimitReachedException();

        }
        
        /// <summary>
        /// User returns book to library
        /// </summary>
        public void ReturnBook(Library lib, int bookId)
        {
            // check if the user has a book
            Book book = null;
            for (int i = 0; i < MyBooks.Count; i++)
            {
                if (MyBooks[i].Id == bookId)
                    book = MyBooks[i];
            }

            // if user has this book, return it to library
            if (book != null)
            {
                lib.TakeBook(bookId);
                ReturnedBook?.Invoke(this,
                        new AccountEventArgs("You've returned book #" + bookId + ", your account ID: " + Id, Id));
                MyBooks.Remove(book);
                Available++;
            }
            else
                throw new BookNotTakenException();
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