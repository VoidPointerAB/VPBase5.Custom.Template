using NUnit.Framework;


namespace VPBase.Text.Command.Tests.TextHandlerTests
{
    [TestFixture]
    public class ReplaceSectionLineTests
    {
        private ITextManipulationService _textManipulationService;

        [SetUp] 
        public void Setup()
        {
            _textManipulationService = new TextManipulationService();
        }

        [Test]
        public void Where_replace_section_lines_success()
        {
            var args = SimpleTestCase.CreateArgs();

            var destinationLines = _textManipulationService.ReplaceSectionLines(args);

            SimpleTestCase.VerifyResult(destinationLines);
        }

        [Test]
        public void Where_replace_section_lines_search_start_line_text_failure()
        {
            var args = SimpleTestCase.CreateArgs();

            args.SearchStartLineText = "xxxx";

            var destinationLines = _textManipulationService.ReplaceSectionLines(args);

            SimpleTestCase.VerifyResult(destinationLines, expectedToFail: true);
        }

        [Test]
        public void Where_replace_section_lines_search_end_line_text_failure()
        {
            var args = SimpleTestCase.CreateArgs();

            args.SearchEndLineText = "yyyy";

            var destinationLines = _textManipulationService.ReplaceSectionLines(args);

            SimpleTestCase.VerifyResult(destinationLines, expectedToFail: true);
        }
    }
}
