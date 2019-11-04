using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryManagement {
    class Manager {
        private static Dictionary<char, int> CharsMap { get; } = new Dictionary<char, int>() {
            {'a', 1},
            {'á', 2},
            {'à', 3},
            {'ả', 4},
            {'ã', 5},
            {'ạ', 6},

            {'ă', 7},
            {'ắ', 8},
            {'ằ', 9},
            {'ẳ', 10},
            {'ẵ', 11},
            {'ặ', 12},

            {'â', 13},
            {'ấ', 14},
            {'ầ', 15},
            {'ẩ', 16},
            {'ẫ', 17},
            {'ậ', 18},

            {'b', 19},
            {'c', 20},
            {'d', 21},
            {'đ', 22},

            {'e', 23},
            {'é', 24},
            {'è', 25},
            {'ẻ', 26},
            {'ẽ', 27},
            {'ẹ', 28},

            {'f', 29},
            {'g', 30},
            {'h', 31},

            {'i', 32},
            {'í', 33},
            {'ì', 34},
            {'ỉ', 35},
            {'ĩ', 36},
            {'ị', 37},

            {'j', 38},
            {'k', 39},
            {'l', 40},
            {'m', 41},
            {'n', 42},

            {'o', 43},
            {'ó', 44},
            {'ò', 45},
            {'ỏ', 46},
            {'õ', 47},
            {'ọ', 48},

            {'ô', 49},
            {'ố', 50},
            {'ồ', 51},
            {'ổ', 52},
            {'ỗ', 53},
            {'ộ', 54},

            {'ơ', 55},
            {'ớ', 56},
            {'ờ', 57},
            {'ở', 58},
            {'ỡ', 59},
            {'ợ', 60},

            {'p', 61},
            {'q', 62},
            {'r', 63},
            {'s', 64},
            {'t', 65},

            {'u', 66},
            {'ú', 67},
            {'ù', 68},
            {'ủ', 69},
            {'ũ', 70},
            {'ụ', 71},

            {'ư', 72},
            {'ứ', 73},
            {'ừ', 74},
            {'ử', 75},
            {'ữ', 76},
            {'ự', 77},

            {'v', 78},
            {'w', 79},
            {'x', 80},

            {'y', 81},
            {'ý', 82},
            {'ỳ', 83},
            {'ỷ', 84},
            {'ỹ', 85},
            {'ỵ', 86},

            {'z', 87},
        };
        public List<string> Words { get; } = new List<string>();
        public void Load() {
            Words.AddRange(File.ReadAllLines("dictionary.txt"));

        }

        private void Sort() {

        }
        private void Insert() {

        }
    }
}
