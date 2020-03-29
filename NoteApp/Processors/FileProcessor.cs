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
        private List<string> fileRepo;
        private readonly FileProcessor fp;
        

        public List<string> LoadFilesToList()
        {
            List<string> thisList = Directory.EnumerateFiles(folderPath, "*.xaml").ToList();
            List<string> newList = new List<string>();
            foreach (string s in thisList)
            {
                newList.Add(s);
            }
            newList.Reverse();
            return newList;
        }
    }
}
