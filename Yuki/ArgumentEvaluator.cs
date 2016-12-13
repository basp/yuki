namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Optional;

    using static Optional.Option;
    using static Utils;

    public class ArgumentEvaluator : IArgumentEvaluator
    {
        private readonly Func<string, object> eval;

        public ArgumentEvaluator()
            : this(ArgumentEvaluator.DefaultEval)
        {
        }

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
                return Some<KeyValuePair<string, object>, Exception>(kvp);
            }
            catch (Exception ex)
            {
                return None<KeyValuePair<string, object>, Exception>(ex);
            }
        }

        private static object DefaultEval(string value)
        {
            return MaybeInt(value)
                .Else(() => MaybeDecimal(value))
                .Else(() => MaybeDateTime(value))
                .Else(() => Some<object>(value));
        }
    }
}
