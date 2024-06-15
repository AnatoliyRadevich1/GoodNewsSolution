namespace GoodNewsTask.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Source { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PositiveLevel { get; set; }
    }
}
