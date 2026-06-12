using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace dbfal.validation
{
    public partial class IPValidation : ObservableValidator
    {
        
        private string? _ipAddress = null;

        [Ipv4Address(ErrorMessage = "Invalid IPV4 format")]
        public string? IpAddress
        {
            get { return this._ipAddress; }
            set
            {
                this.ValidateProperty(value, "IpAddress");
                if (!this.HasErrors)
                {
                    MultiManager.HasValidIP = true;
                }
                SetProperty<string?>(ref this._ipAddress, value, true);
            }
        }
    }
}
