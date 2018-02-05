using System.Collections.Generic;
using FuturisX.Dependency;

namespace FuturisX.Settings
{
    public interface ISettingsService : ISingletonDependency
    {
        T GetSetting<T>(SettingEntryKey key);
        bool TryGetSetting<T>(SettingEntryKey key, out T value);
        IEnumerable<SettingEntryInfo> GetAll();
        void SetSetting<T>(SettingEntryKey key, T value);
        bool TrySetSetting<T>(SettingEntryKey key, T value);
        bool SettingExists(SettingEntryKey key);
    }
}