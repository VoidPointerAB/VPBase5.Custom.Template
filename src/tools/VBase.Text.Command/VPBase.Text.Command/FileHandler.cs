using System.Net.Http.Headers;

namespace VPBase.Text.Command
{
    public interface IFileHandler
    {
        IEnumerable<string> ReadFileToLines(string filePath);

        void WriteLinesToFile(string filePath, IEnumerable<string> lines);
    }

    public class FileHandler : IFileHandler
    {
        public IEnumerable<string> ReadFileToLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public void WriteLinesToFile(string filePath, IEnumerable<string> lines)
        {
            File.WriteAllLines(filePath, lines);
        }
    }

    public class DummyFileHandler : IFileHandler
    {
        public Dictionary<string, List<string>> SourceLines = new Dictionary<string, List<string>>();

        public List<string> DestinationLines = new List<string>();

        public void AddSourceLines(string filePath, List<string> lines)
        {
            SourceLines.Add(filePath, lines);
        }

        public IEnumerable<string> ReadFileToLines(string filePath)
        {
            var lines = new List<string>();
            if (SourceLines.TryGetValue(filePath, out lines))
            {
                return lines;
            }

            throw new Exception($"Source lines not found with filepaht: {filePath}");
        }

        public void WriteLinesToFile(string filePath, IEnumerable<string> lines)
        {
            DestinationLines.Clear();
            DestinationLines.AddRange(lines);
        }
    }
}
