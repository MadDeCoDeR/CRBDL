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
using System.IO;
using System.Reflection;

namespace CDL.filesystem
{
    class UFS
    {
        private string[] paths;
        public UFS()
        {
            paths = new string[2];
            paths[0] = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            paths[1] = Directory.GetCurrentDirectory();
        }

        public bool Exists(string filename)
        {
            for (int i = 0; i < 2; i++)
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
            for (int i = 0; i < 2; i++)
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

        public string getCurrentDirectory(string [] filenames)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
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
                for (int i = 0; i < 2; i++)
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
    }
}
