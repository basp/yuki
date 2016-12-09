﻿namespace Yuki.Maybe
{
    using System;

    public interface IMaybeError
    {
        bool IsError { get; }

        Exception Exception { get; }
    }
}
