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
using System.Security;
using System.Security.Permissions;

namespace CDL.filesystem
{
    class UFS
    {
        private string[] paths;
        DesktopBridge.Helpers helpers;
        public UFS()
        {
            paths = new string[5];
            paths[0] = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            paths[1] = Directory.GetCurrentDirectory();
            paths[2] = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/../DoomBFA";
            if (paths[0].StartsWith("/usr"))
            {
                paths[3] = "/usr/bin";
                List<string> files = searchFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "_common.resources");
                if (files.Count >= 1)
                {
                    paths[4] = files[0].Substring(0, files[0].LastIndexOf("/"));
                    paths[4] = paths[4].Substring(0, paths[4].LastIndexOf("/"));
                } else
                {
                    paths[4] = "";
                }
                
            } else
            {
                paths[3] = "";
                paths[4] = "";
            }
            helpers = new DesktopBridge.Helpers();
        }

        public string createFullPath(string filename) {
            string fullPath;

            if (isUnixFS() || helpers.IsRunningAsUwp())
            {
                fullPath = paths[0]+ "/" + filename;
            } else
            {
                fullPath = filename;
            }

            return fullPath;
        }

        public bool Exists(string filename)
        {
            for (int i = 0; i < paths.Length; i++)
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
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i] + "/" + filename;
                if (File.Exists(path))
                {
                    return path;
                }
            }
            return filename;
        }

        public bool isUnixFS()
        {
            return paths[0].StartsWith("/");
        }

        public bool isRunningAsUWP()
        {
            return helpers.IsRunningAsUwp() || paths[0].StartsWith("/usr");
        }

        public string getParentPath(string relativeFolder)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                if (Directory.Exists(paths[i] + "/" + relativeFolder))
                {
                    return paths[i];
                }
            }
            return "";
        }

        public string getCurrentDirectory(string [] filenames)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                for (int j = 0; j < filenames.Length; j++)
                {
                    string path = paths[i] + "/" + filenames[j];
                    if (File.Exists(path))
                    {
                        return paths[i];
                    }
                }
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
                for (int i = 0; i < paths.Length; i++)
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
            if (!parentPath.Contains("z:"))
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
    }
}
