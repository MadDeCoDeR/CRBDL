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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace CDL.filesystem
{
    class UFS
    {
        private List<string> paths;

        private static readonly List<string> SHA512s = new List<string> {
            "EF7CA86405FC3F70717BB1B58C584787EE5D474880675B1D4B4A33075BAD7492CB5A2A37A57F205E0DAF5520D07A593960CFB30AD797A47BE90E9C88DB06A0C7", //DOOM 3: BFG Edition
            "CD3E74628C4F5609C5A3EF86D71F45C1F58E3746B77DD98604CCBBC93B322D21C649658939B987A1752D4C521B63A9A74EAE580C841960DC2C8D9745CF174EC9" //DOOM 3 re-release (2019)
        };
        private static readonly SHA512 sHA512 = new SHA512Managed();
        private string BFGPath;
        private string NewD3Path;
        private string selectedPath;
        public UFS()
        {
            paths = new List<string>
            {
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                Directory.GetCurrentDirectory(),
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/../DoomBFA"
            };
            if (paths[0].StartsWith("/usr") || paths[0].StartsWith("/app"))
            {
                paths.Add("/usr/bin");
                List<string> files = searchFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "_common.resources");
                files.AddRange(searchFile("/run/media/", "_common.resources"));
                files.AddRange(searchFile("/media/", "_common.resources"));
                if (files.Count >= 1)
                {
                    int index = 3;
                    files.ForEach(filePath =>
                    {
                        byte[] data = File.ReadAllBytes(filePath);
                        string fileSha512 = BitConverter.ToString(sHA512.ComputeHash(data)).ToUpper();
                        if (BFGPath == null && fileSha512 == SHA512s[0])
                        {
                            BFGPath = filePath;
                            string path = filePath.Substring(0, filePath.LastIndexOf("/"));
                            paths.Add( path.Substring(0, path.LastIndexOf("/")));
                            index++;
                            this.selectedPath = paths[index];
                        }
                        else if (NewD3Path == null && fileSha512 == SHA512s[1]) { 
                            NewD3Path = filePath;
                            string path = filePath.Substring(0, filePath.LastIndexOf("/"));
                            paths.Add(path.Substring(0, path.LastIndexOf("/")));
                            index++;
                            this.selectedPath = paths[index];
                        }
                    });
                    
                } else
                {
                    paths.Add("");
                    this.selectedPath = paths[0];
                }
                
            } else
            {
                paths.Add("");
                paths.Add("");
                this.selectedPath = paths[0];
            }
            
        }

        public string createFullPath(string filename) {
            string fullPath;

            if (isUnixFS())
            {
                fullPath = selectedPath + "/" + filename;
            } else
            {
                fullPath = filename;
            }

            return fullPath;
        }

        public bool Exists(string filename)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i] + "/" + filename;
                if (File.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }

        public string getFullPath(string filename)
        {
            List<string> foundPaths = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i] + "/" + filename;
                if (File.Exists(path))
                {
                    foundPaths.Add(path);
                }
            }
            if (foundPaths.Count > 1)
            {
                foreach (string path in foundPaths)
                {
                    if (path.StartsWith(selectedPath))
                    {
                        return path;
                    }
                }
                return foundPaths[0];
            }
            else if (foundPaths.Count > 0)
            {
                return foundPaths[0];
            }

            return filename;
        }

        public bool isUnixFS()
        {
            return selectedPath.StartsWith("/");
        }

        public bool isRunningPackaged()
        {
            return paths[0].StartsWith("/usr") || paths[0].StartsWith("/app");
        }

        public string getParentPath(string relativeFolder)
        {
            List<string> foundPaths = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                if (Directory.Exists(paths[i] + "/" + relativeFolder))
                {
                    foundPaths.Add(paths[i]);
                }
            }
            if (foundPaths.Count > 1) {
                foreach (string path in foundPaths) { 
                    if (path.StartsWith(selectedPath))
                    {
                        return path;
                    }
                }
                return foundPaths[0];
            } else if (foundPaths.Count > 0)
            {
                return foundPaths[0];
            }
            return "";
        }

        public string getCurrentDirectory(string [] filenames)
        {
            List<string> foundPaths = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                for (int j = 0; j < filenames.Length; j++)
                {
                    string path = paths[i] + "/" + filenames[j];
                    if (File.Exists(path))
                    {
                        foundPaths.Add(paths[i]);
                    }
                }
            }
            if (foundPaths.Count > 1)
            {
                foreach (string path in foundPaths)
                {
                    if (path.StartsWith(selectedPath))
                    {
                        return path;
                    }
                }
                return foundPaths[0];
            }
            else if (foundPaths.Count > 0)
            {
                return foundPaths[0];
            }
            return "";
        }

        public string getRelativePath(string path, string subfolder)
        {
            if  (path.Length <= 0)
            {
                return path;
            }
            string ogpath = path;
            if (path[0] == '/')
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    string rempath = paths[i] + "/" + subfolder +"/";
                    path = path.Replace(rempath, "");
                    if (!path.Equals(ogpath))
                    {
                        return path;
                    }
                }
            }
            return path;
        }

        private List<string> searchFile(string parentPath, string filename)
        {
            List<string> filePaths = new List<string>();
            if (!parentPath.Contains("Proton") && Directory.Exists(parentPath)) //GK: Proton search prevention hack
            {
                //GK: This is the worst way to handle this but I couldn't find a better way to do that
                try
                {
                    string[] files = Directory.GetFiles(parentPath, filename);
                    if (files.Length > 0)
                    {
                        filePaths.AddRange(files);
                    }
                    else
                    {
                        string[] folders = Directory.GetDirectories(parentPath);
                        if (folders.Length > 0)
                        {
                            foreach (string path in folders)
                            {
                                filePaths.AddRange(searchFile(path, filename));
                                if (filePaths.Count > 0)
                                {
                                    break; //GK: Abrudly stop for performance
                                }
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    //Do nothing
                }
            }
            return filePaths;
        }

        public string GetBFGPath()
        {
            return BFGPath;
        }

        public string GetNewD3Path()
        {
            return NewD3Path;
        }
    }
}
