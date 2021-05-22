using System;
using System.Collections.Generic;
using System.IO;
using LibraryBack;
using LibraryBack.Accounts;
using LibraryBack.Exceptions;

namespace LibraryFront
{
    class Program
    {
        static void Main()
        {
            Library library = new Library("My Bibla", StorageEventHandler);
            
            Console.Write("Do you want to import list of books from .csv file? (Y/N) ");  // asking to export books from previous run
            string answer = Console.ReadLine();
            
            if (answer == "y" || answer == "Y")  // if yes, import books from .csv file
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Type name of .csv file: ");
                        string fileName = Console.ReadLine();
                        if (fileName == null)
                            throw new Exception("Please, type name of file and try again");
                        
                        using (var reader = new StreamReader(fileName))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                var values = line.Split(",");
                                library.AddBook(values[0], values[1], values[2], Convert.ToInt32(values[3]));
                            }
                        }
                        
                        Console.WriteLine("Import successful!");
                        break;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler(ex);
                    }
                }
            }

            ShowStartMenu(library);
            Console.Clear();
            
            Console.Write("Do you want to export list of books to .csv file? (Y/N) ");  // asking to save books from library to .csv file
            answer = Console.ReadLine();
            
            if (answer == "y" || answer == "Y")  // if yes, writing list ot .csv
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Type name of .csv file: ");
                        string fileName = Console.ReadLine();
                        if (fileName == null)
                            throw new Exception("Please, type name of file and try again");
                        
                        using (var writer = new StreamWriter(fileName))
                        {
                            foreach (Book book in library.Books)
                            {
                                string text = book.Name + "," + book.Author + "," + book.Theme + "," +
                                              Convert.ToString(book.Quantity);
                                writer.WriteLine(text);
                            }
                        }
                        
                        Console.WriteLine("Export successful!");
                        break;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Shows menu of unregistered user (start menu).
        /// </summary>
        static void ShowStartMenu(Library lib)
        {
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("Welcome to " + lib.Name);
                Console.WriteLine("List of options:\n" +
                                  "1. Register new account\n" +
                                  "2. Login\n" +
                                  "3. View all books\n" +
                                  "4. Find book\n" +
                                  "5. Quit program");
                Console.Write("Choose option: ");
                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    int t;
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine("Choose type of account:\n" +
                                              "1. User\n" +
                                              "2. Admin");
                            t = Convert.ToInt32(Console.ReadLine());
                            if (t == 1)
                                lib.AddAccount(AccountType.User, AccountEventHandler);
                            else if (t == 2)
                                lib.AddAccount(AccountType.Librarian, AccountEventHandler);
                            else
                                Console.WriteLine("Please, choose 1 or 2");

                            break;
                        case 2:
                            Console.WriteLine("Choose type of account:\n" +
                                              "1. User\n" +
                                              "2. Admin");
                            t = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Ented ID of your account: ");
                            int id = Convert.ToInt32(Console.ReadLine());
                            if (t == 1)
                            {
                                UserAccount acc = lib.Login(AccountType.User, id) as UserAccount;
                                Console.Clear();
                                ShowUserMenu(lib, acc);
                            }
                            else if (t == 2)
                            {
                                LibrarianAccount acc = lib.Login(AccountType.Librarian, id) as LibrarianAccount;
                                Console.Clear();
                                ShowAdminMenu(lib, acc);
                            }
                            else
                                Console.WriteLine("Please, choose 1 or 2");

                            break;
                        case 3:
                            ShowAllBooks(lib);
                            break;
                        case 4:
                            FindBookMenu(lib);
                            break;
                        case 5:
                            Console.Clear();
                            alive = false;
                            break;
                    }
                }
                catch (FormatException)
                {
                    ExceptionHandler("Wrong input format! Try again.");
                }
                catch (WrongIdException)
                {
                    ExceptionHandler("You've entered wrong ID, try again!");
                }
                catch (BookAlreadyTakenException)
                {
                    ExceptionHandler("You already have this book!");
                }
                catch (BookLimitReachedException)
                {
                    ExceptionHandler("You've reached your book limit!");
                }
                catch (BookNotAvailableException)
                {
                    ExceptionHandler("This book is no more available!");
                }
                catch (BookNotTakenException)
                {
                    ExceptionHandler("You don't have this book!");
                }
                catch (BooksNotReturnedException)
                {
                    ExceptionHandler("You didn't return all books!");
                }
                catch (Exception ex)
                {
                    ExceptionHandler(ex);
                }
            }
        }

        /// <summary>
        /// Shows user menu.
        /// </summary>
        static void ShowUserMenu(Library lib, UserAccount account)
        {
            Console.WriteLine("Welcome to " + lib.Name);
            Console.WriteLine("You are logged in as user");
            Console.WriteLine("Your ID: " + account.Id);
            
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("List of options:\n" +
                                  "1. View my books\n" +
                                  "2. View all books\n" +
                                  "3. Find book\n" +
                                  "4. Take book\n" +
                                  "5. Return book\n" +
                                  "6. Delete account\n" +
                                  "7. Log out\n");
                Console.WriteLine("Choose option: ");
                int option = Convert.ToInt32(Console.ReadLine());
                int id;
                switch (option)
                {
                    case 1:
                        if (account.MyBooks.Count == 0)
                            Console.WriteLine("You have no books");
                        else
                        {
                            Console.WriteLine("My books: ");
                            for (int i = 0; i < account.MyBooks.Count; i++)
                            {
                                Console.Write(i + 1);
                                Console.WriteLine(".\tName: " + account.MyBooks[i].Name + 
                                                  "\n\tAuthor: " + account.MyBooks[i].Author +
                                                  "\n\tTheme: " + account.MyBooks[i].Theme + 
                                                  "\n\tID: " + account.MyBooks[i].Id);
                            }
                        }
                        break;
                    case 2:
                        ShowAllBooks(lib);
                        break;
                    case 3:
                        FindBookMenu(lib);
                        break;
                    case 4:
                        Console.Write("Enter book ID: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        account.TakeBook(lib, id);
                        break;
                    case 5:
                        Console.Write("Enter book ID: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        account.ReturnBook(lib, id);
                        break;
                    case 6: 
                        Console.Write("Type \"DELETE MY ACCOUNT\" to proceed: ");
                        string confirmation = Console.ReadLine();
                        if (confirmation == "DELETE MY ACCOUNT")
                            lib.RemoveAccount(account.Id);
                        Console.Clear();
                        alive = false;
                        break;
                    case 7:
                        Console.Clear();
                        alive = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Shows admin menu.
        /// </summary>
        static void ShowAdminMenu(Library lib, LibrarianAccount account)
        {
            Console.WriteLine("Welcome to " + lib.Name);
            Console.WriteLine("You are logged in as librarian");
            Console.WriteLine("Your ID: " + account.Id);
            
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("List of options:\n" +
                                  "1. Add book to library\n" +
                                  "2. Remove book from library\n" +
                                  "3. View all books\n" +
                                  "4. Find book\n" +
                                  "5. View my books\n" +
                                  "6. Take book\n" +
                                  "7. Return book\n" +
                                  "8. Delete account\n" +
                                  "9. Log out\n");
                                  Console.WriteLine("Choose option: ");
                int option = Convert.ToInt32(Console.ReadLine());
                int id;
                switch (option)
                {
                    case 1:
                        Console.Write("Enter name of book: ");
                        string name = Console.ReadLine();
                        
                        Console.Write("Enter name of author: ");
                        string author = Console.ReadLine();
                        
                        Console.Write("Enter themes: ");
                        string theme = Console.ReadLine();
                        
                        Console.Write("Enter quantity: ");
                        int quantity = Convert.ToInt32(Console.ReadLine());
                        
                        account.AddBook(lib, name, author, theme, quantity);
                        break;
                    case 2:
                        Console.Write("Enter book ID: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        account.RemoveBook(lib, id);
                        break;
                    case 3:
                        ShowAllBooks(lib);
                        break;
                    case 4:
                        FindBookMenu(lib);
                        break;
                    case 5:
                        if (account.MyBooks.Count == 0)
                            Console.WriteLine("You have no books");
                        else
                        {
                            Console.WriteLine("My books: ");
                            for (int i = 0; i < account.MyBooks.Count; i++)
                            {
                                Console.Write(i + 1);
                                Console.WriteLine(".\tName: " + account.MyBooks[i].Name + 
                                                  "\n\tAuthor: " + account.MyBooks[i].Author +
                                                  "\n\tTheme: " + account.MyBooks[i].Theme + 
                                                  "\n\tID: " + account.MyBooks[i].Id);
                            }
                        }
                        break;
                    case 6:
                        Console.Write("Enter book ID: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        account.TakeBook(lib, id);
                        break;
                    case 7:
                        Console.Write("Enter book ID: ");
                        id = Convert.ToInt32(Console.ReadLine());
                        account.ReturnBook(lib, id);
                        break;
                    case 8:
                        Console.Write("Type \"DELETE MY ACCOUNT\" to proceed: ");
                        string confirmation = Console.ReadLine();
                        if (confirmation == "DELETE MY ACCOUNT")
                            lib.RemoveAccount(account.Id);
                        alive = false;
                        Console.Clear();
                        break;
                    case 9:
                        Console.Clear();
                        alive = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Shows a list of all books in library.
        /// </summary>
        private static void ShowAllBooks(Library lib)
        {
            if (lib.Books.Count == 0 || lib.Books == null)
                Console.WriteLine("There is no books in the library");
            else
            {
                Console.WriteLine("List of books in " + lib.Name);
                for (int i = 0; i < lib.Books.Count; i++)
                {
                    Book book = lib.Books[i];
                    Console.WriteLine((i + 1) + ".\tName: " + book.Name + 
                                      "\n\tAuthor: " + book.Author +
                                      "\n\tTheme: " + book.Theme + 
                                      "\n\tID: " + book.Id + 
                                      "\n\tAvailable: " + book.Available + " books\n");
                }
            }
        }

        /// <summary>
        /// Finds a list of books, compatible with your search request.
        /// </summary>
        private static void FindBookMenu(Library lib)
        {   
            Console.WriteLine("Choose type of search:\n" +
                              "1. By name\n" +
                              "2. By author\n" +
                              "3. By theme");
            int t = Convert.ToInt32(Console.ReadLine());
            Console.Write("Type search request: ");
            string param = Console.ReadLine();
                    
            List<Book> suitableBooks = null;
            switch (t)
            {
                case 1:
                    suitableBooks = lib.FindBook(SearchType.ByName, param);
                    break;
                case 2:
                    suitableBooks = lib.FindBook(SearchType.ByAuthor, param);
                    break;
                case 3:
                    suitableBooks = lib.FindBook(SearchType.ByTheme, param);
                    break;
            }

            if (suitableBooks == null || suitableBooks.Count == 0)
                Console.WriteLine("There is no suitable books");
            else
            {
                Console.WriteLine("List of suitable books:");
                for (int i = 0; i < suitableBooks.Count; i++)
                {
                    Book book = suitableBooks[i];
                    Console.WriteLine((i + 1) + ".\tName: " + book.Name + 
                                      "\n\tAuthor: " + book.Author +
                                      "\n\tTheme: " + book.Theme + 
                                      "\n\tID: " + book.Id + 
                                      "\n\tAvailable: " + book.Available + " books\n");
                }
            }
        }

        /// <summary>
        /// Handler of account events (created, removed, book has been taken etc.).
        /// </summary>
        private static void AccountEventHandler(object sender, AccountEventArgs args)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(args.Message);
            Console.ForegroundColor = color;
        }
        
        /// <summary>
        /// Handler of library events (added book, removed book).
        /// </summary>
        private static void StorageEventHandler(object sender, StorageEventArgs args)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(args.Message);
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Handler of exceptions. 
        /// </summary>
        private static void ExceptionHandler(Exception ex)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ForegroundColor = color;
        }
        
        /// <summary>
        /// Handler of exceptions. 
        /// </summary>
        private static void ExceptionHandler(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}