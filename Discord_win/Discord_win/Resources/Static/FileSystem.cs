using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Resources.Static {
    static class FileSystem {
        public static readonly string RootDataDirectory = "D:\\Desktop\\data\\";
        public static readonly string UserDirectory = "user\\";
        public static readonly string ServerDirectory = "server\\";
        public static readonly string UserDataFile = "user.txt";
        public static readonly string ServerDataFile = "server.txt";
        public static void writeUserData(string email, string password) {
            FileDealer fileDealer = new FileDealer();
            fileDealer.WriteLine(Program.UserFilePath, email, true);
            fileDealer.WriteLine(Program.UserFilePath, password, true);
            fileDealer.Close();
        }
        public static void clearData() {
            Directory.Delete(RootDataDirectory, true);
        }
    }
}
