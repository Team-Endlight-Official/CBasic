using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace CBasicCLI;

internal sealed class Program
{
    static int width = 80;
    static int height = 25;

    private static void Main(string[] args)
    {
        CLI.Title = $"CBasic CLIDE v{CLI.Version}";
        CLI.SetSize(width, height);
        CLI.CursorVisible = true;

        CLI.WriteLn($"CBasic CLI v{CLI.Version}");

        while (true)
        {
            string input = CLI.ReadLn();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                CLI.Clear();
                CLI.WriteLn("Exiting ...");
                Thread.Sleep(1000);

                break;
            }
            else if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                CLI.WriteLn("Available commands: ");
                CLI.WriteLn("- help: Show this help message.");
                CLI.WriteLn("- parse: Run a parse test.");
                CLI.WriteLn("- clear: Clear the console.");
                CLI.WriteLn("- exit: Exit the CLI.");
            }
            else if ((input.Equals("clear", StringComparison.OrdinalIgnoreCase)) ||
                    (input.Equals("cls", StringComparison.OrdinalIgnoreCase)))
            {
                CLI.Clear();
                CLI.WriteLn($"CBasic CLI v{CLI.Version}");
            }
            else if (input.Equals("parse", StringComparison.OrdinalIgnoreCase))
            {
                ParseTestNew();
            }
            else if (input.Equals("map", StringComparison.OrdinalIgnoreCase))
            {
                TestLanguageMapper();
            }
            else
            {
                CLI.ForegroundColor = ConsoleColor.DarkRed;
                CLI.WriteLn($"Unknown command: {input}");
                CLI.ResetColors();
            }

            CLI.CursorVisible = true;
        }
    }

    static void TestLanguageMapper()
    {
        LangMapper mapper = new LangMapper("C:\\Users\\jakub\\Documents\\Code Projects\\CBasic\\CBasic-Lang\\Mapper Example\\cbasic_langmap.json");

        CLI.WriteLn("Testing Parsing!");

        while (true)
        {
            string input = CLI.ReadLn();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input.Equals(":esc", StringComparison.OrdinalIgnoreCase))
            {
                CLI.WriteLn("Testing Parsing Ended!\n");
                break;
            }
            else if (input.Contains(":pcbtoc", StringComparison.OrdinalIgnoreCase))
            {
                CLI.WriteLn("Parse CBasic Type to C++");
                string wishedType = input.Remove(0, ":pcbtoc".Length);

                string outType = mapper.EvaluateCBasicToCpp(wishedType);
                CLI.WriteLn($"Your input: {wishedType}, has been evaluated to the C++ type: {outType} !");
            }

            CLI.CursorVisible = true;
        }
    }

    static void ParseTestNew()
    {
        string filePath = string.Empty;

        while (true)
        {
            filePath = CLI.ReadLn();

            if (string.IsNullOrEmpty(filePath))
            {
                continue;
            }
            else break;
        }

        string cbasicSource = File.ReadAllText(filePath);

        CLI.WriteLn($"Input Code:\n{cbasicSource}\n");

        Thread.Sleep(2450);

        Lexer lexer = new Lexer(cbasicSource);
        lexer.Tokenize();

        CppEmitter emitter = new CppEmitter(lexer.Tokens);
        emitter.Parse();
        string cppSource = emitter.String;

        CLI.WriteLn("Translation finished!\nC++ output:");
        CLI.WriteLn(cppSource);

        CompileParsed(cppSource);
    }

    static void CompileParsed(in string cppCode)
    {
        CLI.WriteLn("\nCompiling the parsed code ...");

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string tempFolder = Path.Combine(desktopPath, "CBasicCLI_Temp_1234");

        string runtimeSourcePath = Path.Combine(Environment.ProcessPath, "C:\\Users\\jakub\\Documents\\Code Projects\\CBasic\\CBasic-CLI\\bin\\Debug\\net9.0\\data\\runtime\\include\\cbstd.cpp");
        string runtimeHeaderPath = Path.Combine(Environment.ProcessPath, "C:\\Users\\jakub\\Documents\\Code Projects\\CBasic\\CBasic-CLI\\bin\\Debug\\net9.0\\data\\runtime\\include\\cbstd.h");

        if (!Directory.Exists(tempFolder))
        {
            Directory.CreateDirectory(tempFolder);
        }

        string cppFilePath = Path.Combine(tempFolder, "main.cpp");
        File.WriteAllText(cppFilePath, cppCode);

        File.Copy(runtimeSourcePath, Path.Combine(tempFolder, "cbstd.cpp"), true);
        File.Copy(runtimeHeaderPath, Path.Combine(tempFolder, "cbstd.h"), true);

        Thread.Sleep(1000); // Wait a bit to ensure files are written

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "g++",
            Arguments = $"main.cpp cbstd.cpp -o program.exe",
            WorkingDirectory = tempFolder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process proc = new Process { StartInfo = psi };
        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();
        string error = proc.StandardError.ReadToEnd();
        proc.WaitForExit();

        Console.WriteLine($"Compiler Output: {output}");
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"Compiler Errors: {error}");
        }
    }
}
