using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Augur.BackendToFrontendExporter.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            // Split the string into words.
            string[] words = str.ToSentenceCase().Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        public static string ToPascalCase(this string str)
        {
            if (str.Length < 2) return str.ToUpper();

            // Split the string into words.
            string[] words = str.ToSentenceCase().Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }

        public static string ToSentenceCase(this string str)
        {
            if (str.Length == 0)
            {
                return str;
            }

            if (str.Contains("-"))
            {
                return str.SplitBy("-").JoinBy(" ");
            }
            else if (char.IsUpper(str.First()))
            {
                return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            }
            else if (char.IsLower(str.First()))
            {
                string output = Regex.Replace(str, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());
                return char.ToUpperInvariant(output[0]) + output.Substring(1);
            }

            return str;
        }

        public static string ToDashCase(this string str)
        {
            return str
                .ToSentenceCase()
                .SplitBy(" ")
                .JoinBy("-")
                .ToLowerInvariant();
        }

        public static string ReplaceInAllCases(this string str, string fromName, string toName)
        {
            return str
                .Replace(fromName, toName)
                .Replace(fromName.ToCamelCase(), toName.ToCamelCase())
                .Replace(fromName.ToDashCase(), toName.ToDashCase());
        }

        public static string Reverse(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string ProcessTemplate(this string str)
        {
            var lines = str.SplitBy("\r\n")
                .DiscardFirstIfWhitespace()
                .DiscardLastIfWhitespace();


            return MergeLinesWithoutWhitespacePadding(lines);
        }

        private static string MergeLinesWithoutWhitespacePadding(IEnumerable<string> lines)
        {
            var minWhitespaceCnt = -1;

            foreach (var line in lines)
            {
                var cnt = 0;

                foreach (var chr in line)
                {
                    if (Regex.IsMatch(chr.ToString(), @"\s"))
                    {
                        cnt++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (cnt < minWhitespaceCnt || minWhitespaceCnt == -1)
                {
                    minWhitespaceCnt = cnt;
                }
            }

            return lines
                .Select(line => line.Substring(minWhitespaceCnt, line.Length - minWhitespaceCnt))
                .JoinBy("\r\n");
        }

        public static IEnumerable<string> SplitBy(this string str, string by)
        {
            return str.Split(new string[] {by}, StringSplitOptions.None);
        }

        public static string JoinBy(this IEnumerable<string> strs, string by)
        {
            return string.Join(by, strs);
        }

        public static string JoinLinesBy(this string str, string by)
        {
            return str
                .SplitBy("\r\n")
                .JoinBy("\r\n" + by);
        }

        public static IEnumerable<string> Trim(this IEnumerable<string> strs, string by)
        {
            return strs.Select(str => str.Trim());
        }

        public static IEnumerable<string> DiscardFirstIfWhitespace(this IEnumerable<string> strs)
        {
            var isFirst = true;
            foreach (var str in strs)
            {
                if (!isFirst || str.Any(c => c != '\r' && c != '\n' && c != '\t' && c != ' '))
                {
                    yield return str;
                }

                isFirst = false;
            }
        }

        public static IEnumerable<string> DiscardLastIfWhitespace(this IEnumerable<string> strs)
        {
            string last = null;

            foreach (var str in strs)
            {
                if (last != null)
                {
                    yield return last;
                }

                last = str;
            }

            if (last.Any(c => c != '\r' && c != '\n' && c != '\t' && c != ' '))
            {
                yield return last;
            }
        }

        public static string GetBetween(this string str, string start, string end)
        {
            int pFrom = str.IndexOf(start) + start.Length;
            int pTo = str.LastIndexOf(end);

            return str.Substring(pFrom, pTo - pFrom);
        }
    }
}