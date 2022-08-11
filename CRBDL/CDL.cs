﻿/*
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
using System.Diagnostics;
using System.IO;

namespace CDL
{
    class CDL
    {
        private static string[] filenames = { "DoomBFA.exe", "DoomBFA", "RBDoom3BFG.exe", "RBDoom3BFG", "Doom3BFG.exe" };
        private readonly UFS ufs;
        private bool[] foundExps;

        public CDL()
        {
            ufs = new UFS();
            foundExps = new bool[4];
            for (int i = 0; i < foundExps.Length; i++)
            {
                foundExps[i] = false;
            }
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
            Process crbd = new Process();

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
            if (!ufs.isRunningAsUWP())
            {
                StreamWriter sw = new StreamWriter(ufs.createFullPath("args.txt"));
                sw.Write(args);
                sw.Close();
            }
            crbd.StartInfo.Arguments = args; // if you need some
            crbd.Start();
        }
    }
}
