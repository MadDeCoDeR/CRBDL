using Avalonia.Controls;

public static class AvaloniaExtensions
{
    public static void AddRange(this ItemCollection itemCollection, string[] arr)
    {
        foreach(string name in arr)
        {
            itemCollection.Add(new ComboBoxItem
            {
                Content = name
            });
        }
    }
}