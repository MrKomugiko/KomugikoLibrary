using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomugikoLibrary
{
    public static class NumbersConverter
    {
        // angielski konwerter
        public static string NumberToWords_ENG(int number)
        {
            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords_ENG(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords_ENG(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords_ENG(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }


        // ukrainski konwerter
        public static string GrnPhrase_UA(decimal money)
        {
            return CurPhrase(money, "гривня", "гривні", "гривень", "копійка", "копійки", "копійок");
        }
        public static string NumPhrase(ulong Value, bool IsMale)
        {
            if (Value == 0UL) return "Нуль";
            string[] Dek1 = { "", " од", " дв", " три", " чотири", " п'ять", " шість", " сім", " вісім", " дев'ять", " десять", " одинадцять", " дванадцять", " тринадцять", " чотирнадцять", " п'ятнадцять", " шістнадцять", " сімнадцять", " вісімнадцять", " дев'ятнадцять" };
            string[] Dek2 = { "", "", " двадцять", " тридцять", " сорок", " п'ятдесят", " шістдесят", " сімдесят", " вісімдесят", " дев'яносто" };
            string[] Dek3 = { "", " сто", " двісті", " триста", " чотириста", " п'ятсот", " шістсот", " сімсот", " вісімсот", " дев'ятсот" };
            string[] Th = { "", "", " тисяч", " мільйон", " міліард", " триліон", " квадриліон", " квинтиліон" };
            string str = "";
            for (byte th = 1; Value > 0; th++)
            {
                ushort gr = (ushort)(Value % 1000);
                Value = (Value - gr) / 1000;
                if (gr > 0)
                {
                    byte d3 = (byte)((gr - gr % 100) / 100);
                    byte d1 = (byte)(gr % 10);
                    byte d2 = (byte)((gr - d3 * 100 - d1) / 10);
                    if (d2 == 1) d1 += (byte)10;
                    bool ismale = (th > 2) || ((th == 1) && IsMale);
                    str = Dek3[d3] + Dek2[d2] + Dek1[d1] + EndDek1(d1, ismale) + Th[th] + EndTh(th, d1) + str;
                };
            };
            str = str.Substring(1, 1).ToUpper() + str.Substring(2);
            return str;
        }

        #region Private members
        private static string CurPhrase(decimal money,
            string word1, string word234, string wordmore,
            string sword1, string sword234, string swordmore)
        {
            //money=decimal.Round(money,2);
            decimal decintpart = decimal.Truncate(money);
            ulong intpart = decimal.ToUInt64(decintpart);
            string str = NumPhrase(intpart, true);
            return str;

        }
        private static string EndTh(byte ThNum, byte Dek)
        {
            bool In234 = ((Dek >= 2) && (Dek <= 4));
            bool More4 = ((Dek > 4) || (Dek == 0));
            if (((ThNum > 2) && In234) || ((ThNum == 2) && (Dek == 1))) return "і";
            else if ((ThNum > 2) && More4) return "ів";
            else if ((ThNum == 2) && In234) return "і";
            else return "";
        }
        private static string EndDek1(byte Dek, bool IsMale)
        {
            if ((Dek > 2) || (Dek == 0)) return "";
            else if (Dek == 1)
            {
                if (IsMale) return "ин";
                else return "на";
            }
            else
            {
                if (IsMale) return "а";
                else return "а";
            }
        }
        #endregion


        //konwerter rosyjski
        public static string GrnPhrase_RU(int number)
        {

            int[] array_int = new int[4];
            string[,] array_string = new string[4, 3] {{" миллиард", " миллиарда", " миллиардов"},
                {" миллион", " миллиона", " миллионов"},
                {" тысяча", " тысячи", " тысяч"},
                {"", "", ""}};
            array_int[0] = (number - (number % 1000000000)) / 1000000000;
            array_int[1] = ((number % 1000000000) - (number % 1000000)) / 1000000;
            array_int[2] = ((number % 1000000) - (number % 1000)) / 1000;
            array_int[3] = number % 1000;
            int nas = 0;
            string result = "";
            for (int i = 0; i < 4; i++)
            {
                if (array_int[i] != 0)
                {
                    if (((array_int[i] - (array_int[i] % 100)) / 100) != 0)
                        switch (((array_int[i] - (array_int[i] % 100)) / 100))
                        {
                            case 1: result += " сто"; break;
                            case 2: result += " двести"; break;
                            case 3: result += " триста"; break;
                            case 4: result += " четыреста"; break;
                            case 5: result += " пятьсот"; break;
                            case 6: result += " шестьсот"; break;
                            case 7: result += " семьсот"; break;
                            case 8: result += " восемьсот"; break;
                            case 9: result += " девятьсот"; break;
                        }
                    if (((array_int[i] % 100) - ((array_int[i] % 100) % 10)) / 10 != 1)
                    {
                        switch (((array_int[i] % 100) - ((array_int[i] % 100) % 10)) / 10)
                        {
                            case 2: result += " двадцать"; break;
                            case 3: result += " тридцать"; break;
                            case 4: result += " сорок"; break;
                            case 5: result += " пятьдесят"; break;
                            case 6: result += " шестьдесят"; break;
                            case 7: result += " семьдесят"; break;
                            case 8: result += " восемьдесят"; break;
                            case 9: result += " девяносто"; break;
                        }
                    }
                    switch (array_int[i] % 100)
                    {
                        case 10: result += " десять"; nas = 1; break;
                        case 11: result += " одиннадцать"; nas = 1; break;
                        case 12: result += " двенадцать"; nas = 1; break;
                        case 13: result += " тринадцать"; nas = 1; break;
                        case 14: result += " четырнадцать"; nas = 1; break;
                        case 15: result += " пятнадцать"; nas = 1; break;
                        case 16: result += " шестнадцать"; nas = 1; break;
                        case 17: result += " семнадцать"; nas = 1; break;
                        case 18: result += " восемннадцать"; nas = 1; break;
                        case 19: result += " девятнадцать"; nas = 1; break;
                    }
                    if (nas == 0)
                    {
                        switch (array_int[i] % 10)
                        {

                            case 1: if (i == 2) result += " одна"; else result += " один"; break;
                            case 2: if (i == 2) result += " две"; else result += " два"; break;
                            case 3: result += " три"; break;
                            case 4: result += " четыре"; break;
                            case 5: result += " пять"; break;
                            case 6: result += " шесть"; break;
                            case 7: result += " семь"; break;
                            case 8: result += " восемь"; break;
                            case 9: result += " девять"; break;
                        }
                    }

                }

                if (array_int[i] % 100 >= 10 && array_int[i] % 100 <= 19) result += " " + array_string[i, 2] + " ";
                else switch (array_int[i] % 10)
                    {
                        case 1: result += " " + array_string[i, 0] + " "; break;
                        case 2:
                        case 3:
                        case 4: result += " " + array_string[i, 1] + " "; break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9: result += " " + array_string[i, 2] + " "; break;
                    }
            }
            return result;
        }

    }

}

