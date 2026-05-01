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
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

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
            List<FilePickerFileType> filters = new List<FilePickerFileType>();
            foreach(FilterFormat filter in Filters.CDOOMMODFILTERS)
            {
                filters.Add(new FilePickerFileType(filter.Name)
                {
                    Patterns = filter.Type.Split(";")
                });
            }
            IReadOnlyList<IStorageFile>? files = await TopLevel.GetTopLevel(listBox).StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                FileTypeFilter = filters
            });

            if (files.Count > 0)
            {
                 string filePath = "";
                try
                {
                    Stream myStream;
                    if ((myStream = await files[0].OpenReadAsync()) != null)
                    {
                        using (myStream)
                        {
                            filePath = ufs.getRelativePath(files[0].Path.LocalPath, "base");
                            listBox.Items.Add(filePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    IMsBox<ButtonResult> messageBox = MessageBoxManager.GetMessageBoxStandard("Error", "Error: Could not read file from disk. Original error: " + ex.Message, ButtonEnum.Ok);
                    ButtonResult result = await messageBox.ShowAsync();
                }
                return filePath;
            }
            return "";
        }
    }
}
