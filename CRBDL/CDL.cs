/*
Classic RBDoom 3 BFG Edition Launcher

Copyright(C) 2022 George Kalmpokis

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
using CDL.filesystem;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CDL
{
    class CDL
    {
        private static string[] filenames = { "DoomBFA.exe", "DoomBFA", "RBDoom3BFG.exe", "RBDoom3BFG", "Doom3BFG.exe" };
        private readonly UFS ufs;
        private bool[] foundExps;
        public static bool testPackage = false;
        public bool isRunning {  get; private set; }
        private readonly Process crbd;

        public CDL(UFS ufs)
        {
            this.ufs = ufs;
            foundExps = new bool[4];
            for (int i = 0; i < foundExps.Length; i++)
            {
                foundExps[i] = false;
            }
            this.crbd = new Process();
            crbd.Exited += (s, e) =>
            {
                this.isRunning = false;
            };
            crbd.EnableRaisingEvents = true;
        }

        public int CheckFiles()
        {
            int found = 0;
            foreach (string filename in filenames)
            {
                if (ufs.Exists(filename))
                {
                    found++;
                }
            }

            return found;
        }

        public bool[] checkClassicExpansions(string folderName)
        {
            if (ufs.Exists(folderName + "/wads/NERVE.wad") && !foundExps[0])
            {
                foundExps[0] = true;
            }
            if (ufs.Exists(folderName + "/wads/MASTERLEVELS.wad") && !foundExps[1])
            {
                foundExps[1] = true;
            }
            if (ufs.Exists(folderName + "/wads/PLUTONIA.WAD") && !foundExps[2])
            {
                foundExps[2] = true;
            }
            if (ufs.Exists(folderName + "/wads/TNT.WAD") && !foundExps[3])
            {
                foundExps[3] = true;
            }

            return foundExps;
        }

        public void LaunchGame(string args)
        {
            

            foreach (string filename in filenames)
            {
                if (ufs.Exists(filename))
                {
                    if ((ufs.isUnixFS() && filename.EndsWith(".exe")) || (!ufs.isUnixFS() && !filename.EndsWith(".exe")))
                    {
                        continue;
                    }
                    crbd.StartInfo.FileName = ufs.getFullPath(filename);
                    break;
                }
            }
            /*if (crbd.StartInfo.FileName == null || crbd.StartInfo.FileName == "")
            {
                if (MessageBox.Show("Unable to find the main executable for this System", "Error", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Close();
                }
            }*/
            crbd.StartInfo.WorkingDirectory = ufs.getCurrentDirectory(filenames);
            if (!ufs.isRunningPackaged())
            {
                StreamWriter sw = new StreamWriter(ufs.createFullPath("args.txt"));
                sw.Write(args);
                sw.Close();
            }
            else if (ufs.isRunningPackaged() && ufs.isUnixFS())
            {
                Console.WriteLine("Passing Arguments: " + args);
                crbd.StartInfo.UseShellExecute = true;
                /*crbd.StartInfo.EnvironmentVariables["LD_LIBRARY_PATH"] = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.doombfa/base/lib";
                crbd.StartInfo.UseShellExecute = false;*/
            }
            crbd.StartInfo.Arguments = args; // if you need some
            this.isRunning = crbd.Start();
        }

        public void LaunchAndWaitGame(string args)
        {
            this.LaunchGame(args);
            Task task = Task.Run(() => { 
                while (this.isRunning)
                {
                    Thread.Sleep(4000);
                }
            });

            Task.WaitAny(task);
        } 
    }
}
