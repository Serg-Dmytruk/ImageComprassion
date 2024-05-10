namespace ConsoleApp3.Models;

public static class AllowedResolutions
{
    private static readonly Resolution Res64X64 = new (64, 64, "_64x64");
    private static readonly Resolution Res1280X720 = new (1280, 720, "_1280x720");

    public static readonly List<Resolution> Resolutions =
    [
        Res64X64,
        Res1280X720
    ];
}