using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace CategoryClassifier {
    class TermDistributor {
        private string matchingDictionaryPath;
        internal string MatchingDictionaryPath {
            get => matchingDictionaryPath;
            private set {
                if (!File.Exists(value)) {
                    throw new FileNotFoundException(matchingDictionaryPath);
                }
                matchingDictionaryPath = value ?? throw new ArgumentNullException("matchingDictionaryPath");
                string[] matchingTerms = File.ReadAllLines(MatchingDictionaryPath);
                foreach (var item in matchingTerms) {
                    MatchingTerms.Add(item);
                }
                TermMatcher = new TermMatcher(MatchingTerms);
            }
        }
        private string stoppingDictionaryPath;
        internal string StoppingDictionaryPath {
            get => stoppingDictionaryPath;
            private set {
                if (value != null && !File.Exists(value)) {
                    throw new FileNotFoundException(matchingDictionaryPath);
                }
                stoppingDictionaryPath = value;
                if (StoppingDictionaryPath != null) {
                    string[] stoppingTerms = File.ReadAllLines(StoppingDictionaryPath);
                    foreach (var item in stoppingTerms) {
                        StoppingTerms.Add(item);
                        if (MatchingTerms.Contains(item)) {
                            MatchingTerms.Remove(item);
                        }
                    }
                }
            }
        }
        private string outputFilePath;
        internal string OutputFilePath {
            get => outputFilePath;
            set {
                if (value == null) {
                    throw new ArgumentNullException("OutputFilePath");
                }
                if (!File.Exists(value)) {
                    try {
                        File.Create(value).Dispose();
                    }
                    catch {
                        throw;
                    }
                }
                outputFilePath = value;
            }
        }
        private HashSet<string> MatchingTerms { get; } = new HashSet<string>();
        private HashSet<string> StoppingTerms { get; } = new HashSet<string>();
        private TermMatcher TermMatcher { get; set; }
        private Dictionary<string, Category> Categories { get; } = new Dictionary<string, Category>();
        internal IEnumerable<string> CategoryList {
            get => Categories.Keys;
        }
        private int CategoryCount { get; set; }
        private readonly List<string> generatedTerms = new List<string>();
        internal IEnumerable<string> GeneratedTerms { get => generatedTerms; }
        internal Dictionary<string, float[]> KVTermWeight { get; private set; } = new Dictionary<string, float[]>();

        private int TotalSentences { get; set; }
        private int CurrentProcessedSentence { get; set; }
        private float CurrentPercent { get; set; }
        private long updateInterval = 1000;
        internal long UpdateInterval {
            get => updateInterval;
            set {
                if (value <= 0) {
                    throw new ArgumentOutOfRangeException("UpdateInterval");
                }
                updateInterval = value;
            }
        }
        private Timer UpdaterTimer { get; } = new Timer();
        internal event EventHandler<UpdateProgressEventArgs> UpdateProgress;
        internal TermDistributor(string matchingDictionaryPath, string stoppingDictionaryPath = null) {
            MatchingDictionaryPath = matchingDictionaryPath;
            StoppingDictionaryPath = stoppingDictionaryPath;
        }
        internal Category AddCategory(string name) {
            if (Categories.ContainsKey(name)) {
                return Categories[name];
            }
            Category category = new Category(name);
            Categories.Add(name, category);
            return category;
        }
        internal async Task ExportFileAsync() {
            if (OutputFilePath == null) {
                throw new ArgumentNullException("OutputFilePath");
            }
            StreamWriter streamWriter = new StreamWriter(OutputFilePath);
            foreach (var item in CategoryList) {
                await streamWriter.WriteAsync($"\t{item}");
            }
            await streamWriter.WriteAsync(Environment.NewLine);
            foreach (var entry in KVTermWeight) {
                await streamWriter.WriteAsync(entry.Key);
                foreach (var rate in entry.Value) {
                    await streamWriter.WriteAsync($"\t{rate}");
                }
                await streamWriter.WriteAsync(Environment.NewLine);
            }
            streamWriter.Close();
        }
        private void CalculateTotalSentences() {
            TotalSentences = 0;
            foreach (var entry in Categories) {
                TotalSentences += entry.Value.StandardSentences.Count();
            }
        }
        internal Dictionary<string, float[]> ComputeTermsRate() {
            if (!CategoryList.Any()) {
                return null;
            }
            CategoryCount = CategoryList.Count();
            Dictionary<string, Dictionary<string, int>> kvCategoryTermQuantity = ComputeTermQuantity();
            HashSet<string> tempSet = new HashSet<string>();
            foreach (var category in CategoryList) {
                Dictionary<string, int> kvTermQuantity = kvCategoryTermQuantity[category];
                foreach (var entry in kvTermQuantity) {
                    tempSet.Add(entry.Key);
                }
            }
            generatedTerms.AddRange(tempSet);
            generatedTerms.Sort();
            KVTermWeight = new Dictionary<string, float[]>();
            int[] termQuantity = new int[CategoryCount];
            float max;
            foreach (var term in generatedTerms) {
                for (int i = 0; i < CategoryCount; i++) {
                    if (kvCategoryTermQuantity[CategoryList.ElementAt(i)].ContainsKey(term)) {
                        termQuantity[i] = kvCategoryTermQuantity[CategoryList.ElementAt(i)][term];
                    }
                    else {
                        termQuantity[i] = 0;
                    }
                }
                max = termQuantity.Max();
                float[] termRate = new float[CategoryCount];
                for (int i = 0; i < CategoryCount; i++) {
                    termRate[i] = termQuantity[i] / max;
                }
                KVTermWeight.Add(term, termRate);
            }
            UpdateProgress?.Invoke(this, new UpdateProgressEventArgs(100));
            return KVTermWeight;
        }
        private void StartTimer() {
            UpdaterTimer.Interval = UpdateInterval;
            UpdaterTimer.Elapsed += UpdaterTimer_Elapsed;
            UpdaterTimer.Start();
        }

        private void StopTimer() {
            UpdaterTimer.Elapsed -= UpdaterTimer_Elapsed;
            UpdaterTimer.Stop();
        }

        private void UpdaterTimer_Elapsed(object sender, ElapsedEventArgs e) {
            UpdateProgress?.Invoke(this, new UpdateProgressEventArgs(CurrentProcessedSentence * 100f / TotalSentences));
        }
        private Dictionary<string, Dictionary<string, int>> ComputeTermQuantity() {
            CalculateTotalSentences();
            CurrentProcessedSentence = 0;
            StartTimer();
            Dictionary<string, Dictionary<string, int>> kvCategoryTermQuantity = new Dictionary<string, Dictionary<string, int>>();
            foreach (var c in CategoryList) {
                Category category = Categories[c];
                kvCategoryTermQuantity.Add(c, TermMatcher.FoundTerms(category.StandardSentences));
                CurrentProcessedSentence += category.StandardSentences.Count();

                //Dictionary<string, int> foundTerms = new Dictionary<string, int>();
                //foreach (var sentence in category.StandardSentences) {
                //    CurrentProcessedSentence++;
                //    foundTerms.Add
                //    string[] words;
                //    string strRemain = "";
                //    int stop, start = 0;
                //    bool isTerm, isStop = false;
                //    words = sentence.Split(' ');
                //    stop = words.Length - 1;
                //    while (!isStop && stop >= 0) {
                //        for (int i = start; i <= stop; i++) {
                //            strRemain += words[i] + " ";
                //        }
                //        strRemain = strRemain.Trim().ToLower();
                //        isTerm = false;
                //        foreach (var t in MatchingTerms) {
                //            if (t.Equals(strRemain)) {
                //                if (!foundTerms.ContainsKey(t)) {
                //                    foundTerms.Add(strRemain, 0);
                //                }
                //                foundTerms[strRemain]++;
                //                isTerm = true;
                //                if (start == 0) {
                //                    isStop = true;
                //                }
                //                else {
                //                    stop = start - 1;
                //                    start = 0;
                //                }
                //                break;
                //            }
                //        }
                //        if (!isTerm) {
                //            if (start == stop) {
                //                stop--;
                //                start = 0;
                //            }
                //            else
                //                start += 1;
                //        }
                //        strRemain = "";
                //    }
                //}
                //kvCategoryTermQuantity.Add(c, foundTerms);
            }
            StopTimer();
            return kvCategoryTermQuantity;
        }
    }
    internal class Category {
        private string name;
        internal string Name {
            get => name;
            private set => name = value ?? throw new ArgumentNullException("name");
        }
        private readonly ICollection<string> inputParagraphs = new List<string>();
        internal IEnumerable<string> InputParagraphs { get => inputParagraphs; }
        private readonly List<string> standardSentences = new List<string>();
        internal IEnumerable<string> StandardSentences { get => standardSentences; }
        internal Category(string name) {
            Name = name;
        }
        internal Category AddParagraph(string paragraph) {
            if (paragraph == null) {
                throw new ArgumentNullException("paragraph");
            }
            inputParagraphs.Add(paragraph);
            standardSentences.AddRange(paragraph.ToStandardSentences());
            return this;
        }
        internal Category AddParagraphs(string[] paragraphs) {
            foreach (var item in paragraphs) {
                AddParagraph(item);
            }
            return this;
        }
        internal async Task<Category> AddParagraphsFromFileAsync(string filePath) {
            if (filePath == null) {
                throw new ArgumentNullException("filePath");
            }
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException(filePath);
            }
            string[] paragraphs = await File.ReadAllLinesAsync(filePath);
            return AddParagraphs(paragraphs);
        }
        internal async Task<Category> AddParagraphsFromFilesAsync(string[] filePaths) {
            foreach (var item in filePaths) {
                await AddParagraphsFromFileAsync(item);
            }
            return this;
        }
        internal void Clear() {
            inputParagraphs.Clear();
            standardSentences.Clear();
        }
    }
    internal class UpdateProgressEventArgs : EventArgs {
        internal float CurrentPercent { get; }
        internal UpdateProgressEventArgs(float currentPercent) {
            CurrentPercent = currentPercent;
        }
    }
}
