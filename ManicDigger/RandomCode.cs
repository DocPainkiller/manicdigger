﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ManicDigger
{
    public class InjectAttribute : Attribute
    {
    }
    public interface IGetFilePath
    {
        string GetFile(string p);
    }
    public class GetFilePathDummy : IGetFilePath
    {
        #region IGetFilePath Members
        public string GetFile(string p)
        {
            return p;
        }
        #endregion
    }
    public class GetFilePath : IGetFilePath
    {
        public GetFilePath(IEnumerable<string> datapath)
        {
            this.DataPath = new List<string>(datapath);
        }
        List<string> DataPath;
        Dictionary<string, string> cache = new Dictionary<string, string>();
        public string GetFile(string filename)
        {
            if (!cache.ContainsKey(filename))
            {
                cache[filename] = GetFile1(filename);
            }
            return cache[filename];
        }
        string GetFile1(string filename)
        {
            if (!Directory.Exists("data"))
            {
                throw new Exception("data not found");
            }
            List<string> paths = new List<string>();
            foreach (string s in DataPath)
            {
                paths.Add(Path.Combine("data", s));
            }
            paths.Add("data");
            paths.Add("");
            foreach (string path in paths)
            {
                bool again = false;
                string filename2 = filename;
            tryagain:
                string a = Path.Combine(path, filename2);
                if (File.Exists(a))
                {
                    return a;
                }
                if (!again && filename2.EndsWith(".png"))
                {
                    filename2 = filename2.Replace(".png", ".jpg");
                    again = true;
                    goto tryagain;
                }
                if (!again && filename2.EndsWith(".jpg"))
                {
                    filename2 = filename2.Replace(".jpg", ".png");
                    again = true;
                    goto tryagain;
                }
            }
            throw new FileNotFoundException(filename + " not found.");
        }
    }
}