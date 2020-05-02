using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BibleVersePowerPointCreatorWebApp
{



    // this is used to process our XML file of the bible in Mien
    // we will used this Bible to build the database
    // the database will give Mien word and their matching script in Thai/Roman/Lao
    // for exaple เหฒย = zeix 
    // we do this by going through each word in the bible
    // then match each word with the other script in the same position
    // such as
    //ต้ง ธอ ทิน-ฮู่ง เหฒย ลู่ง เหฒย เดา
    //Dongh cor Tin-Hungh zeix lungh zeix ndau
    // these two sentence will be match word for word to build the XML database.
    public class mienscriptconvertorDataBaseBuilder
    {
        public static string GetBibleVerse(string bookname, int chaptoget, int versetoget, string bibleversion2)
        {



            string strStart = @"<span id=""ch" + chaptoget + @"v"
+ versetoget + @""" class=""verseId""></span>" + versetoget
+ @"</sup><span onmouseover=""setCurrentVerse('" + chaptoget + @":" + versetoget + @"')"">";

            string filepath2 = @"bible\mienbible\" + bibleversion2 + @"\"
               + bookname + @"\" + chaptoget.ToString() + ".htm";
            string fullpath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath2);

            string textfromfile2 = System.IO.File.ReadAllText(fullpath2);
            string VerseRaw = BibleVersePowerPointCreatorWebApp.pptCreatorMain.getStringsBetween(textfromfile2, strStart);

            //clean up the string and get rid of <html> tag
            string VerseClean = BibleVersePowerPointCreatorWebApp.pptCreatorMain.cleanupparsedtext(VerseRaw);
            return VerseClean;
        }
        public static string BuildThaiScriptMatches(string arg)
        {
            IDictionary<string, int> BookNameAndChapterdict =
                new Dictionary<string, int>()
             {

                {"Genesis", 50 },
                {"Exodus", 40 },
                {"Leviticus", 27 },
                {"Numbers", 36 },
                {"Deuteronomy", 34 },
                {"Joshua", 24 },
                {"Judges", 21 },
                {"Ruth", 4 },
                {"1Samuel", 31 },
                {"2Samuel", 24 },
                {"1Kings", 22 },
                {"2Kings", 25 },
                {"1Chronicles", 29 },
                {"2Chronicles", 36 },
                {"Ezra", 10 },
                {"Nehemiah", 13 },
                {"Esther", 10 },
                {"Job", 42 },
                {"Psalm", 150 },
                {"Proverbs",31  },
                {"Ecclesiastes",12  },
                {"SongOfSolomon", 8 },
                {"Isaiah", 66 },
                {"Jeremiah", 52 },
                {"Lamentations",5  },
                {"Ezekiel", 48 },
                {"Daniel", 12 },
                {"Hosea", 14 },
                {"Joel", 3 },
                {"Amos", 9 },
                {"Obadiah",1  },
                {"Jonah",4  },
                {"Micah",  7},
                {"Nahum", 3 },
                {"Habakkuk", 3 },
                {"Zephaniah",  3},
                {"Haggai", 2 },
                {"Zechariah",  14},
                {"Malachi",  4},
                {"Matthew", 28 },
                {"Mark",16  },
                {"Luke", 24 },
                {"John",21  },
                {"Acts", 28 },
                {"Romans", 16 },
                {"1Corinthians",  16},
                {"2Corinthians", 13 },
                {"Galatians", 6 },
                {"Ephesians", 6 },
                {"Philippians", 4 },
                {"Colossians", 4 },
                {"1Thessalonians", 5 },
                {"2Thessalonians", 3 },
                {"1Timothy", 6 },
                {"2Timothy",4  },
                {"Titus",  3},
                {"Philemon", 1 },
                {"Hebrews", 13 },
                {"James", 5 },
                {"1Peter",5  },
                {"2Peter",3},
                {"1John",5},
                {"2John",1},
                {"3John",1},
                {"Jude",1},
                {"Revelation",22}
                };



            string bookname = "1Chronicles";
            int numberOfChapter = 1; //the number of chapter in this book
            int chaptoget = 1;
            int versetoget = 1;
            string notmatchedlog = "\n not matched THAI-ROMAN\n";
            foreach (KeyValuePair<string, int> folder in BookNameAndChapterdict)
            {
                bookname = folder.Key;
                numberOfChapter = folder.Value;
                for (int i = 1; i <= numberOfChapter; i++)
                {
                    chaptoget = i;
                    for (int versei = 1; versei <= 176; versei++)
                    {
                        versetoget = versei;
                        string ThaiRawVerse = "";
                        try
                        {
                            ThaiRawVerse = GetBibleVerse(bookname, chaptoget, versetoget, "ThaiMien");

                        }
                        catch (Exception e)
                        {
                            break;
                        }
                        string ThaiClenedVerse = ThaiRawVerse.Replace(",", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace(".", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace(":", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("*", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\"", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\'", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\n", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\'", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\\(", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\\)", " ");

                        string MienNewRomanRawVerse = GetBibleVerse(bookname, chaptoget, versetoget, "MienNewRoman");
                        string MienNewRomanClenedVerse = MienNewRomanRawVerse.Replace(",", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace(".", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace(":", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("*", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\"", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\'", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\n", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\'", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\\(", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\\)", " ");

                        string[] MienThaiwords = ThaiClenedVerse.Split(' ');
                        string[] MienNewRomanwords = MienNewRomanClenedVerse.Split(' ');
                        if (MienThaiwords.Length != MienNewRomanwords.Length)
                        {
                            notmatchedlog = notmatchedlog + "\n" + bookname + versetoget + chaptoget;
                            break;
                        }

                        //for (int wordi = 0; versei < MienThaiwords.Length; wordi++)
                        //{
                        //    try
                        //    {
                        //        AddWordToDatabase(MienThaiwords[wordi], MienNewRomanwords[wordi]);
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        break;
                        //    }
                        //}

                    }

                }
            }
            return (notmatchedlog);

        }

        // this will build the initial file
        public static string SplitIntoChapterTextThaiScriptMatches(string arg)
        {
            IDictionary<string, int> BookNameAndChapterdict =
                new Dictionary<string, int>()
             {

                {"Genesis", 50 },
                {"Exodus", 40 },
                {"Leviticus", 27 },
                {"Numbers", 36 },
                {"Deuteronomy", 34 },
                {"Joshua", 24 },
                {"Judges", 21 },
                {"Ruth", 4 },
                {"1Samuel", 31 },
                {"2Samuel", 24 },
                {"1Kings", 22 },
                {"2Kings", 25 },
                {"1Chronicles", 29 },
                {"2Chronicles", 36 },
                {"Ezra", 10 },
                {"Nehemiah", 13 },
                {"Esther", 10 },
                {"Job", 42 },
                {"Psalm", 150 },
                {"Proverbs",31  },
                {"Ecclesiastes",12  },
                {"SongOfSolomon", 8 },
                {"Isaiah", 66 },
                {"Jeremiah", 52 },
                {"Lamentations",5  },
                {"Ezekiel", 48 },
                {"Daniel", 12 },
                {"Hosea", 14 },
                {"Joel", 3 },
                {"Amos", 9 },
                {"Obadiah",1  },
                {"Jonah",4  },
                {"Micah",  7},
                {"Nahum", 3 },
                {"Habakkuk", 3 },
                {"Zephaniah",  3},
                {"Haggai", 2 },
                {"Zechariah",  14},
                {"Malachi",  4},
                {"Matthew", 28 },
                {"Mark",16  },
                {"Luke", 24 },
                {"John",21  },
                {"Acts", 28 },
                {"Romans", 16 },
                {"1Corinthians",  16},
                {"2Corinthians", 13 },
                {"Galatians", 6 },
                {"Ephesians", 6 },
                {"Philippians", 4 },
                {"Colossians", 4 },
                {"1Thessalonians", 5 },
                {"2Thessalonians", 3 },
                {"1Timothy", 6 },
                {"2Timothy",4  },
                {"Titus",  3},
                {"Philemon", 1 },
                {"Hebrews", 13 },
                {"James", 5 },
                {"1Peter",5  },
                {"2Peter",3},
                {"1John",5},
                {"2John",1},
                {"3John",1},
                {"Jude",1},
                {"Revelation",22}
                };



            string bookname = "1Chronicles";
            int numberOfChapter = 1; //the number of chapter in this book
            int chaptoget = 1;
            int versetoget = 1;
            string notmatchedlog = "\n not matched THAI-ROMAN\n";
            foreach (KeyValuePair<string, int> folder in BookNameAndChapterdict)
            {
                bookname = folder.Key;
                numberOfChapter = folder.Value;
                for (int i = 1; i <= numberOfChapter; i++)
                {
                    chaptoget = i;
                    string chapter_string = "";
                    string filepath_save = @"new_bible\" + bookname + @"_" + chaptoget.ToString() + ".txt";
                    string filepath_save2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath_save);

                    for (int versei = 1; versei <= 176; versei++)
                    {
                        versetoget = versei;
                        string ThaiRawVerse = "";
                        try
                        {
                            ThaiRawVerse = GetBibleVerse(bookname, chaptoget, versetoget, "ThaiMien");

                            }
                        catch (Exception e)
                        {
                            break;
                        }
                        string ThaiClenedVerse = ThaiRawVerse.Replace(",", "\n");
                        ThaiClenedVerse = ThaiClenedVerse.Replace(".", "\n");
                        ThaiClenedVerse = ThaiClenedVerse.Replace(":", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("*", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\"", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\'", " ");
                        //ThaiClenedVerse = ThaiClenedVerse.Replace("\n", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\'", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\\(", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\\)", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("-", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("^", " ");


                        
                            chapter_string = chapter_string + "\n" + ThaiClenedVerse;


                        



                    }
                    File.WriteAllText(filepath_save2, chapter_string);
                }
            }
            return (notmatchedlog);

        }

        // this will clean the file:
        // choose the most credible match for each word
        public static string cleanThaiScriptMatches()
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
            doc.Load(path1);



            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {
                string thaiscript = xn["thaiscript"].InnerText;

                string newromanscript = xn["newromanscript"].InnerText;
                string[] splittednewromanscript = newromanscript.Split('+');
                //if (splittednewromanscript.Length < 10) 
                //{
                //    continue;
                //}
                string cleanedRomanWord = "";

                var max = (splittednewromanscript.GroupBy(x => x)
                        .Select(x => new { num = x, cnt = x.Count() })
                        .OrderByDescending(g => g.cnt)
                        .Select(g => g.num).First());

                string mostFrequentWord = max.Key;
                cleanedRomanWord = mostFrequentWord;
                xn["newromanscript"].InnerText = cleanedRomanWord;

                doc.Save(path1);
                //System.Threading.Thread.Sleep(100);

            }
            return ("successCLEAN");
        }


        public static string AddWordToDatabase(string inputThaiword, string inputRomanword)
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
            doc.Load(path1);



            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {
                string thaiscript = xn["thaiscript"].InnerText;
                if (inputThaiword == thaiscript)
                {
                    string newromanscript = xn["newromanscript"].InnerText;
                    string addedword = newromanscript + "+" + inputRomanword;
                    xn["newromanscript"].InnerText = addedword;
                    doc.Save(path1);

                    return "Added To Existing";
                }

            }

            XmlNode newword = doc.CreateNode(XmlNodeType.Element, "word", "");

            XmlNode ThaiWord = doc.CreateNode(XmlNodeType.Element, "thaiscript", "");
            ThaiWord.InnerText = inputThaiword;
            newword.AppendChild(ThaiWord);

            XmlNode RomanWord = doc.CreateNode(XmlNodeType.Element, "newromanscript", "");
            RomanWord.InnerText = inputRomanword;
            newword.AppendChild(RomanWord);

            doc.DocumentElement.AppendChild(newword);

            doc.Save(path1);
            return "Added NEW";
        }


        public static string BuildLaoScriptMatches(string arg)
        {
            IDictionary<string, int> BookNameAndChapterdict =
                new Dictionary<string, int>()
             {

                {"Genesis", 50 },
                {"Exodus", 40 },
                {"Leviticus", 27 },
                {"Numbers", 36 },
                {"Deuteronomy", 34 },
                {"Joshua", 24 },
                {"Judges", 21 },
                {"Ruth", 4 },
                {"1Samuel", 31 },
                {"2Samuel", 24 },
                {"1Kings", 22 },
                {"2Kings", 25 },
                {"1Chronicles", 29 },
                {"2Chronicles", 36 },
                {"Ezra", 10 },
                {"Nehemiah", 13 },
                {"Esther", 10 },
                {"Job", 42 },
                {"Psalm", 150 },
                {"Proverbs",31  },
                {"Ecclesiastes",12  },
                {"SongOfSolomon", 8 },
                {"Isaiah", 66 },
                {"Jeremiah", 52 },
                {"Lamentations",5  },
                {"Ezekiel", 48 },
                {"Daniel", 12 },
                {"Hosea", 14 },
                {"Joel", 3 },
                {"Amos", 9 },
                {"Obadiah",1  },
                {"Jonah",4  },
                {"Micah",  7},
                {"Nahum", 3 },
                {"Habakkuk", 3 },
                {"Zephaniah",  3},
                {"Haggai", 2 },
                {"Zechariah",  14},
                {"Malachi",  4},
                {"Matthew", 28 },
                {"Mark",16  },
                {"Luke", 24 },
                {"John",21  },
                {"Acts", 28 },
                {"Romans", 16 },
                {"1Corinthians",  16},
                {"2Corinthians", 13 },
                {"Galatians", 6 },
                {"Ephesians", 6 },
                {"Philippians", 4 },
                {"Colossians", 4 },
                {"1Thessalonians", 5 },
                {"2Thessalonians", 3 },
                {"1Timothy", 6 },
                {"2Timothy",4  },
                {"Titus",  3},
                {"Philemon", 1 },
                {"Hebrews", 13 },
                {"James", 5 },
                {"1Peter",5  },
                {"2Peter",3},
                {"1John",5},
                {"2John",1},
                {"3John",1},
                {"Jude",1},
                {"Revelation",22}
                };



            string bookname = "1Chronicles";
            int numberOfChapter = 1; //the number of chapter in this book
            int chaptoget = 1;
            int versetoget = 1;

            foreach (KeyValuePair<string, int> folder in BookNameAndChapterdict)
            {
                bookname = folder.Key;
                numberOfChapter = folder.Value;
                for (int i = 1; i <= numberOfChapter; i++)
                {
                    chaptoget = i;
                    for (int versei = 1; versei <= 176; versei++)
                    {
                        versetoget = versei;
                        string ThaiRawVerse = "";
                        try
                        {
                            ThaiRawVerse = GetBibleVerse(bookname, chaptoget, versetoget, "MienLao");

                        }
                        catch (Exception e)
                        {
                            break;
                        }
                        string ThaiClenedVerse = ThaiRawVerse.Replace(",", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace(".", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace(":", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("*", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\"", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\'", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\n", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\'", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\\(", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("\\)", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("`", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("“", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("‘", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("?", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("?", " ");
                        ThaiClenedVerse = ThaiClenedVerse.Replace("!", " ");


                        string MienNewRomanRawVerse = GetBibleVerse(bookname, chaptoget, versetoget, "MienNewRoman");
                        string MienNewRomanClenedVerse = MienNewRomanRawVerse.Replace(",", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace(".", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace(":", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("*", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\"", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\'", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\n", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\'", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\\(", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("\\)", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("`", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("“", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("‘", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("?", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("?", " ");
                        MienNewRomanClenedVerse = MienNewRomanClenedVerse.Replace("!", " ");

                        string[] MienThaiwords = ThaiClenedVerse.Split(' ');
                        string[] MienNewRomanwords = MienNewRomanClenedVerse.Split(' ');
                        if (MienThaiwords.Length != MienNewRomanwords.Length)
                        {
                            break;
                        }
                        for (int wordi = 0; wordi < MienThaiwords.Length; wordi++)
                        {
                            try
                            {
                                AddLaoWordToDatabase(MienThaiwords[wordi], MienNewRomanwords[wordi]);
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                        }
                    }

                }
            }
            return ("success");

        }

        // this will clean the file:
        // choose the most credible match for each word
        public static string cleanAnyScriptMatches(string ScriptName)
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman20.xml");
            doc.Load(path1);

            //string ScriptName = "oldromanscript";

            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {

                string newromanscript = xn[ScriptName].InnerText;
                string[] splittednewromanscript = newromanscript.Split('+');
                string cleanedRomanWord = "";

                var max = (splittednewromanscript.GroupBy(x => x)
                .Select(x => new { num = x, cnt = x.Count() })
                .OrderByDescending(g => g.cnt)
                .Select(g => g.num).First());

                string mostFrequentWord = max.Key;
                cleanedRomanWord = mostFrequentWord;
                xn[ScriptName].InnerText = cleanedRomanWord;

                doc.Save(path1);
                // System.Threading.Thread.Sleep(19);

            }
            return ("successCLEAN");
        }

        //here roman is lao, too lazy to change
        public static string AddLaoWordToDatabase(string inputThaiword, string inputRomanword)
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
            doc.Load(path1);



            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {
                string thaiscript = xn["laoscript"].InnerText;
                if (inputThaiword == thaiscript)
                {
                    string newromanscript = xn["newromanscript"].InnerText;
                    string addedword = newromanscript + "+" + inputRomanword;
                    xn["newromanscript"].InnerText = addedword;
                    doc.Save(path1);

                    return "Added To Existing";
                }

            }

            XmlNode newword = doc.CreateNode(XmlNodeType.Element, "word", "");

            XmlNode ThaiWord = doc.CreateNode(XmlNodeType.Element, "laoscript", "");
            ThaiWord.InnerText = inputThaiword;
            newword.AppendChild(ThaiWord);

            XmlNode LaoWord = doc.CreateNode(XmlNodeType.Element, "newromanscript", "");
            LaoWord.InnerText = inputRomanword;
            newword.AppendChild(LaoWord);

            XmlNode newRomanWord = doc.CreateNode(XmlNodeType.Element, "thaiscript", "");
            newword.AppendChild(newRomanWord);

            XmlNode oldRomanWord = doc.CreateNode(XmlNodeType.Element, "oldromanscript", "");
            newword.AppendChild(oldRomanWord);



            doc.DocumentElement.AppendChild(newword);

            doc.Save(path1);
            return "Added NEW";
        }
        public static string cleanArabicNumber()
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
            doc.Load(path1);



            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {
                string thaiscript = xn["thaiscript"].InnerText;
                string laoscript = xn["laoscript"].InnerText;
                string newromanscript = xn["newromanscript"].InnerText;
                string oldromanscript = xn["oldromanscript"].InnerText;
                bool ThaicontainsInt = thaiscript.Any(char.IsDigit);
                bool LaocontainsInt = laoscript.Any(char.IsDigit);
                bool RomancontainsInt = newromanscript.Any(char.IsDigit);
                bool OldRomancontainsInt = oldromanscript.Any(char.IsDigit);
                if (ThaicontainsInt || LaocontainsInt || RomancontainsInt || OldRomancontainsInt)
                {
                    xn.ParentNode.RemoveChild(xn);
                }
                doc.Save(path1);

            }
            return ("successCLEAN");
        }

        public static string GetBibleVerseFromJason(string booknameinput, string chaptoget, string versetoget, string script)
        {
            //script option: MienThai MienOldRoman, MienNewRoman
            string scripttoget = script;
            string jsonfilepath = System.Web.HttpContext.Current.Server.MapPath(scripttoget);

            string bookname = booknameinput;
            string chapterstring = chaptoget;
            string versestring = versetoget;

            string verse_output_1 = "-1";

            // first language
            using (StreamReader r = new StreamReader(jsonfilepath))
            {
                string json = r.ReadToEnd();
                JArray obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(json);

                foreach (var result in obj)
                {
                    var book = result[bookname];
                    var chapter = (book[chapterstring]);
                    var verse = (chapter[versestring]);
                    verse_output_1 = verse.ToString();

                }

            }
            return verse_output_1;



        }

        public static string GetBibleVerseFromJason1book(string chaptoget, string versetoget, string script)
        {
            //script option: MienThai MienOldRoman, MienNewRoman
            string scripttoget = script;
            string jsonfilepath = System.Web.HttpContext.Current.Server.MapPath(scripttoget);

            string chapterstring = chaptoget;
            string versestring = versetoget;

            string verse_output_1 = "-1";

            // first language
            using (StreamReader r = new StreamReader(jsonfilepath))
            {
                string json = r.ReadToEnd();

                var details = JObject.Parse(json);

                var chapter = (details[chapterstring]);
                    var verse = (chapter[versestring]);
                    verse_output_1 = verse.ToString();

             

            }
            return verse_output_1;



        }


        public static string ExtractJson()
        {
            string[] BookNameNewRoman = new string[66]
{
                                "Tin Deic Douh",
                "Cuotv I^yipv",
                "Lewi",
                "Saauv Mienh",
                "Nzamc Leiz-Latc",
                "Yo^su^waa",
                "Domh Jien",
                "Lu^te",
                "1 Saa^mu^en",
                "2 Saa^mu^en",
                "1 Hungh Douh",
                "2 Hungh Douh",
                "1 Zunh Doic Douh",
                "2 Zunh Doic Douh",
                "E^saa^laa",
                "Ne^haa^mi",
                "E^se^te",
                "Yopv",
                "Singx Nzung",
                "Cong-Mengh Waac",
                "Gorngv Seix Zaangc",
                "Saa^lo^morn Nyei Nzung",
                "I^saa^yaa",
                "Ye^le^mi",
                "Naanc Zingh Nzung",
                "E^se^ken",
                "Ndaa^ni^en",
                "Ho^se^yaa",
                "Yo^en",
                "Aamotv",
                "O^mbaa^ndi",
                "Yonaa",
                "Mikaa",
                "Naa^hum",
                "Haa^mbaa^gukc",
                "Se^fan^yaa",
                "Hakv^gai",
                "Se^kaa^li^yaa",
                "Maa^laa^ki",
                "Matv^taai",
                "Maako",
                "Lugaa",
                "Yo^han",
                "Gong-Zoh",
                "Lomaa",
                "1 Ko^lin^to",
                "2 Ko^lin^to",
                "Gaa^laa^tie",
                "E^fe^so",
                "Fi^lipv^poi",
                "Ko^lo^si",
                "1 Te^saa^lo^ni^gaa",
                "2 Te^saa^lo^ni^gaa",
                "1 Ti^mo^tai",
                "2 Ti^mo^tai",
                "Tidatc",
                "Fi^le^mon",
                "Hipv^lu",
                "Yaagorpc",
                "1 Bide",
                "2 Bide",
                "1 Yo^han",
                "2 Yo^han",
                "3 Yo^han",
                "Yiu^ndaa",
                "Laauc Yaangh"

};
            string[] BookNameOldRoman = new string[66]
            {
                                "Tin teig trub",
                "Zwrtq i-yipq",
                "le-wi",
                "saauq myenb",
                "Ramg leib latg",
                "yo-su-waa",
                "tomb Jyen",
                "lu-Te",
                "1 saa-mu-en",
                "2 saa-mu-en",
                "1 huvb trub",
                "2 huvb trub",
                "1 zunb txig trub",
                "2 zunb txig trub",
                "e-saa-laa",
                "ne-haa-mi",
                "e-se-Te",
                "yopq",
                "sivj Ruv",
                "Zovb mevb waag",
                "kxvq seij zavg",
                "saa-lo-mxn Eei Ruv",
                "i-saa-yaa",
                "ye-le-mi",
                "naang zivb Ruv",
                "e-sej-Ken",
                "Daa-ni-en",
                "ho-se-yaa",
                "yo-en",
                "aa-motq",
                "o-Baa-Di",
                "yo-naa",
                "mi-Kaa",
                "naa-hum",
                "haa-Baa-kukg",
                "sej-fan-yaa",
                "hakq-kai",
                "sej-Kaa-li-yaa",
                "maa-laa-Ki",
                "matq-Taai",
                "maa-Ko",
                "lu-kaa",
                "yo-han",
                "kovb zob",
                "lo-maa",
                "1 Ko-lin-To",
                "2 Ko-lin-To",
                "kaa-laa-Tia",
                "e-fe-so",
                "fi-lipq-Pxi",
                "Ko-lo-sij",
                "1 Te-saa-lo-ni-kaa",
                "2 Te-saa-lo-ni-kaa",
                "1 Ti-mo-Tai",
                "2 Ti-mo-Tai",
                "Ti-tatg",
                "fi-le-mon",
                "hipq-lu",
                "yaa-kxpg",
                "1 pi-te",
                "2 pi-te",
                "1 yo-han",
                "2 yo-han",
                "3 yo-han",
                "yiu-Daa",
                "laaug yaavb"

            };
            string[] BookNameThai = new string[66]
            {                "ทิน เต่ย โต้ว" ,
                "ธ้วด อี​ยิบ",
                "เล​วี" ,
                "ซ้าว เมี่ยน" ,
                "หฑั่ม เล์ย-หลัด" ,
                "โย​ซู​วา" ,
                "ต้ม เจียน" ,
                "ลู​เท",
                "1 ซา​มู​เอน",
                "2 ซา​มู​เอน",
                "1 ฮู่ง โต้ว",
                "2 ฮู่ง โต้ว",
                "1 ฒุ่น ต่อย โต้ว",
                "2 ฒุ่น ต่อย โต้ว",
                "เอ​สะ​ลา",
                "เน​หะ​มี",
                "เอ​เซ​เท",
                "โย้บ",
                "สีง ฑูง",
                "ธง-เม่ง หว่า",
                "ก๊อง เสย หฒั่ง",
                "ซา​โล​มอน เญย ฑูง",
                "อิ​สะ​ยา",
                "เย​เล​มี",
                "หน่าน ฒี่ง ฑูง",
                "เอ​เส​เคน",
                "ดา​นี​เอน",
                "โฮ​เซ​ยา",
                "โย​เอน",
                "อา​โม้ด",
                "โอ​บา​ดี",
                "โย​นา",
                "มี​คา",
                "นา​ฮูม",
                "ฮา​บา​กุก",
                "เส​ฟัน​ยา",
                "ฮัก​ไก",
                "เส​คา​ลิ​ยา",
                "มา​ลา​คี",
                "มัด​ทาย",
                "มา​โค",
                "ลู​กา",
                "โย​ฮัน",
                "กง-โฒ่",
                "โล​มา",
                "1 โค​ลิน​โท",
                "2 โค​ลิน​โท",
                "กา​ลา​เทีย",
                "เอ​เฟ​โซ",
                "ฟี​ลิบ​พอย",
                "โค​โล​สี",
                "1 เท​สะ​โล​นิ​กา",
                "2 เท​สะ​โล​นิ​กา",
                "1 ทิ​โม​ไท",
                "2 ทิ​โม​ไท",
                "ทิ​ตัด",
                "ฟี​เล​โมน",
                "ฮิบ​ลู",
                "ยา​กอบ",
                "1 ปี​เต",
                "2 ปี​เต",
                "1 โย​ฮัน",
                "2 โย​ฮัน",
                "3 โย​ฮัน",
                "ยิว​ดา",
                "หล่าว ย่าง"
                };
            int[] numberofchapterarray = new int[66]
            {          50 ,
                40 ,
                 27 ,
                 36 ,
                 34 ,
                 24 ,
                21 ,
                 4 ,
                 31 ,
                24 ,
              22 ,
              25 ,
               29 ,
                36 ,
                10 ,
                13 ,
               10 ,
                42 ,
               150 ,
             31  ,
               12  ,
                8 ,
             66 ,
               52 ,
               5  ,
                48 ,
                12 ,
               14 ,
              3 ,
               9 ,
                1  ,
                4  ,
                 7,
               3 ,
                 3 ,
               3,
                2 ,
                 14,
                4,
               28 ,
              16  ,
             24 ,
              21  ,
                28 ,
                 16 ,
                 16,
              13 ,
                 6 ,
             6 ,
               4 ,
               4 ,
               5 ,
                3 ,
                6 ,
              4  ,
                 3,
            1 ,
             13 ,
                 5 ,
              5  ,
             3,
            5,
                1,
               1,
            1,
               22
            };
            string output = "NOTHING DONE buildfromJSON";


            for (int book_i = 0; book_i < 66; book_i++)
            {
                string bookname = BookNameNewRoman[book_i];


                //script option: MienThai MienOldRoman, MienNewRoman
                string jsonfilepath = System.Web.HttpContext.Current.Server.MapPath("/bible/json/mien/MienNewRoman.json");

                string book_output = "-1";

                // first language
                using (StreamReader r = new StreamReader(jsonfilepath))
                {
                    string json = r.ReadToEnd();
                    JArray obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JArray>(json);

                    foreach (var result in obj)
                    {
                        string savejsonfilepath = System.Web.HttpContext.Current.Server.MapPath("/bible/json/mien/MienNewRoman/" + bookname + ".json");

                        var book = result[bookname];
                        book_output = book.ToString();

                        using (var tw = new StreamWriter(savejsonfilepath, true))
                        {

                            tw.WriteLine(book_output);
                            tw.Close();
                        }
                    }

                }
                output = output + "\n" + bookname;

            }
            return output;



        }



        public static string ExtractJSONchapter()
        {
            string[] BookNameEnglish = new string[66]
                { "Genesis",
"Exodus",
"Leviticus",
"Numbers",
"Deuteronomy",
"Joshua",
"Judges",
"Ruth",
"1Samuel",
"2Samuel",
"1Kings",
"2Kings",
"1Chronicles",
"2Chronicles",
"Ezra",
"Nehemiah",
"Esther",
"Job",
"Psalm",
"Proverbs",
"Ecclesiastes",
"SongofSolomon",
"Isaiah",
"Jeremiah",
"Lamentations",
"Ezekiel",
"Daniel",
"Hosea",
"Joel",
"Amos",
"Obadiah",
"Jonah",
"Micah",
"Nahum",
"Habakkuk",
"Zephaniah",
"Haggai",
"Zechariah",
"Malachi",
"Matthew",
"Mark",
"Luke",
"John",
"Acts",
"Romans",
"1Corinthians",
"2Corinthians",
"Galatians",
"Ephesians",
"Philippians",
"Colossians",
"1Thessalonians",
"2Thessalonians",
"1Timothy",
"2Timothy",
"Titus",
"Philemon",
"Hebrews",
"James",
"1Peter",
"2Peter",
"1John",
"2John",
"3John",
"Jude",
"Revelation"};
            string[] BookNameNewRoman = new string[66]
            {
                                "Tin Deic Douh",
                "Cuotv I^yipv",
                "Lewi",
                "Saauv Mienh",
                "Nzamc Leiz-Latc",
                "Yo^su^waa",
                "Domh Jien",
                "Lu^te",
                "1 Saa^mu^en",
                "2 Saa^mu^en",
                "1 Hungh Douh",
                "2 Hungh Douh",
                "1 Zunh Doic Douh",
                "2 Zunh Doic Douh",
                "E^saa^laa",
                "Ne^haa^mi",
                "E^se^te",
                "Yopv",
                "Singx Nzung",
                "Cong-Mengh Waac",
                "Gorngv Seix Zaangc",
                "Saa^lo^morn Nyei Nzung",
                "I^saa^yaa",
                "Ye^le^mi",
                "Naanc Zingh Nzung",
                "E^se^ken",
                "Ndaa^ni^en",
                "Ho^se^yaa",
                "Yo^en",
                "Aamotv",
                "O^mbaa^ndi",
                "Yonaa",
                "Mikaa",
                "Naa^hum",
                "Haa^mbaa^gukc",
                "Se^fan^yaa",
                "Hakv^gai",
                "Se^kaa^li^yaa",
                "Maa^laa^ki",
                "Matv^taai",
                "Maako",
                "Lugaa",
                "Yo^han",
                "Gong-Zoh",
                "Lomaa",
                "1 Ko^lin^to",
                "2 Ko^lin^to",
                "Gaa^laa^tie",
                "E^fe^so",
                "Fi^lipv^poi",
                "Ko^lo^si",
                "1 Te^saa^lo^ni^gaa",
                "2 Te^saa^lo^ni^gaa",
                "1 Ti^mo^tai",
                "2 Ti^mo^tai",
                "Tidatc",
                "Fi^le^mon",
                "Hipv^lu",
                "Yaagorpc",
                "1 Bide",
                "2 Bide",
                "1 Yo^han",
                "2 Yo^han",
                "3 Yo^han",
                "Yiu^ndaa",
                "Laauc Yaangh"

            };
            string[] BookNameOldRoman = new string[66]
            {
                                "Tin teig trub",
                "Zwrtq i-yipq",
                "le-wi",
                "saauq myenb",
                "Ramg leib latg",
                "yo-su-waa",
                "tomb Jyen",
                "lu-Te",
                "1 saa-mu-en",
                "2 saa-mu-en",
                "1 huvb trub",
                "2 huvb trub",
                "1 zunb txig trub",
                "2 zunb txig trub",
                "e-saa-laa",
                "ne-haa-mi",
                "e-se-Te",
                "yopq",
                "sivj Ruv",
                "Zovb mevb waag",
                "kxvq seij zavg",
                "saa-lo-mxn Eei Ruv",
                "i-saa-yaa",
                "ye-le-mi",
                "naang zivb Ruv",
                "e-sej-Ken",
                "Daa-ni-en",
                "ho-se-yaa",
                "yo-en",
                "aa-motq",
                "o-Baa-Di",
                "yo-naa",
                "mi-Kaa",
                "naa-hum",
                "haa-Baa-kukg",
                "sej-fan-yaa",
                "hakq-kai",
                "sej-Kaa-li-yaa",
                "maa-laa-Ki",
                "matq-Taai",
                "maa-Ko",
                "lu-kaa",
                "yo-han",
                "kovb zob",
                "lo-maa",
                "1 Ko-lin-To",
                "2 Ko-lin-To",
                "kaa-laa-Tia",
                "e-fe-so",
                "fi-lipq-Pxi",
                "Ko-lo-sij",
                "1 Te-saa-lo-ni-kaa",
                "2 Te-saa-lo-ni-kaa",
                "1 Ti-mo-Tai",
                "2 Ti-mo-Tai",
                "Ti-tatg",
                "fi-le-mon",
                "hipq-lu",
                "yaa-kxpg",
                "1 pi-te",
                "2 pi-te",
                "1 yo-han",
                "2 yo-han",
                "3 yo-han",
                "yiu-Daa",
                "laaug yaavb"

            };

            string[] BookNameThai = new string[66]
            {                "ทิน เต่ย โต้ว" ,
                "ธ้วด อี​ยิบ",
                "เล​วี" ,
                "ซ้าว เมี่ยน" ,
                "หฑั่ม เล์ย-หลัด" ,
                "โย​ซู​วา" ,
                "ต้ม เจียน" ,
                "ลู​เท",
                "1 ซา​มู​เอน",
                "2 ซา​มู​เอน",
                "1 ฮู่ง โต้ว",
                "2 ฮู่ง โต้ว",
                "1 ฒุ่น ต่อย โต้ว",
                "2 ฒุ่น ต่อย โต้ว",
                "เอ​สะ​ลา",
                "เน​หะ​มี",
                "เอ​เซ​เท",
                "โย้บ",
                "สีง ฑูง",
                "ธง-เม่ง หว่า",
                "ก๊อง เสย หฒั่ง",
                "ซา​โล​มอน เญย ฑูง",
                "อิ​สะ​ยา",
                "เย​เล​มี",
                "หน่าน ฒี่ง ฑูง",
                "เอ​เส​เคน",
                "ดา​นี​เอน",
                "โฮ​เซ​ยา",
                "โย​เอน",
                "อา​โม้ด",
                "โอ​บา​ดี",
                "โย​นา",
                "มี​คา",
                "นา​ฮูม",
                "ฮา​บา​กุก",
                "เส​ฟัน​ยา",
                "ฮัก​ไก",
                "เส​คา​ลิ​ยา",
                "มา​ลา​คี",
                "มัด​ทาย",
                "มา​โค",
                "ลู​กา",
                "โย​ฮัน",
                "กง-โฒ่",
                "โล​มา",
                "1 โค​ลิน​โท",
                "2 โค​ลิน​โท",
                "กา​ลา​เทีย",
                "เอ​เฟ​โซ",
                "ฟี​ลิบ​พอย",
                "โค​โล​สี",
                "1 เท​สะ​โล​นิ​กา",
                "2 เท​สะ​โล​นิ​กา",
                "1 ทิ​โม​ไท",
                "2 ทิ​โม​ไท",
                "ทิ​ตัด",
                "ฟี​เล​โมน",
                "ฮิบ​ลู",
                "ยา​กอบ",
                "1 ปี​เต",
                "2 ปี​เต",
                "1 โย​ฮัน",
                "2 โย​ฮัน",
                "3 โย​ฮัน",
                "ยิว​ดา",
                "หล่าว ย่าง"
                };
            int[] numberofchapterarray = new int[66]
            {          50 ,
                40 ,
                 27 ,
                 36 ,
                 34 ,
                 24 ,
                21 ,
                 4 ,
                 31 ,
                24 ,
              22 ,
              25 ,
               29 ,
                36 ,
                10 ,
                13 ,
               10 ,
                42 ,
               150 ,
             31  ,
               12  ,
                8 ,
             66 ,
               52 ,
               5  ,
                48 ,
                12 ,
               14 ,
              3 ,
               9 ,
                1  ,
                4  ,
                 7,
               3 ,
                 3 ,
               3,
                2 ,
                 14,
                4,
               28 ,
              16  ,
             24 ,
              21  ,
                28 ,
                 16 ,
                 16,
              13 ,
                 6 ,
             6 ,
               4 ,
               4 ,
               5 ,
                3 ,
                6 ,
              4  ,
                 3,
            1 ,
             13 ,
                 5 ,
              5  ,
             3,
            5,
                1,
               1,
            1,
               22
            };
            string output = "NOTHING DONE buildfromJSON";



            string script_NewRoman = ("/bible/json/mien/MienNewRoman.json");


            for (int book_i = 57; book_i < 66; book_i++)
            {
                string bookname1 = BookNameNewRoman[book_i];
                string book_name_english = BookNameEnglish[book_i];
                string chapterstring = "1";
                string versestring = "1";

                string verse_output_1 = "-1";



                int numberOfChapter = numberofchapterarray[book_i];
                for (int chapter_i = 1; chapter_i <= numberOfChapter; chapter_i++)
                {
                    chapterstring = chapter_i.ToString();
                    string chapter_string = "";
                    string filepath_save = @"new_bible\" + book_name_english + @"_" + chapterstring + ".txt";
                    string filepath_save2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filepath_save);

                    for (int versei = 1; versei <= 176; versei++)
                    {
                        versestring = versei.ToString();

                        try
                        {
                            verse_output_1 = GetBibleVerseFromJason(bookname1, chapterstring, versestring, script_NewRoman);

                        }
                        catch (Exception e)
                        {
                            output = output + "\n" + bookname1 + chapterstring + " " + versestring + e;
                            break;
                        }

                        string verse_output_1_cleaned = verse_output_1.Replace(",", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(".", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(":", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\"", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("”", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("“", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\\(", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\\)", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("(", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(")", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("(", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("?", "\n");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("!", "\n");
                        
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("*", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("^", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("-", " ");
                        // verse_output_1_cleaned = verse_output_1_cleaned.Replace("\n", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\'", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("`", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("‘", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("’", " ");
                        string lower_verse = verse_output_1_cleaned.ToLower();
                        char[] charsToTrim = { ' ', '\t' };
                        string trim_verse = lower_verse.Trim(charsToTrim);
                        if (trim_verse == "") { continue; }
                        if (trim_verse == " ") { continue; }
                        if (trim_verse == " \n") { continue; }
                        if (trim_verse == "\n") { continue; }
                        if (trim_verse.Length > 3) {
                        string    add_new_line = trim_verse + "\n";
                        add_new_line = add_new_line.Replace("\n\n", "\n");
                        add_new_line = add_new_line.Replace("\n\n\n", "\n");
                        add_new_line = add_new_line.Replace("\n\n\n\n", "\n");
                        chapter_string = chapter_string + add_new_line;

                        }




                    }
                    // using the method 
                    String[] chapter_string_splitterd= chapter_string.Split(new Char[] { '\n' });
                    string Out_final = "";
                    foreach (string one_line in chapter_string_splitterd)
                        {
                        if (one_line.Length > 3)
                        { Out_final = Out_final + one_line + "\n"; }
                        }

                    File.WriteAllText(filepath_save2, Out_final);
                }

                output = bookname1;
            }
            return output;
        }

        public static string buildfromJSON()
        {
            string[] BookNameNewRoman = new string[66]
            {
                                "Tin Deic Douh",
                "Cuotv I^yipv",
                "Lewi",
                "Saauv Mienh",
                "Nzamc Leiz-Latc",
                "Yo^su^waa",
                "Domh Jien",
                "Lu^te",
                "1 Saa^mu^en",
                "2 Saa^mu^en",
                "1 Hungh Douh",
                "2 Hungh Douh",
                "1 Zunh Doic Douh",
                "2 Zunh Doic Douh",
                "E^saa^laa",
                "Ne^haa^mi",
                "E^se^te",
                "Yopv",
                "Singx Nzung",
                "Cong-Mengh Waac",
                "Gorngv Seix Zaangc",
                "Saa^lo^morn Nyei Nzung",
                "I^saa^yaa",
                "Ye^le^mi",
                "Naanc Zingh Nzung",
                "E^se^ken",
                "Ndaa^ni^en",
                "Ho^se^yaa",
                "Yo^en",
                "Aamotv",
                "O^mbaa^ndi",
                "Yonaa",
                "Mikaa",
                "Naa^hum",
                "Haa^mbaa^gukc",
                "Se^fan^yaa",
                "Hakv^gai",
                "Se^kaa^li^yaa",
                "Maa^laa^ki",
                "Matv^taai",
                "Maako",
                "Lugaa",
                "Yo^han",
                "Gong-Zoh",
                "Lomaa",
                "1 Ko^lin^to",
                "2 Ko^lin^to",
                "Gaa^laa^tie",
                "E^fe^so",
                "Fi^lipv^poi",
                "Ko^lo^si",
                "1 Te^saa^lo^ni^gaa",
                "2 Te^saa^lo^ni^gaa",
                "1 Ti^mo^tai",
                "2 Ti^mo^tai",
                "Tidatc",
                "Fi^le^mon",
                "Hipv^lu",
                "Yaagorpc",
                "1 Bide",
                "2 Bide",
                "1 Yo^han",
                "2 Yo^han",
                "3 Yo^han",
                "Yiu^ndaa",
                "Laauc Yaangh"

            };
            string[] BookNameOldRoman = new string[66]
            {
                                "Tin teig trub",
                "Zwrtq i-yipq",
                "le-wi",
                "saauq myenb",
                "Ramg leib latg",
                "yo-su-waa",
                "tomb Jyen",
                "lu-Te",
                "1 saa-mu-en",
                "2 saa-mu-en",
                "1 huvb trub",
                "2 huvb trub",
                "1 zunb txig trub",
                "2 zunb txig trub",
                "e-saa-laa",
                "ne-haa-mi",
                "e-se-Te",
                "yopq",
                "sivj Ruv",
                "Zovb mevb waag",
                "kxvq seij zavg",
                "saa-lo-mxn Eei Ruv",
                "i-saa-yaa",
                "ye-le-mi",
                "naang zivb Ruv",
                "e-sej-Ken",
                "Daa-ni-en",
                "ho-se-yaa",
                "yo-en",
                "aa-motq",
                "o-Baa-Di",
                "yo-naa",
                "mi-Kaa",
                "naa-hum",
                "haa-Baa-kukg",
                "sej-fan-yaa",
                "hakq-kai",
                "sej-Kaa-li-yaa",
                "maa-laa-Ki",
                "matq-Taai",
                "maa-Ko",
                "lu-kaa",
                "yo-han",
                "kovb zob",
                "lo-maa",
                "1 Ko-lin-To",
                "2 Ko-lin-To",
                "kaa-laa-Tia",
                "e-fe-so",
                "fi-lipq-Pxi",
                "Ko-lo-sij",
                "1 Te-saa-lo-ni-kaa",
                "2 Te-saa-lo-ni-kaa",
                "1 Ti-mo-Tai",
                "2 Ti-mo-Tai",
                "Ti-tatg",
                "fi-le-mon",
                "hipq-lu",
                "yaa-kxpg",
                "1 pi-te",
                "2 pi-te",
                "1 yo-han",
                "2 yo-han",
                "3 yo-han",
                "yiu-Daa",
                "laaug yaavb"

            };

            string[] BookNameThai = new string[66]
            {                "ทิน เต่ย โต้ว" ,
                "ธ้วด อี​ยิบ",
                "เล​วี" ,
                "ซ้าว เมี่ยน" ,
                "หฑั่ม เล์ย-หลัด" ,
                "โย​ซู​วา" ,
                "ต้ม เจียน" ,
                "ลู​เท",
                "1 ซา​มู​เอน",
                "2 ซา​มู​เอน",
                "1 ฮู่ง โต้ว",
                "2 ฮู่ง โต้ว",
                "1 ฒุ่น ต่อย โต้ว",
                "2 ฒุ่น ต่อย โต้ว",
                "เอ​สะ​ลา",
                "เน​หะ​มี",
                "เอ​เซ​เท",
                "โย้บ",
                "สีง ฑูง",
                "ธง-เม่ง หว่า",
                "ก๊อง เสย หฒั่ง",
                "ซา​โล​มอน เญย ฑูง",
                "อิ​สะ​ยา",
                "เย​เล​มี",
                "หน่าน ฒี่ง ฑูง",
                "เอ​เส​เคน",
                "ดา​นี​เอน",
                "โฮ​เซ​ยา",
                "โย​เอน",
                "อา​โม้ด",
                "โอ​บา​ดี",
                "โย​นา",
                "มี​คา",
                "นา​ฮูม",
                "ฮา​บา​กุก",
                "เส​ฟัน​ยา",
                "ฮัก​ไก",
                "เส​คา​ลิ​ยา",
                "มา​ลา​คี",
                "มัด​ทาย",
                "มา​โค",
                "ลู​กา",
                "โย​ฮัน",
                "กง-โฒ่",
                "โล​มา",
                "1 โค​ลิน​โท",
                "2 โค​ลิน​โท",
                "กา​ลา​เทีย",
                "เอ​เฟ​โซ",
                "ฟี​ลิบ​พอย",
                "โค​โล​สี",
                "1 เท​สะ​โล​นิ​กา",
                "2 เท​สะ​โล​นิ​กา",
                "1 ทิ​โม​ไท",
                "2 ทิ​โม​ไท",
                "ทิ​ตัด",
                "ฟี​เล​โมน",
                "ฮิบ​ลู",
                "ยา​กอบ",
                "1 ปี​เต",
                "2 ปี​เต",
                "1 โย​ฮัน",
                "2 โย​ฮัน",
                "3 โย​ฮัน",
                "ยิว​ดา",
                "หล่าว ย่าง"
                };
            int[] numberofchapterarray = new int[66]
            {          50 ,
                40 ,
                 27 ,
                 36 ,
                 34 ,
                 24 ,
                21 ,
                 4 ,
                 31 ,
                24 ,
              22 ,
              25 ,
               29 ,
                36 ,
                10 ,
                13 ,
               10 ,
                42 ,
               150 ,
             31  ,
               12  ,
                8 ,
             66 ,
               52 ,
               5  ,
                48 ,
                12 ,
               14 ,
              3 ,
               9 ,
                1  ,
                4  ,
                 7,
               3 ,
                 3 ,
               3,
                2 ,
                 14,
                4,
               28 ,
              16  ,
             24 ,
              21  ,
                28 ,
                 16 ,
                 16,
              13 ,
                 6 ,
             6 ,
               4 ,
               4 ,
               5 ,
                3 ,
                6 ,
              4  ,
                 3,
            1 ,
             13 ,
                 5 ,
              5  ,
             3,
            5,
                1,
               1,
            1,
               22
            };
            string output = "NOTHING DONE buildfromJSON";



            string script_Thai = ("/bible/json/mien/MienThai.json");
            string script_OldRoman = ("/bible/json/mien/MienOldRoman.json");

            for (int book_i = 0; book_i < 66; book_i++)
            {
                string bookname1 = BookNameThai[book_i];
                string bookname2 = BookNameOldRoman[book_i];

                string chapterstring = "1";
                string versestring = "1";

                string verse_output_1 = "-1";
                string verse_output_2 = "-1";


                int numberOfChapter = numberofchapterarray[book_i];
                for (int chapter_i = 1; chapter_i <= numberOfChapter; chapter_i++)
                {
                    chapterstring = chapter_i.ToString();

                    for (int versei = 1; versei <= 176; versei++)
                    {
                        versestring = versei.ToString();

                        try
                        {
                            verse_output_1 = GetBibleVerseFromJason(bookname1, chapterstring, versestring, script_Thai);
                            verse_output_2 = GetBibleVerseFromJason(bookname2, chapterstring, versestring, script_OldRoman);

                        }
                        catch (Exception e)
                        {
                            output = output + "\n" + bookname1 + chapterstring + " " + versestring + e;
                            break;
                        }

                        string verse_output_1_cleaned = verse_output_1.Replace(",", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(".", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(":", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("*", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\"", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\'", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\n", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\'", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\\(", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\\)", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("`", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("“", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("‘", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("?", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("?", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("!", " ");


                        string verse_output_2_cleaned = verse_output_2.Replace(",", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace(".", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace(":", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("*", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\"", " ");
                        //verse_output_2_cleaned = verse_output_2_cleaned.Replace("\'", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\n", " ");
                        //verse_output_2_cleaned = verse_output_2_cleaned.Replace("\'", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\\(", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\\)", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("`", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("“", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("‘", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("?", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("?", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("!", " ");

                        string[] verse_output_1_array = verse_output_1_cleaned.Split(' ');
                        string[] verse_output_2_array = verse_output_2_cleaned.Split(' ');
                        if (verse_output_1_array.Length != verse_output_2_array.Length)
                        {
                            continue;
                        }
                        for (int wordi = 0; wordi < verse_output_1_array.Length; wordi++)
                        {
                            try
                            {
                                AddWordToDatabaseAnyScript(verse_output_1_array[wordi], "thaiscript", verse_output_2_array[wordi], "oldromanscript", "laoscript", "newromanscript");
                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }


                    }
                }

                output = bookname1;
            }
            return output;
        }
        public static string AddWordToDatabaseAnyScript(string inputExistingword, string script1, string inputNewword, string script2, string scriptNotAdding1, string scriptNotAdding2)
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman20.xml");
            doc.Load(path1);
            
            XmlNode wordfound = doc.SelectSingleNode("ThaiToNewRoman/word[thaiscript = "+"'"+ inputExistingword + "'"+"]");
            if (wordfound != null) 
            {
           
                string new_script2 = wordfound[script2].InnerText;
                string addedword = new_script2 + "+" + inputNewword;
                wordfound[script2].InnerText = addedword;
                doc.Save(path1);

                return "Added To Existing";

            }

            //XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            //foreach (XmlNode xn in xnList)
            //{
            //    string existing_script1 = xn[script1].InnerText;
            //    if (inputExistingword == existing_script1)
            //    {
            //        string new_script2 = xn[script2].InnerText;
            //        string addedword = new_script2 + "+" + inputNewword;
            //        xn[script2].InnerText = addedword;
            //        doc.Save(path1);

            //        return "Added To Existing";
            //    }

            //}

            XmlNode newword = doc.CreateNode(XmlNodeType.Element, "word", "");

            XmlNode Script1Word = doc.CreateNode(XmlNodeType.Element, script1, "");
            Script1Word.InnerText = inputExistingword;
            newword.AppendChild(Script1Word);

            XmlNode Script2Word = doc.CreateNode(XmlNodeType.Element, script2, "");
            Script2Word.InnerText = inputNewword;
            newword.AppendChild(Script2Word);

            XmlNode ScriptNotAddingWord1 = doc.CreateNode(XmlNodeType.Element, scriptNotAdding1, "");
            newword.AppendChild(ScriptNotAddingWord1);


            XmlNode ScriptNotAddingWord2 = doc.CreateNode(XmlNodeType.Element, scriptNotAdding2, "");
            newword.AppendChild(ScriptNotAddingWord2);


            doc.DocumentElement.AppendChild(newword);

            doc.Save(path1);
            return "Added NEW";
        }

        public static string AddWordArrayToDatabaseAnyScript(List<string> inputExistingwordArray, string script1, List<string> inputNewwordArray, string script2, string scriptNotAdding1, string scriptNotAdding2)
        {
            XmlDocument doc = new XmlDocument();

            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman20.xml");
            doc.Load(path1);


            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            
            List<string> ExistingStringThatMustBeAdd = inputExistingwordArray;
            List<string> NewStringThatMustBeAdd =  inputNewwordArray;

            foreach (XmlNode xn in xnList)
            {
                string existing_script1 = xn[script1].InnerText;

                for (int input_i = 0; input_i < ExistingStringThatMustBeAdd.Count; input_i++)
                {
                    string inputExistingword = ExistingStringThatMustBeAdd[input_i];
                    string inputNewword = NewStringThatMustBeAdd[input_i];
                    if (inputExistingword == existing_script1)
                    {
                        string new_script2 = xn[script2].InnerText;
                        string addedword = new_script2 + "+" + inputNewword;
                        xn[script2].InnerText = addedword;
                        doc.Save(path1);
                        ExistingStringThatMustBeAdd.RemoveAt(input_i);
                        NewStringThatMustBeAdd.RemoveAt(input_i);
                    }
                }

            }

            for (int new_i = 0; new_i < ExistingStringThatMustBeAdd.Count; new_i++)
            {
                string inputExistingword = ExistingStringThatMustBeAdd[new_i];
                string inputNewword = NewStringThatMustBeAdd[new_i];

                XmlNode newword = doc.CreateNode(XmlNodeType.Element, "word", "");

                XmlNode Script1Word = doc.CreateNode(XmlNodeType.Element, script1, "");
                Script1Word.InnerText = inputExistingword;
                newword.AppendChild(Script1Word);

                XmlNode Script2Word = doc.CreateNode(XmlNodeType.Element, script2, "");
                Script2Word.InnerText = inputNewword;
                newword.AppendChild(Script2Word);

                XmlNode ScriptNotAddingWord1 = doc.CreateNode(XmlNodeType.Element, scriptNotAdding1, "");
                newword.AppendChild(ScriptNotAddingWord1);


                XmlNode ScriptNotAddingWord2 = doc.CreateNode(XmlNodeType.Element, scriptNotAdding2, "");
                newword.AppendChild(ScriptNotAddingWord2);


                doc.DocumentElement.AppendChild(newword);
            }
            doc.Save(path1);
            
            return "Added NEW";
        }


        public static string buildfromJSONoneBOOKatAtime()
        {
            string[] BookNameNewRoman = new string[66]
            {
                                "Tin Deic Douh",
                "Cuotv I^yipv",
                "Lewi",
                "Saauv Mienh",
                "Nzamc Leiz-Latc",
                "Yo^su^waa",
                "Domh Jien",
                "Lu^te",
                "1 Saa^mu^en",
                "2 Saa^mu^en",
                "1 Hungh Douh",
                "2 Hungh Douh",
                "1 Zunh Doic Douh",
                "2 Zunh Doic Douh",
                "E^saa^laa",
                "Ne^haa^mi",
                "E^se^te",
                "Yopv",
                "Singx Nzung",
                "Cong-Mengh Waac",
                "Gorngv Seix Zaangc",
                "Saa^lo^morn Nyei Nzung",
                "I^saa^yaa",
                "Ye^le^mi",
                "Naanc Zingh Nzung",
                "E^se^ken",
                "Ndaa^ni^en",
                "Ho^se^yaa",
                "Yo^en",
                "Aamotv",
                "O^mbaa^ndi",
                "Yonaa",
                "Mikaa",
                "Naa^hum",
                "Haa^mbaa^gukc",
                "Se^fan^yaa",
                "Hakv^gai",
                "Se^kaa^li^yaa",
                "Maa^laa^ki",
                "Matv^taai",
                "Maako",
                "Lugaa",
                "Yo^han",
                "Gong-Zoh",
                "Lomaa",
                "1 Ko^lin^to",
                "2 Ko^lin^to",
                "Gaa^laa^tie",
                "E^fe^so",
                "Fi^lipv^poi",
                "Ko^lo^si",
                "1 Te^saa^lo^ni^gaa",
                "2 Te^saa^lo^ni^gaa",
                "1 Ti^mo^tai",
                "2 Ti^mo^tai",
                "Tidatc",
                "Fi^le^mon",
                "Hipv^lu",
                "Yaagorpc",
                "1 Bide",
                "2 Bide",
                "1 Yo^han",
                "2 Yo^han",
                "3 Yo^han",
                "Yiu^ndaa",
                "Laauc Yaangh"

            };
            string[] BookNameOldRoman = new string[66]
            {
                                "Tin teig trub",
                "Zwrtq i-yipq",
                "le-wi",
                "saauq myenb",
                "Ramg leib latg",
                "yo-su-waa",
                "tomb Jyen",
                "lu-Te",
                "1 saa-mu-en",
                "2 saa-mu-en",
                "1 huvb trub",
                "2 huvb trub",
                "1 zunb txig trub",
                "2 zunb txig trub",
                "e-saa-laa",
                "ne-haa-mi",
                "e-se-Te",
                "yopq",
                "sivj Ruv",
                "Zovb mevb waag",
                "kxvq seij zavg",
                "saa-lo-mxn Eei Ruv",
                "i-saa-yaa",
                "ye-le-mi",
                "naang zivb Ruv",
                "e-sej-Ken",
                "Daa-ni-en",
                "ho-se-yaa",
                "yo-en",
                "aa-motq",
                "o-Baa-Di",
                "yo-naa",
                "mi-Kaa",
                "naa-hum",
                "haa-Baa-kukg",
                "sej-fan-yaa",
                "hakq-kai",
                "sej-Kaa-li-yaa",
                "maa-laa-Ki",
                "matq-Taai",
                "maa-Ko",
                "lu-kaa",
                "yo-han",
                "kovb zob",
                "lo-maa",
                "1 Ko-lin-To",
                "2 Ko-lin-To",
                "kaa-laa-Tia",
                "e-fe-so",
                "fi-lipq-Pxi",
                "Ko-lo-sij",
                "1 Te-saa-lo-ni-kaa",
                "2 Te-saa-lo-ni-kaa",
                "1 Ti-mo-Tai",
                "2 Ti-mo-Tai",
                "Ti-tatg",
                "fi-le-mon",
                "hipq-lu",
                "yaa-kxpg",
                "1 pi-te",
                "2 pi-te",
                "1 yo-han",
                "2 yo-han",
                "3 yo-han",
                "yiu-Daa",
                "laaug yaavb"

            };

            string[] BookNameThai = new string[66]
            {                "ทิน เต่ย โต้ว" ,
                "ธ้วด อี​ยิบ",
                "เล​วี" ,
                "ซ้าว เมี่ยน" ,
                "หฑั่ม เล์ย-หลัด" ,
                "โย​ซู​วา" ,
                "ต้ม เจียน" ,
                "ลู​เท",
                "1 ซา​มู​เอน",
                "2 ซา​มู​เอน",
                "1 ฮู่ง โต้ว",
                "2 ฮู่ง โต้ว",
                "1 ฒุ่น ต่อย โต้ว",
                "2 ฒุ่น ต่อย โต้ว",
                "เอ​สะ​ลา",
                "เน​หะ​มี",
                "เอ​เซ​เท",
                "โย้บ",
                "สีง ฑูง",
                "ธง-เม่ง หว่า",
                "ก๊อง เสย หฒั่ง",
                "ซา​โล​มอน เญย ฑูง",
                "อิ​สะ​ยา",
                "เย​เล​มี",
                "หน่าน ฒี่ง ฑูง",
                "เอ​เส​เคน",
                "ดา​นี​เอน",
                "โฮ​เซ​ยา",
                "โย​เอน",
                "อา​โม้ด",
                "โอ​บา​ดี",
                "โย​นา",
                "มี​คา",
                "นา​ฮูม",
                "ฮา​บา​กุก",
                "เส​ฟัน​ยา",
                "ฮัก​ไก",
                "เส​คา​ลิ​ยา",
                "มา​ลา​คี",
                "มัด​ทาย",
                "มา​โค",
                "ลู​กา",
                "โย​ฮัน",
                "กง-โฒ่",
                "โล​มา",
                "1 โค​ลิน​โท",
                "2 โค​ลิน​โท",
                "กา​ลา​เทีย",
                "เอ​เฟ​โซ",
                "ฟี​ลิบ​พอย",
                "โค​โล​สี",
                "1 เท​สะ​โล​นิ​กา",
                "2 เท​สะ​โล​นิ​กา",
                "1 ทิ​โม​ไท",
                "2 ทิ​โม​ไท",
                "ทิ​ตัด",
                "ฟี​เล​โมน",
                "ฮิบ​ลู",
                "ยา​กอบ",
                "1 ปี​เต",
                "2 ปี​เต",
                "1 โย​ฮัน",
                "2 โย​ฮัน",
                "3 โย​ฮัน",
                "ยิว​ดา",
                "หล่าว ย่าง"
                };
            int[] numberofchapterarray = new int[66]
            {          50 ,
                40 ,
                 27 ,
                 36 ,
                 34 ,
                 24 ,
                21 ,
                 4 ,
                 31 ,
                24 ,
              22 ,
              25 ,
               29 ,
                36 ,
                10 ,
                13 ,
               10 ,
                42 ,
               150 ,
             31  ,
               12  ,
                8 ,
             66 ,
               52 ,
               5  ,
                48 ,
                12 ,
               14 ,
              3 ,
               9 ,
                1  ,
                4  ,
                 7,
               3 ,
                 3 ,
               3,
                2 ,
                 14,
                4,
               28 ,
              16  ,
             24 ,
              21  ,
                28 ,
                 16 ,
                 16,
              13 ,
                 6 ,
             6 ,
               4 ,
               4 ,
               5 ,
                3 ,
                6 ,
              4  ,
                 3,
            1 ,
             13 ,
                 5 ,
              5  ,
             3,
            5,
                1,
               1,
            1,
               22
            };
            string output = "NOTHING DONE buildfromJSON";




            for (int book_i = 47; book_i < 66; book_i++)
            {

                string bookname1 = BookNameThai[book_i];
                string bookname2 = BookNameNewRoman[book_i];

                string script_Thai = ("/bible/json/mien/MienThai/" + bookname1 + ".json");
                string script_OldRoman = ("/bible/json/mien/MienNewRoman/" + bookname2 + ".json");


                string chapterstring = "1";
                string versestring = "1";

                string verse_output_1 = "-1";
                string verse_output_2 = "-1";


                int numberOfChapter = numberofchapterarray[book_i];
                for (int chapter_i = 1; chapter_i <= numberOfChapter; chapter_i++)
                {
                    chapterstring = chapter_i.ToString();
                    var listChapterString1 = new List<string>();
                    var listChapterString2 = new List<string>();
                    for (int versei = 1; versei <= 176; versei++)
                    {
                        versestring = versei.ToString();

                        try
                        {
                            verse_output_1 = GetBibleVerseFromJason1book( chapterstring, versestring, script_Thai);
                            verse_output_2 = GetBibleVerseFromJason1book( chapterstring, versestring, script_OldRoman);

                        }
                        catch (Exception e)
                        {
                            output = output + "\n" + bookname1 + chapterstring + " " + versestring + e;
                            break;
                        }

                        string verse_output_1_cleaned = verse_output_1.Replace(",", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(".", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace(":", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("*", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\"", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\n", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\\(", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\\)", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("`", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("“", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("‘", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("?", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("?", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("!", " ");

                        //for old roman only
                        //verse_output_1_cleaned = verse_output_1_cleaned.Replace("ทิน-ฮู่ง", "ทิน ฮู่ง");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\'", " ");
                        verse_output_1_cleaned = verse_output_1_cleaned.Replace("\'", " ");

                        string verse_output_2_cleaned = verse_output_2.Replace(",", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace(".", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace(":", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("*", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\"", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\n", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\\(", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\\)", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("`", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("“", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("‘", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("?", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("?", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("!", " ");
                        // for old roman only
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\'", " ");
                        verse_output_2_cleaned = verse_output_2_cleaned.Replace("\'", " ");


                        string[] verse_output_1_array = verse_output_1_cleaned.Split(' ');
                        string[] verse_output_2_array = verse_output_2_cleaned.Split(' ');
                        if (verse_output_1_array.Length != verse_output_2_array.Length)
                        {
                            continue;
                        }
                        else 
                        {
                            listChapterString1.AddRange(verse_output_1_array);
                            listChapterString2.AddRange(verse_output_2_array);



                        }


                        //for (int wordi = 0; wordi < verse_output_1_array.Length; wordi++)
                        //{
                        //    try
                        //    {
                        //        AddWordToDatabaseAnyScript(verse_output_1_array[wordi], "thaiscript", verse_output_2_array[wordi], "oldromanscript", "laoscript", "newromanscript");
                        //    }
                        //    catch (Exception e)
                        //    {
                        //        continue;
                        //    }
                        //}


                    }
                    AddWordArrayToDatabaseAnyScript(listChapterString1, "thaiscript", listChapterString2, "newromanscript", "laoscript", "oldromanscript");

                }

                output = bookname1;
            }
            return output;
        }
        public static string cleanCaratAndDash(char carat, string carat_Str)
        {
            XmlDocument doc = new XmlDocument();
            //char carat = '^';
            //string carat_Str = "^";
            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
            doc.Load(path1);

            XmlDocument docOut = new XmlDocument();
            string pathOut = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman20.xml");
            docOut.Load(pathOut);


            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {
                string thaiscript = xn["thaiscript"].InnerText;
                string laoscript = xn["laoscript"].InnerText;
                string newromanscript = xn["newromanscript"].InnerText;
                string oldromanscript = xn["oldromanscript"].InnerText;
                int ThaicontainsCarat = thaiscript.Count(x => x == carat);
                int LaocontainsCarat = laoscript.Count(x => x == carat);
                int RomancontainsCarat = newromanscript.Count(x => x == carat);
                int OldRomancontainsCarat = oldromanscript.Count(x => x == carat);
                if (ThaicontainsCarat==0 & LaocontainsCarat ==0 & RomancontainsCarat ==0 & OldRomancontainsCarat == 0) 
                { continue; }

                

                XmlNode newword = docOut.CreateNode(XmlNodeType.Element, "word", "");

                XmlNode Script1Word = docOut.CreateNode(XmlNodeType.Element, "thaiscript", "");
                Script1Word.InnerText = thaiscript.Replace(carat_Str, "");
                newword.AppendChild(Script1Word);

                XmlNode Script2Word = docOut.CreateNode(XmlNodeType.Element, "laoscript", "");
                Script2Word.InnerText = laoscript.Replace(carat_Str, "");
                newword.AppendChild(Script2Word);

                XmlNode Script3Word = docOut.CreateNode(XmlNodeType.Element, "newromanscript", "");
                Script3Word.InnerText = newromanscript.Replace(carat_Str, "");
                newword.AppendChild(Script3Word);

                XmlNode Script4Word = docOut.CreateNode(XmlNodeType.Element, "oldromanscript", "");
                Script4Word.InnerText = oldromanscript.Replace(carat_Str, "");
                newword.AppendChild(Script4Word);

                docOut.DocumentElement.AppendChild(newword);

            }

            docOut.Save(pathOut);



            return ("successCLEAN");
        }

        public static string ExpandCaratAndDash(char carat) 
        {
            XmlDocument doc = new XmlDocument();
            //char carat = '^';
            string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
            doc.Load(path1);



            XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
            foreach (XmlNode xn in xnList)
            {
                string thaiscript = xn["thaiscript"].InnerText;
                string laoscript = xn["laoscript"].InnerText;
                string newromanscript = xn["newromanscript"].InnerText;
                string oldromanscript = xn["oldromanscript"].InnerText;
                int ThaicontainsCarat = thaiscript.Count(x => x == carat);
                int LaocontainsCarat = laoscript.Count(x => x == carat);
                int RomancontainsCarat = newromanscript.Count(x => x == carat);
                int OldRomancontainsCarat = oldromanscript.Count(x => x == carat);
                
                IDictionary<string, string> StringDict = new Dictionary<string, string>();
                StringDict.Add("thaiscript", thaiscript);
                StringDict.Add("laoscript", laoscript);
                StringDict.Add("newromanscript", newromanscript);
                StringDict.Add("oldromanscript", oldromanscript);

                IDictionary<string,int > CaratCountDict = new Dictionary<string, int>();
                if (ThaicontainsCarat>0) { CaratCountDict.Add("thaiscript", ThaicontainsCarat); }
                if (LaocontainsCarat > 0) { CaratCountDict.Add("laoscript", LaocontainsCarat); }
                if (RomancontainsCarat > 0) { CaratCountDict.Add("newromanscript", RomancontainsCarat); }
                if (OldRomancontainsCarat >0) { CaratCountDict.Add("oldromanscript", OldRomancontainsCarat); }

// check if at least there more than one script contains carat
                if (CaratCountDict.Count < 2)
                {
                    continue;
                }

// check if those script have the same number of carat characters
                if (CaratCountDict.Values.Distinct().Count() != 1) 
                {
                    continue;
                }
                for (int i = 0; i < CaratCountDict.Count-1; i++)
                {
                    List<string> UnactiveScripts = new List<string>() { "thaiscript", "laoscript", "newromanscript", "oldromanscript" };

                    string script1key = CaratCountDict.Keys.ElementAt(i);
                    string script2key = CaratCountDict.Keys.ElementAt(i+1);
                    string FirstScriptStr = StringDict[script1key];
                    string SecondScriptStr = StringDict[script2key];
                    UnactiveScripts.RemoveAll(sc => sc.Contains(script1key));
                    UnactiveScripts.RemoveAll(sc => sc.Contains(script2key));
                    List<string> strlist1 = new List<string>(FirstScriptStr.Split(carat));
                    List<string> strlist2 = new List<string>(SecondScriptStr.Split(carat));

                    AddWordArrayToDatabaseAnyScript(strlist1, script1key, strlist2,  script2key, UnactiveScripts[0], UnactiveScripts[1]);




                }
            }



        
            return ("successCLEAN");
        }


    }
}

