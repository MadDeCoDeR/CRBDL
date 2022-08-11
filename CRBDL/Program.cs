/*
Classic RBDoom 3 BFG Edition Launcher

Copyright(C) 2019 George Kalmpokis

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files(the "Software"), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions :

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Windows.Forms;
using System.Linq;
using CDL.Arguments;
using System.IO;

namespace CRBDL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!args.Contains("-cli"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            } else
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
                
                CDL.CDL cdl = new CDL.CDL();
                if (cdl.CheckFiles() == 0)
                {
                    Console.WriteLine("Main Executable not found");
                    throw new Exception("Main Executable not found");
                }
                Console.WriteLine(largs);
                cdl.LaunchGame(largs);
            }
        }
    }
}
