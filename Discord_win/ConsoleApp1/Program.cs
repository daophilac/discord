using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

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
        //static HttpClient client = new HttpClient();
        static async void GetMessage() {
            HttpClient client = new HttpClient();
            HttpResponseMessage message = new HttpResponseMessage();
            message = await client.GetAsync("http://localhost:33357/api/user/1");
            if (message.IsSuccessStatusCode) {
                User u = await message.Content.ReadAsAsync<User>();
                Console.WriteLine(u.Email);
            }
            
        }
        static void Main(string[] args) {
            //client.BaseAddress = new Uri("http://localhost:33357");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("Hello World!");
            GetMessage();
            //Console.ReadLine();
            //string localIP;
            //using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
            //    socket.Connect("8.8.8.8", 65530);
            //    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            //    localIP = endPoint.Address.ToString();
            //    Console.WriteLine(localIP);
            //}
            //string test = "param0:{0}, param1:{1}";
            //string test2 = String.Format(test, 23, 45);
            //Console.WriteLine(test2);
            Console.ReadLine();
        }
    }
}
