using System;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Advexp
{
    public partial class SettingsT<ST> : IPluginHolder, IPluginContext where ST : new()
    {
        // Load settings
        //------------------------------------------------------------------------------
        async public static Task LoadSettingsAsync()
        {
            await Task.Run(() => LoadSettings(Instance));
        }

        //------------------------------------------------------------------------------
        async public Task LoadObjectSettingsAsync()
        {
            await Task.Run(() => LoadSettings(this));
        }

        //------------------------------------------------------------------------------
        async public static Task LoadSettingsAsync(object settings)
        {
            await Task.Run(() => LoadSettings(settings));
        }

        // Save settings
        //------------------------------------------------------------------------------
        async public static Task SaveSettingsAsync()
        {
            await Task.Run(() => SaveSettings(Instance));
        }

        //------------------------------------------------------------------------------
        async public Task SaveObjectSettingsAsync()
        {
            await Task.Run(() => SaveSettings(this));
        }

        //------------------------------------------------------------------------------
        async public static Task SaveSettingsAsync(object settings)
        {
            await Task.Run(() => SaveSettings(settings));
        }

        // Delete settings
        //------------------------------------------------------------------------------
        async public static Task DeleteSettingsAsync()
        {
            await Task.Run(() => DeleteSettings(Instance));
        }

        //------------------------------------------------------------------------------
        async public Task DeleteObjectSettingsAsync()
        {
            await Task.Run(() => DeleteSettings(this));
        }

        //------------------------------------------------------------------------------
        async public static Task DeleteSettingsAsync(object settings)
        {
            await Task.Run(() => DeleteSettings(settings));
        }

        // Contains Setting
        //------------------------------------------------------------------------------
        async public static Task<bool> ContainsSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            return await Task.Run(() => ContainsObjectSetting(Instance, value));
        }

        //------------------------------------------------------------------------------
        async public Task<bool> ContainsObjectSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            return await Task.Run(() => ContainsObjectSetting(this, value));
        }

        // Load default settings
        //------------------------------------------------------------------------------
        async public static Task LoadDefaultSettingsAsync()
        {
            await Task.Run(() => LoadDefaultSettings(Instance));
        }

        //------------------------------------------------------------------------------
        async public Task LoadObjectDefaultSettingsAsync()
        {
            await Task.Run(() => LoadDefaultSettings(this));
        }

        //------------------------------------------------------------------------------
        async public static Task LoadDefaultSettingsAsync(object settings)
        {
            await Task.Run(() => LoadDefaultSettings(settings));
        }

        // Single setting operations
        //------------------------------------------------------------------------------
        async public static Task LoadObjectSettingAsync<T>(object settings, Expression<Func<ST, T>> value)
        {
            await Task.Run(() => ProcessExpression(settings, value, SettingsEnumerationMode.Load));
        }

        //------------------------------------------------------------------------------
        async public static Task SaveObjectSettingAsync<T>(object settings, Expression<Func<ST, T>> value)
        {
            await Task.Run(() => ProcessExpression(settings, value, SettingsEnumerationMode.Save));
        }

        //------------------------------------------------------------------------------
        async public static Task DeleteObjectSettingAsync<T>(object settings, Expression<Func<ST, T>> value)
        {
            await Task.Run(() => ProcessExpression(settings, value, SettingsEnumerationMode.Delete));
        }

        //------------------------------------------------------------------------------
        async public static Task LoadObjectDefaultSettingAsync<T>(object settings, Expression<Func<ST, T>> value)
        {
            await Task.Run(() => ProcessExpression(settings, value, SettingsEnumerationMode.LoadDefaults));
        }

        //------------------------------------------------------------------------------
        async public static Task LoadSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => LoadObjectSetting(Instance, value));
        }

        //------------------------------------------------------------------------------
        async public Task LoadObjectSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => LoadObjectSetting(this, value));
        }

        //------------------------------------------------------------------------------
        async public static Task SaveSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => SaveObjectSetting(Instance, value));
        }

        //------------------------------------------------------------------------------
        async public Task SaveObjectSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => SaveObjectSetting(this, value));
        }

        //------------------------------------------------------------------------------
        async public static Task DeleteSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => DeleteObjectSetting(Instance, value));
        }

        //------------------------------------------------------------------------------
        async public Task DeleteObjectSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => DeleteObjectSetting(this, value));
        }

        //------------------------------------------------------------------------------
        async public static Task LoadDefaultSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => LoadObjectDefaultSetting(Instance, value));
        }

        //------------------------------------------------------------------------------
        async public Task LoadObjectDefaultSettingAsync<T>(Expression<Func<ST, T>> value)
        {
            await Task.Run(() => LoadObjectDefaultSetting(this, value));
        }

    }
}