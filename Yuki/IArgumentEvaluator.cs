namespace Yuki
{
    using System;
    using System.Collections.Generic;
    using Optional;

    public interface IArgumentEvaluator
    {
        Option<KeyValuePair<string, object>, Exception> Eval(
            string name,
            string value);
    }
}
