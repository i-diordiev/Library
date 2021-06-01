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
            Library library = new Library("KPI Library", StorageEventHandler);
            ShowStartMenu(library);
        }

        /// <summary>
        /// Shows menu of unregistered user (start menu).
        /// </summary>
        static void ShowStartMenu(Library lib)
        {
            Console.WriteLine("Welcome to " + lib.Name);
            
            bool alive = true;
            while (alive)
            {
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
                    switch (option)
                    {
                        case 1:
                            lib.AddAccount(AccountEventHandler);
                            break;
                        case 2:
                            Console.WriteLine("Choose type of account:\n" +
                                              "1. User\n" +
                                              "2. Admin");
                            int t = Convert.ToInt32(Console.ReadLine());
                            if (t == 1)
                            {
                                Console.Write("Enter ID of your account: ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                UserAccount acc = lib.Login(id);
                                Console.WriteLine("\n\n\n\n\n\n\n");
                                ShowUserMenu(lib, acc);
                            }
                            else if (t == 2)
                            {
                                Console.Write("Enter password: ");
                                string pass = Console.ReadLine();
                                LibrarianAccount acc = lib.LoginAsAdmin(pass);
                                Console.WriteLine("\n\n\n\n\n\n\n");
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
                catch (WrongPasswordException)
                {
                    ExceptionHandler("You've entered wrong password, try again!");
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
                Console.Write("Choose option: ");
                try
                {
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
                            alive = false;
                            Console.WriteLine("\n\n\n\n\n\n\n");
                            break;
                        case 7:
                            account.LogOut();
                            alive = false;
                            Console.WriteLine("\n\n\n\n\n\n\n");
                            break;
                    }
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
                catch (WrongIdException)
                {
                    ExceptionHandler("You've entered wrong ID, try again!");
                }
                catch (FormatException)
                {
                    ExceptionHandler("Wrong input format! Try again.");
                }
                catch (Exception ex)
                {
                    ExceptionHandler(ex);
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
                                  "5. Import books from .csv\n" +
                                  "6. Log out\n");
                Console.Write("Choose option: ");
                try
                {
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
                                    lib.AddBook(values[0], values[1], values[2], Convert.ToInt32(values[3]));
                                }
                            }

                            Console.WriteLine("Import successful!");
                            break;
                        case 6:
                            account.LogOut();
                            alive = false;
                            Console.WriteLine("\n\n\n\n\n\n\n");
                            break;
                    }
                }
                catch (WrongIdException)
                {
                    ExceptionHandler("You've entered wrong ID, try again!");
                }
                catch (BooksNotReturnedException)
                {
                    ExceptionHandler("Not all users returned this book!");
                }
                catch (FormatException)
                {
                    ExceptionHandler("Wrong input format! Try again.");
                }
                catch (Exception ex)
                {
                    ExceptionHandler(ex);
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
            Console.WriteLine("\n" + args.Message + "\n");
            Console.ForegroundColor = color;
        }
        
        /// <summary>
        /// Handler of library events (added book, removed book).
        /// </summary>
        private static void StorageEventHandler(object sender, StorageEventArgs args)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + args.Message + "\n");
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Handler of exceptions. 
        /// </summary>
        private static void ExceptionHandler(Exception ex)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + ex.Message + "\n");
            Console.ForegroundColor = color;
        }
        
        /// <summary>
        /// Handler of exceptions. 
        /// </summary>
        private static void ExceptionHandler(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + message + "\n");
            Console.ForegroundColor = color;
        }
    }
}