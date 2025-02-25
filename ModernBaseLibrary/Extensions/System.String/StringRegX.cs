namespace ModernBaseLibrary.Extension
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class StringExtension
    {
        /// <summary>
        /// Does the input string contain a regex pattern match, and if so, return the parts before and after the (first) match.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <param name="textBeforeMatch"></param>
        /// <param name="textAfterMatch"></param>
        /// <returns></returns>
        public static bool RegexSplitTextExcludingMatch(this string input, string regexPattern, out string textBeforeMatch, out string textAfterMatch)
        {
            return RegexSplitTextExcludingMatch(input, regexPattern, false, out textBeforeMatch, out textAfterMatch);
        }

        /// <summary>
        /// Does the input string contain a regex pattern match, and if so, return the parts before and after the (first) match.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="textBeforeMatch"></param>
        /// <param name="textAfterMatch"></param>
        /// <returns></returns>
        public static bool RegexSplitTextExcludingMatch(this string input, string regexPattern, bool caseSensitive, out string textBeforeMatch, out string textAfterMatch)
        {
            textBeforeMatch = string.Empty;
            textAfterMatch = string.Empty;

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regexPattern))
            {
                return false;
            }

            var options = caseSensitive
                ? RegexOptions.None
                : RegexOptions.IgnoreCase;

            var regex = new Regex(regexPattern, options);
            var splits = regex.Split(input, 2); // split text into 2 parts, before and after the pattern

            if (splits.Length == 1) // i.e. no match found for pattern
            {
                return false;
            }

            textBeforeMatch = splits[0];
            textAfterMatch = splits[1];
            return true;
        }

        /// <summary>
        /// Does the input string contain a regex pattern match, and if so, return the parts before and after the (first) match.
        /// Ignore the first skipCount occurrences of the match though.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <param name="skipCount"></param>
        /// <param name="textBeforeMatch"></param>
        /// <param name="textAfterMatch"></param>
        /// <returns></returns>
        public static bool RegexSplitTextExcludingMatchWithSkip(this string input, string regexPattern, int skipCount, out string textBeforeMatch, out string textAfterMatch)
        {
            return RegexSplitTextExcludingMatchWithSkip(input, regexPattern, skipCount, false, out textBeforeMatch, out textAfterMatch);
        }

        /// <summary>
        /// Does the input string contain a regex pattern match, and if so, return the parts before and after the (first) match.
        /// Ignore the first skipCount occurrences of the match though.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <param name="skipCount"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="textBeforeMatch"></param>
        /// <param name="textAfterMatch"></param>
        /// <returns></returns>
        public static bool RegexSplitTextExcludingMatchWithSkip(this string input, string regexPattern, int skipCount, bool caseSensitive, out string textBeforeMatch, out string textAfterMatch)
        {
            textBeforeMatch = string.Empty;
            textAfterMatch = string.Empty;

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regexPattern))
            {
                return false;
            }

            if (skipCount == 0)
            {
                return RegexSplitTextExcludingMatch(input, regexPattern, caseSensitive, out textBeforeMatch, out textAfterMatch);
            }

            var options = caseSensitive
                              ? RegexOptions.None
                              : RegexOptions.IgnoreCase;

            // The "()" around the pattern means include the actual matched text in the splits array.
            // We do an "Inclusive" match here, as we want to include the skipped matches in the before string.
            var regex = new Regex("(" + regexPattern + ")", options);

            var splits = regex.Split(input, 2 + skipCount); // split text into as many parts as is required to cover for the skipped items

            if (splits.Length < 2 + (skipCount * 2)) // i.e. no match found for pattern (after accounting for skips)
            {
                return false;
            }

            // Include any skipped matches in the before string.
            // The actual match we want is discarded.
            textBeforeMatch = string.Join(string.Empty, splits.Take(1 + (skipCount * 2)));
            textAfterMatch = splits.Last();
            return true;
        }

        /// <summary>
        /// Does the input string contain a regex pattern match, and if so, return the parts before and after the (first) match plus the actual matched string 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <param name="textBeforeMatch"></param>
        /// <param name="matchedText"></param>
        /// <param name="textAfterMatch"></param>
        /// <returns></returns>
        public static bool RegexSplitTextIncludingMatch(this string input, string regexPattern, out string textBeforeMatch, out string matchedText, out string textAfterMatch)
        {
            return RegexSplitTextIncludingMatch(input, regexPattern, false, out textBeforeMatch, out matchedText, out textAfterMatch);
        }

        /// <summary>
        /// Does the input string contain a regex pattern match, and if so, return the parts before and after the (first) match plus the actual matched string 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regexPattern"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="textBeforeMatch"></param>
        /// <param name="matchedText"></param>
        /// <param name="textAfterMatch"></param>
        /// <returns></returns>
        public static bool RegexSplitTextIncludingMatch(this string input, string regexPattern, bool caseSensitive, out string textBeforeMatch, out string matchedText, out string textAfterMatch)
        {
            textBeforeMatch = string.Empty;
            matchedText = string.Empty;
            textAfterMatch = string.Empty;

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regexPattern))
            {
                return false;
            }

            var options = caseSensitive
                ? RegexOptions.None
                : RegexOptions.IgnoreCase;

            // the "()" around the pattern means include the actual matched text in the splits array.
            var regex = new Regex("(" + regexPattern + ")", options);
            var splits = regex.Split(input, 2); // split text into 2 parts, before and after the pattern

            if (splits.Length < 3) // i.e. no match found for pattern
            {
                return false;
            }

            textBeforeMatch = splits[0];
            matchedText = splits[1];
            textAfterMatch = splits[2];

            return true;
        }

        // count how many times a Regex pattern is matched within the input string
        public static int CountRegexMatches(this string input, string regexPattern)
        {
            return CountRegexMatches(input, regexPattern, false);
        }

        public static int CountRegexMatches(this string input, string regexPattern, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regexPattern))
            {
                return 0;
            }

            var options = caseSensitive
                              ? RegexOptions.None
                              : RegexOptions.IgnoreCase;

            // the "()" around the pattern means include the actual matched text in the splits array.
            var regex = new Regex("(" + regexPattern + ")", options);
            var splits = regex.Split(input, int.MaxValue); // find as many matches as possible

            return splits.Length < 3 // i.e. no match found for pattern
                       ? 0
                       : (splits.Length - 1) / 2;
        }
    }
}
