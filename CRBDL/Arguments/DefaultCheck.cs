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
    class DefaultCheck
    {
        public static bool checkControls(ControlDefs controls, string key, Form1 form1)
        {
            bool isDefault = true;
            foreach (string internalkey in controls.getControls()[key].Keys)
            {
                isDefault = checkControl(controls.getControls()[key][internalkey], key, internalkey, form1);
                if (!isDefault) break;
            }
            return isDefault;
        }

        public static bool checkControl(Control control, string superKey, string key, Form1 form1)
        {
            bool isDefault = true;
            switch (key)
            {
                case ArgKeys.CHEAT:
                    if (((CheckBox)control).Checked) { isDefault = false; }
                    break;
                case ArgKeys.EPISODE:
                case ArgKeys.MAP:
                case ArgKeys.SKILL:
                    if (((ComboBox)control).SelectedIndex > 0) { isDefault = false; }
                    break;
                case ArgKeys.MOD:
                    if (superKey == "DOOM2")
                    {
                        foreach (List<string> mods in form1.ml)
                        {
                            if (mods.Count > 0) { isDefault = false; }
                        }
                    }
                    else
                    {
                        if (((ListBox)control).Items.Count > 0) { isDefault = false; }
                    }
                    break;
                case ArgKeys.EXTRA:
                    if (((TextBox)control).Text != "")
                    {
                        isDefault = false;
                    }
                    break;
            }
            return isDefault;
        }
    }
}
