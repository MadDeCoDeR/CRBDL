using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CDL.Arguments;
using CDL.filesystem;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace dbfal;

public partial class MainWindow : Window
{

    public List<string>[] ml { set; get; }
    public string[] adcoms;
    private UFS ufs;
    private CDLSetting setting = new CDLSetting();
    private ModLoader modLoader;
    private bool[] foundExps;
    private readonly CDL.CDL cdl;

    private static string[] filenames = { "DoomBFA.exe", "DoomBFA.sh", "DoomBFA", "RBDoom3BFG.exe", "RBDoom3BFG", "Doom3BFG.exe" };
    public MainWindow()
    {
        InitializeComponent();

        ufs = new UFS();
            cdl = new CDL.CDL(ufs);
            modLoader = new ModLoader(ufs);

            adcoms = new string[3];
            for (int i = 0; i < 3; i++)
            {
                adcoms[i] = "";
            }

            ml = new List<string>[5];
            for (int i = 0; i < 5; i++)
            {
                ml[i] = new List<string>();
            }

            foundExps = new bool[5];
            for (int i = 0; i < foundExps.Length; i++)
            {
                foundExps[i] = false;
            }

            if (ufs.isRunningPackaged())
            {
                Launch_Button.IsEnabled = false;
                checkIfNewPathsAdded();
            }
    }

    private async void checkIfNewPathsAdded()
        {
            await Task.Run(() =>
            {
                bool runOnce = false;
                bool enableLaunch = true;
                while (true)
                {
                    while (!cdl.isRunning)
                    {
                        if (ufs.BFGPaths.Count + ufs.NewD3Paths.Count > 1 && !runOnce)
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                GamePath_Group.IsVisible = true;
                                GamePath.IsVisible = true;
                                GamePath.Items.AddRange(ufs.BFGPaths.Select(path => "(D3: BFG Edition) -- " + path).ToArray());
                                GamePath.Items.AddRange(ufs.NewD3Paths.Select(path => "(D3: 2019) -- " + path).ToArray());
                                GamePath.SelectedIndex = 1;
                            }));
                            runOnce = true;
                        }
                        if (ufs.BFGPaths.Count + ufs.NewD3Paths.Count > 0 && enableLaunch)
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                Launch_Button.IsEnabled = true;
                                this.updateD3Mods();
                            }));
                            enableLaunch = false;
                        }

                        Thread.Sleep(4000);
                    }
                    if (ufs.isRunningPackaged())
                    {
                        while (cdl.isRunning)
                        {
                            Thread.Sleep(4000);
                        }
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            this.Show();
                        }));
                    }
                    Thread.Sleep(4000);
                }
            });
        }

    private void Launch_OnClick(object? sender, RoutedEventArgs e)
    {
        Launchgame();
    }

    public void Launchgame()
    {
        string args = ArgParser.ParseArgsFromForm(this);
        cdl.LaunchGame(args);
    }

    private void Window_Loaded(object? sender, RoutedEventArgs e)
        {
            if (!ufs.isRunningPackaged())
            {
                if (CheckFiles())
                {
                    List<string> dirs = new List<string>(Directory.GetDirectories(ufs.getParentPath("base")));
                    foreach (var dir in dirs)
                    {
                        string tdir = dir.Substring(dir.LastIndexOf("\\") + 1);
                        tdir = tdir.Substring(tdir.LastIndexOf("/") + 1);
                        if (tdir != "base" && tdir != "directx" && !tdir.StartsWith("msvc") && tdir != "base_new" && tdir != "base_BFG" && tdir != "third-party-licenses")
                        {
                            fs_game.Items.Add(tdir);
                            fs_game_base.Items.Add(tdir);
                        }
                    }
                    // if (Settings.Default.defaultSettings != "")
                    // {
                    //     Stream stream = new FileStream(Settings.Default.defaultSettings, FileMode.OpenOrCreate);
                    //     setting.loadSettings(stream, this, Settings.Default.defaultSettings);
                    //     button12.Enabled = true;
                    // }

                }
            }
            
        }

    private void checkClassicExpansions(string folderName)
        {
            foundExps = cdl.checkClassicExpansions(folderName);
            if (!foundExps[0])
            {
                int episodeIndex = D2Episode.Items.IndexOf(D2Episode.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "No Rest For the Living"));
                int expansionIndex = D2Expansion.Items.IndexOf(D2Expansion.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "No Rest For the Living"));
                D2Episode.Items.RemoveAt(episodeIndex);
                D2Expansion.Items.RemoveAt(expansionIndex);
            }
            if (!foundExps[1])
            {
                int episodeIndex = D2Episode.Items.IndexOf(D2Episode.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "Master Levels"));
                int expansionIndex = D2Expansion.Items.IndexOf(D2Expansion.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "Master Levels"));
                D2Episode.Items.RemoveAt(episodeIndex);
                D2Expansion.Items.RemoveAt(expansionIndex);
            }
            if (!foundExps[2])
            {
                int episodeIndex = D2Episode.Items.IndexOf(D2Episode.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "The Plutonia Experiment"));
                int expansionIndex = D2Expansion.Items.IndexOf(D2Expansion.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "The Plutonia Experiment"));
                D2Episode.Items.RemoveAt(episodeIndex);
                D2Expansion.Items.RemoveAt(expansionIndex);
            }
            if (!foundExps[3])
            {
                int episodeIndex = D2Episode.Items.IndexOf(D2Episode.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "TNT: Evilution"));
                int expansionIndex = D2Expansion.Items.IndexOf(D2Expansion.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "TNT: Evilution"));
                D2Episode.Items.RemoveAt(episodeIndex);
                D2Expansion.Items.RemoveAt(expansionIndex);
            }
            if (!foundExps[4])
            {
                int episodeIndex = D2Episode.Items.IndexOf(D2Episode.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "Legacy of Rust"));
                int expansionIndex = D2Expansion.Items.IndexOf(D2Expansion.Items.FirstOrDefault(item => ((ComboBoxItem)item).Content == "Legacy of Rust"));
                D2Episode.Items.RemoveAt(episodeIndex);
                D2Expansion.Items.RemoveAt(expansionIndex);
            }
        }

    private bool CheckFiles()
        {
            int found = cdl.CheckFiles();

            if (found == 0)
            {
                // IMsBox<ButtonResult> messageBox = MessageBoxManager.GetMessageBoxStandard("Error", "Main executable not found", ButtonEnum.Ok);
                // ButtonResult result = await messageBox.ShowAsync();
                // if (result == ButtonResult.Ok)
                {
                    Close();
                    return false;
                }
            }
            checkClassicExpansions("base");
            return true;
        }

    public void resetUi()
        {
            
            Game_Mode.SelectedIndex = 0;
            fs_game.SelectedIndex = 0;
            fs_game_base.SelectedIndex = 0;
            GamePath.SelectedIndex = 0;
            D12Skill.SelectedIndex = 0;
            D1Skill.SelectedIndex = 0;
            D1Episode.SelectedIndex = 0;
            D1Level.SelectedIndex = 0;
            D2Skill.SelectedIndex = 0;
            D2Episode.SelectedIndex = 0;
            D2Level.SelectedIndex = 0;
            D2Expansion.SelectedIndex = 0;
            Console_D3.IsChecked = false;
            SkipIntro.IsChecked = false;
            D12Cheat.IsChecked = false;
            D1Cheat.IsChecked = false;
            D2Cheat.IsChecked = false;
            D12Mods.Items.Clear();
            D1Mods.Items.Clear();
            D2Mods.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                ml[i].Clear();
            }
            D3Extra_Args.Text = "";
            D12Extra_Args.Text = "";
            D1Extra_Args.Text = "";
            D2Extra_Args.Text = "";
        }


        private void updateD3Mods()
        {
            if (CheckFiles())
            {
                fs_game.Items.Clear();
                fs_game.Items.Add("(none)");
                fs_game.SelectedIndex = 0;
                fs_game_base.Items.Clear();
                fs_game_base.Items.Add("(none)");
                fs_game_base.SelectedIndex = 0;
                List<string> dirs = new List<string>(Directory.GetDirectories(ufs.getParentPath("base")));
                foreach (var dir in dirs)
                {
                    string tdir = dir.Substring(dir.LastIndexOf("\\") + 1);
                    tdir = tdir.Substring(tdir.LastIndexOf("/") + 1);
                    if (tdir != "base" && tdir != "directx" && !tdir.StartsWith("msvc") && tdir != "base_new" && tdir != "base_BFG" && tdir != "third-party-licenses")
                    {
                        fs_game.Items.Add(tdir);
                        fs_game_base.Items.Add(tdir);
                    }
                }
                
            }
        }
    public ComboBox GetGameMode()
    {
        return Game_Mode;
    }

    public TextBox GetExtraArgs()
    {
        return Extra_Args;
    }

    public CheckBox GetConsole()
    {
        return Console_D3;
    }

    public ComboBox GetFsGame()
    {
        return fs_game;
    }

    public ComboBox GetFsGameBase()
    {
        return fs_game_base;
    }

    public CheckBox GetSkipIntro()
    {
        return SkipIntro;
    }

    public ComboBox GetGamePath()
    {
        return GamePath;
    }

    public TextBox GetD3ExtraArgs()
    {
        return D3Extra_Args;
    }

    public CheckBox GetD12Cheat()
    {
        return D12Cheat;
    }

    public ListBox GetD12Mods()
    {
        return D12Mods;
    }

    public ComboBox GetD12Skill()
    {
        return D12Skill;
    }

    public TextBox GetD12ExtraArgs()
    {
        return D12Extra_Args;
    }

    public CheckBox GetD1Cheat()
    {
        return D1Cheat;
    }

    public ListBox GetD1Mods()
    {
        return D1Mods;
    }

    public ComboBox GetD1Skill()
    {
        return D1Skill;
    }

    public ComboBox GetD1Episode()
    {
        return D1Episode;
    }

    public ComboBox GetD1Level()
    {
        return D1Level;
    }

    public TextBox GetD1ExtraArgs()
    {
        return D1Extra_Args;
    }

    public CheckBox GetD2Cheat()
    {
        return D2Cheat;
    }

    public ListBox GetD2Mods()
    {
        return D2Mods;
    }

    public ComboBox GetD2Skill()
    {
        return D2Skill;
    }

    public ComboBox GetD2Episode()
    {
        return D2Episode;
    }

    public ComboBox GetD2Level()
    {
        return D2Level;
    }

    public TextBox GetD2ExtraArgs()
    {
        return D2Extra_Args;
    }

    public ComboBox GetD2Expansion()
    {
        return D2Expansion;
    }
}