namespace StructurizrDslPlantumlDirectionUpdater;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal partial class ProcessFileExecutor
{
    [GeneratedRegex(@"([^\s]*\s\.)(\[[^\]]+\])?(.>\s[^\s]*\s:\s""[^""]+"")")]
    private static partial Regex SinglePlantUmlLineRegexMethod();

    public async Task<int> ProcessFile(string fileName)
    {
        var fileContent= await File.ReadAllTextAsync(fileName);
        var result = ProcessContent(fileContent);
        await File.WriteAllTextAsync(fileName, result);
        return 1;
    }

    internal string ProcessContent(string fileContent)
    {
        var lines = fileContent.Split("\n");

        var newLines = new List<string>(lines.Length);
        foreach (var line in lines)
        {
            newLines.Add(ProcessSingleLine(line));
        }

        return string.Join('\n', newLines);
    }

    private string ProcessSingleLine(string line)
    {
        if (line.Contains("[puml"))
        {
            return ProcessSingleLinePlantUml(line);
        }

        return line;
    }

    private string ProcessSingleLinePlantUml(string line)
    {
        var length = 0;
        string direction = string.Empty;
        string newLine = line;

        while (newLine.Contains("[puml length:"))
        {
            if (newLine.Contains("[puml length:1]"))
            {
                length = 1;
                newLine = newLine.Replace("[puml length:1]", string.Empty);
            }
            else if (newLine.Contains("[puml length:2]"))
            {
                length = 2;
                newLine = newLine.Replace("[puml length:2]", string.Empty);
            }
            else
            {
                throw new NotSupportedException("Only puml length 1 or 2 are supported.");
            }
        }

        while (newLine.Contains("[puml up]"))
        {
            direction = "u";
            newLine = newLine.Replace("[puml up]", string.Empty);
        }

        while (newLine.Contains("[puml down]"))
        {
            direction = "d";
            newLine = newLine.Replace("[puml down]", string.Empty);
        }

        while (newLine.Contains("[puml left]"))
        {
            direction = "l";
            newLine = newLine.Replace("[puml left]", string.Empty);
        }

        while (newLine.Contains("[puml right]"))
        {
            direction = "r";
            newLine = newLine.Replace("[puml right]", string.Empty);
        }

        var replacement = direction + new string('.', length);

        Match match = SinglePlantUmlLineRegexMethod().Match(newLine);
        if (match.Success)
        {
            return match.Groups[1].Value + match.Groups[2].Value + replacement + match.Groups[3].Value;
        }

        return line;
    }
}