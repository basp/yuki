namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Optional;

    public class ArgumentEvaluator : IArgumentEvaluator
    {
        private readonly Func<string, object> eval;

        public ArgumentEvaluator(Func<string, object> eval)
        {
            Contract.Requires(eval != null);

            this.eval = eval;
        }

        public Option<KeyValuePair<string, object>, Exception> Eval(
            string name,
            string value)
        {
            try
            {
                var obj = this.eval(value);
                var kvp = Utils.CreateKeyValuePair(name, obj);
                return Option.Some<KeyValuePair<string, object>, Exception>(kvp);
            }
            catch (Exception ex)
            {
                return Option.None<KeyValuePair<string, object>, Exception>(ex);
            }
        }

        public static object DefaultEval(string value)
        {
            DateTime dt;
            if (DateTime.TryParse(value, out dt))
            {
                return dt;
            }

            decimal dec;
            if (decimal.TryParse(value, out dec))
            {
                return dec;
            }

            int i;
            if (int.TryParse(value, out i))
            {
                return i;
            }

            return value;
        }
    }
}
