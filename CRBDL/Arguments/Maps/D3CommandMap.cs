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
using System.Collections.Generic;

namespace CDL.Arguments
{
    class D3CommandMap
    {
        private Dictionary<string, string> commands = new Dictionary<string, string>();

        public D3CommandMap()
        {
            commands.Add(ArgKeys.GAME_MODE, "+set com_game_mode ");
            commands.Add(ArgKeys.CONSOLE, "+seta com_allowconsole ");
            commands.Add(ArgKeys.VIDEO, "+set com_skipIntroVideos ");
            commands.Add(ArgKeys.MOD, "+set fs_game ");
            commands.Add(ArgKeys.MOD_BASE, "+set fs_game_base ");
            commands.Add(ArgKeys.AF, "+set r_maxAnisotropicFiltering ");
            commands.Add(ArgKeys.HLL, "+set r_useHalfLambertLighting ");
            commands.Add(ArgKeys.TF, "+set r_useTrilinearFiltering ");
            commands.Add(ArgKeys.EXPOSURE, "+set r_exposure ");
            commands.Add(ArgKeys.SSGI, "+set r_useSSGI ");
            commands.Add(ArgKeys.LANG, "+set sys_lang ");
            commands.Add(ArgKeys.FA, "+set r_forceAmbient ");
        }

        public Dictionary<string, string> getCommands() { return commands; }
    }
}
