using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CDL.Arguments;
using CDL.Expansions;
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

            ml = new List<string>[7];
            for (int i = 0; i < 7; i++)
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
                        if (ufs.BFGPaths.Count + ufs.NewD3Paths.Count + ufs.BFAClassicPaths.Count > 1 && !runOnce)
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                GamePath_Group.IsVisible = true;
                                GamePath.IsVisible = true;
                                D3_Spacing.Height = 285.0f;
                                GamePath.Items.AddRangeComboBox(ufs.BFGPaths.Select(path => "(D3: BFG Edition) -- " + path).ToArray());
                                GamePath.Items.AddRangeComboBox(ufs.NewD3Paths.Select(path => "(D3: 2019) -- " + path).ToArray());
                                GamePath.Items.AddRangeComboBox(ufs.BFAClassicPaths.Select(path => "(BFA Classic) -- " + path).ToArray());
                                GamePath.SelectedIndex = 1;
                            }));
                            runOnce = true;
                        }
                        if (ufs.BFGPaths.Count + ufs.NewD3Paths.Count + ufs.BFAClassicPaths.Count > 0 && enableLaunch)
                        {
                            this.Dispatcher.Invoke(new Action(async () =>
                            {
                                Launch_Button.IsEnabled = true;
                                await this.updateD3Mods();
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

    private async void CDOOM_Add_Mod(object? sender, RoutedEventArgs e)
    {
        int page = GameTab.SelectedIndex;
        switch(page) {
            case 1:
                await modLoader.loadMod(D12Mods);
                break;
            case 2:
                await modLoader.loadMod(D1Mods);
                break;
            case 3:
                ml[D2Expansion.SelectedIndex].Add(ufs.getRelativePath(await modLoader.loadMod(D2Mods), "base"));
                break;
        }
    }

    public void EnableRemButton(object? sender, SelectionChangedEventArgs e)
    {
        int page = GameTab.SelectedIndex;
        switch(page)
        {
            case 1:
                D12RemMod.IsEnabled = true;
                break;
            case 2:
                D1RemMod.IsEnabled = true;
                break;
            case 3:
                D2RemMod.IsEnabled = true;
                break;
        }
    }

    private void CDOOM_Rem_Mod(object? sender, RoutedEventArgs e)
    {
        int page = GameTab.SelectedIndex;
        switch(page) {
            case 1:
                D12Mods.Items.Remove(D12Mods.SelectedItem);
                D12RemMod.IsEnabled = false;
                break;
            case 2:
                D1Mods.Items.Remove(D1Mods.SelectedItem);
                D1RemMod.IsEnabled = false;
                break;
            case 3:
                ml[D2Expansion.SelectedIndex].Remove(D2Mods.SelectedItem.ToString());
                D2Mods.Items.Remove(D2Mods.SelectedItem);
                D2RemMod.IsEnabled = false;
                break;
        }
    }

    public void UpdateD2ModList(object? sender, SelectionChangedEventArgs e)
    {
        if (D2Expansion == null || D2Mods == null)
        {
            return;
        }
        int exp = D2Expansion.SelectedIndex;
        if (exp < 0 || exp >= 7)
        {
            return;
        }
        D2Mods.Items.Clear();
        D2Mods.Items.AddRangeListBox(ml[exp]);
    } 

    public void UpdateD2LeveCount(object? sender, SelectionChangedEventArgs e)
    {
        if (D2Episode == null)
        {
            return;
        }
        if (D2Episode.Items.Count == 0)
        {
            return;
        }
        Levels levels = new Levels();
            switch (((ComboBoxItem)D2Episode.SelectedItem).Content.ToString())
            {
                case Names.D2:
                    levels.setLevels(33, D2Level);
                    break;
                case Names.TNT:
                case Names.PLUTONIA:
                    levels.setLevels(32, D2Level);
                    break;
                case Names.NERVE:
                    levels.setLevels(9, D2Level);
                    break;
                case Names.MASTER:
                    levels.setLevels(21, D2Level);
                    break;
                case Names.LOR:
                levels.setLevels(16, D2Level);
                break;
                default:
                    if (D2Level.SelectedIndex > 0) D2Level.SelectedIndex = 0;
                    break;

            }
    }

    private async void SaveSetting(object? sender, RoutedEventArgs e)
    {
        List<FilePickerFileType> filters = new List<FilePickerFileType>();
                filters.Add(new FilePickerFileType(Filters.SETTINGSFILTER.Name)
                {
                    Patterns = Filters.SETTINGSFILTER.Type.Split(";")
                });
            IStorageFile? file = await TopLevel.GetTopLevel(this).StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
            {
                FileTypeChoices = filters,
                Title = "Save current configuration"
            });

            if (file != null)
            {
                setting.saveSettings(file.Path.AbsolutePath, this);
                //button11.Enabled = true;
            }
    }

    private async void LoadSettings(object? sender, RoutedEventArgs e)
    {
        Stream myStream;
            List<FilePickerFileType> filters = new List<FilePickerFileType>();
                filters.Add(new FilePickerFileType(Filters.SETTINGSFILTER.Name)
                {
                    Patterns = Filters.SETTINGSFILTER.Type.Split(";")
                });
            IReadOnlyList<IStorageFile>? files = await TopLevel.GetTopLevel(this).StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                FileTypeFilter = filters,
                Title = "Load configuration"
            });

            if (files.Count > 0)
            {
                if ((myStream = await files[0].OpenReadAsync()) != null)
                {
                    setting.loadSettings(myStream, this, files[0].Path.AbsolutePath);
                    //button11.Enabled = true;
                }
            }
    }

    private async void UpdateGamePath(object? sender, SelectionChangedEventArgs e)
        {
            if (GamePath == null)
        {
            return;
        }
            string[] values = ((ComboBoxItem)GamePath.Items[GamePath.SelectedIndex]).Content.ToString().Split("--".ToCharArray());
            if (values.Length == 3)
            {
                ufs.SetSelectedPath(values[2].Trim());
                await this.updateD3Mods();
            }
        }

    public void Launchgame()
    {
        string args = ArgParser.ParseArgsFromForm(this);
        cdl.LaunchGame(args);
    }

    private async void Window_Loaded(object? sender, RoutedEventArgs e)
        {
            if (!ufs.isRunningPackaged())
            {
                if (await CheckFiles())
                {
                    List<string> dirs = new List<string>(Directory.GetDirectories(ufs.getParentPath("base")));
                    foreach (var dir in dirs)
                    {
                        string tdir = dir.Substring(dir.LastIndexOf("\\") + 1);
                        tdir = tdir.Substring(tdir.LastIndexOf("/") + 1);
                        if (tdir != "base" && tdir != "directx" && !tdir.StartsWith("msvc") && tdir != "base_new" && tdir != "base_BFG" && tdir != "third-party-licenses")
                        {
                            fs_game.Items.Add(new ComboBoxItem { Content = tdir });
                            fs_game_base.Items.Add(new ComboBoxItem { Content = tdir });
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
            D2Episode.Items.Clear();
            D2Episode.Items.AddRangeComboBox(new object[] {
            "(none)",
            "Hell on Earth",
            "No Rest For the Living",
            "TNT: Evilution",
            "The Plutonia Experiment",
            "Master Levels",
            "Legacy of Rust"});
            D2Episode.SelectedIndex = 0;
            D2Expansion.Items.Clear();
            D2Expansion.Items.AddRangeComboBox(new object[] {
            "(all)",
            "Hell on Earth",
            "TNT: Evilution",
            "The Plutonia Experiment",
            "Master Levels",
            "No Rest For the Living",
            "Legacy of Rust"});
            D2Expansion.SelectedIndex = 0;
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

    private async Task<bool> CheckFiles()
        {
            int found = cdl.CheckFiles();

            if (found == 0)
            {
                IMsBox<ButtonResult> messageBox = MessageBoxManager.GetMessageBoxStandard("Error", "Main executable not found", ButtonEnum.Ok);
                ButtonResult result = await messageBox.ShowAsync();
                if (result == ButtonResult.Ok)
                {
                    this.Close();
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


        private async Task updateD3Mods()
        {
            if (await CheckFiles())
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
                        fs_game.Items.Add(new ComboBoxItem { Content = tdir });
                        fs_game_base.Items.Add(new ComboBoxItem { Content = tdir });
                    }
                }
                
            }
        }
    public ComboBox GetGameMode()
    {
        return Game_Mode;
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

    public Label GetSettingsPath()
    {
        return SettingsPath;
    }
}