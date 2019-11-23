using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CategoryClassifier {
    public class CategoryResult {
        public string Paragraph { get; }
        public string CategoryBelong { get; }
        public float Accuracy { get; }
        internal CategoryResult(string paragraph, string categoryBelong, float accuracy) {
            Paragraph = paragraph;
            CategoryBelong = categoryBelong;
            Accuracy = accuracy;
        }
    }
    public class CategoryClassifier {
        public List<string> Categories { get; }
        private Dictionary<string, float[]> termsWeight;
        public Dictionary<string, float[]> TermsWeight {
            get => termsWeight;
            private set {
                if (value == null) {
                    throw new ArgumentNullException("TermsWeight");
                }
                if (!value.Any()) {
                    throw new ArgumentOutOfRangeException("TermsWeight");
                }
                HashSet<string> terms = new HashSet<string>();
                foreach (var entry in value) {
                    if (terms.Contains(entry.Key)) {
                        throw new DuplicateTermException(entry.Key);
                    }
                }
                foreach (var entry in value) {
                    if (Categories.Count() != entry.Value.Length) {
                        throw new InvalidWeightArray(entry.Key, "Unmatched between category quantity and weight array length.");
                    }
                    foreach (var rate in entry.Value) {
                        if (rate < 0 || rate > 1) {
                            throw new InvalidWeightArray(entry.Key, "Invalid weight range.");
                        }
                    }
                }
                termsWeight = value.ToDictionary(entry => entry.Key, entry => entry.Value);
                TermMatcher = new TermMatcher(TermsWeight.Keys.ToHashSet());
            }
        }
        private TermMatcher TermMatcher { get; set; }
        public CategoryClassifier(IEnumerable<string> categories) {
            if(categories == null) {
                throw new ArgumentNullException("categories");
            }
            if (!categories.Any()) {
                throw new ArgumentOutOfRangeException("categories");
            }
            Categories = categories.ToList();





            ExtractTermsWeightFromFile("D:/Desktop/repos/discord/Server/Server/Data/term_dist.txt");
            //var x = DetermineCategory("cầu thủ");
            //var x = DetermineCategory("Cựu thủ quân Barca sở hữu chuỗi 56 trận bất bại khi ra sân cho đội bóng từ năm 2010 đến năm 2012. Luôn cống hiến và chiến đấu không biết mệt mỏi, Puyol sở hữu những phẩm chất của trung vệ đẳng cấp thế giới.");
            var x = DetermineCategory("Cơ quan công an tỉnh Thái Bình đã bắt giữ khẩn cấp người chồng giết vợ rồi tẩm xăng đốt xác..");
        }
        public void ExtractTermsWeightFromFile(string filePath) {
            TermsWeight = filePath.ExtractTermsWeightFromFile();
        }
        public CategoryResult DetermineCategory(string paragraph) {
            Dictionary<string, int> foundTerms = TermMatcher.FoundTerms(paragraph.ToStandardSentences());
            if (!foundTerms.Any()) {
                return new CategoryResult(paragraph, null, 0);
            }
            float[] points = new float[Categories.Count()];
            for (int i = 0; i < Categories.Count(); i++) {
                points[i] = 0;
                foreach (var entry in foundTerms) {
                    points[i] += entry.Value * TermsWeight[entry.Key][i];
                }
            }
            float max = points.Max();
            CategoryResult categoryResult = null;
            for (int i = 0; i < points.Length; i++) {
                if(points[i] == max) {
                    categoryResult = new CategoryResult(paragraph, Categories.ElementAt(i), points[i] / points.Sum());
                }
            }
            return categoryResult;
        }
        public ICollection<CategoryResult> DetermineCategories(string[] paragraphs) {
            ICollection<CategoryResult> result = new List<CategoryResult>();
            foreach (var item in paragraphs) {
                result.Add(DetermineCategory(item));
            }
            return result;
        }
    }
    public class DuplicateTermException : Exception {
        public string DuplicateTerm { get; }
        internal DuplicateTermException(string duplicateTerm) : base("Duplicate term detected.") {
            DuplicateTerm = duplicateTerm;
        }
    }
    public class InvalidTermException : Exception {
        public string Term { get; }
        internal InvalidTermException(string term, string message) : base(message) {
            Term = term;
        }
    }
    public class InvalidWeightArray : Exception {
        public string Term { get; }
        internal InvalidWeightArray(string term, string message) : base(message) {
            Term = term;
        }
    }
}
