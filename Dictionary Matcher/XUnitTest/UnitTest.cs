using XUnitTest;
using System;
using Xunit;
using System.IO;
using Xunit.Sdk;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace XUnitTest {
    public class UnitTest {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTest(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
        }
        #region constructor tests
        /// <summary>
        /// Test case 1: file path for matching dictionary is null
        /// Expect: ArgumentNullException
        /// </summary>
        [Fact]
        void TConstructor1() {
            Assert.Throws<ArgumentNullException>(() => { Matcher matcher = new Matcher(null); });
        }

        /// <summary>
        /// Test case 2: matching dictionary file doesn't exist
        /// Expect: FileNotFoundException
        /// </summary>
        [Fact]
        void TConstructor2() {
            Assert.Throws<FileNotFoundException>(() => { Matcher matcher = new Matcher("a"); });
        }

        /// <summary>
        /// Test case 3: matching dictionary file is valid
        /// Expect: Pass
        /// </summary>
        [Fact]
        void TConstructor3() {
            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt");
            Assert.NotNull(matcher);
        }

        /// <summary>
        /// Test case 4: file path for stopping dictionary is null
        /// Expect: Pass
        /// </summary>
        [Fact]
        void TConstructor4() {
            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt", null);
            Assert.NotNull(matcher);
        }

        /// <summary>
        /// Test case 5: stopping dictionary is valid
        /// Expect: pass
        /// </summary>
        [Fact]
        void TConstructor5() {
            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt", "D:/Desktop/stop.txt");
            Assert.NotNull(matcher);
        }

        /// <summary>
        /// Test case 6: stopping dictionary doesn't exist
        /// Expect: FileNotFoundException
        /// </summary>
        [Fact]
        void TConstructor6() {
            Assert.Throws<FileNotFoundException>(() => { Matcher matcher = new Matcher("D:/Desktop/dictionary.txt", "a"); });
        }
        #endregion

        /// <summary>
        /// Test case 7: a simple paragraph without stop dictionary
        /// Expect: the following result in the output screen. Order doesn't matter
        /// là 1
        /// cầu thủ 2
        /// bóng đá 1
        /// </summary>
        [Fact]
        void TMatchPara1() {
            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt");
            Dictionary<string, int> pairs = matcher.MatchPara("cầu thủ bóng đá cầu thủ là");
            Assert.NotNull(pairs);
            Assert.Equal(3, pairs.Count);
            Assert.True(pairs.ContainsKey("cầu thủ"));
            Assert.True(pairs.ContainsKey("bóng đá"));
            Assert.True(pairs.ContainsKey("là"));
            Assert.Equal(2, pairs["cầu thủ"]);
            Assert.Equal(1, pairs["bóng đá"]);
            Assert.Equal(1, pairs["là"]);
            foreach (var item in pairs) {
                _testOutputHelper.WriteLine($"{item.Key} {item.Value}");
            }
        }

        /// <summary>
        /// Test case 8: a simple paragraph with stop dictionary
        /// Expect: the following result in the output screen. Order doesn't matter
        /// cầu thủ 2
        /// bóng đá 1
        /// </summary>
        [Fact]
        void TMatchPara2() {
            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt", "D:/Desktop/stop.txt");
            Dictionary<string, int> pairs = matcher.MatchPara("cầu thủ bóng đá cầu thủ là cho nên");
            Assert.NotNull(pairs);
            Assert.Equal(2, pairs.Count);
            Assert.True(pairs.ContainsKey("cầu thủ"));
            Assert.True(pairs.ContainsKey("bóng đá"));
            Assert.False(pairs.ContainsKey("là"));
            Assert.False(pairs.ContainsKey("cho nên"));
            Assert.False(pairs.ContainsKey("cho"));
            Assert.False(pairs.ContainsKey("nên"));
            Assert.Equal(2, pairs["cầu thủ"]);
            Assert.Equal(1, pairs["bóng đá"]);
            foreach (var item in pairs) {
                _testOutputHelper.WriteLine($"{item.Key} {item.Value}");
            }
        }

        /// <summary>
        /// Test case 9: two paragraphs with stop dictionary
        /// Expect: the following result in the output screen. Order doesn't matter
        /// cầu thủ 2
        /// bóng đá 1
        /// </summary>
        [Fact]
        void TMatchParas1() {
            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt", "D:/Desktop/stop.txt");
            string[] paras = new string[] { "cầu thủ bóng đá", "cầu thủ là cho nên" };
            Dictionary<string, int> pairs = matcher.MatchParas(paras);
            Assert.NotNull(pairs);
            Assert.Equal(2, pairs.Count);
            Assert.True(pairs.ContainsKey("cầu thủ"));
            Assert.True(pairs.ContainsKey("bóng đá"));
            Assert.False(pairs.ContainsKey("là"));
            Assert.False(pairs.ContainsKey("cho nên"));
            Assert.False(pairs.ContainsKey("cho"));
            Assert.False(pairs.ContainsKey("nên"));
            Assert.Equal(2, pairs["cầu thủ"]);
            Assert.Equal(1, pairs["bóng đá"]);
            foreach (var item in pairs) {
                _testOutputHelper.WriteLine($"{item.Key} {item.Value}");
            }
        }

        /// <summary>
        /// Test case 10: a file contains paragraphs without stop dictionary
        /// Expect: the following result in the output screen. Order doesn't matter
        /// là 1
        /// cầu thủ 2
        /// bóng đá 1
        /// </summary>
        [Fact]
        void TMatchFile1() {
            string filePath = "D:/Desktop/test.txt";
            File.WriteAllLines(filePath, new string[] { "cầu thủ bóng đá cầu thủ là" });

            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt");
            Dictionary<string, int> pairs = matcher.MatchFile(filePath);
            Assert.NotNull(pairs);
            Assert.Equal(3, pairs.Count);
            Assert.True(pairs.ContainsKey("cầu thủ"));
            Assert.True(pairs.ContainsKey("bóng đá"));
            Assert.True(pairs.ContainsKey("là"));
            Assert.Equal(2, pairs["cầu thủ"]);
            Assert.Equal(1, pairs["bóng đá"]);
            Assert.Equal(1, pairs["là"]);
            foreach (var item in pairs) {
                _testOutputHelper.WriteLine($"{item.Key} {item.Value}");
            }
        }

        /// <summary>
        /// Test case 11: two files contains paragraphs without stop dictionary
        /// Expect: the following result int the output screen. Order doesn't matter
        /// là 1
        /// cầu thủ 2
        /// bóng đá 1
        /// </summary>
        [Fact]
        void TMatchFiles1() {
            string filePath1 = "D:/Desktop/test1.txt";
            string filePath2 = "D:/Desktop/test2.txt";
            File.WriteAllLines(filePath1, new string[] { "cầu thủ bóng đá" });
            File.WriteAllLines(filePath2, new string[] { "cầu thủ là" });

            Matcher matcher = new Matcher("D:/Desktop/dictionary.txt");
            Dictionary<string, int> pairs = matcher.MatchFiles(new string[] { filePath1, filePath2 });
            Assert.NotNull(pairs);
            Assert.Equal(3, pairs.Count);
            Assert.True(pairs.ContainsKey("cầu thủ"));
            Assert.True(pairs.ContainsKey("bóng đá"));
            Assert.True(pairs.ContainsKey("là"));
            Assert.Equal(2, pairs["cầu thủ"]);
            Assert.Equal(1, pairs["bóng đá"]);
            Assert.Equal(1, pairs["là"]);
            foreach (var item in pairs) {
                _testOutputHelper.WriteLine($"{item.Key} {item.Value}");
            }
        }
    }
}
