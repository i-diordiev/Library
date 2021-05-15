using System;
using System.Collections.Generic;

namespace LibraryBack
{
    /// <summary>
    /// Unregistered user. Can only view catalogue
    /// </summary>
    public abstract class Account : IAccount
    {
        protected internal virtual event AccountEventDelegate Created;  // event, account created
        
        protected internal virtual event AccountEventDelegate Deleted;  // event, account deleted
        
        protected internal virtual event AccountEventDelegate LoggedIn;  // event, logged in 
        
        protected internal virtual event AccountEventDelegate LoggedOut;  // event, logged out
        
        protected internal event AccountEventDelegate TakenBook;  // event, book taken from library
        
        protected internal event AccountEventDelegate ReturnedBook;  // event, book returned to library
        
        public int Id { get; private set; }  // personal ID of account

        public int Available { get; private set; }  // number of available books, default = 10

        public List<Book> MyBooks;  // array with books, taken by user

        public Account(int id, int amount)
        {
            Id = id;
            Available = amount;
            MyBooks = new List<Book>();
        }  // constructor, 

        // methods for calling events
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
                    if (MyBooks[i].ID == bookId)
                        isMyBook = true;
                }

                // if book isn't already taken, take it
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
        
        /// <summary>
        /// User returns book to library
        /// </summary>
        public void ReturnBook(Library lib, int bookId)
        {
            // check if the user has a book
            Book book = null;
            for (int i = 0; i < MyBooks.Count; i++)
            {
                if (MyBooks[i].ID == bookId)
                    book = MyBooks[i];
            }

            // if user has this book, return it to library
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