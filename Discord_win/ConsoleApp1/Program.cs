using System;
using System.Windows;

namespace BuyOrNot{
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
        static void BuyOrNot() {
            Random random = new Random();
            int result = random.Next(1);
            if (result == 0) {
                MessageBox.Show("No.");
            }
            else {
                MessageBox.Show("Yesssssssssssssssss.");
            }
        }
        static void Main(string[] args) {
            BuyOrNot();
        }
    }
}
