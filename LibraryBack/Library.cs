using System;

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
        protected internal event StorageStateHandler AddedBook;
        
        protected internal event StorageStateHandler RemovedBook;
        public string Name { get; private set; }

        public UserAccount[] Users;

        public LibrarianAccount[] Admins;
        
        private static int _totalAccounts = 0;

        public Book[] Books;

        private static int _totalBooks = 0;

        public Library(string name, StorageStateHandler addedHandler, StorageStateHandler removedHandler)
        {
            Name = name;
            AddedBook += addedHandler;
            RemovedBook += removedHandler;
        }
      
        public void AddAccount(AccountType type,
            AccountStateHandler createdHandler, AccountStateHandler deletedHandler,
            AccountStateHandler takenHandler, AccountStateHandler returnedHandler,
            AccountStateHandler loggedInHandler, AccountStateHandler loggedOutHandler)
        {
            switch (type)
            {
                case AccountType.Librarian:
                    LibrarianAccount newAdmin = new LibrarianAccount(_totalAccounts++, 10000);
                    newAdmin.Created += createdHandler;
                    newAdmin.Deleted += deletedHandler;
                    newAdmin.TakenBook += takenHandler;
                    newAdmin.ReturnedBook += returnedHandler;
                    newAdmin.LoggedIn += loggedInHandler;
                    newAdmin.LoggedOut += loggedOutHandler;

                    if (Admins == null)
                    {
                        Admins = new LibrarianAccount[] {newAdmin};
                    }
                    else
                    {
                        LibrarianAccount[] tempArray = new LibrarianAccount[Admins.Length + 1];
                        for (int i = 0; i < Admins.Length; i++)
                            tempArray[i] = Admins[i];
                        tempArray[Admins.Length] = newAdmin;
                        Admins = tempArray;
                    }

                    newAdmin.Create();
                    break;
                case AccountType.User:
                    UserAccount newUser = new UserAccount(_totalAccounts++, 10);
                    
                    newUser.Created += createdHandler;
                    newUser.Deleted += deletedHandler;
                    newUser.TakenBook += takenHandler;
                    newUser.ReturnedBook += returnedHandler;
                    newUser.LoggedIn += loggedInHandler;
                    newUser.LoggedOut += loggedOutHandler;
                    
                    if (Users == null)
                    {
                        Users = new UserAccount[] {newUser};
                    }
                    else
                    {
                        UserAccount[] tempArray = new UserAccount[Users.Length + 1];
                        for (int i = 0; i < Users.Length; i++)
                            tempArray[i] = Users[i];
                        tempArray[Users.Length] = newUser;
                        Users = tempArray;
                    }
                    
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
                    if (acc.MyBooks.Length != 0)
                        throw new Exception("You can't delete this account! You need return all books that you've taken.");
                    UserAccount[] tempArray = new UserAccount[Users.Length - 1];
                    for (int i = 0; i < Users.Length; i++)
                    {
                        if (i < pos)
                            tempArray[i] = Users[i];
                        else
                            tempArray[i] = Users[i - 1];
                    }
                    Users = tempArray;
                }
                else if (acc is LibrarianAccount)
                {
                    if (acc.MyBooks.Length != 0)
                        throw new Exception("You can't delete this account! You need return all books that you've taken.");
                    LibrarianAccount[] tempArray = new LibrarianAccount[Admins.Length - 1];
                    for (int i = 0; i < Admins.Length; i++)
                    {
                        if (i < pos)
                            tempArray[i] = Admins[i];
                        else
                            tempArray[i] = Admins[i - 1];
                    }

                    Admins = tempArray;
                }
                else
                    throw new Exception("Something has gone wrong");
            }
        }
        
        public Account FindAccount(int id)
        {
            for (int i = 0; i < Users.Length; i++)
            {
                if (Users[i].Id == id)
                {
                    return Users[i];
                }
            }
            for (int i = 0; i < Admins.Length; i++)
            {
                if (Admins[i].Id == id)
                {
                    return Admins[i];
                }
            }
            return null;
        }
        
        public Account FindAccount(int id, ref int pos)
        {
            if (Users == null || Users.Length == 0)
                return null;
            for (int i = 0; i < Users.Length; i++)
            {
                if (Users[i].Id == id)
                {
                    pos = i;
                    return Users[i];
                }
            }
            if (Admins == null || Admins.Length == 0)
                return null;
            for (int i = 0; i < Admins.Length; i++)
            {
                if (Admins[i].Id == id)
                {
                    pos = i;
                    return Admins[i];
                }
            }
            return null;
        }

        public int Login(AccountType type, int id)
        {
            int pos = 0;
            Account acc = FindAccount(id, ref pos);
            if (acc != null)
            {
                if (acc is LibrarianAccount)
                    Admins[pos].LogIn();
                else if (acc is UserAccount)
                    Users[pos].LogIn();
                else
                    throw new Exception("Something has gone wrong");
            }
            else
                throw new Exception("Wrong ID!");
            return pos;
        }

        public void AddBook(string name, string author, string theme, int quantity)
        {
            int bookId = _totalBooks++;
            Book newBook = new Book(bookId, name, author, theme, quantity);
            if (Books == null)
                Books = new Book[] {newBook};
            else
            {
                Book[] tempArray = new Book[Books.Length + 1];
                for (int i = 0; i < Books.Length; i++)
                    tempArray[i] = Books[i];
                tempArray[Books.Length] = newBook;
                Books = tempArray;
            }

            if (AddedBook != null)
                AddedBook(this, new StorageEventArgs("You've successfully added new book, ID - " + bookId, bookId));
        }

        public void RemoveBook(int bookid)
        {
            int pos = 0;
            Book b = FindBookById(bookid, ref pos);
            if (b != null)
            {
                Book[] tempArray = new Book[Books.Length - 1];
                for (int i = 0; i < Books.Length; i++)
                {
                    if (i < pos)
                        tempArray[i] = Books[i];
                    else
                        tempArray[i] = Books[i - 1];
                }

                Books = tempArray;
                if (RemovedBook != null)
                    RemovedBook(this, new StorageEventArgs("You've successfully removed book from library \""
                                                           + Name + "\", book ID - " + bookid, bookid));

            }
        }

        public Book[] FindBook(SearchType type, string param)
        {
            Book[] ArrayToReturn = null;
            if (Books == null || Books.Length == 0)
                return ArrayToReturn;
            for (int i = 0; i < Books.Length; i++)
            {
                bool IsSuitable = false;
                switch (type)
                {
                    case SearchType.ByName:
                        IsSuitable = Books[i].Name.Contains(param);
                        break;
                    case SearchType.ByAuthor:
                        IsSuitable = Books[i].Author.Contains(param);
                        break;
                    case SearchType.ByTheme:
                        IsSuitable = Books[i].Theme.Contains(param);
                        break;
                }
                if (IsSuitable)
                {
                    if (ArrayToReturn == null)
                        ArrayToReturn = new Book[] {Books[i]};
                    else
                    {
                        Book[] t1 = new Book[ArrayToReturn.Length + 1];
                        for (int j = 0; j < ArrayToReturn.Length; j++)
                            t1[j] = ArrayToReturn[j];
                        t1[ArrayToReturn.Length] = Books[i];
                        ArrayToReturn = t1;
                    }
                }
            }
            return ArrayToReturn;
        }
        
        public Book FindBookById(int bookid)
        {
            for (int i = 0; i < Books.Length; i++)
            {
                if (Books[i].ID == bookid)
                {
                    return Books[i];
                }
            }
            return null;
        }

        public Book FindBookById(int bookid, ref int pos)
        {
            for (int i = 0; i < Books.Length; i++)
            {
                if (Books[i].ID == bookid)
                {
                    pos = i;
                    return Books[i];
                }
            }
            return null;
        }

        public void GiveBook(int bookid)
        {
            int pos = 0;
            Book b = FindBookById(bookid, ref pos);
            if (b == null)
                throw new Exception("Book not found!");
            else
                if (Books[pos].Available > 0)
                    Books[pos].Available--;
                else
                    throw new Exception("This book in no longer available!");
        }

        public void TakeBook(int bookid)
        {
            int pos = 0;
            Book b = FindBookById(bookid, ref pos);
            if (b == null)
                throw new Exception("Book not found!");
            else
                Books[pos].Available++;
        }
    }
}