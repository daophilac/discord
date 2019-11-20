using System;
using System.Collections.Generic;
using System.Text;

namespace CategoryClassifier {
    internal class TermMatcher {
        internal HashSet<string> Terms { get; set; }
        internal TermMatcher(HashSet<string> terms) {
            Terms = terms;
        }
        internal Dictionary<string, int> FoundTerms(IEnumerable<string> standardSentences) {
            Dictionary<string, int> foundTerms = new Dictionary<string, int>();
            foreach (var sentence in standardSentences) {
                string[] words;
                string strRemain = "";
                int stop, start = 0;
                bool isTerm, isStop = false;
                words = sentence.Split(' ');
                stop = words.Length - 1;
                while (!isStop && stop >= 0) {
                    for (int i = start; i <= stop; i++) {
                        strRemain += words[i] + " ";
                    }
                    strRemain = strRemain.Trim().ToLower();
                    isTerm = false;
                    foreach (var t in Terms) {
                        if (t.Equals(strRemain)) {
                            if (!foundTerms.ContainsKey(t)) {
                                foundTerms.Add(strRemain, 0);
                            }
                            foundTerms[strRemain]++;
                            isTerm = true;
                            if (start == 0) {
                                isStop = true;
                            }
                            else {
                                stop = start - 1;
                                start = 0;
                            }
                            break;
                        }
                    }
                    if (!isTerm) {
                        if (start == stop) {
                            stop--;
                            start = 0;
                        }
                        else
                            start += 1;
                    }
                    strRemain = "";
                }
            }
            return foundTerms;
        }
    }
}
