using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CBasicCLI;

internal sealed class LangMapper
{
    private readonly string langMapSource;

    private LangInfo info;
    private LangKeywords keywords;
    private LangTypes types;

    public LangMapper(string mapPath)
    {
        using (FileStream fs = new FileStream(mapPath, FileMode.Open, FileAccess.Read))
        using (JsonDocument json = JsonDocument.Parse(fs))
        {
            var root = json.RootElement;

            // Read info!
            info = new LangInfo
            (
                version: root.GetProperty("version").ToString(),
                dependencies: root.GetProperty("dependencies").EnumerateArray().Select(t => t.ToString()).ToList()
            );

            // Read Keywords!
            keywords = new LangKeywords
            (
                keywords: root.GetProperty("keywords").EnumerateArray().Select(t => t.ToString()).ToList()
            );

            // Read types!
            types = new LangTypes
            (
                cbasicTypes: root.GetProperty("data_types").EnumerateArray().Select(t => t.GetProperty("cbasic").ToString()).ToList(),
                cppTypes: root.GetProperty("data_types").EnumerateArray().Select(t => t.GetProperty("cpp").ToString()).ToList()
            );
        }

        /*
        CLI.WriteLn(info);
        CLI.WriteLn(keywords);
        CLI.WriteLn(types);
        */
    }

    public List<string> Keywords => keywords.Keywords;
    public List<string> CBasicDataTypes => types.CBasicTypes;
    public List<string> CppDataTypes => types.CppTypes;

    public string GetCppType(string cppType)
    {
        if (!types.CppTypes.Contains(cppType))
        {
            throw new Exception("Could not find a specific CPP type from the language map!");
        }

        return types.CppTypes.Find(r => r == cppType);
    }

    public string GetCBasicType(string cbasicType)
    {
        if (!types.CBasicTypes.Contains(cbasicType))
        {
            throw new Exception("Could not find a specific CPP type from the language map!");
        }

        return types.CBasicTypes.Find(r => r == cbasicType);
    }

    public string EvaluateCBasicToCpp(string cbasicType)
    {
        if (!types.CBasicToCppTypes.ContainsKey(cbasicType))
        {
            throw new Exception("Could not find a specific CBasic type from the language map!");
        }

        return types.CBasicToCppTypes[cbasicType];
    }
}

internal struct LangInfo
{
    string version;
    List<string> dependencies;

    public LangInfo(string version, List<string> dependencies)
    {
        this.version = version;
        this.dependencies = dependencies;
    }

    public string Version => version;
    public List<string> Dependencies => dependencies;

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.AppendLine("CBasic Language Information:");
        str.AppendLine($"Version: {version}");
        str.AppendLine($"Dependencies: ");

        foreach (string dependency in dependencies)
        {
            str.Append($"{dependency}\n");
        }

        return str.ToString();
    }
}

internal struct LangKeywords
{
    List<string> keywords;

    public LangKeywords(List<string> keywords)
    {
        this.keywords = keywords;
    }

    public List<string> Keywords => keywords;

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.AppendLine("CBasic Keywords: ");
        foreach (string keyword in keywords)
        {
            str.Append($"{keyword}\n");
        }

        return str.ToString();
    }
}

internal struct LangTypes
{
    List<string> cbasicTypes;
    List<string> cppTypes;

    Dictionary<string, string> cbasicToCppTypes;

    public LangTypes(List<string> cbasicTypes, List<string> cppTypes)
    {
        this.cbasicTypes = cbasicTypes;
        this.cppTypes = cppTypes;

        cbasicToCppTypes = new Dictionary<string, string>();

        for (int i = 0; i < cbasicTypes.ToArray().Length; i++)
        {
            cbasicToCppTypes.Add(cbasicTypes[i], cppTypes[i]);
        }
    }

    public List<string> CBasicTypes => cbasicTypes;
    public List<string> CppTypes => cppTypes;
    public Dictionary<string, string> CBasicToCppTypes => cbasicToCppTypes;

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.AppendLine("CBasic Datatypes: ");

        for (int index = 0; index < cbasicTypes.ToArray().Length; index++)
        {
            str.Append("\n");
            str.Append("[\n");
            str.Append($"   CBasic: {cbasicTypes[index]}\n");
            str.Append($"   Cpp:    {cppTypes[index]}\n");
            str.Append("]\n");
        }

        return str.ToString();
    }
}
