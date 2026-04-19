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
using Avalonia.Controls;
using dbfal;
using System.Collections.Generic;

namespace CDL.setting
{
    class D3defs
    {
        private Dictionary<string, Control> d3defs = new Dictionary<string, Control>();

        public D3defs(MainWindow mainWindow)
        {
            d3defs.Add("mods", mainWindow.GetFsGame());
            d3defs.Add("marg", mainWindow.GetD3ExtraArgs());
            d3defs.Add("Game", mainWindow.GetGameMode());
            d3defs.Add("console", mainWindow.GetConsole());
            d3defs.Add("Skip Intro", mainWindow.GetSkipIntro());
            d3defs.Add("modbase", mainWindow.GetFsGameBase());
            d3defs.Add("game_path", mainWindow.GetGamePath());
        }

        public Dictionary<string, Control> getDefs()
        {
            return d3defs;
        }
    }
}
