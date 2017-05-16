namespace Yuki
{
    using System;
    using System.Text;

    public static class Extensions
    {
        public static string ToBase64String(this Guid guid) =>
            Convert.ToBase64String(guid.ToByteArray());

        public static string ToBase64String(this string str) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }
}
