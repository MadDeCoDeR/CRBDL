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
using System.Security.AccessControl;
using System.Security.Cryptography;

namespace CDL.filesystem
{
    class UFS
    {
        private List<string> paths;

        private static readonly List<string> SHA256s = new List<string> {
            "B683AC1B1D3F0CA6B92111DB85FC77ECE9D5C034CE5461EB8A7C4ADD8E239A22", //DOOM 3: BFG Edition
            "6DAECF3E621756C8A77B3C3064ED5FB488AFE357A80B7C14BEF35B6811B073CE" //DOOM 3 re-release (2019)
        };
        private static readonly SHA256 sHA256 = new SHA256Managed();
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
            selectedPath = paths[0];
            if (paths[0].StartsWith("/usr") || paths[0].StartsWith("/app") || CDL.testPackage)
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
                        string fileSha256 = BitConverter.ToString(sHA256.ComputeHash(data)).ToUpper().Replace("-", "");
                        if (BFGPath == null && fileSha256 == SHA256s[0])
                        {
                            string path = filePath.Substring(0, filePath.LastIndexOf(GetPathSeparator()));
                            BFGPath = path.Substring(0, path.LastIndexOf(GetPathSeparator()));
                            paths.Add(BFGPath);
                            index++;
                            this.selectedPath = paths[index];
                        }
                        else if (NewD3Path == null && fileSha256 == SHA256s[1]) { 
                            string path = filePath.Substring(0, filePath.LastIndexOf(GetPathSeparator()));
                            NewD3Path = path.Substring(0, path.LastIndexOf(GetPathSeparator()));
                            paths.Add(NewD3Path);
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
                fullPath = selectedPath + GetPathSeparator() + filename;
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
                string path = paths[i] + GetPathSeparator() + filename;
                if (File.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetPathSeparator()
        {
            return isUnixFS() ? "/" : "\\";
        }

        public string getFullPath(string filename)
        {
            List<string> foundPaths = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i] + GetPathSeparator() + filename;
                if (File.Exists(path))
                {
                    return path;
                }
            }
            /*if (foundPaths.Count > 1)
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
            }*/

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
                if (Directory.Exists(paths[i] + GetPathSeparator() + relativeFolder))
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
                    string path = paths[i] + GetPathSeparator() + filenames[j];
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
                DirectoryInfo directoryInfo = new DirectoryInfo(parentPath);
                if (directoryInfo.Attributes.HasFlag(FileAttributes.ReparsePoint)) { 
                    return filePaths;
                }
                //GK: This is the worst way to handle this but I couldn't find a better way to do that
                try
                {
                    if (parentPath.Contains("base"))
                    {
                        string[] files = Directory.GetFiles(parentPath, filename);
                        if (files.Length > 0)
                        {
                            filePaths.AddRange(files);
                        }
                    }
                    else
                    {
                        List<string> folders = new List<string>(Directory.EnumerateDirectories(parentPath, "*", SearchOption.TopDirectoryOnly));
                        if (folders.Count > 0)
                        {
                            foreach (string path in folders)
                            {
                                filePaths.AddRange(searchFile(path, filename));
                                /*if (filePaths.Count > 0)
                                {
                                    break; //GK: Abrudly stop for performance
                                }*/
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

        public void SetSelectedPath(string path)
        {
            this.selectedPath = path;
        }
    }
}
