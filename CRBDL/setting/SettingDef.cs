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
using CDL.setting;
using CRBDL;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CDL.filesystem
{
    class SettingDef
    {
        private Dictionary<string, Dictionary<string, Control>> defs = new Dictionary<string, Dictionary<string, Control>>();

        public SettingDef(Form1 form1)
        {
            D1defs d1Defs = new D1defs(form1);
            D2defs d2Defs = new D2defs(form1);
            D12defs d12Defs = new D12defs(form1);
            D3defs d3Defs = new D3defs(form1);
            defs.Add("[DOOM]", d1Defs.getDefs());
            defs.Add("[DOOMII]", d2Defs.getDefs());
            defs.Add("[DOOM I & II]", d12Defs.getDefs());
            defs.Add("[DOOM3]", d3Defs.getDefs());
        }

        public Dictionary<string, Dictionary<string, Control>> getDefs() { return defs; }
    }
}
