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
    class D3ControlMap
    {
        private Dictionary<string, Control> controls = new Dictionary<string, Control>();

        public D3ControlMap(Form1 form1)
        {
            controls.Add(ArgKeys.GAME_MODE, form1.getComboBox1());
            controls.Add(ArgKeys.CONSOLE, form1.getCheckBox3());
            controls.Add(ArgKeys.VIDEO, form1.getCheckBox10());
            controls.Add(ArgKeys.MOD, form1.getComboBox11());
            controls.Add(ArgKeys.MOD_BASE, form1.getComboBox15());
            controls.Add(ArgKeys.AF, form1.getNumericUpDown2());
            controls.Add(ArgKeys.HLL, form1.getCheckBox4());
            controls.Add(ArgKeys.TF, form1.getCheckBox5());
            controls.Add(ArgKeys.EXPOSURE, form1.getNumericUpDown1());
            controls.Add(ArgKeys.SSGI, form1.getCheckBox9());
            controls.Add(ArgKeys.JLAY, form1.getComboBox12());
            controls.Add(ArgKeys.FA, form1.getNumericUpDown3());
            controls.Add(ArgKeys.EXTRA, form1.getTextBox15());
        }

        public Dictionary<string, Control> getControls() { return controls; }
    }
}
