namespace SklepKsiegarniaMvcUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
        Task<Book> GetBookById(int id);
        IEnumerable<Book> SearchBooks(string searchTerm);
    }
}