using System;

namespace LibraryBack
{
    
    public abstract class Account : IAccount
    {
        protected internal virtual event AccountStateHandler Created;
        
        protected internal virtual event AccountStateHandler Deleted;
        
        protected internal virtual event AccountStateHandler LoggedIn;
        
        protected internal virtual event AccountStateHandler LoggedOut;
        
        protected internal event AccountStateHandler TakenBook;
        
        protected internal event AccountStateHandler ReturnedBook;
        
        public int Id { get; private set; }

        public int Available { get; private set; }

        public int[] MyBooks;

        public Account(int id, int amount)
        {
            Id = id;
            Available = amount;
        }

        public virtual void Create()
        {
            Created?.Invoke(this, new AccountEventArgs("You've successfully created account, ID - " + Id, Id));
        }
        
        public virtual void Delete()
        {
            Deleted?.Invoke(this, new AccountEventArgs("You've successfully deleted account, ID - " + Id, Id));
        }
        
        public virtual void LogIn()
        {
            LoggedIn?.Invoke(this, new AccountEventArgs("You logged in, account ID" + Id, Id));
        }

        public virtual void LogOut()
        {
            LoggedOut?.Invoke(this, new AccountEventArgs("You logged off, account ID" + Id, Id));
        }

        public void TakeBook(Library lib, int bookId)
        {
            if (Available > 0)
            {
                bool isMyBook = false;
                for (int i = 0; i < MyBooks.Length; i++)
                {
                    if (MyBooks[i] == bookId)
                        isMyBook = true;
                }

                if (!isMyBook)
                {
                    lib.GiveBook(bookId);
                    TakenBook?.Invoke(this,
                            new AccountEventArgs("You've taken book #" + bookId + ", your account #" + Id, Id));
                    if (MyBooks == null)
                        MyBooks = new[] {bookId};
                    else
                    {
                        int[] tempArray = new int[MyBooks.Length + 1];
                        for (int i = 0; i < MyBooks.Length; i++)
                            tempArray[i] = MyBooks[i];
                        tempArray[MyBooks.Length] = bookId;
                        MyBooks = tempArray;
                    }
                    Available--;
                }
                else
                    throw new Exception("You already have this book!");
            }
            else
                throw new Exception("You've reached maximum of books!");

        }

        public void ReturnBook(Library lib, int bookId)
        {
            bool isMyBook = false;
            for (int i = 0; i < MyBooks.Length; i++)
            {
                if (MyBooks[i] == bookId)
                    isMyBook = true;
            }

            if (isMyBook)
            {
                lib.TakeBook(bookId);
                ReturnedBook?.Invoke(this,
                        new AccountEventArgs("You've returned book #" + bookId + ", your account #" + Id, Id));
                int[] tempArray = new int[MyBooks.Length - 1];
                bool isDeleted = false;
                for (int i = 0; i < MyBooks.Length; i++)
                {
                    if (MyBooks[i] == bookId)
                        isDeleted = true;
                    if (isDeleted)
                        tempArray[i] = MyBooks[i + 1];
                    else
                        tempArray[i] = MyBooks[i];
                }
                MyBooks = tempArray;
                Available++;
            }
            else
                throw new Exception("You don't have this book!");
        }
    }
}