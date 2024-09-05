namespace GoodNewsTask.Models
{
    public class Article
    {
        public Guid Id { get; set; } //уникальный идентификатор
        public string? Title { get; set; } //заголовок статьи
        public string? Text { get; set; } //текст статьи
        public string? Source { get; set; } //картинка статьи
        public DateTime PublicationDate { get; set; } //дата публикации
        public int PositiveLevel { get; set; } //уровень позитива статьи
    }
}
