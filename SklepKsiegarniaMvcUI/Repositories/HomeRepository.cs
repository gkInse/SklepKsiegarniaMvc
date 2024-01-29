using Microsoft.EntityFrameworkCore;

namespace SklepKsiegarniaMvcUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }

        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            var query = _db.Books
                .Include(b => b.Genre)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var searchTerms = searchTerm.Split(',').Select(term => term.Trim().ToLower()).ToArray();

                // Pobierz dane z bazy danych do pamięci
                var booksInMemory = query.ToList();

                var filteredBooks = booksInMemory
                    .Where(book =>
                        searchTerms.All(searchTerm =>
                            (book.BookName != null && book.BookName.ToLower().Contains(searchTerm)) ||
                            (book.AuthorName != null && book.AuthorName.ToLower().Contains(searchTerm)) ||
                            (book.Genre != null && book.Genre.GenreName.ToLower().Contains(searchTerm)) ||
                            (book.Price.ToString() != null && book.Price.ToString().ToLower().Contains(searchTerm)) ||
                            (book.ReleaseDate.ToString("M/d/yyyy") != null && book.ReleaseDate.ToString("M/d/yyyy").ToLower().Contains(searchTerm)) ||
                            (book.CoverType.ToString() != null && book.CoverType.ToString().ToLower().Contains(searchTerm))
                        )
                    )
                    .AsQueryable();

                // Zaktualizuj oryginalne zapytanie
                query = filteredBooks.AsQueryable();
            }

            return query.ToList();
        }

        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();

            var query = _db.Books.Include(b => b.Genre)
                            .Where(book =>
                                (string.IsNullOrEmpty(sTerm) ||
                                book.BookName.ToLower().Contains(sTerm) ||
                                book.AuthorName.ToLower().Contains(sTerm)) &&
                                (genreId == 0 || book.GenreId == genreId));

            return await query.ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _db.Books.Include(b => b.Genre)
                                   .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
