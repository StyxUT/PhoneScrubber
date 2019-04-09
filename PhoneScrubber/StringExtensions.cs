using System.Linq;

namespace PhoneScrubber
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string haystack, params string[] needles)
        {

            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }
    }
}