using ConsumingLibraryMgmtSystem.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7238/api/")
            };
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<dynamic>(result)?.token;

                // Save the token in cookies or session
                HttpContext.Session.GetString("JWToken");
                return RedirectToAction("Index", "Home"); // Redirect to dashboard/home
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Auth/signup", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }

            ViewBag.ErrorMessage = "Registration failed. Try again.";
            return View(model);
        }
    }
}