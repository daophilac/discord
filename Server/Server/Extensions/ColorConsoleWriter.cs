using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Extensions {
    class ColorConsoleWriter {
        private static ConsoleColor DefaultForegroundColor { get; } = ConsoleColor.Gray;
        private static ConsoleColor DefaultBackgroundColor { get; } = ConsoleColor.Black;
        internal ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;
        internal ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        internal bool ResetColorAfterWriting { get; set; } = false;
        internal void ResetColor() {
            ForegroundColor = DefaultForegroundColor;
            BackgroundColor = DefaultBackgroundColor;
            Console.ResetColor();
        }
        internal void Write(string value) {
            SetColor();
            Console.Write(value);
            Finish();
        }
        internal void WriteLine(string value) {
            SetColor();
            Console.WriteLine(value);
            Finish();
        }
        private void Finish() {
            if (ResetColorAfterWriting) {
                ResetColor();
            }
        }
        private void SetColor() {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
        }
    }
}
