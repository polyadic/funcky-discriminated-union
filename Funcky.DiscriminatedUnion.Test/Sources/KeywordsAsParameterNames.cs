namespace Funcky.DiscriminatedUnion.Test;

[DiscriminatedUnion]
public abstract partial record Bool
{
    public sealed partial record True : Bool;

    public sealed partial record False : Bool;
}

public static class BoolTest
{
    public static void BoolFn(Bool @bool)
    {
        _ = @bool.Match(@true: _ => true, @false: _ => false);
    }
}
