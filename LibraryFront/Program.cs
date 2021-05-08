using System;
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
                            int pos = lib.Login(AccountType.User, id);
                            Console.Clear();
                            ShowUserMenu(lib, pos);
                        }
                        else if (t == 2)
                        {
                            int pos = lib.Login(AccountType.Librarian, id);
                            Console.Clear();
                            ShowAdminMenu(lib, pos);
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

        static void ShowUserMenu(Library lib, int q)
        {
            bool alive = true;
            while (alive)
            {
                Account current = lib.Users[q];
                
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Welcome to " + lib.Name);
                Console.WriteLine("You are logged in as user");
                Console.WriteLine("Your ID - " + current.Id);
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
                        FindBookMenu(lib);
                        break;
                    case 2:
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            current.TakeBook(lib, id);
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
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            current.ReturnBook(lib, id);
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
                            Console.Write("Type \"DELETE MY ACCOUNT\" to proceed: ");
                            string confirmation = Console.ReadLine();
                            if (confirmation == "DELETE MY ACCOUNT")
                                lib.RemoveAccount(current.Id);
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
                    case 5:
                        Console.Clear();
                        alive = false;
                        break;
                }
            }
        }

        static void ShowAdminMenu(Library lib, int q)
        {
            bool alive = true;
            while (alive)
            {
                Account current = lib.Users[q];
                
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Welcome to " + lib.Name);
                Console.WriteLine("You are logged in as librarian");
                Console.WriteLine("Your ID - " + current.Id);
                Console.WriteLine("List of options:\n" +
                                  "1. Add book to library\n" +
                                  "2. Remove book from library\n" +
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
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            current.TakeBook(lib, id);
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
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            current.ReturnBook(lib, id);
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
                            Console.Write("Type \"DELETE MY ACCOUNT\" to proceed: ");
                            string confirmation = Console.ReadLine();
                            if (confirmation == "DELETE MY ACCOUNT")
                                lib.RemoveAccount(current.Id);
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
                    case 7:
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
                    
            Book[] Suitable = null;
            switch (t)
            {
                case 1:
                    Suitable = lib.FindBook(SearchType.ByName, param);
                    break;
                case 2:
                    Suitable = lib.FindBook(SearchType.ByAuthor, param);
                    break;
                case 3:
                    Suitable = lib.FindBook(SearchType.ByTheme, param);
                    break;
            }

            if (Suitable == null)
            {
                Console.WriteLine("There is no suitable books");
            }
            else
            {
                Console.WriteLine("List of suitable books:");
                for (int i = 0; i < Suitable.Length; i++)
                {
                    Console.Write(i + 1);
                    Console.WriteLine(".\tName: " + Suitable[i].Name + "\n\tAuthor: " + Suitable[i].Author +
                                      "\n\tTheme: " + Suitable[i].Theme + "\n\tID: " + Suitable[i].ID + 
                                      "\n\tAvailable: " + Suitable[i].Available + " books\n");
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