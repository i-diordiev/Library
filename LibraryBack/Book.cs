namespace LibraryBack
{
    /// <summary>
    /// Class that describe book. Has ID, name, author, theme and quantity
    /// </summary>
    public class Book
    {
        public int Id { get; private set; }
        
        public string Name { get; private set; }
        
        public string Author { get; private set; }
        
        public string Theme { get; private set; }
        
        public int Quantity { get; private set; }
        
        public int Available { get; protected internal set; }

        public Book(int bookId, string name, string author, string theme, int quantity)
        {
            Id = bookId;
            Name = name;
            Author = author;
            Theme = theme;
            Quantity = quantity;
            Available = quantity;
        }
    }
}