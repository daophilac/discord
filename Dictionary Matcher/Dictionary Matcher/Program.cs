using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryMatcher {
    class Program {
        static async Task Main(string[] args) {
            TermDistributor termDistributor = new TermDistributor("D:/Desktop/dictionary.txt");
            await termDistributor.AddCategory("Sport").AddParagraphsFromFilesAsync(new string[] {
                "D:/Desktop/feed/sport1.txt",
                "D:/Desktop/feed/sport2.txt",
                "D:/Desktop/feed/sport3.txt",
                "D:/Desktop/feed/sport4.txt",
                "D:/Desktop/feed/sport5.txt"
            });
            await termDistributor.AddCategory("Technology").AddParagraphsFromFilesAsync(new string[] {
                "D:/Desktop/feed/tech1.txt",
                "D:/Desktop/feed/tech2.txt",
                "D:/Desktop/feed/tech3.txt",
                "D:/Desktop/feed/tech4.txt",
                "D:/Desktop/feed/tech5.txt"
            });
            await termDistributor.AddCategory("Law").AddParagraphsFromFilesAsync(new string[] {
                "D:/Desktop/feed/law1.txt",
                "D:/Desktop/feed/law2.txt",
                "D:/Desktop/feed/law3.txt",
                "D:/Desktop/feed/law4.txt",
                "D:/Desktop/feed/law5.txt"
            });
            await termDistributor.AddCategory("Economy").AddParagraphsFromFilesAsync(new string[] {
                "D:/Desktop/feed/economy1.txt",
                "D:/Desktop/feed/economy2.txt",
                "D:/Desktop/feed/economy3.txt",
                "D:/Desktop/feed/economy4.txt",
                "D:/Desktop/feed/economy5.txt"
            });
            await termDistributor.AddCategory("ESport").AddParagraphsFromFilesAsync(new string[] {
                "D:/Desktop/feed/esport1.txt",
                "D:/Desktop/feed/esport2.txt",
                "D:/Desktop/feed/esport3.txt",
                "D:/Desktop/feed/esport4.txt",
                "D:/Desktop/feed/esport5.txt"
            });
            termDistributor.UpdateInterval = 100;
            termDistributor.UpdateProgress += TermDistributor_UpdateProgress;
            termDistributor.ComputeTermsRate();
            termDistributor.OutputFilePath = "D:/Desktop/sum.txt";
            await termDistributor.ExportFileAsync();
        }

        private static void TermDistributor_UpdateProgress(object sender, TermDistributor.UpdateProgressEventArgs e) {
            Console.SetCursorPosition(0, 0);
            Console.Write($"{e.CurrentPercent}                   \n");
        }
    }
}
