using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace SearchFiles
{
    public static class FuncCommon// по идее нужен отдельный файл с общими самопальными функциями, но для тест задания лень
    {
        public static void Swap<T>(ref List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }

    public static class SearchFile
    {
        public class Files
        {
            public List<string> Tags = new();
            public readonly string FilePathTarget;
            public readonly string NameFile;

            private Regex RgNameFile = new(@"[^\\]+\..+");

            public Files(string _FilePathTarget, string Tag)
            {
                FilePathTarget = _FilePathTarget;

                NameFile = RgNameFile.Match(_FilePathTarget).Value.Trim('.');

                Tags.Add(Tag);
            }


        }
        public static List<Files> Search(string Query, string PathFile)
        {
            List<Files> _Fls = new();
            List<string> WordsTag = CraftTags(Query);

            List<string> NewFilesPaths = new();

            List<string> extensions;


            for (int j = 0; j < WordsTag.Count; j++)
            {
                extensions = new() { "* " + WordsTag[j] + " *", WordsTag[j] + " *", "* " + WordsTag[j] + ".*" };

                for (int k = 0; k < extensions.Count; k++)
                {
                    NewFilesPaths.AddRange(NotSearchedFiles(ref _Fls, Directory.EnumerateFiles(PathFile, extensions[k], SearchOption.AllDirectories).ToList(), WordsTag[j]));

                    if (NewFilesPaths.Count != 0)
                    {
                        for (int i = 0; i < NewFilesPaths.Count; i++)
                            _Fls.Add(new Files(NewFilesPaths[i], WordsTag[j]));

                        NewFilesPaths.Clear();
                    }
                }


            }

            return _Fls;
        }

        private static List<string> CraftTags(string Query)
        {
            List<string> Tags = new();
            List<string[]> MorphemesFile = new();


            Regex RgWordsTagAlone = new Regex(@"\S+");
            Regex RgWordsTagPhrase = new Regex(@"\S+\s\S+", RegexOptions.RightToLeft);

            Tags.AddRange(RgWordsTagPhrase.Matches(Query).OfType<Match>().Select(m => m.Groups[0].Value).ToList());
            Tags.AddRange(RgWordsTagAlone.Matches(Query).OfType<Match>().Select(m => m.Groups[0].Value).ToList());

            //for (int i = 0; i < MorphemesFile.Count; i++)
            //if (!RgWordsTagPhrase.IsMatch(Tags[i])) Tags.AddRange(SearchMorpheme(Tags[i], MorphemesFile[j]));            

            return Tags;
        }
        private static List<string> NotSearchedFiles(ref List<Files> _FilesSearched, List<string> FilesPaths, string Tag)
        {
            for (int i = 0; i < _FilesSearched.Count; i++)
                for (int j = 0; j < FilesPaths.Count; j++)
                    if (_FilesSearched[i].FilePathTarget == FilesPaths[j])
                    {
                        FilesPaths.RemoveAt(j);
                        _FilesSearched[i].Tags.Add(Tag);
                    }

            return FilesPaths;
        }

        public static void Sort(ref List<Files> _FilesSearched)
        {
            for (int i = 0; i < _FilesSearched.Count; i++)
                for (int j = 0; j < _FilesSearched.Count; j++)
                    if (_FilesSearched[i].Tags.Count > _FilesSearched[j].Tags.Count)
                        FuncCommon.Swap(ref _FilesSearched, i, j);
        }


        private static List<string> SearchMorpheme(string word)// я это притащил с другого калькулятора
        {
            List<string> NewWords = new();

            return NewWords;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Введите путь каталога каталога ");

            string PathFile = "C:\\Users\\VINKOS\\source\\repos\\WorkMain_ITB\\WorkMain_ITB\\Книги";

            Console.WriteLine("Введите искомые файлы ");

            List<SearchFile.Files> Files = SearchFile.Search(Console.ReadLine(), PathFile);

            SearchFile.Sort(ref Files);

            for (int i = 0; i < Files.Count; i++) Console.WriteLine("[" + Files[i].NameFile + ", " + Files[i].FilePathTarget + "]");
        }
    }
}
