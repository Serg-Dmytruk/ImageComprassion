namespace ConsoleApp3.Models;

public class Resolution(int width, int height, string prefix)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public string Prefix { get; } = prefix;
}