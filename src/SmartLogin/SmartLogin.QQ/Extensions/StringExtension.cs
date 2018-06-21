using System.Text.RegularExpressions;

namespace SmartLogin.QQ
{
    public static class QQExtension
    {
        public static bool IsMatch(this string source, string pattern, RegexOptions options = RegexOptions.IgnoreCase)
        {
            Regex regex = new Regex(pattern, options);
            return regex.IsMatch(source);
        }
    }
}
