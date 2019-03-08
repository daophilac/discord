using System;
using System.Net.Http;

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
        static async void GetMessage() {
            HttpResponseMessage message = new HttpResponseMessage();
            message = await client.GetAsync("http://localhost:33357/api/user/1");
            if (message.IsSuccessStatusCode) {
                User u = await message.Content.ReadAsAsync<User>();
                Console.WriteLine(message.Content.ToString());
            }
            
        }
        static void Main(string[] args) {
            client.BaseAddress = new Uri("http://localhost:33357");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
