using System;

namespace Funcky.DiscriminatedUnion.Test;

[DiscriminatedUnion(NonExhaustive = true)]
public abstract partial record Result<T>
    where T : notnull
{
    public sealed partial record Ok(T Result) : Result<T>;

    public sealed partial record Error(Exception Exception) : Result<T>;
}

public static class ResultTest
{
    public static void ResultFn(Result<int> result)
    {
        _ = result.Match(ok: ok => ok.Result, error: _ => 0);
    }
}
