using System;
using System.Collections.Generic;
using LibraryBack.Accounts;
using LibraryBack.Exceptions;

namespace LibraryBack
{
    /// <summary>
    /// Searching books types
    /// </summary>
    public enum SearchType
    {
        ByName, ByAuthor, ByTheme
    }

    /// <summary>
    /// Main class. Has users, admins, books. Can add/remove user, give/take book, add/remove book. Controlled by admin (librarian).
    /// </summary>
    public class Library
    {
        protected internal event StorageEventDelegate AddedBook;
        
        protected internal event StorageEventDelegate RemovedBook;
        
        public string Name { get; }

        private List<UserAccount> Users;

        private LibrarianAccount Admin;
        
        private static int _totalAccounts = 0;  // global count of registered accounts

        public List<Book> Books { get; }  // list of books

        private static int _totalBooks = 0;  // global count of books
        
        /// <summary>
        /// Constructor. Requires name of library and some handler of storage events (like adding and removing book).
        /// </summary>
        public Library(string name, StorageEventDelegate storageEventHandler)
        {
            Name = name;
            AddedBook += storageEventHandler;
            RemovedBook += storageEventHandler;
            Users = new List<UserAccount>();
            Admin = new LibrarianAccount(0, "admin");
            Books = new List<Book>();
        }
        
        /// <summary>
        /// Adds account. Requires type of account (admin/user) and some handler of account events.
        /// </summary>
        public void AddAccount(AccountEventDelegate accountEventHandler)
        {
            UserAccount newUser = new UserAccount(++_totalAccounts, 10);
                    
            newUser.Created += accountEventHandler;
            newUser.Deleted += accountEventHandler;
            newUser.TakenBook += accountEventHandler;
            newUser.ReturnedBook += accountEventHandler;
            newUser.LoggedIn += accountEventHandler;
            newUser.LoggedOut += accountEventHandler;

            Users.Add(newUser);
            newUser.Create();
        }

        /// <summary>
        /// Removes account. Requires ID of account. Warning! Account can't be removed if it has unreturned books!
        /// </summary>
        public void RemoveAccount(int userid)
        {
            int pos = 0;
            UserAccount acc = FindAccount(userid, ref pos);
            if (acc != null)
            {
                if (acc.MyBooks.Count != 0)
                    throw new BooksNotReturnedException();
                acc.Delete();
                Users.RemoveAt(pos);
            }
            else
                throw new WrongIdException();
        }
        
        /// <summary>
        /// Finds account. Requires ID.
        /// </summary>
        private UserAccount FindAccount(int id)
        {
            foreach (UserAccount user in Users)
            {
                if (user.Id == id)
                    return user;
            }
            return null;
        }
        
        /// <summary>
        /// Overloaded version of FindAccount. Requires ID and some int number to save position of account in list of users.
        /// </summary>
        private UserAccount FindAccount(int id, ref int pos)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Id == id)
                {
                    pos = i;
                    return Users[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Log in. Requires ID.
        /// </summary>
        public UserAccount Login(int id)
        {
            UserAccount acc = FindAccount(id);
            if (acc != null)
                acc.LogIn();
            else
                throw new WrongIdException();
            return acc;
        }

        /// <summary>
        /// Log in as admin. Requires password.
        /// </summary>
        public LibrarianAccount LoginAsAdmin(string password)
        {
            Admin.LogIn(password);
            return Admin;
        }

        /// <summary>
        /// Adds book to storage. Requires name of book, author name, thematics and quantity. ID will be given automatically.
        /// </summary>
        public void AddBook(string name, string author, string theme, int quantity)
        {
            int bookId = ++_totalBooks;
            Book newBook = new Book(bookId, name, author, theme, quantity);
            Books.Add(newBook);
            AddedBook?.Invoke(this, new StorageEventArgs("You've successfully added new book \"" + newBook.Name + "\", ID: " + bookId, bookId));
        }

        /// <summary>
        /// Removes book from storage. Warning! Book can't be removed if book 
        /// </summary>
        public void RemoveBook(int bookid)
        {
            int pos = 0;
            Book book = FindBookById(bookid, ref pos);
            if (book != null)
            {
                if (book.Available == book.Quantity)
                {
                    Books.RemoveAt(pos);
                    RemovedBook?.Invoke(this, new StorageEventArgs("You've successfully removed book \"" + book.Name +
                                                                   "\" from library \"" + Name + "\", book ID: " +
                                                                   bookid, bookid));
                }
                else
                    throw new BooksNotReturnedException();
            }
            else
                throw new WrongIdException();
        }

        /// <summary>
        /// Finds list of books. Requires search type (by name/author/theme) and some string with data.
        /// </summary>
        public List<Book> FindBook(SearchType type, string param)
        {
            List<Book> arrayToReturn = new List<Book>();
            foreach (Book currentBook in Books)
            {
                bool isSuitable = false;
                switch (type)
                {
                    case SearchType.ByName:
                        isSuitable = currentBook.Name.ToLower().Contains(param.ToLower());
                        break;
                    case SearchType.ByAuthor:
                        isSuitable = currentBook.Author.ToLower().Contains(param.ToLower());
                        break;
                    case SearchType.ByTheme:
                        isSuitable = currentBook.Theme.ToLower().Contains(param.ToLower());
                        break;
                }
                if (isSuitable)
                    arrayToReturn.Add(currentBook);
            }
            return arrayToReturn;
        }
        
        /// <summary>
        /// Used to taking/returning book by user. Requires ID.
        /// </summary>
        private Book FindBookById(int bookid)
        {
            foreach (Book book in Books)
            {
                if (book.Id == bookid)
                {
                    return book;
                }
            }
            return null;
        }

        /// <summary>
        /// Overloaded version. Requires ID and some int number to save position in books list.
        /// </summary>
        private Book FindBookById(int bookid, ref int pos)
        {
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].Id == bookid)
                {
                    pos = i;
                    return Books[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gives book to user. Requires ID.
        /// </summary>
        public Book GiveBook(int bookid)
        {
            Book book = FindBookById(bookid);
            if (book == null)
                throw new WrongIdException();
            if (book.Available > 0)
            {
                book.Available--;
                return book;
            }
            else
                throw new BookNotAvailableException();
        }

        /// <summary>
        /// Takes book from user. Requires ID.
        /// </summary>
        public void TakeBook(int bookid)
        {
            Book book = FindBookById(bookid);
            if (book == null)
                throw new WrongIdException();
            book.Available++;
        }
    }
}