using NUnit.Framework;
using System.Collections.Generic;

namespace VPBase.Bundler.Tests
{
    [TestFixture]
    public class Testing
    {
        [Test]
        public void Test_run_bundle()
        {
            var configPath = @"D:\Code\VPBase\VPBase\src\base\VPBase.Base.Server\bundleconfig.test.json";

            var arguments = new List<string>();

            ProgramHandler.Execute(arguments, configPath);
        }

        [Test]
        public void Test_run_clean_bundle()
        {
            var configPath = @"D:\Code\VPBase\VPBase\src\base\VPBase.Base.Server\bundleconfig.test.json";

            var arguments = new List<string>();

            arguments.Add("clean");

            ProgramHandler.Execute(arguments, configPath);
        }
    }
}
