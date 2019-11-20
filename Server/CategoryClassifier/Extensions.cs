using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CategoryClassifier {
    internal static class Extensions {
        internal static string[] ToStandardSentences(this string paragraph) {
            paragraph = paragraph.ToLower();
            paragraph = paragraph.Trim();
            paragraph = Regex.Replace(paragraph, "(\"|\\(|\\[|\\{|\\}|\\]|\\)|\\*|“|”)+", "");
            paragraph = Regex.Replace(paragraph, "\\s+", " ");
            paragraph = Regex.Replace(paragraph, "(\\.|!|\\n|:)+", ",");
            paragraph = Regex.Replace(paragraph, ",+\\s+", ",");
            if (paragraph.EndsWith(',')) {
                paragraph = paragraph[0..^1];
            }
            return paragraph.Split("'");
        }
        internal static Dictionary<string, float[]> ExtractTermsWeightFromFile(this string filePath) {
            if (!File.Exists(filePath)) {
                throw new FileNotFoundException(filePath);
            }
            Dictionary<string, float[]> result = new Dictionary<string, float[]>();
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) {
                string[] parts = lines[i].Split("\t");
                float[] weights = new float[parts.Length - 1];
                for (int j = 0; j < parts.Length - 1; j++) {
                    weights[j] = float.Parse(parts[j + 1]);
                }
                result.Add(parts[0], weights);
            }
            return result;
        }
    }
}
