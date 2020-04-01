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

        
        /*----------------Methods to retrive and process files from directory-------------------------*/

        //Returns a list of files within specified directory
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

        //Return last edit in string format
        public string LastEdit(string path)
        {
            int lastEdit = CalculateEdit(path);
            if (lastEdit <= 1)
            {
                if(lastEdit == 0)
                {
                    return "Today";

                }
                return "Yesterday";
            }
            else
            {
                string edit = lastEdit + " Days Ago";
                return edit;
            }

        }

        //Calculate how long ago file was last edited
        public int CalculateEdit(string path)
        {
            DateTime now = DateTime.Now;
            DateTime lastEdit = File.GetLastWriteTime(path);
            var temp = now - lastEdit;
            var newTemp = (int)temp.TotalDays;
            return newTemp;
        }








    }
}
