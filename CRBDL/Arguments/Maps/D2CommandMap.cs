﻿/*
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
    class D2CommandMap
    {
        private Dictionary<string, string> commands = new Dictionary<string, string>();

        public D2CommandMap()
        {
            commands.Add(ArgKeys.CHEAT, "-classich ");
            commands.Add(ArgKeys.MOD, "-file ");
            commands.Add(ArgKeys.SKILL, "-skill ");
            commands.Add(ArgKeys.EPISODE, "-exp ");
            commands.Add(ArgKeys.MAP, "-warp ");
            commands.Add(ArgKeys.MENU, "+set cl_expMenu ");
        }

        public Dictionary<string, string> getCommands() { return commands; }
    }
}
