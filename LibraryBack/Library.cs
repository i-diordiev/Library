using System;
using System.Collections.Generic;

namespace LibraryBack
{
    public enum AccountType
    {
        User,
        Librarian
    }

    public enum SearchType
    {
        ByName, ByAuthor, ByTheme
    }

    public class Library

    {
        protected internal event StorageEventDelegate AddedBook;
        
        protected internal event StorageEventDelegate RemovedBook;
        public string Name { get; private set; }

        private List<UserAccount> Users;

        private List<LibrarianAccount> Admins;
        
        private static int _totalAccounts = 0;

        private List<Book> Books;

        private static int _totalBooks = 0;

        public Library(string name, StorageEventDelegate storageEventHandler)
        {
            Name = name;
            AddedBook += storageEventHandler;
            RemovedBook += storageEventHandler;
            Users = new List<UserAccount>();
            Admins = new List<LibrarianAccount>();
            Books = new List<Book>();
        }
      
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

        public void AddBook(string name, string author, string theme, int quantity)
        {
            int bookId = _totalBooks++;
            Book newBook = new Book(bookId, name, author, theme, quantity);
            Books.Add(newBook);
            AddedBook?.Invoke(this, new StorageEventArgs("You've successfully added new book, ID - " + bookId, bookId));
        }

        public void RemoveBook(int bookid)
        {
            int pos = 0;
            Book b = FindBookById(bookid, ref pos);
            if (b != null)
            {
                Books.RemoveAt(pos);
                RemovedBook?.Invoke(this, new StorageEventArgs("You've successfully removed book from library \""
                                                       + Name + "\", book ID - " + bookid, bookid));
            }
        }

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