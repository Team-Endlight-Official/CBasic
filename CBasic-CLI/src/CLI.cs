using System;
using System.Collections.Generic;

namespace CBasicCLI;

internal sealed class CLI
{
    public static ConsoleColor ForegroundColor
    {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }

    public static ConsoleColor BackgroundColor
    {
        get => Console.BackgroundColor;
        set => Console.BackgroundColor = value;
    }

    public static int WindowWidth
    {
        get => Console.WindowWidth;
        set => Console.WindowWidth = value;
    }

    public static int WindowHeight
    {
        get => Console.WindowHeight;
        set => Console.WindowHeight = value;
    }

    public static int BufferWidth
    {
        get => Console.BufferWidth;
        set => Console.BufferWidth = value;
    }

    public static int BufferHeight
    {
        get => Console.BufferHeight;
        set => Console.BufferHeight = value;
    }

    public static void SetSize(int width, int height)
    {
        Console.SetWindowSize(width, height);
    }

    public static void SetBufferSize(int width, int height)
    {
        Console.SetBufferSize(width, height);
    }

    public static void ResetSize()
    {
        Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight);
        Console.SetWindowSize(Console.BufferWidth, Console.BufferHeight);
    }

    public static void ResetColors()
    {
        Console.ResetColor();
    }

    public static void Clear()
    {
        Console.Clear();
    }

    public static void SetCursorPosition(int x, int y)
    {
        Console.SetCursorPosition(x, y);
    }

    public static void WriteLn(object? value)
    {
        Console.Write($"{value}\n");
    }

    public static void Write(object? value)
    {
        Console.Write(value);
    }

    public static void WriteMenuBarTop(string title)
    {
        string bar = $"{RGB(100, 30, 10)}";
        int titlelen = title.Length;

        for (int w = 0; w < 1; w++)
        {
            bar += "█";
        }
        bar += $"▌{title}▐";

        int remainingWidth = Console.WindowWidth - bar.Length;

        for (int w = 0; w < remainingWidth; w++)
        {
            bar += "█";
        }
        bar += $"{RGBReset}\n";

        Console.Write(bar);
        ResetColors();
        ResetSize();
    }

    public static string? ReadLn()
    {
        return Console.ReadLine();
    }

    public static ConsoleKeyInfo ReadKey(bool intercept)
    {
        return Console.ReadKey(intercept);
    }

    public static string RGB(int r, int g, int b)
    {
        return $"\u001b[38;2;{r};{g};{b}m";
    }

    public static string RGBReset => "\u001b[0m";

    public static string Title
    {
        get => Console.Title;
        set => Console.Title = value;
    }

    public static string Version => "0.0.1";

    public static bool CursorVisible
    {
        get => Console.CursorVisible;
        set => Console.CursorVisible = value;
    }
}
