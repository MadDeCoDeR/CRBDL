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
using CDL.Arguments.Maps.Settings;
using CDL.parser;
using CRBDL;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace CDL.Arguments
{
    class ArgParser
    {
        public static string ParseArgsFromForm(Form1 form1)
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
                            if (!DefaultCheck.checkControl(controlDefs.getControls()[key][subKey], key, subKey, form1))
                            {
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
                                if (((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem != null)
                                {
                                    args += commandDefs.getCommands()[key][subKey]
                                        + $"{((string)((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem).ToLower()} ";
                                }
                            }
                            else
                            {
                                if (form1.GetTextBox1().Text != null && form1.GetTextBox1().Text != "")
                                {
                                    args += commandDefs.getCommands()[key][subKey]
                                        + $"{form1.GetTextBox1().Text.ToLower()} ";
                                }
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
                        case ArgKeys.GAME_PATH:
                            if (((ComboBox)controlDefs.getControls()[key][subKey]).SelectedIndex > 0)
                            {
                                args += commandDefs.getCommands()[key][subKey]
                                    + $"\"{((ComboBox)controlDefs.getControls()[key][subKey]).SelectedItem.ToString().Split("--".ToCharArray())[2].Trim()}\" ";
                            }
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

        public static string ParseArgsFromSettings(Stream stream)
        {
            string args = "";
            string hargs = " ";
            ExpParser parser = new ExpParser();
            ModParser modParser = new ModParser();
            using (stream)
            {
                try
                {
                    StreamReader sr = new StreamReader(stream);
                    string key = "";
                    string line = sr.ReadLine();
                    string[] inline;
                    int ex = 0;
                    int firstEx = 1;
                    bool useHargs = false;
                    while (!sr.EndOfStream)
                    {
                        if (SettingsArgsDef.settingsArgDefs.ContainsKey(line))
                        {
                            key = line;
                            useHargs = false;
                            switch (key)
                            {
                                case "[DOOM]":
                                    args += "-doom ";
                                    break;
                                case "[DOOMII]":
                                    args += "-doom2 ";
                                    break;
                                case "[DOOM I & II]":
                                    useHargs = true;

                                    break;
                            }
                        }
                        else
                        {
                            inline = line.Split(new string[] { " = " }, StringSplitOptions.None);


                            switch (inline[0])
                            {
                                /*case "AA":
                                case "EX":
                                case "fo":
                                    args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {Convert.ToDecimal(inline[1]).ToString(CultureInfo.InvariantCulture)} ";
                                    break;*/
                                case "classich":
                                case "console":
                                /*case "SM":
                                case "HDR":
                                case "SSAO":*/
                                case "Skip Intro":
                                    /*case "as":
                                    case "USE_CUL":*/
                                    bool value = Convert.ToBoolean(inline[1]);
                                    if (value)
                                    {
                                        if (useHargs)
                                        {
                                            hargs += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} ";
                                        }
                                        else
                                        {
                                            args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} ";
                                        }
                                    }
                                    break;
                                case "Episode":
                                    int val = Convert.ToInt32(inline[1]);
                                    if (val > 0)
                                    {
                                        val -= 1;
                                        if (key == "[DOOMII]")
                                        {
                                            args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {val} ";
                                        }
                                        else
                                        {
                                            args += $"-episode {val} ";
                                        }
                                    }
                                    break;
                                case "Map":
                                case "Skill":
                                case "Game":
                                    /*case "CL":*/
                                    if (inline[1] != "0")
                                    {
                                        int va = Convert.ToInt32(inline[1]);
                                        if (inline[0] != "Game")
                                        {
                                            va -= 1;
                                        }
                                        if (useHargs)
                                        {
                                            hargs += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {va} ";
                                        }
                                        else
                                        {
                                            args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {va} ";
                                        }
                                    }
                                    break;
                                case "marg":
                                    /*case "CUL":*/
                                    if (inline[1].Length > 0)
                                    {
                                        if (useHargs)
                                        {
                                            hargs += inline[1] + " ";
                                        }
                                        else
                                        {
                                            args += inline[1] + " ";
                                        }
                                    }
                                    break;
                                case "modex":
                                    ex = ExpParser.getD2MExp(ExpParser.setD2MExp(Convert.ToInt32(inline[1]))) + 1;
                                    break;
                                case "mods":
                                    if (key == "[DOOMII]")
                                    {
                                        string[] mods = modParser.deserializeMods(inline[1]);
                                        if (mods.Length > 0 && mods[0].Length > 0)
                                        {
                                            if (ex == firstEx)
                                            {
                                                args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} ";
                                            }

                                            args += $"ex {ex} {string.Join(" ", mods)} ";
                                        }
                                        else
                                        {
                                            firstEx++;
                                        }
                                    }
                                    else if (key == "[DOOM3]")
                                    {
                                        if (inline[1] != "(none)")
                                        {
                                            args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {inline[1]} ";
                                        }
                                    }
                                    else
                                    {
                                        string[] mods = modParser.deserializeMods(inline[1]);
                                        if (mods[0].Length != 0)
                                        {
                                            if (useHargs)
                                            {
                                                hargs += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {string.Join(" ", mods)} ";
                                            }
                                            else
                                            {
                                                args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {string.Join(" ", mods)} ";
                                            }
                                        }
                                    }
                                    break;
                                case "modbase":
                                    if (inline[1] != "(none)")
                                    {
                                        args += $"{SettingsArgsDef.settingsArgDefs[key][inline[0]]} {inline[1]} ";
                                    }
                                    break;
                                case "game_path":
                                    if (inline[1] != "(default)")
                                    {
                                        args += SettingsArgsDef.settingsArgDefs[key][inline[0]] +  $" \"{inline[1].Split("--".ToCharArray())[2].Trim()}\" ";
                                    }
                                    break;
                            }
                        }
                        line = sr.ReadLine();
                    }
                    sr.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return hargs + args;
        }

        public static bool IsNewArgument(string arg)
        {
            bool result = arg.Equals("+set");
            if (!result )
            {
                if (arg.Length> 1)
                {
                    result = arg[0] == '-' && int.TryParse(arg.Substring(1), out int test);
                }
            }

            return result;
        }
    }
}
