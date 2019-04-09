using System.IO;

namespace PhoneScrubber
{
    class FileLoader
    {

        public string[] LoadFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            return lines;
        }
    }
}
