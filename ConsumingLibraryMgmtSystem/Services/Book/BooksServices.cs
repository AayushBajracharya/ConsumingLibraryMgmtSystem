using ConsumingLibraryMgmtSystem.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumingLibraryMgmtSystem.Services.Book
{
    public class BooksServices : IBooksServices
    {
        private readonly HttpClient _httpClient;

        public BooksServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all books
        public async Task<IEnumerable<Books>> GetAllBooksAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7238/api/Books");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Books>>(content);
        }

        // Get a book by ID
        public async Task<Books> GetBookByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"https://localhost:7238/api/Books/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Books>(content);
            }
            return null; // Return null if the book isn't found or an error occurred
        }

        // Add a book
        public async Task<bool> AddBookAsync(Books book, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7238/api/Books", jsonContent);
            return response.IsSuccessStatusCode;
        }

        // Update a book
        public async Task<string> UpdateBookAsync(Books book, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7238/api/Books/{book.BookId}", jsonContent);

            return response.IsSuccessStatusCode
                ? "Book updated successfully!"
                : $"Error updating book: {await response.Content.ReadAsStringAsync()}";
        }

        // Delete a book
        public async Task<string> DeleteBookAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7238/api/Books/{id}");
            return response.IsSuccessStatusCode
                ? "Book deleted successfully!"
                : $"Error deleting book: {await response.Content.ReadAsStringAsync()}";
        }
    }
}
