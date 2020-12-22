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
using CDL.parser;
using CRBDL;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace CDL.Arguments
{
    class ArgParser
    {
        public static string ParseArgs(Form1 form1)
        {
            string args = " ";
            bool extrArg = false;
            CommandDefs commandDefs = new CommandDefs();
            ControlDefs controlDefs = new ControlDefs(form1);
            foreach (string key in commandDefs.getCommands().Keys)
            {
                if (key == "DOOM1" || key == "DOOM2")
                {
                    if (DefaultCheck.checkControls(controlDefs, key, form1)) { continue; }
                    switch (key)
                    {
                        case "DOOM1":
                            args += "-doom ";
                            break;
                        case "DOOM2":
                            args += "-doom2 ";
                            break;
                    }
                }
                foreach (string subKey in commandDefs.getCommands()[key].Keys)
                {
                    switch (subKey)
                    {
                        case ArgKeys.AF:
                        case ArgKeys.EXPOSURE:
                        case ArgKeys.FA:
                            if (form1.getCheckBox12().Checked)
                            {
                                string value = 
                                    ((NumericUpDown)controlDefs.getControls()[key][subKey]).Value.ToString(CultureInfo.InvariantCulture);
                                args += commandDefs.getCommands()[key][subKey] + value + " ";
                            }
                            break;
                        case ArgKeys.CHEAT:
                            if (!DefaultCheck.checkControl(controlDefs.getControls()[key][subKey], key, subKey, form1))
                            {
                                args += commandDefs.getCommands()[key][subKey];
                            }
                            break;
                        case ArgKeys.CONSOLE:
                        case ArgKeys.VIDEO:
                            args += commandDefs.getCommands()[key][subKey] 
                                + $"{Convert.ToInt32(((CheckBox)controlDefs.getControls()[key][subKey]).Checked)} ";
                            break;
                        case ArgKeys.EPISODE:
                            if (!DefaultCheck.checkControl(controlDefs.getControls()[key][subKey], key, subKey, form1)) {
                                if (key == "DOOM1")
                                {
                                    args += commandDefs.getCommands()[key][subKey]
                                        + $"{((ComboBox)controlDefs.getControls()[key][subKey]).SelectedIndex}"
                                        + $" {((ComboBox)controlDefs.getControls()[key][ArgKeys.MAP]).SelectedIndex} ";
                                }
                                else
                                {
                                    int level = ((ComboBox)controlDefs.getControls()[key][ArgKeys.MAP]).SelectedIndex;
                                    if (level > 0)
                                    {
                                        args += commandDefs.getCommands()[key][subKey]
                                            + $"{ExpParser.getD2Exp(((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem.ToString())} ";
                                    }
                                }
                            }
                            break;
                        case ArgKeys.GAME_MODE:
                            args += commandDefs.getCommands()[key][subKey]
                                + $"{((ComboBox)controlDefs.getControls()[key][subKey]).SelectedIndex} ";
                            break;
                        case ArgKeys.HLL:
                        case ArgKeys.SSGI:
                        case ArgKeys.TF:
                            if (form1.getCheckBox12().Checked)
                            {
                                args += commandDefs.getCommands()[key][subKey]
                                    + $"{Convert.ToInt32(((CheckBox)controlDefs.getControls()[key][subKey]).Checked)} ";
                            }
                            break;
                        case ArgKeys.LANG:
                            if (!form1.GetCheckBox6().Checked)
                            {
                                args += commandDefs.getCommands()[key][subKey]
                                    + $"{((string)((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem).ToLower()} ";
                            } else
                            {
                                args += commandDefs.getCommands()[key][subKey]
                                    + $"{form1.GetTextBox1().Text.ToLower()} ";
                            }
                            break;
                        case ArgKeys.MAP:
                        case ArgKeys.SKILL:
                            if (!DefaultCheck.checkControl(controlDefs.getControls()[key][subKey], key, subKey, form1))
                            {
                                args += commandDefs.getCommands()[key][subKey]
                                    + $"{((ComboBox)controlDefs.getControls()[key][subKey]).SelectedIndex} ";
                            }
                            break;
                        case ArgKeys.MOD:
                        case ArgKeys.MOD_BASE:
                            if (key == "DOOM3")
                            {
                                if (((ComboBox)controlDefs.getControls()[key][subKey]).SelectedIndex > 0)
                                {
                                    if (!extrArg)
                                    {
                                        args += "+set fs_resourceLoadPriority 0 ";
                                        extrArg = true;
                                    }
                                    args += commandDefs.getCommands()[key][subKey]
                                        + $"{((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem.ToString()} ";

                                }
                            }
                            else if (key == "DOOM2")
                            {
                                if (!DefaultCheck
                                    .checkControl(controlDefs.getControls()[key][subKey],
                                    key, subKey, form1))
                                {
                                    args += commandDefs.getCommands()[key][subKey];
                                    for (int i = 0; i < form1.ml.Length; i++)
                                    {
                                        if (form1.ml[i].Count > 0)
                                        {
                                            args += $"ex {ExpParser.getD2MExp(ExpParser.setD2MExp(i)) + 1} ";
                                            foreach (string mod in form1.ml[i])
                                            {
                                                args += $"\"{mod}\" ";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!DefaultCheck
                                    .checkControl(controlDefs.getControls()[key][subKey],
                                    key, subKey, form1))
                                {
                                    args += commandDefs.getCommands()[key][subKey];
                                    foreach (string mod in ((ListBox)controlDefs.getControls()[key][subKey]).Items)
                                    {
                                        args += $"\"{mod}\" ";
                                    }
                                }
                            }
                            break;
                        case ArgKeys.MENU:
                            args += commandDefs.getCommands()[key][subKey]
                                        + $"{ExpParser.getD2MenuExp(((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem.ToString())} ";
                            break;
                    }
                }
                if (!DefaultCheck.checkControl(controlDefs.getControls()[key][ArgKeys.EXTRA], key, ArgKeys.EXTRA, form1))
                {
                    args += controlDefs.getControls()[key][ArgKeys.EXTRA].Text + " ";
                }
            }

            return args;
        }
    }
}
