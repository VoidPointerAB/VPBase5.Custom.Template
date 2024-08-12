using NUnit.Framework;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace VPBase.Text.Command.Tests
{
    public class SimpleTestCase
    {
        public static string SearchStartLineText = "TAG START";
        public static string StartLineText = "<!-- TAG START -->";

        public static string SearchEndLineText = "TAG END";
        public static string EndLineText = "<!-- TAG END -->";

        public static string DummyLine = "..";

        public static string ReplaceLine1 = "replace1";
        public static string ReplaceLine2 = "replace2";

        public static ReplaceSectionLineArguments CreateArgs()
        {
            var args = new ReplaceSectionLineArguments
            {
                SearchStartLineText = SearchStartLineText,
                SearchEndLineText = SearchEndLineText
            };

            var startLine = $"<!-- {SearchStartLineText}-->";

            args.SourceLines = new List<string>()
            {
                DummyLine,

                StartLineText,
                "text to replace with replacement lines #1",
                EndLineText,

                DummyLine,

                StartLineText,
                "text to replace with replacement lines #2",
                EndLineText,

                DummyLine,
            };

            args.ReplacementLines = new List<string>()
            {
                ReplaceLine1,
                ReplaceLine2,
            };

            return args;
        }

        public static void VerifyResult(List<string> destinationLines, bool expectedToFail = false)
        {
            var index = 0;

            var errors = new List<string>();

            VerifyStringEqual(index++, destinationLines, DummyLine, errors);

            VerifyStringEqual(index++, destinationLines, StartLineText, errors);
            VerifyStringEqual(index++, destinationLines, ReplaceLine1, errors);
            VerifyStringEqual(index++, destinationLines, ReplaceLine2, errors);
            VerifyStringEqual(index++, destinationLines, EndLineText, errors);

            VerifyStringEqual(index++, destinationLines, DummyLine, errors);

            VerifyStringEqual(index++, destinationLines, StartLineText, errors);
            VerifyStringEqual(index++, destinationLines, ReplaceLine1, errors);
            VerifyStringEqual(index++, destinationLines, ReplaceLine2, errors);
            VerifyStringEqual(index++, destinationLines, EndLineText, errors);

            VerifyStringEqual(index++, destinationLines, DummyLine, errors);

            if (errors.Count > 0 && !expectedToFail)
            {
                foreach(var error in errors)
                {
                    Assert.Fail(error);
                }
            }

            if (expectedToFail)
            {
                if (errors.Count() == 0)
                {
                    Assert.Fail("Expected no errors! Method should fail!");
                }

                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
        }

        private static string TryGetIndexValue(int index, List<string> destinationLines)
        {
            if (index < destinationLines.Count)
            {
                return destinationLines[index];
            }

            return string.Empty;
        }

        private static void VerifyStringEqual(int index, List<string> destinationLines, string expectedResult, List<string> errors)
        {
            var source = TryGetIndexValue(index, destinationLines);

            if (!source.Equals(expectedResult))
            {
                errors.Add($"Error index: {index}, expected result: {expectedResult}");
            }
        }
    }
}
