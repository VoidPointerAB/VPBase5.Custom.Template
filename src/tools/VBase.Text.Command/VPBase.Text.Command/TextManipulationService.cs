using System.Text;

namespace VPBase.Text.Command
{
    public interface ITextManipulationService
    {
        List<string> ReplaceSectionLines(IEnumerable<string> sourceLines, string searchStartLineText, string searchEndLineText, IEnumerable<string> replacementLines);

        List<string> ReplaceSectionLines(IEnumerable<string> sourceLines, string searchStartLineText, string searchEndLineText, string replacementText);

        List<string> ReplaceSectionLines(ReplaceSectionLineArguments replaceSectionLineArguments);

        string BuildStringFromList(IEnumerable<string> lines);

        List<string> BuildListFromText(string text);
    }

    public class TextManipulationService : ITextManipulationService
    {
        public string BuildStringFromList(IEnumerable<string> lines)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string line in lines)
            {
                sb.AppendLine(line); 
            }

            return sb.ToString();
        }

        public List<string> BuildListFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<string>();
            }

            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lines.ToList();
        }

        public List<string> ReplaceSectionLines(IEnumerable<string> sourceLines, string searchStartLineText, string searchEndLineText, string replacementText)
        {
            var replacementLines  = BuildListFromText(replacementText);

            return ReplaceSectionLines(sourceLines, searchStartLineText, searchEndLineText, replacementLines);
        }

        public List<string> ReplaceSectionLines(IEnumerable<string> sourceLines, string searchStartLineText, string searchEndLineText, IEnumerable<string> replacementLines)
        {
            var args = new ReplaceSectionLineArguments()
            {
                SourceLines = sourceLines.ToList(), 
                SearchStartLineText = searchStartLineText, 
                SearchEndLineText = searchEndLineText,
                ReplacementLines = replacementLines.ToList()
            };

            return ReplaceSectionLines(args);
        }

        public List<string> ReplaceSectionLines(ReplaceSectionLineArguments replaceSectionLineArguments)
        {
            var destinationLines = new List<string>();

            var numOfReplacements = 0;

            var addLine = true;

            Console.WriteLine($"Started section replacement...");

            foreach (var sourceLine in replaceSectionLineArguments.SourceLines)
            {
                if (sourceLine.Contains(replaceSectionLineArguments.SearchStartLineText))
                {
                    Console.WriteLine($"Found search start line text: '{replaceSectionLineArguments.SearchStartLineText}'!");

                    addLine = false;
                    destinationLines.Add(sourceLine);
                    destinationLines.AddRange(replaceSectionLineArguments.ReplacementLines);

                    numOfReplacements++;
                }

                if (sourceLine.Contains(replaceSectionLineArguments.SearchEndLineText))
                {
                    Console.WriteLine($"Found search end line text: '{replaceSectionLineArguments.SearchEndLineText}'!");

                    addLine = true;
                }

                if (addLine)
                {
                    destinationLines.Add(sourceLine);
                }
            }

            if (numOfReplacements > 0)
            {
                Console.WriteLine($"Number of replacements: {numOfReplacements}");
            }
            else
            {
                Console.WriteLine($"No replacements found! Check search start and end text and source text!");
            }
            

            return destinationLines;
        }
    }

    public class ReplaceSectionLineArguments
    {
        public ReplaceSectionLineArguments()
        {
            SourceLines = new List<string>();
            ReplacementLines = new List<string>();
            SearchStartLineText = string.Empty;
            SearchEndLineText = string.Empty;
        }

        public List<string> SourceLines { get; set; }

        public string SearchStartLineText { get; set; }

        public string SearchEndLineText { get; set; }

        public List<string> ReplacementLines { get; set; }
    }
}
