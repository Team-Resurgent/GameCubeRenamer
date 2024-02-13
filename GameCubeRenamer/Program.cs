using GameCubeRenamer;
using Mono.Options;
using System.Diagnostics.Metrics;

internal class Program
{
    private static bool shouldShowHelp = false;
    private static string input = string.Empty;

//    GW7D69

//Console ID(G) + Game Code(W7) + Region(D) + Maker Code(69)

//All cdoes are based on converting the hex value to int and using ascii code

//Console id = Offset 0 1 byte
//Game code id = Offset 1 2 bytes
//Region = offset 3, 1 byte
//maker code = offset 4, 2 bytes
    public static void Process()
    {
        var inputFiles = Directory.GetFiles(input, "*.iso", SearchOption.AllDirectories);

        for (int i = 0; i < inputFiles.Length; i++)
        {
            var file = inputFiles[i];
            Console.WriteLine($"Processing ${file}");
            var info = new GCDiscInfo(file);
            var gameid = "[" + GCDiscInfo.convertHexToAscii(info.getConsoleId()) + info.getGameCode() + GCDiscInfo.convertHexToAscii(info.getCountryCode()) + info.getMakerCode() + "]";
            var folder = Path.GetDirectoryName(file);
            var newFolder = folder;
            var index = folder.IndexOf('[');
            if (index != -1)
            {
                newFolder = folder.Substring(0, index).Trim() + " " + gameid;
            }
            else
            {
                newFolder = folder.Trim() + " " + gameid;
            }
            if (folder != newFolder)
            {
                Directory.Move(folder, newFolder);
            }
        }

        Console.WriteLine("Done!");
    }

    private static void Main(string[] args)
    {
        var options = new OptionSet {
            { "i|input=", "Folder containing folders of gamecube isos.", i => input = i },
            { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
        };

        try
        {
            List<string> extra = options.Parse(args);

            if (shouldShowHelp || args.Length == 0)
            {
                Console.WriteLine("GameCubeRenamer: ");
                options.WriteOptionDescriptions(System.Console.Out);
                return;
            }

            if (string.IsNullOrEmpty(input) == true)
            {
                throw new OptionException("input path is invalid", "input");
            }

            input = Path.GetFullPath(input);
            if (Directory.Exists(input) == false)
            {
                throw new OptionException("input path does not exist.", "input");
            }

            Process();
        }
        catch (OptionException e)
        {
            Console.Write("GameCubeRenamer: ");
            Console.WriteLine(e.Message);
            Console.WriteLine("Try `GameCubeRenamer --help' for more information.");
            return;
        }
    }
}