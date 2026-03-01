using System;
using System.Collections.Generic;
using System.Text;

namespace CBasicCLI;

internal sealed class CppEmitter
{
    List<Token> tokens;
    int pos = 0;

    StringBuilder sb;
    int indent = 0;

    public CppEmitter(List<Token> tokens)
    {
        this.tokens = tokens;
        sb = new();
    }

    public void Parse()
    {
        Emit("#include \"cbstd.h\"");
        Emit("");

        while (!Match(TokenType.EndOfFile))
        {
            ParseStatement();
        }
    }

    void ParseStatement()
    {
        if (CheckKeyword("i32"))
        {
            ParseVarDecl();
        }
        else if (CheckKeyword("DECL_MAIN_BEGIN"))
        {
            ParseMainBlockBegin();
        }
        else if (CheckKeyword("DECL_MAIN_END"))
        {
            ParseMainBlockEnd();
        }
        else if (CheckKeyword("print"))
        {
            ParsePrint();
        }
        else if (CheckKeyword("breakpoint"))
        {
            ParseBreakpoint();
        }
        else if (Current.Type == TokenType.Identifier)
        {
            ParseAssignment();
        }
        else Advance();
    }

    void ParseMainBlockBegin()
    {
        Advance();

        Emit($"int main()");
        BeginBlock();
    }

    void ParseMainBlockEnd()
    {
        Advance();

        string value = Consume(TokenType.Number).Value;

        Emit($"return {value};");
        EndBlock();
    }

    void ParseVarDecl()
    {
        Advance();

        string name = Consume(TokenType.Identifier).Value;
        Consume(TokenType.Equals);
        string value = ParseExpression();

        Emit($"i32 {name} = {value};");
    }

    void ParsePrint()
    {
        Advance();

        bool isString = Match(TokenType.String);
        string expr = ParseExpression();

        if (isString)
        {
            Emit($"cb_print(\"{expr}\");");
        }
        else
        {
            Emit($"cb_print(cb_tostr({expr}));");
        }
    }

    void ParseBreakpoint()
    {
        Advance();
        Emit("cb_breakpoint(\"\");");
    }

    void ParseAssignment()
    {
        string name = Advance().Value;
        Consume(TokenType.Equals);
        string value = ParseExpression();

        Emit($"{name} = {value};");
    }

    string ParseExpression()
    {
        StringBuilder expr = new();

        while (!Match(TokenType.NewLine) && !Match(TokenType.EndOfFile))
        {
            expr.Append(Advance().Value);
        }

        return expr.ToString();
    }

    public void Emit(string line)
    {
        sb.AppendLine(new string(' ', indent * 4) + line);
    }

    public void BeginBlock()
    {
        Emit("{");
        indent++;
    }

    public void EndBlock()
    {
        indent--;
        Emit("}");
    }

    private Token Current => tokens[pos];
    private Token Advance() => tokens[pos++];
    private bool Match(TokenType type)
    {
        return Current.Type == type;
    }
    private bool CheckKeyword(string k)
    {
        return Current.Type == TokenType.Keyword && Current.Value == k;
    }
    private Token Consume(TokenType type)
    {
        if (Current.Type != type)
            throw new Exception($"Unexpected token at line: {Current.Line}");

        return Advance();
    }

    public string String => sb.ToString();
}
