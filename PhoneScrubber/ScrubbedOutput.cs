
namespace PhoneScrubber
{
    public class ScrubbedOutput
    {
        public string CaseSafeID { get; set; }
        public string OriginalPhone { get; set; }
        public string ScrubbedPhone { get; set; }

        public ScrubbedOutput(string caseSafeId, string originalPhone, string scrubbedPhone)
        {
            CaseSafeID = caseSafeId;
            OriginalPhone = originalPhone;
            ScrubbedPhone = scrubbedPhone;
        }

    }

}
