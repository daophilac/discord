using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    public enum Gender {
        Male, Female
    }
    public class User {
        public int ID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public string Image { get; set; }
    }
    class Program {
        static HttpClient client = new HttpClient();
        static void ShowUser(User user) {
            Console.WriteLine($"Email: {user.Email}\tUserName: " + $"{user.UserName}\tFirstName: {user.FirstName}");
        }
        static async Task<User> GetProductAsync(string path) {
            User user = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }
        static async Task Main(string[] args) {
            client.BaseAddress = new Uri("http://localhost:64195");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            User u = await GetProductAsync("http://localhost:33357//api//user//1");
            Console.WriteLine(u.Email);
        }
    }
}
