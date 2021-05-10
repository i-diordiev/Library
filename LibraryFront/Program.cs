using System;
using System.Collections.Generic;
using LibraryBack;

namespace LibraryFront
{
    class Program
    {
        static void Main()
        {
            Library Bibla = new Library("My Bibla", AddedBookHandler, RemovedBookHandler);
            ShowStartMenu(Bibla);
        }

        static void ShowStartMenu(Library lib)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("Welcome to " + lib.Name);
                Console.WriteLine("List of options:\n" +
                                  "1. Register new account\n" +
                                  "2. Login\n" +
                                  "3. Find book\n" +
                                  "4. Quit program");
                Console.WriteLine("Choose option: ");
                int option = Convert.ToInt32(Console.ReadLine());
                int t;
                switch (option)
                {
                    case 1:
                        try
                        {
                            Console.WriteLine("Choose type of account:\n" +
                                              "1. User\n" +
                                              "2. Admin");
                            t = Convert.ToInt32(Console.ReadLine());
                            if (t == 1)
                                lib.AddAccount(AccountType.User, CreatedHandler, DeletedHandler,
                                    TakenBookHandler, ReturnedBookHandler, 
                                    LoggedInHandler, LoggedOutHandler);
                            else if (t == 2)
                                lib.AddAccount(AccountType.Librarian, CreatedHandler, DeletedHandler,
                                    TakenBookHandler, ReturnedBookHandler, 
                                    LoggedInHandler, LoggedOutHandler);
                            else
                                Console.WriteLine("Please, choose 1 or 2");
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 2:
                        try
                        {
                            Console.WriteLine("Choose type of account:\n" +
                                              "1. User\n" +
                                              "2. Admin");
                            t = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Ented ID of your account\n");
                            int id = Convert.ToInt32(Console.ReadLine());
                            if (t == 1)
                            {
                                Account acc = lib.Login(AccountType.User, id);
                                Console.Clear();
                                ShowUserMenu(lib, acc);
                            }
                            else if (t == 2)
                            {
                                Account acc = lib.Login(AccountType.User, id);
                                Console.Clear();
                                ShowAdminMenu(lib, acc);
                            }
                            else
                                Console.WriteLine("Please, choose 1 or 2");
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 3:
                        FindBookMenu(lib);
                        break;
                    case 4:
                        alive = false;
                        break;
                }
            }
        }

        static void ShowUserMenu(Library lib, Account account)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Welcome to " + lib.Name);
            Console.WriteLine("You are logged in as user");
            Console.WriteLine("Your ID - " + account.Id);
            
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("List of options:\n" +
                                  "1. Find book\n" +
                                  "2. Take book\n" +
                                  "3. Return book\n" +
                                  "4. Delete account" +
                                  "5. Log out\n");
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
                                Console.WriteLine(".\tName: " + account.MyBooks[i].Name + "\n\tAuthor: " + account.MyBooks[i].Author +
                                                  "\n\tTheme: " + account.MyBooks[i].Theme + "\n\tID: " + account.MyBooks[i].ID);
                            }
                        }
                        break;
                    case 2:
                        FindBookMenu(lib);
                        break;
                    case 3:
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            account.TakeBook(lib, id);
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 4:
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            account.ReturnBook(lib, id);
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 5:
                        try
                        {
                            Console.Write("Type \"DELETE MY ACCOUNT\" to proceed: ");
                            string confirmation = Console.ReadLine();
                            if (confirmation == "DELETE MY ACCOUNT")
                                lib.RemoveAccount(account.Id);
                            Console.Clear();
                            alive = false;
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 6:
                        Console.Clear();
                        alive = false;
                        break;
                }
            }
        }

        static void ShowAdminMenu(Library lib, Account account)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Welcome to " + lib.Name);
            Console.WriteLine("You are logged in as librarian");
            Console.WriteLine("Your ID - " + account.Id);
            
            bool alive = true;
            while (alive)
            {
                Console.WriteLine("List of options:\n" +
                                  "1. Add book to library\n" +
                                  "2. Remove book from library\n" +
                                  "3. Find book\n" +
                                  "4. View my books\n" +
                                  "5. Take book\n" +
                                  "6. Return book\n" +
                                  "7. Delete account\n" +
                                  "8. Log out\n");
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
                        lib.AddBook(name, author, theme, quantity);
                        break;
                    case 2:
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            lib.RemoveBook(id);

                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        
                        break;
                    case 3:
                        FindBookMenu(lib);
                        break;
                    case 4:
                        if (account.MyBooks.Count == 0)
                            Console.WriteLine("You have no books");
                        else
                        {
                            Console.WriteLine("My books: ");
                            for (int i = 0; i < account.MyBooks.Count; i++)
                            {
                                Console.Write(i + 1);
                                Console.WriteLine(".\tName: " + account.MyBooks[i].Name + "\n\tAuthor: " + account.MyBooks[i].Author +
                                                  "\n\tTheme: " + account.MyBooks[i].Theme + "\n\tID: " + account.MyBooks[i].ID);
                            }
                        }
                        break;
                    case 5:
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            account.TakeBook(lib, id);
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 6:
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            account.ReturnBook(lib, id);
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 7:
                        try
                        {
                            Console.Write("Type \"DELETE MY ACCOUNT\" to proceed: ");
                            string confirmation = Console.ReadLine();
                            if (confirmation == "DELETE MY ACCOUNT")
                                lib.RemoveAccount(account.Id);
                            alive = false;
                            Console.Clear();
                        }
                        catch (Exception ex)
                        {
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = color;
                        }
                        break;
                    case 8:
                        Console.Clear();
                        alive = false;
                        break;
                }
            }
        }

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
            {
                Console.WriteLine("There is no suitable books");
            }
            else
            {
                Console.WriteLine("List of suitable books:");
                for (int i = 0; i < suitableBooks.Count; i++)
                {
                    Console.Write(i + 1);
                    Console.WriteLine(".\tName: " + suitableBooks[i].Name + "\n\tAuthor: " + suitableBooks[i].Author +
                                      "\n\tTheme: " + suitableBooks[i].Theme + "\n\tID: " + suitableBooks[i].ID + 
                                      "\n\tAvailable: " + suitableBooks[i].Available + " books\n");
                }
            }
        }
        
        private static void CreatedHandler(object sender, AccountEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void DeletedHandler(object sender, AccountEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void TakenBookHandler(object sender, AccountEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void ReturnedBookHandler(object sender, AccountEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void LoggedInHandler(object sender, AccountEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void LoggedOutHandler(object sender, AccountEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void AddedBookHandler(object sender, StorageEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
        
        private static void RemovedBookHandler(object sender, StorageEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
    }
}