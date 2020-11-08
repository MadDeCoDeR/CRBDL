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
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CDL.filesystem
{
    class CDLSetting
    {
        private static ExpParser parser = new ExpParser();
        private static ModParser modParser = new ModParser();
        public void saveSettings(string path, Form1 form1)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path);

                SettingDef settingDef = new SettingDef(form1);
                foreach (string key in settingDef.getDefs().Keys) {
                    sw.WriteLine(key);
                    foreach (string subkey in settingDef.getDefs()[key].Keys)
                    {
                        switch (subkey)
                        {
                            case "AA":
                            case "EX":
                            case "fo":
                                sw.WriteLine(subkey + " = " + ((NumericUpDown)settingDef.getDefs()[key][subkey]).Value);
                                break;
                            case "classich":
                            case "console":
                            case "SM":
                            case "HDR":
                            case "SSAO":
                            case "Skip Intro":
                            case "as":
                                sw.WriteLine(subkey + " = " + ((CheckBox)settingDef.getDefs()[key][subkey]).Checked);
                                break;
                            case "Episode":
                                if (key == "[DOOMII]")
                                {
                                    sw.WriteLine(subkey + " = " + ExpParser.getD2Exp(((ComboBox)settingDef.getDefs()[key][subkey]).SelectedItem.ToString()));
                                }
                                else {
                                    sw.WriteLine(subkey + " = " + ((ComboBox)settingDef.getDefs()[key][subkey]).SelectedIndex);
                                }
                                break;
                            case "Map":
                            case "Skill":
                            case "Game":
                            case "CL":
                                sw.WriteLine(subkey + " = " + ((ComboBox)settingDef.getDefs()[key][subkey]).SelectedIndex);
                                break;
                            case "marg":
                                sw.WriteLine(subkey + " = " + ((TextBox)settingDef.getDefs()[key][subkey]).Text);
                                break;
                            case "mods":
                                if (key == "[DOOMII]")
                                {
                                    modParser.saveD2Mods(form1, sw, settingDef);
                                }
                                else if (key == "[DOOM3]")
                                {
                                    sw.WriteLine(subkey + " = " + ((ComboBox)settingDef.getDefs()[key][subkey]).SelectedItem.ToString());
                                }
                                else
                                {
                                    sw.WriteLine(subkey + " = " + modParser.serializeMods(((ListBox)settingDef.getDefs()[key][subkey]).Items.Cast<string>().ToArray()));
                                }
                                break;
                            case "modbase":
                                sw.WriteLine(subkey + " = " + ((ComboBox)settingDef.getDefs()[key][subkey]).SelectedItem.ToString());
                                break;
                        }
                    }
                }
                sw.WriteLine("");
                //Close the file
                sw.Close();
                form1.getLabel10().Text = path;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        public void loadSettings(Stream stream, Form1 form1, string filename)
        {
            SettingDef settingDef = new SettingDef(form1);
            using (stream)
            {
                form1.resetUi();
                try
                {
                    StreamReader sr = new StreamReader(stream);
                    string key = "";
                    string line = sr.ReadLine();
                    string[] inline;
                    while (!sr.EndOfStream)
                    {
                        if (settingDef.getDefs().ContainsKey(line))
                        {
                            key = line;
                        }
                        else
                        {
                            inline = line.Split(new string[] { " = " }, StringSplitOptions.None);
                            switch (inline[0])
                            {
                                case "AA":
                                case "EX":
                                case "fo":
                                   ((NumericUpDown)settingDef.getDefs()[key][inline[0]]).Value = Convert.ToDecimal(inline[1]);
                                    break;
                                case "classich":
                                case "console":
                                case "SM":
                                case "HDR":
                                case "SSAO":
                                case "Skip Intro":
                                case "as":
                                    ((CheckBox)settingDef.getDefs()[key][inline[0]]).Checked = Convert.ToBoolean(inline[1]);
                                    break;
                                case "Episode":
                                    if (key == "[DOOMII]")
                                    {
                                        ((ComboBox)settingDef.getDefs()[key][inline[0]]).SelectedItem = parser.setD2Exp(Convert.ToInt32(inline[1]));
                                    }
                                    else {
                                        ((ComboBox)settingDef.getDefs()[key][inline[0]]).SelectedIndex = Convert.ToInt32(inline[1]);
                                    }
                                    break;
                                case "Map":
                                case "Skill":
                                case "Game":
                                case "CL":
                                    ((ComboBox)settingDef.getDefs()[key][inline[0]]).SelectedIndex = Convert.ToInt32(inline[1]);
                                    break;
                                case "marg":
                                    ((TextBox)settingDef.getDefs()[key][inline[0]]).Text = inline[1];
                                    break;
                                case "mods":
                                    if (key == "[DOOMII]")
                                    {
                                    }
                                    else if (key == "[DOOM3]")
                                    {
                                        ((ComboBox)settingDef.getDefs()[key][inline[0]]).SelectedIndex = modParser.SetD3Mod(inline[1], (ComboBox)settingDef.getDefs()[key][inline[0]]);
                                    }
                                    else
                                    {
                                        string[] mods = modParser.deserializeMods(inline[1]);
                                        if (mods[0].Length != 0)((ListBox)settingDef.getDefs()[key][inline[0]]).Items.AddRange(mods);
                                    }
                                    break;
                                case "modbase":
                                    ((ComboBox)settingDef.getDefs()[key][inline[0]]).SelectedIndex = modParser.SetD3Mod(inline[1], (ComboBox)settingDef.getDefs()[key][inline[0]]);
                                    break;
                                case "smod":
                                    line = modParser.loadD2Mods(form1, settingDef, inline[1], sr);
                                    if (line != null) continue;
                                    break;
                            }
                        }
                        line = sr.ReadLine();
                        }
                    sr.Close();
                    form1.getLabel10().Text = filename;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}