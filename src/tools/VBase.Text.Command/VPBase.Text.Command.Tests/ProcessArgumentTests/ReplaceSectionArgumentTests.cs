using NUnit.Framework;
using System.Collections.Generic;

namespace VPBase.Text.Command.Tests.ProcessArgumentTests
{
    [TestFixture]
    public class ReplaceSectionArgumentTests
    {
        private ProcessArgumentManager _processArgumentManager;
        private DummyFileHandler _dummyFileHandler = new DummyFileHandler();
        private TextManipulationService _textManipulationService = new TextManipulationService();

        [SetUp]
        public void Setup()
        {
            _processArgumentManager = new ProcessArgumentManager(_dummyFileHandler, _textManipulationService);
            _dummyFileHandler.SourceLines.Clear();
        }

        [Test]
        public void Where_execute_replace_section_success_arguments()
        {
            var args = SimpleTestCase.CreateArgs();

            var filePath = "test.txt";

            _dummyFileHandler.SourceLines.Add(filePath, args.SourceLines);

            var replacementText = _textManipulationService.BuildStringFromList(args.ReplacementLines);

            var arguments = new List<string>()
            {
                ProcessArgumentManager.Command_ReplaceSection,      // Replacement with text
                filePath,
                args.SearchStartLineText,
                args.SearchEndLineText,
                replacementText
            };

            _processArgumentManager.Execute(arguments.ToArray());

            var destinationLines = _dummyFileHandler.DestinationLines;

            SimpleTestCase.VerifyResult(destinationLines);
        }

        [Test]
        public void Where_execute_replace_section_file_success_arguments()
        {
            var args = SimpleTestCase.CreateArgs();

            var filePath = "test.txt";

            _dummyFileHandler.SourceLines.Add(filePath, args.SourceLines);

            var replacementFilePath = "replacement.txt";

            _dummyFileHandler.SourceLines.Add(replacementFilePath, args.ReplacementLines);

            var arguments = new List<string>()
            {
                ProcessArgumentManager.Command_ReplaceSectionFile,     // Replacement with File Path
                filePath,
                args.SearchStartLineText,
                args.SearchEndLineText,
                replacementFilePath
            };

            _processArgumentManager.Execute(arguments.ToArray());

            var destinationLines = _dummyFileHandler.DestinationLines;

            SimpleTestCase.VerifyResult(destinationLines);
        }
    }
}
