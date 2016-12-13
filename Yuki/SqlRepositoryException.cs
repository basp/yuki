namespace Yuki
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SqlRepositoryException : Exception
    {
        public SqlRepositoryException()
            : base()
        {
        }

        public SqlRepositoryException(string message)
            : base(message)
        {
        }

        public SqlRepositoryException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public SqlRepositoryException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
