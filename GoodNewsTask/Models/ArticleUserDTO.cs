namespace GoodNewsTask.Models
{
    public class ArticleUserDTO //Я знаю, что DTO-модели используют, чтобы скрыть часть данных обычных моделей, но я попробовал таким образом проработать DTO-модели
    {
        public List<Article> ListArticles { get; set; } = null!;
        public List<User> ListUsers { get; set; } = null!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        
    }
}
