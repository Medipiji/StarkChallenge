using System.Text.RegularExpressions;

namespace StarkChallenge.Utils
{
    public static class DataUtil
    {
        public static bool ValidateSpecialChar(this string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z0-9_\-]+$");
        }
        public static bool ValidateIntNegativeNumber(this int value)
        {
            return value < 0;
        }
        public static bool ValidateLongNegativeNumber(this long value)
        {
            return value < 0;
        }

    }
}
