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
using Avalonia.Controls;
using CDL.filesystem;
using dbfal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CDL.parser
{
    class ModParser
    {
        private static ExpParser parser = new ExpParser();
        public int SetD3Mod(string name, ComboBox comboBox)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (name == ((ComboBoxItem)comboBox.Items[i]).Content.ToString())
                {
                    return i;
                }
            }
            return 0;
        }

        public string[] deserializeMods(string mods)
        {
            return mods.Split(',');
        }
        public string serializeMods(string[] items)
        {
            string result = "";
            bool one = true;
            foreach (string item in items)
            {
                if (!one)
                    result += ",";

                result += item;
                one = false;
            }
            return result;
        }

        public void saveD2Mods(MainWindow mainWindow, StreamWriter sw, SettingDef settingDef)
        {
            sw.WriteLine("smod = " + ExpParser.getD2MExp(((ComboBoxItem)((ComboBox)settingDef.getDefs()["[DOOMII]"]["smod"]).SelectedItem).Content.ToString()));
            for (int i = 0; i < 7; i++)
            {
                sw.WriteLine("modex = " + i);
                sw.WriteLine("mods = " + serializeMods(mainWindow.ml[i].ToArray()));
            }
        }

        public string loadD2Mods(MainWindow mainWindow, SettingDef settingDef, string smod, StreamReader reader)
        {
            int selected = Convert.ToInt32(smod);
            string line = "";
            string[] inline;
            int index = 0;
            ((ComboBox)settingDef.getDefs()["[DOOMII]"]["smod"]).SelectedItem = ((ComboBox)settingDef.getDefs()["[DOOMII]"]["smod"]).
            Items.First(item => ((ComboBoxItem)item).Content.ToString() == ExpParser.setD2MExp(selected));
            for (int i = 0; i < 7; i++)
            {
                line = reader.ReadLine();
                inline = line.Split(new string[] { " = " }, StringSplitOptions.None);
                if (inline[0] != "modex") break;
                index = Convert.ToInt32(inline[1]);
                line = reader.ReadLine();
                inline = line.Split(new string[] { " = " }, StringSplitOptions.None);
                string[] mods = deserializeMods(inline[1]);
                if (mods[0].Length != 0)
                {
                    if (index == selected) ((ListBox)settingDef.getDefs()["[DOOMII]"]["mods"]).Items.AddRangeListBox(mods);
                    mainWindow.ml[index].AddRange(mods);
                }
                if (i == 6) line = null;
            }
            return line;
        }
    }
}
