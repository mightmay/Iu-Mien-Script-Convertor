using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BibleVersePowerPointCreatorWebApp
{

    public class mienscriptconvertor
    {
        public static bool ContainsAny(string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
        // should only use on character not string
        public static string ReplaceFirstOccurrenceChar(string Source, char Find, string Replace)
        {
            char[] characters = Source.ToCharArray();
            System.Text.StringBuilder outputstring = new System.Text.StringBuilder("");


            foreach (char character in characters)
            {
                if (character == Find)
                {
                    outputstring.Append(Replace);
                }
                else { outputstring.Append(character); }
            }


            return outputstring.ToString();
        }
        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }
        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.LastIndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        // this contains some manual rules for converting thai to new roman,
        // however, it is not good, therefore, this is no longer being use
        public static string newromantothai(string input)
        {
            //tonemark วรรณยุกต์ , if exist, will be at the end of the word, it is only a sigle character
            IDictionary<string, string> tonemarks_dict = new Dictionary<string, string>();
            tonemarks_dict.Add("c", "อ่");//"เอก"
            tonemarks_dict.Add("h", "อ้");//"โท"
            tonemarks_dict.Add("v", "อ๊");// "ตรี"
            tonemarks_dict.Add("x", "อ๋");//"จัตวา"

            //สระ  vowel

            IDictionary<string, string> basicvowel_dict = new Dictionary<string, string>();
            basicvowel_dict.Add("a", "อา");
            basicvowel_dict.Add("aa", "อา");
            basicvowel_dict.Add("e", "เอ");
            basicvowel_dict.Add("i", "อี");
            basicvowel_dict.Add("o", "โอ");
            basicvowel_dict.Add("u", "อู");

            IDictionary<string, string> vowelplusconsonant_dict = new Dictionary<string, string>();

            //a
            vowelplusconsonant_dict.Add("a", "อา");
            vowelplusconsonant_dict.Add("ae", "แอ");
            vowelplusconsonant_dict.Add("aen", "แอน");
            vowelplusconsonant_dict.Add("aeng", "เอง");
            vowelplusconsonant_dict.Add("aeq", "แอ๊ะ");
            vowelplusconsonant_dict.Add("aet", "แอ๊ด");
            vowelplusconsonant_dict.Add("ai", "ไอ");
            vowelplusconsonant_dict.Add("ak", "อัค");
            vowelplusconsonant_dict.Add("am", "อัม");
            vowelplusconsonant_dict.Add("an", "อัน");
            vowelplusconsonant_dict.Add("ang", "อัง");
            vowelplusconsonant_dict.Add("ap", "อับ");
            vowelplusconsonant_dict.Add("aq", "อะ");
            vowelplusconsonant_dict.Add("at", "อัท");
            vowelplusconsonant_dict.Add("au", "เอา");


            //aa
            vowelplusconsonant_dict.Add("aa", "อา");
            vowelplusconsonant_dict.Add("aai", "อาย");
            vowelplusconsonant_dict.Add("aam", "อาม");
            vowelplusconsonant_dict.Add("aan", "อาน");
            vowelplusconsonant_dict.Add("aang", "อาง");
            vowelplusconsonant_dict.Add("aap", "อาบ");
            vowelplusconsonant_dict.Add("aat", "อาท");
            vowelplusconsonant_dict.Add("aau", "อาว");

            //e
            vowelplusconsonant_dict.Add("e", "เอ");
            vowelplusconsonant_dict.Add("ei", "เอย");
            vowelplusconsonant_dict.Add("ek", "เอก");
            vowelplusconsonant_dict.Add("em", "เอม");
            vowelplusconsonant_dict.Add("en", "เอน");
            vowelplusconsonant_dict.Add("eng", "เอง");
            vowelplusconsonant_dict.Add("ep", "เอบ");
            vowelplusconsonant_dict.Add("eq", "เอะ");
            vowelplusconsonant_dict.Add("er", "เออ");
            vowelplusconsonant_dict.Add("ern", "เออน");
            vowelplusconsonant_dict.Add("et", "เอท");
            vowelplusconsonant_dict.Add("eu", "เอว");

            //i
            vowelplusconsonant_dict.Add("i", "อี");
            vowelplusconsonant_dict.Add("iaa", "อีอา");
            vowelplusconsonant_dict.Add("iaai", "อีอาย");
            vowelplusconsonant_dict.Add("iaam", "อีอาม");
            vowelplusconsonant_dict.Add("iaan", "อีอาน");
            vowelplusconsonant_dict.Add("iaau", "อีอาว");
            vowelplusconsonant_dict.Add("iang", "เอียง");
            vowelplusconsonant_dict.Add("iat", "เอียท");
            vowelplusconsonant_dict.Add("iau", "อีเอา");
            vowelplusconsonant_dict.Add("ie", "เอีย");
            vowelplusconsonant_dict.Add("iei", "เอย");
            vowelplusconsonant_dict.Add("iem", "เอียม");
            vowelplusconsonant_dict.Add("ien", "เอียน");
            vowelplusconsonant_dict.Add("iep", "เอียบ");
            vowelplusconsonant_dict.Add("ieq", "เอียะ");
            vowelplusconsonant_dict.Add("iet", "เอียท");
            vowelplusconsonant_dict.Add("ik", "อิค");
            vowelplusconsonant_dict.Add("im", "อิม");
            vowelplusconsonant_dict.Add("in", "อิน");
            vowelplusconsonant_dict.Add("ing", "อิง");
            vowelplusconsonant_dict.Add("iong", "โอง");
            vowelplusconsonant_dict.Add("iorng", "อีออง");
            vowelplusconsonant_dict.Add("iop", "โอบ");
            vowelplusconsonant_dict.Add("iorp", "โออบ");
            vowelplusconsonant_dict.Add("iorq", "อีเอาะ");
            vowelplusconsonant_dict.Add("iort", "อียอด");
            vowelplusconsonant_dict.Add("iou", "เอียว");
            vowelplusconsonant_dict.Add("ip", "อิบ");
            vowelplusconsonant_dict.Add("iq", "อิ");
            vowelplusconsonant_dict.Add("it", "อิท");
            vowelplusconsonant_dict.Add("iu", "อิว");
            vowelplusconsonant_dict.Add("iui", "อีอุย");

            //O
            vowelplusconsonant_dict.Add("o", "โอ");
            vowelplusconsonant_dict.Add("oi", "ออย");
            vowelplusconsonant_dict.Add("ok", "โอค");
            vowelplusconsonant_dict.Add("om", "อม");
            vowelplusconsonant_dict.Add("on", "อน");
            vowelplusconsonant_dict.Add("ong", "อง");
            vowelplusconsonant_dict.Add("op", "อล");
            vowelplusconsonant_dict.Add("oq", "โอะ");
            vowelplusconsonant_dict.Add("or", "ออ");
            vowelplusconsonant_dict.Add("ork", "ออก");
            vowelplusconsonant_dict.Add("orm", "ออม");
            vowelplusconsonant_dict.Add("orn", "ออน");
            vowelplusconsonant_dict.Add("orng", "ออง");
            vowelplusconsonant_dict.Add("orp", "ออบ");
            vowelplusconsonant_dict.Add("orq", "เอาะ");
            vowelplusconsonant_dict.Add("ort", "ออท");
            vowelplusconsonant_dict.Add("ot", "อท");
            vowelplusconsonant_dict.Add("ou", "โอว");

            //u
            vowelplusconsonant_dict.Add("u", "อู");
            vowelplusconsonant_dict.Add("uaa", "อวา");
            vowelplusconsonant_dict.Add("uai", "อวย");
            vowelplusconsonant_dict.Add("uan", "อัน");
            vowelplusconsonant_dict.Add("uang", "อัง");
            vowelplusconsonant_dict.Add("uat", "อัท");
            vowelplusconsonant_dict.Add("uei", "เอวย");
            vowelplusconsonant_dict.Add("uen", "อีเอน");
            vowelplusconsonant_dict.Add("ui", "อุย");
            vowelplusconsonant_dict.Add("uin", "อุยน");
            vowelplusconsonant_dict.Add("un", "อุน");
            vowelplusconsonant_dict.Add("ung", "อูง");
            vowelplusconsonant_dict.Add("uo", "อัว");
            vowelplusconsonant_dict.Add("uom", "อวม");
            vowelplusconsonant_dict.Add("uon", "อวน");
            vowelplusconsonant_dict.Add("uoq", "อัวะ");
            vowelplusconsonant_dict.Add("uot", "อวท");
            vowelplusconsonant_dict.Add("uq", "อุ");
            vowelplusconsonant_dict.Add("ut", "อุท");

            IDictionary<string, string> letter_dict = new Dictionary<string, string>();

            letter_dict.Add("b", "ป");
            letter_dict.Add("c", "ธ");
            letter_dict.Add("d", "ต");
            letter_dict.Add("f", "ฟ"); //letter_dict.Add("f", "ฝ");

            letter_dict.Add("g", "ก");
            letter_dict.Add("h", "ห"); //letter_dict.Add("h", "ฮ");

            letter_dict.Add("hi", "ฮย");
            letter_dict.Add("hl", "ฮล");
            letter_dict.Add("hm", "ฮม");
            letter_dict.Add("hn", "ฮน");
            letter_dict.Add("hng", "ฮง");
            letter_dict.Add("hny", "ฮญ");
            letter_dict.Add("hu", "ฮว");
            letter_dict.Add("j", "จ");
            letter_dict.Add("k", "ค"); //letter_dict.Add("k", "ข");

            letter_dict.Add("l", "ล");
            letter_dict.Add("m", "ม");
            letter_dict.Add("mb", "บ");
            letter_dict.Add("n", "น");
            letter_dict.Add("nd", "ด");
            letter_dict.Add("ng", "ง");
            letter_dict.Add("nj", "ฌ");
            letter_dict.Add("nq", "ฆ");
            letter_dict.Add("ny", "ญ");
            letter_dict.Add("nz", "ฑ");
            letter_dict.Add("p", "พ"); //letter_dict.Add("p", "ผ");

            letter_dict.Add("q", "ช"); //letter_dict.Add("q", "ฉ");

            letter_dict.Add("s", "ซ"); //letter_dict.Add("s", "ส");

            letter_dict.Add("t", "ท"); //letter_dict.Add("t", "ถ");

            letter_dict.Add("w", "ว");
            letter_dict.Add("y", "ย");
            letter_dict.Add("z", "ฒ");
            string debuglog = "";
            //string[] words = input.Split(' ');


            string[] words = Regex.Split(input, @"([.,;?\[\{\(\)\}\]\s])");

            string stringtoreturn = "";
            //loop over result array.
            foreach (string word in words)
            {
                string word_out_temp = word;

                string scriptInput = "newromanscript";
                string scriptToFind = "thaiscript";
                
                string word_from_database_search = searchInDataBase(word, scriptInput, scriptToFind);
                if (word_from_database_search != "error-WORDnotFOUND") 
                {
                    stringtoreturn = stringtoreturn + " " + word_from_database_search;
                    continue;
                }

                // try with all lower case
                string lowerInput = word.ToLower();
                word_from_database_search = searchInDataBase(lowerInput, scriptInput, scriptToFind);
                if (word_from_database_search != "error-WORDnotFOUND")
                {
                    stringtoreturn = stringtoreturn + " " + word_from_database_search;
                    continue;
                }
                
                string tonemark = null;
                // this is the logic for checking the tone mark of the word
                if (word_out_temp.Length > 2)
                {
                    //get last char
                    char last_char = word_out_temp[word_out_temp.Length - 1];
                    string last_string_char = last_char.ToString();
                    if (tonemarks_dict.ContainsKey(last_string_char))
                    {
                        string value = tonemarks_dict[last_string_char];
                        tonemark = value;
                        //remove the tonemark
                        word_out_temp= word_out_temp.Remove(word_out_temp.Length - 1);
                    }

                }
                debuglog = " tone mark=>"+tonemark;
              
                string StringWithExtendedVowelReplaced = word_out_temp;
                foreach (KeyValuePair<string, string> entry in vowelplusconsonant_dict)
                {
                    string vowelplusconsonant = entry.Value;

                    if (word.Contains(entry.Key))
                    {
                        // set the tonemark first
                        if (tonemark != null)
                        {
                            if (vowelplusconsonant.Contains("อ"))
                            {
                                vowelplusconsonant = ReplaceFirstOccurrence(vowelplusconsonant, "อ", tonemark);
                            }
                        }
                        StringWithExtendedVowelReplaced = word_out_temp.Replace(entry.Key, vowelplusconsonant);
                    }
                }
                word_out_temp = StringWithExtendedVowelReplaced;
                Console.WriteLine(word_out_temp);
                debuglog = debuglog + ":StringWithExtendedVowelReplaced:" + word_out_temp;
                string StringWithVowelReplaced = word_out_temp;
                foreach (KeyValuePair<string, string> entry in basicvowel_dict)
                {
                    string vowelplusconsonant = entry.Value;


                    if (word.Contains(entry.Key))
                    {                    
                        // set the tonemark first
                        if (tonemark != null)
                        {
                            if (vowelplusconsonant.Contains("อ"))
                            {
                                vowelplusconsonant = ReplaceFirstOccurrence(vowelplusconsonant, "อ", tonemark);
                            }
                        }
                        StringWithVowelReplaced = word_out_temp.Replace(entry.Key, vowelplusconsonant);
                    }
                }

                word_out_temp = StringWithVowelReplaced;
                debuglog = debuglog + ":StringWithVowelReplaced:" + word_out_temp;

                foreach (KeyValuePair<string, string> entry in letter_dict)
                {
                    int indexofletter = word_out_temp.IndexOf(entry.Key);
                    if (indexofletter > -1)
                    {
                        string[] thaivowel;
                        thaivowel = new string[] { "ะ", "า", "ิ", "ี","ึ","ื","ุ","ู","เ","แ","ั","ว","ำ","ใ","ไ","ๅ" };
                        if (word_out_temp.Contains("อ"))
                        {
                            // if it is อ and a vowel then replace อ with the letter
                            if (ContainsAny(word_out_temp, thaivowel))
                            {
                                word_out_temp = ReplaceFirstOccurrenceChar(word_out_temp, 'อ', entry.Value);
                                
                                // replace roman letter with empty

                                word_out_temp = word_out_temp.Replace(entry.Key, "");
                            }

                            // if อ is the vowel then keep the อ replace roman letter with thai
                            else                             

                            {
                                word_out_temp = word_out_temp.Replace(entry.Key, entry.Value);

                            }
                        }

                        // if there is no อ then replace roman letter with thai

                        else
                        {
                            word_out_temp= word_out_temp.Replace(entry.Key, entry.Value);
                        }
                    }

                }
                debuglog = debuglog + ":letter replaced:" + word_out_temp;

                stringtoreturn = stringtoreturn + word_out_temp;
            }

            Console.WriteLine(stringtoreturn);
            
            return (stringtoreturn);
        }

        public static string AnyToAnyScript(string inputsearchstring , string inputscript, string outputscript) 
        {
            string stringtoreturn = "";
            string[] words = Regex.Split(inputsearchstring, @"([.\[\{\(\)\}\],;?\s])");
          
            
            foreach (string word in words)
            {
                string word_out_temp = word;

                string scriptInput = inputscript;
                string scriptToFind = outputscript;

                string word_from_database_search = searchInDataBase(word, scriptInput, scriptToFind);
                if (word_from_database_search != "error-WORDnotFOUND")
                {
                    stringtoreturn = stringtoreturn + " " + word_from_database_search;
                    continue;
                }

                //try searching without capitalization
                if (scriptInput == "newromanscript" | scriptInput == "oldromanscript")
                {
                    string lowerInput = word.ToLower();
                    word_from_database_search = searchInDataBase(lowerInput, scriptInput, scriptToFind);
                    if (word_from_database_search != "error-WORDnotFOUND")
                    {
                        stringtoreturn = stringtoreturn + " " + word_from_database_search;
                        continue;
                    }
                }
                //if still not found then just return
                stringtoreturn = stringtoreturn + word_out_temp;
            }
            return stringtoreturn;
        }
        public static string searchInDataBase(string wordToFind, string scriptInput,string scriptToFind) 
        {

            if (wordToFind == "") 
            {
                return "error-WORDnotFOUND";
            }
            if (wordToFind == " ") 
            {
                return "error-WORDnotFOUND";
            }
            XmlDocument doc = new XmlDocument();

                string path1 = System.Web.HttpContext.Current.Server.MapPath("/App_Data/MienWordsMatches/ThaiToNewRoman.xml");
                doc.Load(path1);



                XmlNodeList xnList = doc.SelectNodes("/ThaiToNewRoman/word");
                foreach (XmlNode xn in xnList)
                {
                    string INPUTscriptWORD = xn[scriptInput].InnerText;
                    if (INPUTscriptWORD == wordToFind)
                    {
                        string OUTPUTscriptWORD = xn[scriptToFind].InnerText;
                        if (OUTPUTscriptWORD != "")
                        {
                            return (OUTPUTscriptWORD);
                        }
                    }
                }

            

            return "error-WORDnotFOUND";
        }
    }



}