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
using CRBDL;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CDL.Arguments
{
    class ControlDefs
    {
        private Dictionary<string, Dictionary<string, Control>> controls = new Dictionary<string, Dictionary<string, Control>>();

        public ControlDefs(Form1 form1)
        {
            D3ControlMap d3Command = new D3ControlMap(form1);
            D12ControlMap d12Command = new D12ControlMap(form1);
            D1ControlMap d1Command = new D1ControlMap(form1);
            D2ControlMap d2Command = new D2ControlMap(form1);
            controls.Add("DOOM3", d3Command.getControls());
            controls.Add("DOOM1&2", d12Command.getControls());
            controls.Add("DOOM1", d1Command.getControls());
            controls.Add("DOOM2", d2Command.getControls());
        }

        public Dictionary<string, Dictionary<string, Control>> getControls() { return controls; }
    }
}
