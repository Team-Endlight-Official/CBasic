using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CBasicCLI;

internal sealed class Lexer
{
    private readonly string source;
    private readonly int sourceLength;

    private int pos = 0;
    private int line = 1;
    private int column = 1;

    private List<Token> tokens;

    private readonly HashSet<string> keywords = new()
    {
        "DECL_MAIN_BEGIN", "DECL_MAIN_END", "print", "breakpoint", "i32"
    };

    public Lexer(string source)
    {
        this.source = source;
        sourceLength = source.Length;
        tokens = new List<Token>();
    }

    public void Tokenize()
    {
        while (!IsAtEnd)
        {
            char c = Current;

            if (char.IsWhiteSpace(c))
            {
                if (c == '\n')
                {
                    tokens.Add(new Token(TokenType.NewLine, "\n", line, column));
                    line++;
                }
                AdvancePos();
            }
            else if (char.IsLetter(c))
            {
                ReadIdentifier();
            }
            else if (char.IsDigit(c))
            {
                ReadNumber();
            }
            else if (c == '"')
            {
                ReadString();
            }
            else if (c == '=')
            {
                tokens.Add(new Token(TokenType.Equals, "=", line, column));
                AdvancePos();
            }
            else if ("+-*/><".Contains(c))
            {
                tokens.Add(new Token(TokenType.Operator, c.ToString(), line, column));
                AdvancePos();
            }
            else AdvancePos();
        }

        tokens.Add(new Token(TokenType.EndOfFile, "", line, column));
    }

    void ReadIdentifier()
    {
        int start = pos;
        
        while (!IsAtEnd && char.IsLetterOrDigit(Current) || Current == '_')
        {
            AdvancePos();
        }

        string text = source[start..pos];

        if (keywords.Contains(text))
        {
            tokens.Add(new Token(TokenType.Keyword, text, line, column));
        }
        else
        {
            tokens.Add(new Token(TokenType.Identifier, text, line, column));
        }
    }

    void ReadNumber()
    {
        int start = pos;

        while (!IsAtEnd && char.IsDigit(Current))
        {
            AdvancePos();
        }

        tokens.Add(new Token(TokenType.Number, source[start..pos], line, column));
    }

    void ReadString()
    {
        AdvancePos(); // Skip opening quote

        int start = pos;

        while (!IsAtEnd && Current != '"')
        {
            AdvancePos();
        }

        if (IsAtEnd)
        {
            throw new Exception($"Unterminated string at line {line}");
        }

        string value = source[start..pos];

        AdvancePos(); // Skip closing quote

        tokens.Add(new Token(TokenType.String, value, line, column));
    }

    bool IsAtEnd => pos >= sourceLength;
    void AdvancePos() => pos++;
    char Current => source[pos];
    public List<Token> Tokens => tokens;
}
