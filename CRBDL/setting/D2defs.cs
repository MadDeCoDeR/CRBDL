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
    class D2defs
    {
        private Dictionary<string, Control> d2defs = new Dictionary<string, Control>();

        public D2defs(Form1 form1)
        {
            d2defs.Add("classich", form1.getCheckBox1());
            d2defs.Add("mods", form1.getListBox2());
            d2defs.Add("Skill", form1.getComboBox7());
            d2defs.Add("marg", form1.getTextBox24());
            d2defs.Add("Episode", form1.getComboBox2());
            d2defs.Add("Map", form1.getComboBox3());
            d2defs.Add("smod", form1.getComboBox10());
        }

        public Dictionary<string, Control> getDefs() {
            return d2defs;
        }
    }
}
