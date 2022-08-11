/*
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
using System.Collections.Generic;

namespace CDL.Arguments.Maps.Settings
{
    class SettingsArgsDef
    {
        private static readonly Dictionary<string, string> classicDefs = new Dictionary<string, string>()
        {
            { "classich", "-classich" },
            { "mods", "-file" },
            { "Map", "-warp" },
            { "Episode", "-exp" },
            { "modex", "ex" },
            { "Skill", "-skill" }
        };

        private static readonly Dictionary<string, string> neoDefs = new Dictionary<string, string>()
        {
            { "mods", "+set fs_game" },
            { "Game", "+set com_game_mode" },
            { "console", "+set com_allowConsole" },
            { "Skip Intro", "+set com_skipIntroVideos" },
            { "modbase", "+set fs_game_base" }
        };

        public static readonly Dictionary<string, Dictionary<string, string>> settingsArgDefs = new Dictionary<string, Dictionary<string, string>>()
        {
            { "[DOOM]", classicDefs },
            { "[DOOMII]", classicDefs },
            { "[DOOM I & II]", classicDefs },
            { "[DOOM3]", neoDefs }
        };
    }
}
