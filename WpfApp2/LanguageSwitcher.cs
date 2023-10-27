using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp2
{
    internal class LanguageSwitcher
    {
        private readonly List<string> _langs;
        public List<string> Langs
        {
            get { return _langs; }
        }
        public LanguageSwitcher(string path , string mask = ".resx")
        {
            _langs = new List<string>();
            InitLanguages(path, mask);
        }

        private void InitLanguages(string pathToDir, string mask = ".resx")
        {
            if(Directory.Exists(pathToDir)) 
            {
                string[] files = Directory.GetFiles(pathToDir);
                foreach(string oneFilePath in files) 
                {
                    if(oneFilePath.ToLower().EndsWith(mask.ToLower()))
                    {
                        Regex regex = new Regex(@"\.(\w{2})\.resx",RegexOptions.IgnoreCase);
                        Match match = regex.Match(oneFilePath);
                        Group group = match.Groups[1];
                        _langs.Add(group.Value.Length>0 ? group.Value : "en");

                    }
                }
            }
           
        }
    }
}
