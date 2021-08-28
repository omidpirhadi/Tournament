using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PersianFix : MonoBehaviour
{
    public class Persian
    {



        public static String init = "ﺍﺑﺗﺛﺟﺣﺧدذرزﺳﺷﺻﺿﻃﻇﻋﻏﻓﻗﻛﻟﻣﻧﻫﻭﻳﻳﺁﺃﺇﺅﺀﺓﺋﻻﻵﻷﻹﮔﭘﮊﭼ";// avali -2
        public static String mid = "ﺎﺒﺘﺜﺠﺤﺨﺪﺬﺮﺰﺴﺸﺼﻀﻄﻈﻌﻐﻔﻘﮑﻠﻤﻨﻬﻮﻴﻴﺂﺄﺈﺆﺀﺔﺌﻼﻶﻸﻺﮕﭙﮋﭽ";// avali - 1
        public static String last = "ﺎﺐﺖﺚﺞﺢﺦﺪﺬﺮﺰﺲﺶﺺﺾﻂﻆﻊﻎﻒﻖﮏﻞﻢﻦﻪﻮﻲﻰﺂﺄﺈﺆﺀﺔﺊﻼﻶﻸﻺﮓﭗﮋﭻ";//akhari - 1
        public static String alefs = "اآأإ";
        public static String las = "ﻻﻵﻷﻹ";

        public static String separators = " )(:;,!?؟،.\'\"";

        public static string Fix(string input, int Plength)
        {

            
            String result = "";
		
            for (int i = input.Length - 1; i >= 0; i--)
            {
                result += input[i];
            }

            LinkedList<char> seps = new LinkedList<char>();
            for (int i = 0; i < result.Length; i++)
            {
                if (separators.Contains(result[i]))
                    seps.AddLast(result[i]);

            }

            String[] words = result.Split(separators.ToCharArray());
            result = "";
            for (int i = 0; i < words.Length; i++)
            {
                String word = words[i];
                for (int j = 0; j < alefs.Length; j++)
                {
                    String rep1 = alefs[j] + "ل";
                    word = word.Replace(rep1, las[j] + "");
                }
                String newWord = "";

                if (word.Length > 1 && IsPureArabic(word))
                {
                    for (int j = 0; j < word.Length; j++)
                    {
                        if (j == 0)
                        {
                            if (IsCut(word[j + 1]))
                            {
                                newWord += word[j];
                            }
                            else
                            {
                                newWord += last[GetIndex(word[j])];
                            }
                        }
                        else if (j == word.Length - 1)
                        {
                            newWord += init[GetIndex(word[j])];
                        }
                        else
                        {
                            if (IsCut(word[j + 1]))
                            {
                                newWord += init[GetIndex(word[j])];
                            }
                            else
                            {
                                newWord += mid[GetIndex(word[j])];
                            }
                        }
                    }
                }
                else
                {
                    for (int x = word.Length - 1; x >= 0; x--)
                    {
                        newWord += word[x];
                    }
                }


                result = newWord + (i < seps.Count ? seps.ElementAt(i) + "" : " ") + result;

            }


            return ResolveTextSize(result, Plength);

        }




        static string ResolveTextSize(string input, int lineLength)
        {

            // Split string by char " "
            string[] words = input.Split(" "[0]);

            // Prepare result
            string result = "";

            // Temp line string
            string line = "";

            // for each all words
            foreach (string s in words)
            {
                // Append current word into line
                string temp = s + " " + line;

                // If line length is bigger than lineLength
                if (temp.Length > lineLength)
                {

                    // Append current line into result
                    result += line + "\n";
                    // Remain word append into new line
                    line = s;
                }
                // Append current word into current line
                else
                {
                    line = temp;
                }
            }

            // Append last line into result
            result += line;

            // Remove first " " char
            return result;
        }




        private static bool IsCut(char c)
        {
            int x = GetIndex(c);
            return x == 0 ||
                (x >= 7 && x <= 10) ||
                 x == 26 ||
                (x >= 29 && x <= 34) ||
                (x >= 36 && x <= 39) ||
                (x == 42);
        }

        private static bool IsPureArabic(String word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (GetIndex(word[i]) == -1)
                {
                    return false;
                }
            }
            return true;
        }

        private static int GetIndex(char c)
        {
            switch (c)
            {
                case 'ا': return 0;
                case 'ب': return 1;
                case 'ت': return 2;
                case 'ث': return 3;
                case 'ج': return 4;
                case 'ح': return 5;
                case 'خ': return 6;
                case 'د': return 7;
                case 'ذ': return 8;
                case 'ر': return 9;
                case 'ز': return 10;
                case 'س': return 11;
                case 'ش': return 12;
                case 'ص': return 13;
                case 'ض': return 14;
                case 'ط': return 15;
                case 'ظ': return 16;
                case 'ع': return 17;
                case 'غ': return 18;
                case 'ف': return 19;
                case 'ق': return 20;
                case 'ک': return 21;
                case 'ل': return 22;
                case 'م': return 23;
                case 'ن': return 24;
                case 'ه': return 25;
                case 'و': return 26;
                case 'ي': return 27;
                case 'ی': return 28;
                case 'آ': return 29;
                case 'أ': return 30;
                case 'إ': return 31;
                case 'ؤ': return 32;
                case 'ء': return 33;
                case 'ة': return 34;
                case 'ئ': return 35;
                case 'ﻻ': return 36;
                case 'ﻵ': return 37;
                case 'ﻷ': return 38;
                case 'ﻹ': return 39;
                case 'گ': return 40;
                case 'پ': return 41;
                case 'ژ': return 42;
                case 'چ': return 43;
            }
            return -1;
        }





    }





    // Wrap text by line height

}