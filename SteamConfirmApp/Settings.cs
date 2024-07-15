using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SteamConfirmApp
{
    public class Settings : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _passkey = string.Empty;
        private bool _autoCheckConfirms = true;
        private bool _onlyCheckCurrentAccount = true;
        private bool _autoConfirmMarketListing = false;
        private bool _autoConfirmTrades = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Passkey
        {
            get => _passkey;
            set
            {
                if (value == _passkey) return;
                _passkey = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 自动查询确认
        /// </summary>
        public bool AutoCheckConfirms
        {
            get => _autoCheckConfirms;
            set
            {
                if (value == _autoCheckConfirms) return;
                _autoCheckConfirms = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 仅查询当前账号确认
        /// </summary>
        public bool OnlyCheckCurrentAccount
        {
            get => _onlyCheckCurrentAccount;
            set
            {
                if (value == _onlyCheckCurrentAccount) return;
                _onlyCheckCurrentAccount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 自动确认上架
        /// </summary>
        public bool AutoConfirmMarketListing
        {
            get => _autoConfirmMarketListing;
            set
            {
                if (value == _autoConfirmMarketListing) return;
                _autoConfirmMarketListing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 自动确认交易
        /// </summary>
        public bool AutoConfirmTrades
        {
            get => _autoConfirmTrades;
            set
            {
                if (value == _autoConfirmTrades) return;
                _autoConfirmTrades = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 将当前实例的属性值更新为另一个实例中的属性值
        /// </summary>
        /// <param name="settings"></param>
        public void UpdateFrom(Settings settings)
        {
            foreach (var property in typeof(Settings).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(settings));
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
