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
using CDL.Arguments;
using CDL.Expansions;
using CDL.filesystem;
using CDL.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace CRBDL
{

    public partial class Form1 : Form
    {
        public List<string>[] ml { set; get; }
        public string[] adcoms;
        private UFS UFS;
        private CDLSetting setting = new CDLSetting();
        private ModLoader modLoader = new ModLoader();

        private static string[] filenames = { "DoomBFA.exe", "DoomBFA", "RBDoom3BFG.exe", "RBDoom3BFG", "Doom3BFG.exe" };

        public Form1()
        {
            InitializeComponent();

            UFS = new UFS();

            adcoms = new string[3];
            for(int i = 0; i < 3; i++)
            {
                adcoms[i] = "";
            }

            ml = new List<string>[5];
            for (int i=0; i < 5; i++)
            {
                ml[i]= new List<string>();
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox10.SelectedIndex = 0;
            comboBox11.SelectedIndex = 0;
            comboBox12.SelectedIndex = 0;
            comboBox14.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;

            int found = 0;
            foreach (string filename in filenames)
            {
                if (UFS.Exists(filename))
                {
                    found++;
                }
            }

            if (found == 0)
            {
                if (MessageBox.Show("Main executable not found", "Error", MessageBoxButtons.OK) == DialogResult.OK) {
                    Process.GetCurrentProcess().Kill();
                }
            }
            int offset = 0;
            if (!UFS.Exists("base/wads/NERVE.wad"))
            {
                comboBox2.Items.Remove(comboBox2.Items[2]);
                comboBox10.Items.Remove(comboBox10.Items[4]);
                offset++;
            }
            if (!UFS.Exists("base/wads/MASTERLEVELS.wad"))
            {
                comboBox2.Items.Remove(comboBox2.Items[5 - offset]);
                comboBox10.Items.Remove(comboBox10.Items[3]);
            }
            if (!UFS.Exists("base/wads/PLUTONIA.WAD"))
            {
                comboBox2.Items.Remove(comboBox2.Items[4 - offset]);
                comboBox10.Items.Remove(comboBox10.Items[2]);
            }
            if (!UFS.Exists("base/wads/TNT.WAD"))
            {
                comboBox2.Items.Remove(comboBox2.Items[3 - offset]);
                comboBox10.Items.Remove(comboBox10.Items[1]);
            }
           
            folderBrowserDialog1 = new FolderBrowserDialog();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Launchgame();
        }

        public void Launchgame()
        {
            string args = ArgParser.ParseArgs(this);
            Process crbd = new Process();
            
            foreach (string filename in filenames) {
                if (UFS.Exists(filename))
                {
                    if ((UFS.isUnixFS() && filename.EndsWith(".exe")) || (!UFS.isUnixFS() && !filename.EndsWith(".exe")))
                    {
                        continue;
                    }
                    crbd.StartInfo.FileName = UFS.getFullPath(filename);
                    break;
                }
            }
            if (crbd.StartInfo.FileName == null || crbd.StartInfo.FileName == "")
            {
                if (MessageBox.Show("Unable to find the main executable for this System", "Error", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
            crbd.StartInfo.WorkingDirectory = UFS.getCurrentDirectory(filenames);
            StreamWriter sw = new StreamWriter("args.txt");
            sw.Write(args);
            sw.Close();
            crbd.StartInfo.Arguments = args; // if you need some
            crbd.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<string> dirs = new List<string>(Directory.GetDirectories(UFS.getCurrentDirectory(filenames)));
            foreach (var dir in dirs)
            {
                string tdir = dir.Substring(dir.LastIndexOf("\\") + 1);
                tdir = tdir.Substring(tdir.LastIndexOf("/") + 1);
                if (tdir != "base" && tdir != "directx" && !tdir.StartsWith("msvc"))
                {
                    comboBox11.Items.Add(tdir);
                    comboBox15.Items.Add(tdir);
                }
            }
            if (Settings.Default.defaultSettings != "")
            {
                Stream stream = new FileStream(Settings.Default.defaultSettings, FileMode.OpenOrCreate);
                setting.loadSettings(stream, this, Settings.Default.defaultSettings);
                button12.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            modLoader.loadMod(listBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ml[comboBox10.SelectedIndex].Add(UFS.getRelativePath(modLoader.loadMod(listBox2), "base"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ml[comboBox10.SelectedIndex].Remove(listBox2.SelectedItem.ToString());
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Levels levels = new Levels();
            switch (comboBox2.SelectedItem.ToString())
            {
                case Names.D2:
                    levels.setLevels(33, comboBox3);
                    break;
                case Names.TNT:
                case Names.PLUTONIA:
                    levels.setLevels(32, comboBox3);
                    break;
                case Names.NERVE:
                    levels.setLevels(9, comboBox3);
                    break;
                case Names.MASTER:
                    levels.setLevels(21, comboBox3);
                    break;
                default:
                    if (comboBox3.SelectedIndex > 0) comboBox3.SelectedIndex = 0;
                    break;

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != 0 && comboBox2.SelectedIndex <= 0)
            {
                comboBox2.SelectedIndex = 1;
            }
            /*else if (comboBox3.SelectedIndex == 0 && comboBox2.SelectedIndex > 0)
            {
                comboBox2.SelectedIndex = 0;
            }*/
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex != 0 && comboBox5.SelectedIndex <= 0)
            {
                comboBox5.SelectedIndex = 1;
            }else if (comboBox4.SelectedIndex == 0 && comboBox5.SelectedIndex > 0)
            {
                comboBox5.SelectedIndex = 0;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex != 0 && comboBox4.SelectedIndex <= 0)
            {
                comboBox4.SelectedIndex = 1;
            }
            else if (comboBox5.SelectedIndex == 0 && comboBox4.SelectedIndex > 0)
            {
                comboBox4.SelectedIndex = 0;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Filters.SETTINGFILE;
            sfd.Title = "Save current configuration";
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                setting.saveSettings(sfd.FileName, this);
                button11.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = Filters.SETTINGFILE;
            ofd.Title = "Load configuration";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = ofd.OpenFile()) != null)
                {
                    setting.loadSettings(myStream, this, ofd.FileName);
                    button11.Enabled = true;
                }
            }
        }

        public void resetUi()
        {
            comboBox1.SelectedIndex = 0;
           // comboBox1.ResetText();
            comboBox2.SelectedIndex = 0;
          //  comboBox2.ResetText();
            comboBox3.SelectedIndex = 0;
          //  comboBox3.ResetText();
            comboBox4.SelectedIndex = 0;
          //  comboBox4.ResetText();
            comboBox5.SelectedIndex = 0;
           // comboBox5.ResetText();
            comboBox6.SelectedIndex = 0;
          //  comboBox6.ResetText();
            comboBox7.SelectedIndex = 0;
          //  comboBox7.ResetText();
            numericUpDown2.Value = 8;
          //  comboBox8.ResetText();
            comboBox9.SelectedIndex = 0;
          //  comboBox9.ResetText();
            comboBox10.SelectedIndex = 0;
            comboBox11.SelectedIndex = 0;
            comboBox12.SelectedIndex = 0;
            numericUpDown3.Value = (decimal)0.01;
            comboBox14.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            //    comboBox11.ResetText();
            checkBox1.CheckState = CheckState.Unchecked;
            checkBox2.CheckState = CheckState.Unchecked;
            checkBox3.CheckState = CheckState.Unchecked;
            checkBox4.CheckState = CheckState.Checked;
            checkBox5.CheckState = CheckState.Checked;
            checkBox8.CheckState = CheckState.Checked;
            checkBox9.CheckState = CheckState.Unchecked;
            checkBox10.CheckState = CheckState.Unchecked;
            checkBox11.CheckState = CheckState.Unchecked;
            checkBox12.CheckState = CheckState.Unchecked;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            numericUpDown1.Value=0.5M;
            for (int i =0; i < 5; i++)
            {
                ml[i].Clear();
            }
            textBox15.Text = "";
            textBox20.Text = "";
            textBox22.Text = "";
            textBox24.Text = "";

            button3.Enabled = false;
            button4.Enabled = false;
            button9.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            modLoader.loadMod(listBox3);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox3.Items.Remove(listBox3.SelectedItem);
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            button4.Enabled = false;
            if ( ml[comboBox10.SelectedIndex].Count >= 1)
            {
                foreach (string file in ml[comboBox10.SelectedIndex])
                {
                    listBox2.Items.Add(file);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CDL.Form2 f2 = new CDL.Form2(this);
            f2.ShowDialog();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
                label11.Enabled = ((CheckBox)sender).Checked;
                numericUpDown2.Enabled = ((CheckBox)sender).Checked;
                checkBox4.Enabled = ((CheckBox)sender).Checked;
                checkBox5.Enabled = ((CheckBox)sender).Checked;
                label12.Enabled = ((CheckBox)sender).Checked;
                numericUpDown1.Enabled = ((CheckBox)sender).Checked;
                checkBox9.Enabled = ((CheckBox)sender).Checked;
                //label13.Enabled = ((CheckBox)sender).Checked;
                //comboBox12.Enabled = ((CheckBox)sender).Checked;
                label25.Enabled = ((CheckBox)sender).Checked;
                numericUpDown3.Enabled = ((CheckBox)sender).Checked;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = listBox1.SelectedItems.Count > 0;
        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button4.Enabled = listBox2.SelectedItems.Count > 0;
        }

        private void ListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button9.Enabled = listBox3.SelectedItems.Count > 0;
        }

        public ListBox getListBox1() { return listBox1; }
        public ComboBox getComboBox1() { return comboBox1; }
        public ListBox getListBox2() { return listBox2; }
        public ComboBox getComboBox2() { return comboBox2; }
        public ListBox getListBox3() { return listBox3; }
        public ComboBox getComboBox3() { return comboBox3; }
        public ComboBox getComboBox4() { return comboBox4; }
        public ComboBox getComboBox5() { return comboBox5; }
        public ComboBox getComboBox6() { return comboBox6; }
        public ComboBox getComboBox7() { return comboBox7; }
        public ComboBox getComboBox9() { return comboBox9; }
        public ComboBox getComboBox10() { return comboBox10; }
        public ComboBox getComboBox11() { return comboBox11; }
        public ComboBox getComboBox12() { return comboBox12; }
        public ComboBox getComboBox14() { return comboBox14; }
        public ComboBox getComboBox15() { return comboBox15; }
        public Label getLabel10() { return label10; }
        public TextBox getTextBox15() { return textBox15; }
        public TextBox getTextBox20() { return textBox20; }
        public TextBox getTextBox22() { return textBox22; }
        public TextBox getTextBox24() { return textBox24; }
        public NumericUpDown getNumericUpDown1() { return numericUpDown1; }
        public NumericUpDown getNumericUpDown2() { return numericUpDown2; }
        public NumericUpDown getNumericUpDown3() { return numericUpDown3; }
        public CheckBox getCheckBox1() { return checkBox1; }
        public CheckBox getCheckBox2() { return checkBox2; }
        public CheckBox getCheckBox3() { return checkBox3; }
        public CheckBox getCheckBox4() { return checkBox4; }
        public CheckBox getCheckBox5() { return checkBox5; }
        public CheckBox getCheckBox8() { return checkBox8; }
        public CheckBox getCheckBox9() { return checkBox9; }
        public CheckBox getCheckBox10() { return checkBox10; }
        public CheckBox getCheckBox11() { return checkBox11; }
        public CheckBox getCheckBox12() { return checkBox12; }

        private void button11_Click(object sender, EventArgs e)
        {
            Settings.Default.defaultSettings = label10.Text;
            Settings.Default.Save();
            button11.Enabled = false;
            button12.Enabled = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Settings.Default.defaultSettings = "";
            Settings.Default.Save();
            button12.Enabled = false;
            if (label10.Text != "")
            {
                button11.Enabled = true;
            }
        }
    }


}
