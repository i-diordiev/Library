using System;
using System.Collections.Generic;

namespace LibraryBack
{
    /// <summary>
    /// Types of accounts
    /// </summary>
    public enum AccountType
    {
        User,
        Librarian
    }

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

        private List<LibrarianAccount> Admins;
        
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
            Admins = new List<LibrarianAccount>();
            Books = new List<Book>();
        }
        
        /// <summary>
        /// Adds account. Requires type of account (admin/user) and some handler of account events.
        /// </summary>
        public void AddAccount(AccountType type,
            AccountEventDelegate accountEventHandler)
        {
            switch (type)
            {
                case AccountType.Librarian:
                    LibrarianAccount newAdmin = new LibrarianAccount(_totalAccounts++, 10000);
                    newAdmin.Created += accountEventHandler;
                    newAdmin.Deleted += accountEventHandler;
                    newAdmin.TakenBook += accountEventHandler;
                    newAdmin.ReturnedBook += accountEventHandler;
                    newAdmin.LoggedIn += accountEventHandler;
                    newAdmin.LoggedOut += accountEventHandler;

                    Admins.Add(newAdmin);
                    newAdmin.Create();
                    break;
                case AccountType.User:
                    UserAccount newUser = new UserAccount(_totalAccounts++, 10);
                    
                    newUser.Created += accountEventHandler;
                    newUser.Deleted += accountEventHandler;
                    newUser.TakenBook += accountEventHandler;
                    newUser.ReturnedBook += accountEventHandler;
                    newUser.LoggedIn += accountEventHandler;
                    newUser.LoggedOut += accountEventHandler;

                    Users.Add(newUser);
                    newUser.Create();
                    break;
            }
        }

        /// <summary>
        /// Removes account. Requires ID of account. Warning! Account can't be removed if it has unreturned books!
        /// </summary>
        public void RemoveAccount(int userid)
        {
            int pos = 0;
            Account acc = FindAccount(userid, ref pos);
            if (acc != null)
            {
                if (acc is UserAccount)
                {
                    if (acc.MyBooks.Count != 0)
                        throw new Exception(
                            "You can't delete this account! You need return all books that you've taken.");
                    else
                        Users.RemoveAt(pos);
                }
                else if (acc is LibrarianAccount)
                {
                    if (acc.MyBooks.Count != 0)
                        throw new Exception(
                            "You can't delete this account! You need return all books that you've taken.");
                    else
                        Admins.RemoveAt(pos);
                }
                else
                    throw new Exception("Something has gone wrong");
            }
        }
        
        /// <summary>
        /// Finds account. Requires ID.
        /// </summary>
        private Account FindAccount(int id)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Id == id)
                {
                    return Users[i];
                }
            }
            for (int i = 0; i < Admins.Count; i++)
            {
                if (Admins[i].Id == id)
                {
                    return Admins[i];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Overloaded version of FindAccount. Requires ID and some int number to save position of account in list of users.
        /// </summary>
        private Account FindAccount(int id, ref int pos)
        {
            if (Users == null || Users.Count == 0)
                return null;
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Id == id)
                {
                    pos = i;
                    return Users[i];
                }
            }
            if (Admins == null || Admins.Count == 0)
                return null;
            for (int i = 0; i < Admins.Count; i++)
            {
                if (Admins[i].Id == id)
                {
                    pos = i;
                    return Admins[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Log in. Requires account type (user/admin) and ID.
        /// </summary>
        public Account Login(AccountType type, int id)
        {
            Account acc = FindAccount(id);
            if (acc != null)
            {
                acc.LogIn();
            }
            else
                throw new Exception("Wrong ID!");
            return acc;
        }

        /// <summary>
        /// Adds book to storage. Requires name of book, author name, thematics and quantity. ID will be given automatically.
        /// </summary>
        public void AddBook(string name, string author, string theme, int quantity)
        {
            int bookId = _totalBooks++;
            Book newBook = new Book(bookId, name, author, theme, quantity);
            Books.Add(newBook);
            AddedBook?.Invoke(this, new StorageEventArgs("You've successfully added new book, ID - " + bookId, bookId));
        }

        /// <summary>
        /// Removes book from storage. Warning! Book can't be removed if book 
        /// </summary>
        public void RemoveBook(int bookid)
        {
            int pos = 0;
            Book b = FindBookById(bookid, ref pos);
            if (b != null)
            {
                if (b.Available == b.Quantity)
                {
                    Books.RemoveAt(pos);
                    RemovedBook?.Invoke(this, new StorageEventArgs("You've successfully removed book from library \""
                                                                   + Name + "\", book ID - " + bookid, bookid));
                }
                else
                    throw new Exception("Not all users returned this book!");
            }
        }

        /// <summary>
        /// Finds list of books. Requires search type (by name/author/theme) and some string with data.
        /// </summary>
        public List<Book> FindBook(SearchType type, string param)
        {
            List<Book> arrayToReturn = new List<Book>();
            for (int i = 0; i < Books.Count; i++)
            {
                Book currentBook = Books[i];
                bool isSuitable = false;
                switch (type)
                {
                    case SearchType.ByName:
                        isSuitable = currentBook.Name.Contains(param);
                        break;
                    case SearchType.ByAuthor:
                        isSuitable = currentBook.Author.Contains(param);
                        break;
                    case SearchType.ByTheme:
                        isSuitable = currentBook.Theme.Contains(param);
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
            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].ID == bookid)
                {
                    return Books[i];
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
                if (Books[i].ID == bookid)
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
                throw new Exception("Book not found!");
            else if (book.Available > 0)
            {
                book.Available--;
                return book;
            }
            else
                throw new Exception("This book in no longer available!");
        }

        /// <summary>
        /// Takes book from user. Requires ID.
        /// </summary>
        public void TakeBook(int bookid)
        {
            Book book = FindBookById(bookid);
            if (book == null)
                throw new Exception("Book not found!");
            else
                book.Available++;
        }
    }
}