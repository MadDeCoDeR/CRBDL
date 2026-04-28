using Avalonia;
using CDL.Arguments;
using CDL.filesystem;
using System;
using System.IO;

namespace dbfal;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) { 
        CDL.CDL.testPackage = args.Contains("-testPack");
        if (!args.Contains("-cli"))
        {
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }
        else
        {
            string largs = "";
            
            if (args.Contains("-conf"))
            {
                int index = Array.FindIndex(args, o => o == "-conf");
                largs += ArgParser.ParseArgsFromSettings(new FileStream(args[index + 1], FileMode.Open));
            }

            if (args.Contains("-pass"))
            {
                int index = Array.FindIndex(args, o => o == "-pass");
                string[] pargs = new string[args.Length];
                Array.Copy(args, index + 1, pargs, 0, pargs.Length - (index + 1));
                largs += string.Join(" ", pargs);
            }
            string[] eargs = largs.Split(' ');
            int pathIndex = Array.IndexOf(eargs, "fs_basepath");
            string path = "";
            if (pathIndex != -1)
            {
                int index = 1;
                while (!ArgParser.IsNewArgument(eargs[pathIndex + index]))
                {
                    path += " " + eargs[pathIndex + index];
                    index++;
                    if ((pathIndex + index) >= eargs.Length)
                    {
                        break;
                    }
                }
                path = path.Trim().Replace('\"', ' ').Trim();
            }
            UFS ufs = new UFS(true, path);
            CDL.CDL cdl = new CDL.CDL(ufs);
            if (cdl.CheckFiles() == 0)
            {
                Console.WriteLine("Main Executable not found");
                throw new Exception("Main Executable not found");
            }
            Console.WriteLine(largs);
            if (ufs.isRunningPackaged())
            {
                cdl.LaunchAndWaitGame(largs);
            } 
            else
            {
                cdl.LaunchGame(largs);
            } 
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            .WithInterFont()
            .LogToTrace();
}
