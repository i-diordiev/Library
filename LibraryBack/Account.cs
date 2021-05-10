using System;
using System.Collections.Generic;

namespace LibraryBack
{
    
    public abstract class Account : IAccount
    {
        protected internal virtual event AccountEventDelegate Created;
        
        protected internal virtual event AccountEventDelegate Deleted;
        
        protected internal virtual event AccountEventDelegate LoggedIn;
        
        protected internal virtual event AccountEventDelegate LoggedOut;
        
        protected internal event AccountEventDelegate TakenBook;
        
        protected internal event AccountEventDelegate ReturnedBook;
        
        public int Id { get; private set; }

        public int Available { get; private set; }

        public List<Book> MyBooks;

        public Account(int id, int amount)
        {
            Id = id;
            Available = amount;
            MyBooks = new List<Book>();
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
                for (int i = 0; i < MyBooks.Count; i++)
                {
                    if (MyBooks[i].ID == bookId)
                        isMyBook = true;
                }

                if (!isMyBook)
                {
                    Book newBook = lib.GiveBook(bookId);
                    TakenBook?.Invoke(this,
                            new AccountEventArgs("You've taken book #" + bookId + ", your account #" + Id, Id));
                    MyBooks.Add(newBook);
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
            Book book = null;
            for (int i = 0; i < MyBooks.Count; i++)
            {
                if (MyBooks[i].ID == bookId)
                    book = MyBooks[i];
            }

            if (book != null)
            {
                lib.TakeBook(bookId);
                ReturnedBook?.Invoke(this,
                        new AccountEventArgs("You've returned book #" + bookId + ", your account #" + Id, Id));
                MyBooks.Remove(book);
                Available++;
            }
            else
                throw new Exception("You don't have this book!");
        }
    }
}