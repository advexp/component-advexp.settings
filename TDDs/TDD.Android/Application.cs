﻿using System;
using Android.App;
using Android.Runtime;
using Advexp;

namespace TDD.Android
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

            SettingsConfiguration.KeyStoreFileProtectionPassword = "password";
            SettingsConfiguration.KeyStoreFileName = "keystore";
            SettingsBaseConfiguration.EncryptionServiceID = "Advexp.Settings.TDD";

            OneTimeSetUpImpl.SetUp();
        }
    }
}

