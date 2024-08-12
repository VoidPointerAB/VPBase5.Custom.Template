namespace VPBase.Text.Command
{
    public class ProcessArgumentManager
    {
        private IFileHandler _fileHandler;
        private ITextManipulationService _textManipulationService;

        // Lower
        public const string Command_ReplaceSection = "-replacesection";

        public const string Command_ReplaceSectionFile = "-replacesectionfile";

        public const string Command_Help = "-help";
        public const string Command_Help_Short = "-h";

        public ProcessArgumentManager(IFileHandler fileHandler,
            ITextManipulationService textManipulationService)
        {
            _fileHandler = fileHandler;
            _textManipulationService = textManipulationService;
        }

        public void Execute(string[] parameters)
        {
            var commandFound = false;

            for(int i = 0; i < parameters.Count(); ++i)
            {
                var parameter = parameters[i].Trim().ToLower();

                if (parameter.Equals(Command_ReplaceSection))
                {
                    var filePath = TryGetParameter(parameters, i + 1);
                    var searchStartLineText = TryGetParameter(parameters, i + 2);
                    var searchEndLineText = TryGetParameter(parameters, i + 3);
                    var replacementText = TryGetParameter(parameters, i + 4);

                    ReplaceSectionLines(filePath, searchStartLineText, searchEndLineText, replacementText, string.Empty);

                    commandFound = true;
                }
                if (parameter.Equals(Command_ReplaceSectionFile))
                {
                    var filePath = TryGetParameter(parameters, i + 1);
                    var searchStartLineText = TryGetParameter(parameters, i + 2);
                    var searchEndLineText = TryGetParameter(parameters, i + 3);
                    var replacementFilePath = TryGetParameter(parameters, i + 4);

                    ReplaceSectionLines(filePath, searchStartLineText, searchEndLineText, string.Empty, replacementFilePath);

                    commandFound = true;
                }
                else if (parameter.Equals(Command_Help) || parameter.Equals(Command_Help_Short))
                {
                    ShowHelp();

                    commandFound = true;
                }
            }

            if (parameters.Length == 0)
            {
                Console.WriteLine("No command arguments found! Run help using the -help or -h command!");
            }
            else
            {
                if (!commandFound)
                {
                    Console.WriteLine("No command found but arguments found! Run help using the -help or -h command!");
                }
            } 
        }

        private string TryGetParameter(string[] parameters, int index)
        {
            var paramList = parameters.ToList();

            if (index < paramList.Count)
            {
                var param = paramList[index];

                return param;
            }

            return string.Empty;
        }

        public void ShowHelp()
        {
            Console.WriteLine("Use: Program [VPBase.Text.Command]");

            Console.WriteLine("Usage:");
            
            Console.WriteLine(" -help, -h                Show this help text");

            Console.WriteLine(" -replaceSection          Replace text section");
            Console.WriteLine("   arg1: <filePath>             File Path to the file to be proccessed");
            Console.WriteLine("   arg2: <searchStartLineText>  Search Start line text");
            Console.WriteLine("   arg3: <searchEndLineText>    Search End line text");
            Console.WriteLine("   arg4: <replacementText>      Replacement text");

            Console.WriteLine(" -replaceSectionFile      Replace text section with file");
            Console.WriteLine("   arg1: <filePath>             File Path to the file to be proccessed");
            Console.WriteLine("   arg2: <searchStartLineText>  Search Start line text");
            Console.WriteLine("   arg3: <searchEndLineText>    Search End line text");
            Console.WriteLine("   arg4: <replacementFilePath>  Replacement file containing the replacement text");
        }

        public void ReplaceSectionLines(string filePath, string searchStartLineText, string searchEndLineText, string replacementText, string replacementFilePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Error no file path specified when replacing section lines!");
                return;
            }
            if (string.IsNullOrEmpty(searchStartLineText))
            {
                Console.WriteLine("Error no search start line text specified when replacing section lines!");
                return;
            }
            if (string.IsNullOrEmpty(searchEndLineText))
            {
                Console.WriteLine("Error no search end line text specified when replacing section lines!");
                return;
            }

            if (string.IsNullOrEmpty(replacementText) && string.IsNullOrEmpty(replacementFilePath))
            {
                Console.WriteLine("Error no replacement text or file specified when replacing section lines!");
                return;
            }

            var replacementLines = new List<string>(); 

            if (!string.IsNullOrEmpty(replacementText))
            {
                replacementLines = _textManipulationService.BuildListFromText(replacementText);
            }
            if (!string.IsNullOrEmpty(replacementFilePath))
            {
                replacementLines = _fileHandler.ReadFileToLines(replacementFilePath).ToList();
            }

            var sourceLines = _fileHandler.ReadFileToLines(filePath);
            if (sourceLines.Any())
            {
                var destinationLines = _textManipulationService.ReplaceSectionLines(sourceLines, searchStartLineText, searchEndLineText, replacementLines);

                _fileHandler.WriteLinesToFile(filePath, destinationLines);
            }
        }
    }

    public static class ProcessArgumentManagerFactory
    {
        public static ProcessArgumentManager Create()
        {
            return new ProcessArgumentManager(new FileHandler(), new TextManipulationService());
        }
    }
}
