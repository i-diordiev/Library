using System.Xml;

namespace LibraryBack
{
    public class Book
    {
        public int ID { get; private set; }
        
        public string Name { get; private set; }
        
        public string Author { get; private set; }
        
        public string Theme { get; private set; }
        
        public int Quantity { get; protected set; }
        
        public int Available { get; protected internal set; }

        public Book(int bookId, string name, string author, string theme, int quantity)
        {
            ID = bookId;
            Name = name;
            Author = author;
            Theme = theme;
            Quantity = quantity;
            Available = quantity;
        }
    }
}