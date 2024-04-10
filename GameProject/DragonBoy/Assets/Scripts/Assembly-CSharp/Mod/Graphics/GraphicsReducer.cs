using Mod.Graphics;

internal class GraphicsReducer
{
    static ReduceGraphicsLevel _level;
    internal static ReduceGraphicsLevel level
    {
        get => _level;
        set
        {
            _level = value;
            if (level != ReduceGraphicsLevel.None)
                CustomBackground.StopAllBackgroundVideo();
        }
    }
}

internal enum ReduceGraphicsLevel
{
    None,
    Low,
    Medium,
    High
}