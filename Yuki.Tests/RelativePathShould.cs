namespace Yuki.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RelativePathShould
    {
        [TestMethod]
        public void AlwaysStartWithDirectorySeparator()
        {
            var fullPath = @"d:\foo\bar\quux\frotz";
            var basePaths = new[]
            {
                @"d:\foo\bar",
                @"d:\foo\bar\",
            };

            var relativePaths = basePaths
                .Select(x => Utils.RelativePath(x, fullPath))
                .ToArray();

            Array.ForEach(
                relativePaths,
                x => Assert.AreEqual(@"\quux\frotz", x));
        }
    }
}