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

namespace CDL.setting
{
    class D3defs
    {
        private Dictionary<string, Control> d3defs = new Dictionary<string, Control>();

        public D3defs(Form1 form1)
        {
            d3defs.Add("mods", form1.getComboBox11());
            d3defs.Add("marg", form1.getTextBox15());
            d3defs.Add("Game", form1.getComboBox1());
            d3defs.Add("console", form1.getCheckBox3());
            d3defs.Add("AA", form1.getNumericUpDown2());
            d3defs.Add("SM", form1.getCheckBox4());
            d3defs.Add("HDR", form1.getCheckBox5());
            d3defs.Add("EX", form1.getNumericUpDown1());
            d3defs.Add("SSAO", form1.getCheckBox9());
            d3defs.Add("Skip Intro", form1.getCheckBox10());
            d3defs.Add("CL", form1.getComboBox12());
            d3defs.Add("fo", form1.getNumericUpDown3());
            d3defs.Add("as", form1.getCheckBox12());
            d3defs.Add("modbase", form1.getComboBox15());
        }

        public Dictionary<string, Control> getDefs() {
            return d3defs;
        }
    }
}
