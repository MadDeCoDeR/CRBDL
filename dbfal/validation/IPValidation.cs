using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace dbfal.validation
{
    public partial class IPValidation : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Ipv4Address(ErrorMessage = "Invalid IPV4 format")]
        private string? _ipAddress = null;

        [RelayCommand]
        private void Submit()
        {
            ValidateAllProperties();
            if (!HasErrors)
            {
                MultiManager.HasValidIP = true;
            }
        }
    }
}
