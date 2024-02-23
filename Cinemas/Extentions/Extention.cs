using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cinemas.Extentions
{
    public static class Extention
    {   
        public static string ToVnd(this double donGia)
        {
            return donGia.ToString("#.##0") + "VNĐ";
        }
        public static string ToTittleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for(int i = 0; i < words.Length; i++)
                {
                    var s = words[i];
                    if(s.Length > 0) {
                        words[i] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static string ToUrlFriendly(this string url)
        {
            var result = url.ToLower().Trim();
            result = Regex.Replace(result, "áàạảãâấầậẩẫ", "a");
            return result;
        }
    }
}
