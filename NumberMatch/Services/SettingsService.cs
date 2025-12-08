using System.ComponentModel;
using Microsoft.Maui.Storage;

namespace NumberMatch.Services;
public class SettingsService : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private bool _oledDarkmode;
    public bool OledDarkmode
    {
        get => _oledDarkmode;
        set
        {
            if (_oledDarkmode == value) return;
            _oledDarkmode = value;
            Preferences.Set("OledDarkmode", value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OledDarkmode)));
        }
    }

    private bool _vibration;
    public bool Vibration
    {
        get => _vibration;
        set
        {
            if (_vibration == value) return;
            _vibration = value;
            Preferences.Set("Vibration", value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vibration)));
        }
    }

    private bool _developerOptions;
    public bool DeveloperOptions
    {
        get => _developerOptions;
        set
        {
            if (_developerOptions == value) return;
            _developerOptions = value;
            Preferences.Set("DeveloperOptions", value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeveloperOptions)));
        }
    }

    public SettingsService()
    {
        // load saved values (provide defaults)
        _oledDarkmode = Preferences.Get("OledDarkmode", false);
        _vibration = Preferences.Get("Vibration", true);
        _developerOptions = Preferences.Get("DeveloperOptions", false);
    }
}