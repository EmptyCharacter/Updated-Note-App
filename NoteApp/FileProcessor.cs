using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoteApp
{
    public class FileProcessor
    {
        public string folderPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
        public string createFilePath = $@"{ DateTime.Now.Ticks}.xaml";
        private readonly List<string> fileRepo;
        private readonly FileProcessor fp;
        public FileProcessor()
        {
            fp = new FileProcessor();
            fileRepo = fp.LoadFilesToList();
        }

        public List<string> LoadFilesToList()
        {
            List<string> thisList = Directory.EnumerateFiles(folderPath, "*.xaml").ToList();
            List<string> filesAdded = new List<string>();
            foreach (string s in thisList)
            {
                filesAdded.Add(s);
            }
            filesAdded.Reverse();
            return filesAdded;
        }
    }
}
