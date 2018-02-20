using System;
using Android.App;
using Android.Runtime;
using Advexp;
using System.Collections.Generic;
using Advexp.LocalDynamicSettings.Plugin;

namespace Sample.DynamicSettings.Android
{
    [Application]
    public class MainApplication : global::Android.App.Application
    {
        //------------------------------------------------------------------------------
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        //------------------------------------------------------------------------------
        public override void OnCreate()
        {
            base.OnCreate();

            var lds = DynamicSettings.GetPlugin<ILocalDynamicSettingsPlugin>();

            lds.SetDefaultSettings(new Dictionary<string, object>()
            {
                {DynamicSettings.CheckBoxSettingName, false},
                {DynamicSettings.SwitchSettingName, false},
                {DynamicSettings.EditTextSettingName, "default setting value"},
                {DynamicSettings.EnumSettingName, EnumPreference.Zero},
            });
        }
    }
}

