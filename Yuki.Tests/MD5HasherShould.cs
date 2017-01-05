namespace Yuki.Tests
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Text.Encoding;

    [TestClass]
    public class MD5HasherShould
    {
        [TestMethod]
        public void ReturnSomeHashForValidUtf8String()
        {
            var md5 = MD5.Create();
            var hasher = new MD5Hasher();
            var exp = System.Convert.ToBase64String(
                md5.ComputeHash(UTF8.GetBytes("frotz")));

            var res = hasher.GetHash("frotz");

            res.MatchSome(x => Assert.AreEqual(exp, x));
            res.MatchNone(x => Assert.Fail());
        }

        [TestMethod]
        public void ReturnNoneForInvalidUtf8String()
        {
            var hasher = new MD5Hasher();
            var res = hasher.GetHash(null);

            res.MatchSome(x => Assert.Fail());
            res.MatchNone(x => Assert.IsInstanceOfType(x, typeof(Exception)));
        }
    }
}
