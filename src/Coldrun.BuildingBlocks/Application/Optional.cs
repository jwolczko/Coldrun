using System;
using System.Collections.Generic;
using System.Text;

namespace Coldrun.BuildingBlocks.Application;

public readonly record struct Optional<T>(bool HasValue, T? Value)
{
    public static Optional<T> None => default;

    public static Optional<T> Some(T? value)
    {
        return new Optional<T>(true, value);
    }
}