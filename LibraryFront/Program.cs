using System;
using System.Collections.Generic;
using System.IO;
using LibraryBack;

namespace LibraryFront
{
    class Program
    {
        static void Main()
        {
            Library Bibla = new Library("My Bibla", StorageEventHandler);
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
                                lib.AddAccount(AccountType.User, AccountEventHandler);
                            else if (t == 2)
                                lib.AddAccount(AccountType.Librarian, AccountEventHandler);
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
                                UserAccount acc = lib.Login(AccountType.User, id) as UserAccount;
                                Console.Clear();
                                ShowUserMenu(lib, acc);
                            }
                            else if (t == 2)
                            {
                                LibrarianAccount acc = lib.Login(AccountType.User, id) as LibrarianAccount;
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

        static void ShowUserMenu(Library lib, UserAccount account)
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
                                  "1. View my books\n" +
                                  "2. Find book\n" +
                                  "3. Take book\n" +
                                  "4. Return book\n" +
                                  "5. Delete account\n" +
                                  "6. Log out\n");
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

        static void ShowAdminMenu(Library lib, LibrarianAccount account)
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
                                  "3. Import books from CSV\n" +
                                  "4. Export books to CSV\n" +
                                  "5. Find book\n" +
                                  "6. View my books\n" +
                                  "7. Take book\n" +
                                  "8. Return book\n" +
                                  "9. Delete account\n" +
                                  "10. Log out\n");
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
                        try
                        {
                            Console.Write("Enter book ID: ");
                            id = Convert.ToInt32(Console.ReadLine());
                            account.RemoveBook(lib, id);

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
                        Console.WriteLine("Type path to csv file: ");
                        string path = Console.ReadLine();
                        using (var reader = new StreamReader(@path))
                        {
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(",");
                                lib.AddBook(values[0], values[1], values[2], Convert.ToInt32(values[3]));
                            }
                        }
                        Console.WriteLine("Import successful!");
                        break;
                    case 4:
                        Console.WriteLine("Type path to csv file: ");
                        string path1 = Console.ReadLine();
                        using (StreamWriter writer = new StreamWriter(@path1, false))
                        {
                            List<Book> books = lib.FindBook(SearchType.ByAuthor, "");
                            foreach (Book book in books)
                            {
                                string text = book.Name + "," + book.Author + "," + book.Theme + "," +
                                              Convert.ToString(book.Quantity);
                                writer.WriteLine(text);
                            }
                        }
                        Console.WriteLine("Export successful!");
                        break;
                    case 5:
                        FindBookMenu(lib);
                        break;
                    case 6:
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
                    case 7:
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
                    case 8:
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
                    case 9:
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
                    case 10:
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
                    Book book = suitableBooks[i];
                    Console.WriteLine((i + 1) + ".\tName: " + book.Name + "\n\tAuthor: " + book.Author +
                                      "\n\tTheme: " + book.Theme + "\n\tID: " + book.ID + 
                                      "\n\tAvailable: " + book.Available + " books\n");
                }
            }
        }

        private static void AccountEventHandler(object sender, AccountEventArgs args)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(args.Message);
            Console.ForegroundColor = color;
        }
        
        private static void StorageEventHandler(object sender, StorageEventArgs args)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(args.Message);
            Console.ForegroundColor = color;
        }
    }
}