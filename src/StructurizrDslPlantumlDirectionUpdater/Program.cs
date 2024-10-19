namespace StructurizrDslPlantumlDirectionUpdater;

using System;
using System.Threading.Tasks;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 1)
        {
            var file = args[0];
            return await ProcessFile(file);
        }

        throw new ArgumentException("Please provide a file name as the first argument.");
    }

    private static async Task<int> ProcessFile(string fileName)
    {
        var executor = new ProcessFileExecutor();
        return await executor.ProcessFile(fileName);
    }
}