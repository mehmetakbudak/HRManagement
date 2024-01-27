using System.Text.RegularExpressions;

namespace Chinook.Service.Helpers
{
    public static class UrlHelper
    {
        public static string FriendlyUrl(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            text = text.ToLower();
            text = text.Trim();
            if (text.Length > 100)
            {
                text = text.Substring(0, 100);
            }
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ç", "c");
            text = text.Replace("Ç", "C");
            text = text.Replace("ö", "o");
            text = text.Replace("Ö", "O");
            text = text.Replace("ş", "s");
            text = text.Replace("Ş", "S");
            text = text.Replace("ü", "u");
            text = text.Replace("Ü", "U");
            text = text.Replace("'", "");
            text = text.Replace("\"", "");
            char[] replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
            for (int i = 0; i < replacerList.Length; i++)
            {
                string strChr = replacerList[i].ToString();
                if (text.Contains(strChr))
                {
                    text = text.Replace(strChr, string.Empty);
                }
            }
            Regex r = new Regex("[^a-zA-Z0-9_-]");
            text = r.Replace(text, "-");
            while (text.IndexOf("--") > -1)
                text = text.Replace("--", "-");
            return text;
        }
    }

}
