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
namespace CDL.filesystem
{
    class Filters
    {
        public static readonly FilterFormat[] CDOOMMODFILTERS =
        {
            new FilterFormat{ Name = "All Files", Type = "*.wad;*.WAD;*.deh;*.DEH;*.bex;*.BEX;*.dlc;*.DLC;*.zip;*.ZIP"},
            new FilterFormat{ Name = "wad files (*.wad)", Type = "*.wad;*.WAD"},
            new FilterFormat{ Name = "DeHackeD files (*.deh)", Type = "*.deh;*.DEH"},
            new FilterFormat{ Name = "Extended DeHackeD files (*.bex)", Type = "*.bex;*.BEX"},
            new FilterFormat{ Name = "Expansion Info (*.dlc)", Type = "*.dlc;*.DLC"},
            new FilterFormat{ Name = "zip files (*.zip)", Type = "*.zip;*.ZIP"}
        };
        public static readonly FilterFormat SETTINGSFILTER = new FilterFormat
        {
            Name = "DOOM BFA Launcher Settings (*.cdl)",
            Type = "*.cdl"
        };
    }

    class FilterFormat
    {
        public string? Name {get; set;}
        public string? Type {get; set;}
    }
}
