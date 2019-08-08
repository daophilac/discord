using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Discord.Tools {
    class FileDealer {
        private StreamWriter streamWriter;
        private StreamReader streamReader;
        private Encoding encoding;
        private string filePath { get; set; }
        public FileDealer() {
            this.encoding = Encoding.UTF8;
        }
        public FileDealer(string filePath) {
            this.filePath = filePath;
        }
        public FileDealer(Encoding encoding) {
            this.encoding = encoding;
        }
        public FileDealer(string filePath, Encoding encoding) {
            this.filePath = filePath;
            this.encoding = encoding;
        }
        public void SetFilePath(string filePath) {
            this.filePath = filePath;
        }
        public void SetEncoding(Encoding encoding) {
            this.encoding = encoding;
        }
        public void SetProperties(string filePath, Encoding encoding) {
            this.filePath = filePath;
            this.encoding = encoding;
        }
        private void CreateDirectoryIfNotExist(string filePath) {
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);
        }
        public void Write(string filePath, string content, bool append) {
            CreateDirectoryIfNotExist(filePath);
            this.streamWriter = new StreamWriter(filePath, append, this.encoding);
            this.streamWriter.Write(content);
            this.streamWriter.Close();
        }
        public void Write(string content, bool append) {
            if (this.filePath == null) {
                throw new Exception(Program.ExceptionNullFilePath);
            }
            CreateDirectoryIfNotExist(filePath);
            this.streamWriter = new StreamWriter(filePath, append, this.encoding);
            this.streamWriter.Write(content);
            this.streamWriter.Close();
        }
        public void WriteLine(string filePath, string line, bool append) {
            CreateDirectoryIfNotExist(filePath);
            this.streamWriter = new StreamWriter(filePath, append, this.encoding);
            this.streamWriter.WriteLine(line);
            this.streamWriter.Close();
        }
        public void WriteLine(string line, bool append) {
            if (this.filePath == null) {
                throw new Exception(Program.ExceptionNullFilePath);
            }
            CreateDirectoryIfNotExist(filePath);
            this.streamWriter = new StreamWriter(this.filePath, append, this.encoding);
            this.streamWriter.WriteLine(line);
            this.streamWriter.Close();
        }
        public void WriteLines(string filePath, string[] lines, bool append) {
            if (lines.Length == 0) {
                return;
            }
            CreateDirectoryIfNotExist(filePath);
            if (!append) {
                this.streamWriter = new StreamWriter(filePath, false, this.encoding);
                this.streamWriter.Close();
                this.streamWriter.WriteLine(lines[0]);
                this.streamWriter = new StreamWriter(filePath, true, this.encoding);
                for(int i = 1; i < lines.Length; i++) {
                    this.streamWriter.WriteLine(lines[i]);
                }
                this.streamWriter.Close();
            }
            else {
                this.streamWriter = new StreamWriter(filePath, true, this.encoding);
                for (int i = 0; i < lines.Length; i++) {
                    this.streamWriter.WriteLine(lines[i]);
                }
                this.streamWriter.Close();
            }
        }
        public void WriteLines(string[] lines, bool append) {
            if (lines.Length == 0) {
                return;
            }
            if (this.filePath == null) {
                throw new Exception(Program.ExceptionNullFilePath);
            }
            CreateDirectoryIfNotExist(filePath);
            if (!append) {
                this.streamWriter = new StreamWriter(filePath, false, this.encoding);
                this.streamWriter.Close();
                this.streamWriter.WriteLine(lines[0]);
                this.streamWriter = new StreamWriter(filePath, true, this.encoding);
                for (int i = 1; i < lines.Length; i++) {
                    this.streamWriter.WriteLine(lines[i]);
                }
                this.streamWriter.Close();
            }
            else {
                this.streamWriter = new StreamWriter(filePath, true, this.encoding);
                for (int i = 0; i < lines.Length; i++) {
                    this.streamWriter.WriteLine(lines[i]);
                }
                this.streamWriter.Close();
            }
        }
        public string ReadLine(string filePath) {
            if (!File.Exists(filePath)) {
                throw new Exception(Program.ExceptionFileNotFound);
            }
            if(this.streamReader == null) {
                this.streamReader = new StreamReader(filePath);
            }
            string line = this.streamReader.ReadLine();
            return line;
        }
        public List<string> ReadAllLines(string filePath) {
            if (!File.Exists(filePath)) {
                throw new Exception(Program.ExceptionFileNotFound);
            }
            if (this.streamReader == null) {
                this.streamReader = new StreamReader(filePath);
            }
            List<string> lines = new List<string>();
            string line = this.streamReader.ReadLine();
            while (line != null) {
                lines.Add(line);
                line = this.streamReader.ReadLine();
            }
            return lines;
        }
        public void Close() {
            try {
                this.streamReader.Close();
            }
            catch (Exception) { }
            finally {

            }
        }
    }
}