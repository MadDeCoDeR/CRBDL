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
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace CDL.filesystem
{
    class ModLoader
    {
        private UFS ufs;

        public ModLoader(UFS ufs)
        {
            this.ufs = ufs;
        }
        public async Task<string> loadMod(ListBox listBox)
        {
            IReadOnlyList<IStorageFile>? files = await TopLevel.GetTopLevel(listBox).StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                SuggestedFileType = new FilePickerFileType(Filters.ALLCLASSICDOOMMODFILES)
            });

            if (files.Count > 0)
            {
                try
                {
                    Stream myStream;
                    if ((myStream = await files[0].OpenReadAsync()) != null)
                    {
                        using (myStream)
                        {
                            listBox.Items.Add(ufs.getRelativePath(files[0].Name, "base"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return files[0].Name;
        }
    }
}
