using ConsumingLibraryMgmtSystem.Models;

namespace ConsumingLibraryMgmtSystem.Services.Book
{
    public interface IBooksServices
    {
        Task<IEnumerable<Books>> GetAllBooksAsync(string token);
        Task<Books> GetBookByIdAsync(int id, string token); // New method
        Task<bool> AddBookAsync(Books book, string token);
        Task<string> UpdateBookAsync(Books book, string token);
        Task<string> DeleteBookAsync(int id, string token);
    }
}
