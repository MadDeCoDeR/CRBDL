using System.Collections;
using Avalonia.Controls;

public static class AvaloniaExtensions
{
    public static void AddRangeComboBox(this ItemCollection itemCollection, IEnumerable arr)
    {
        foreach(string name in arr)
        {
            itemCollection.Add(new ComboBoxItem
            {
                Content = name
            });
        }
    }

    public static void AddRangeListBox(this ItemCollection itemCollection, IEnumerable arr)
    {
        foreach(string name in arr)
        {
            itemCollection.Add(name);
        }
    }
}