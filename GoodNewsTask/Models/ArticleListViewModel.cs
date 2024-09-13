namespace GoodNewsTask.Models
{
    public class ArticleListViewModel //модель для пагинации
    {
        public IEnumerable<Article>? Articles { get; set; } //список всех собранных новостных статей
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
