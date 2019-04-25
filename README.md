###Advexp.Settings

Cross-platform .NET app settings for .NET Framework, .NET Core, Xamarin.Mac, Xamarin.iOS or Xamarin.Android application

####Details

Create cross-platform .NET app settings and make them accessible in your .NET Framework, .NET Core, Xamarin.Mac, Xamarin.iOS or Xamarin.Android application natively.  
Ability to save settings locally or to the cloud and sync them across different devices by using [Amazon Cognito Sync](http://docs.aws.amazon.com/cognito/latest/developerguide/cognito-sync.html) service.  
Ability to remotely configure your mobile application by using [Google Firebase Remote Config](https://firebase.google.com/docs/remote-config/).  
Ability to load cryptographic keys and secrets by using [Microsoft Azure](https://azure.microsoft.com) service.

- **.NET Framework**, **.NET Core**: Save settings in a normal form using Isolated Storage
- **.NET Framework**: Save settings in an encrypted form using Data Protection API and Isolated Storage
- **Xamarin.Mac**, **Xamarin.iOS**: Save settings in a normal form using *NSUserDefaults*
- **Xamarin.Mac**, **Xamarin.iOS**: Save settings in an encrypted form using Keychain
- **Xamarin.Android**: Use *SharedPreferences* to save settings in a normal form
- **Xamarin.Android**: Use KeyStore to save confidential settings in an encrypted form
- Save settings as dynamic parameters (name - value pairs)
- Use [Amazon Cognito Sync](http://docs.aws.amazon.com/cognito/latest/developerguide/cognito-sync.html) service to save settings to the cloud and sync them across different devices
- **Xamarin.iOS**, **Xamarin.Android**: Use [Google Firebase Remote Config](https://firebase.google.com/docs/remote-config/) service to remotely configure your mobile application
- **.NET Framework**, **.NET Core**: Load cryptographic keys and secrets by using [Microsoft Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) service
- Use user storage for settings
- Use any build-in or user-defined types which can be saved as a setting
- **Xamarin.iOS**: Ability to link settings from Advexp.Settings with settings from the Settings App
- **Xamarin.iOS**: The possibility of use [InAppSettingsKit](https://www.nuget.org/packages/Xamarin.InAppSettingsKit/) along with Advexp.Settings. Both for creating fully functional GUI of the app settings and for locating them in the Settings App and accessing them from C# code.
- Use environment variables as settings
- Use library in NetStandard/PCL projects
- Save or load settings by using JSON. In this case, the additional NuGet package [Json.NET](https://www.nuget.org/packages/newtonsoft.json) is used

Project home page:
<https://advexp.bitbucket.io>

NuGet package “Advexp.Settings Local” you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Local>
 
NuGet package “Advexp.Settings Cloud”, evaluation version, you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Cloud.Evaluation>

Samples, Unit Tests and additional information you can find here:  
<https://bitbucket.org/advexp/component-advexp.settings>

NuGet package "Advexp.Settings Cloud", full version, you can buy here:
<https://advexp.bitbucket.io>

#####Example of settings declaration

    :::csharp
    [CognitoSyncDatasetInfo(Name = "MyCognitoSyncDatasetName")]
    class Settings : Advexp.Settings<Settings>
    {
        [CognitoSyncSetting(Name = "CognitoSync.Boolean", Default = false)]
        public static Boolean CognitoSync {get; set;}
    
       [FirebaseRemoteConfig(Name = “FirebaseRemoteConfig.String”, 
                     Default = “default string configuration”)]
        public static String FirebaseRemoteConfig {get; set;}
    
        [Setting(Name = "Local.Setting", Default = 3)]
        public static Int32 LocalSetting {get; set;}
    
        [Setting(Name = "Local.SecureSetting", 
                     Secure = true, 
                     Default = "2009-06-15T13:45:30.0000000Z")]
        public static DateTime LocalSecureSetting {get; set;}
     
        // In this case, the automatic setting name in storage will be
        // "{NamespaceName}.{ClassName}.{FieldName}"
        [Setting]
        public static String SettingWithAutoName {get; set;}
    }


#####Example of settings usage

    :::csharp
    class Application
    {
        static void Main(string[] args)
        {
             // Setup CognitoSyncSettings plugin and it params
             // Setup FirebaseRemoteConfig plugin and it params
    
            // Will be saved to Amazon Cognito Sync
            // and will be available for syncing to another device
            Settings.CognitoSync = true;
            // Will be saved to NSUserDefaults for iOS and to SharedPreferences for Android
            Settings.LocalSetting = 5;
            Settings.SettingWithAutoName = "Data2";
            // Will be saved to Keychain for iOS and to KeyStore for Android
            Settings.LocalSecureSetting = DateTime.Now;
    
            Settings.SaveSettings();
    
            var firebaseRemoteConfigPlugin = Settings.GetPlugin<IFirebaseRemoteConfigPlugin>();  
            firebaseRemoteConfigPlugin.FetchCompletionHandler = (status) =>
            {
                switch (status)
                {
                    case Advexp.FetchStatus.Success:
                        Settings.LoadSettings();
                        // This value will be loaded from Google Firebase Remote Config service
                        var  configuration = Settings.FirebaseRemoteConfig;
                        break;
                }
            };
    
            firebaseRemoteConfigPlugin.Fetch(); 
    
            // Dynamic settings
            
            var lds = Settings.
              GetPlugin<Advexp.LocalDynamicSettings.Plugin.ILocalDynamicSettingsPlugin>();
            
            lds.SetSetting("dynamic_setting1", "value1");
            lds.SetSetting("dynamic_setting2", false);
            lds.SetSetting("dynamic_setting3", DateTime.Now);
    
            lds.SaveSettings();
    
            var dt = lds.GetSetting<DateTime>("dynamic_setting3");
        }
    }


The evaluation version of the Amazon Cognito Sync plugin does not allow specifying the name of the Cognito Sync dataset and uses the name "Advexp.Settings.Evaluation"

In evaluation version of the Google Firebase Remote Config plugin “v2\_AdvexpSettingsEvaluation\_” prefix will be added to setting names. Functions, to specify default values using xml resource (Android) or plist file (iOS) does not implemented. Also *FirebaseRemoteConfigConfiguration.ExpirationDuration* value cannot be changed and function *IFirebaseRemoteConfigPlugin.Fetch(long expirationDuration)* is not implemented. By default, value of expiration duration is 43200 seconds (12 hours).

####Getting Started
#####Create settings

Create a new class with the name "Settings" (for example) and inherit it as follows:

    :::csharp
    [CognitoSyncDatasetInfo(Name = "MyCognitoSyncDatasetName")]
    class Settings : Advexp.Settings<Settings>
    {
    // settings go here
    }

Specify those settings in the definition of this class that are required by your application. They should be accompanied by required attributes:

    :::csharp
    [CognitoSyncSetting(Name = "CognitoSync.Boolean", Default = false)]
    public static Boolean CognitoSync {get; set;}
    
    [FirebaseRemoteConfig(Name = “FirebaseRemoteConfig.String”, 
                     Default = “default string configuration”)]
    public static String FirebaseRemoteConfig {get; set;}
    
    [Setting(Name = "Local.Setting", Default = 3)]
    public static Int32 LocalSetting {get; set;}
        
    [Setting(Name = "Local.SecureSetting", 
                 Secure = true, 
                 Default = "2009-06-15T13:45:30.0000000Z")]
    public static DateTime LocalSecureSetting {get; set;}

You can use the class fields instead of properties. For example:

    :::csharp
    [CognitoSyncSetting(Name = "StringSetting")]
    public static String StringSetting;

The settings do not have to be static. In that case, they can be accessed through the *Instance* static property or through the class object which you manage independently.

Also, you can use dynamic parameters as saved settings. Their use is described below.

Call the appropriate method in order to perform the desired action.


#####Using user-defined types as settings

The library allows the use of any user-defined types which can be saved as settings. User-defined types do not require modification and addition of special attributes. Usage case - CustomObjectTest in TDD projects: <https://bitbucket.org/advexp/component-advexp.settings>. 

Settings serialization is done via the [SharpSerializer](http://sharpserializer.com/en/index.html) library.   
The serializer parameters can be modified by using the following property:  
*SettingsBaseConfiguration.AdvancedConfiguration.SharpSerializerSettings*

#####Save, load and delete settings

Library users can specify their own method for storing, loading or deleting settings by using the *ISettingsSerializer* interface and the *SerializerAttribute* attribute or the *MethodSerializerAttribute* attribute.

#####Priorities for saving, loading, or deleting settings

The following serializers are available:

- Library level
- Class level
- Field level

Serializers are used with the following priority (the higher in the list, the higher the priority):

- Field level
- Class level
- Library level
- If none of the above is present then the built-in serializer is used depending on the attribute type or its parameters

In short, the priority for settings serialization is assigned in a cascading fashion, from the library level serializer to the field level serializer, and it can be overridden at a lower level of abstraction.

#####Using proprietary attributes to specify settings

You can use proprietary attributes to specify settings. To do this, you need to register your attribute type and the corresponding serializer type:

    :::csharp
    SettingsBaseConfiguration.
        RegisterSettingsAttribute<MyAttributeType, MySerializerType>();

#####Specifying the default settings values

You can assign a default settings value which will be used for various exceptional cases, inability to load or when deleting. This is done with the help of the *SettingBaseAttribute.Default* property. This property can be assigned any type of value allowed in C# during attribute initialization or the *DefaulValueMode.TypeDefaultValue* value. In this case, the default value will be the value used by default for this type in C#. Many parameters can also be assigned by using a string value. For example, the default value for the DateTime setting type can be assigned as a string “2009-06-15T13:45:30.0000000Z”. In case the *Default* property has not been set up, then to delete the settings or in various exceptional cases (for example, inability to convert the loaded value into a setting type), the default value utilized for this type in C# will be used; in the case of inability to load, the setting value will remains unchanged.

#####Dynamic settings

Dynamic settings are parameters of the name - value pairs.  Where "name" is the text name of the setting and "value" is the value of any system or user-defined type which can be saved as a setting. Local dynamic settings can be accessed via the *ILocalDynamicSettingsPlugin* interface of the corresponding plugin:

    :::csharp
    var lds = MySettingsClass.GetPlugin<ILocalDynamicSettingsPlugin>();

This plugin is built into the library and does not require registration.

To manipulate dynamic settings, the following methods are available.

Load, save or delete all dynamic settings:

    :::csharp
    void LoadSettings();
    void SaveSettings();
    void DeleteSettings();
        

Load, save, or delete a dynamic setting with a specific name:

    :::csharp
    void LoadSetting(string settingName);
    void SaveSetting(string settingName);
    void DeleteSetting(string settingName);


Determine whether a dynamic setting with a specific name is contained in the collection:

    :::csharp
    bool Contains(string settingName);


Set the order of the dynamic settings. By default, the order of the settings corresponds to the order at which they have been added to the collection. But you can change it to any other. If you input *null* into this function, the order will be reset to the original one. If a setting name from the collection isn't present in the installed sequence order, then this setting will not be listed. The custom and default orders of the dynamic settings are saved and will be restored the next time the dynamic parameters are loaded.
An example of use can be found in the TDD project (UnitTests/DynamicSettings/DynamicSettingsTest.TestDynamicSettingsCustomOrder):

    :::csharp
    void SetSettingsOrder(IEnumerable<string> settingsOrder);


Get the dynamic setting value for a specific name and bring it to the T type.
For example, if the text "10" was saved as the dynamic setting, then an attempt to get a value of int type returns a number 10 of int type. Default value will be returned if the setting is not in the collection:

    :::csharp
    T GetSetting<T>(string settingName);
    T GetSetting<T>(string settingName, T defaultValue);


Set or add (if the setting is not in the collection) a dynamic setting:

    :::csharp
    void SetSetting<T>(string settingName, T settingValue);


Set the default values. This value will be returned if there is no dynamic setting in the collection. Reset all default values by inputting *null* into the function.
An example of use can be found in the TDD project (UnitTests/DynamicSettings/DynamicSettingsTest.TestDefaultValues):

    :::csharp
    void SetDefaultSettings(IDictionary<string, object> defaultSettings);


Listing of all settings in the collection:

    :::csharp
    IEnumerator<string> GetEnumerator();

The following action is valid:

    :::csharp
    var lds = MySettingsClass.GetPlugin<ILocalDynamicSettingsPlugin>();
    foreach(var dynamicSettingName in lds)
    {
    }


Get the count of dynamic settings in the collection:

    :::csharp
    int Count { get; }


The name format of the dynamic setting is following:   
D\_v2\_{namespace name}\_{class name}\_{setting name}

To see the names of the settings and their values during the process of loading or saving, the option SettingsBaseConfiguration.LogLevel needs to be set to value LogLevel.Info. To avoid various problems and operate settings locally, the *SettingAttribute* attribute needs to be temporarily applied to the configuration (if you want to monitor non dynamic settings). After this operations, Advexp.Settings will wrote the setting names and values into the application log in the form in which they will be stored in the storage.

The default order of dynamic settings is determined by the following setting:   
*S\_v2\_{namespace name}\_{class name}\_DynamicSettingsDefaultOrder*

The user-defined order of dynamic settings is determined by the following setting:   
S\_v2\_{namespace name}\_{class name}\_DynamicSettingsCustomOrder

These settings are created automatically and cannot be obtained using the API. They contain the names of user-defined dynamic settings separated by a comma. The order can be changed using the *IDynamicSettingsPlugin.SetSettingsOrder* method. 

Examples of using dynamic settings can be seen in the corresponding example (Samples/DynamicSettings) or in the TDD project (TDD/UnitTests/DynamicSettings)

#####Cloud: Syncing settings by using Amazon Cognito Sync

To add this ability, you need to install the [AWSSDK.CognitoSync](https://www.nuget.org/packages/AWSSDK.CognitoSync/) NuGet package in your project, add the **Advexp.CognitoSyncSettings.Plugin.PCL** assembly to the references, and register the plugin:

    :::csharp
    SettingsBaseConfiguration.
        RegisterSettingsPlugin<ICognitoSyncSettingsPlugin, CognitoSyncSettingsPlugin>();

Create the needed Identity pool in the Amazon Cognito management console.
Add the CognitoSyncDatasetInfoAttribute attribute to the settings class definition
and specify there the name of the dataset to which this class will pertain.

Then specify the plugin parameters:

    :::csharp
    CognitoSyncSettingsConfiguration.Config = new AmazonCognitoSyncConfig()
    {
        RegionEndpoint = RegionEndpoint.USEast1
    };

    CognitoSyncSettingsConfiguration.Credentials = 
        new CognitoAWSCredentials("MyIdentityPoolId", RegionEndpoint.USEast1);

Also, you need to assign an authorization token to the relevant plugin parameter:

    :::csharp
    CognitoSyncSettingsConfiguration.Credentials.AddLogin(
        "MyProviderName", "MyAccessToken");

As an authorization server you can use Amazon, Facebook, Twitter, Digits, Google or any other identity provider compatible with OpenID Connect. For more detailed information, see [Amazon Cognito’s documentation](http://docs.aws.amazon.com/cognito/latest/developerguide/getting-started.html)

The evaluation version of the plugin does not allow specifying the name of the Cognito Sync dataset and uses the name "Advexp.Settings.Evaluation"

#####Cloud: Dynamic settings and Amazon Cognito Sync

Added to the Amazon Cognito Sync console, dynamic settings can be obtained via *IDynamicSettingsPlugin* interface. This interface can be obtained by casting *ICognitoSyncSettingsPlugin* to *IDynamicSettingsPlugin* interface.
The following operations for this dynamic settings are available: *Load*, *Save* and *Delete*.

A way of using this plugin may be seen in the component examples: 
Sample.CognitoSyncSettings.Android and Sample.CognitoSyncSettings.iOS as well as TODOList.iOS and TODOList.Android, reflecting the interaction between Amazon Cognito Sync and Advexp.Settings dynamic settings (this example is an adaptation of the example from Amazon)

#####Cloud: Configure mobile app remotely by using Google Firebase Remote Config

To add this ability, you need to install the Google Firebase Remote Config NuGet package for [iOS](https://www.nuget.org/packages/Xamarin.Firebase.iOS.RemoteConfig/) or [Android](https://www.nuget.org/packages/Xamarin.Firebase.Config/), add the **Advexp.FirebaseRemoteConfig.Plugin.(iOS|Android)** and **Advexp.FirebaseRemoteConfig.Plugin.Standard** assemblies to the references, and register the plugin:

    SettingsBaseConfiguration.
        RegisterSettingsPlugin<IFirebaseRemoteConfigPlugin, FirebaseRemoteConfigPlugin>();

Next, follow instructions in the Google [Firebase console](https://console.firebase.google.com/u/0/) section Grow -> Remote Config

#####Cloud: Dynamic settings and Google Firebase Remote Config

Dynamic settings added to the Google Firebase console can be obtained via *IDynamicSettingsPlugin* interface. This interface can be obtained by casting *IFirebaseRemoteConfigPlugin* to *IDynamicSettingsPlugin* interface. For these dynamic settings, only the *Load* operation is available.

In evaluation version of the Google Firebase Remote Config plugin “v2\_AdvexpSettingsEvaluation\_” prefix will be added to setting names. Functions, to specify default values using xml resource (Android) or plist file (iOS) does not implemented. Also *FirebaseRemoteConfigConfiguration.ExpirationDuration* value cannot be changed and function *IFirebaseRemoteConfigPlugin.Fetch(long expirationDuration)* is not implemented. By default, value of expiration duration is 43200 seconds (12 hours).

A way of using this plugin may be seen in the component examples: 
Sample.FirebaseRemoteConfig.Android, Sample.FirebaseRemoteConfig.iOS and Sample.FirebaseRemoteConfig.Standard-iOS

#####Cloud: Load cryptographic keys and secrets by using Microsoft Azure service

To add this ability, you need to install the [Microsoft.Azure.KeyVault](https://www.nuget.org/packages/Microsoft.Azure.KeyVault/) NuGet package in your project, add the **Advexp.AzureKeyVaultSettings.Plugin** assembly to the references, and register the plugin:

    :::csharp
    SettingsBaseConfiguration.
        RegisterSettingsPlugin<IAzureKeyVaultSettingsPlugin, AzureKeyVaultSettingsPlugin>();

Set authentication callback:

    :::csharp
    AzureKeyVaultConfiguration.AuthenticationCallback = authCallback;
    
You can use *AzureKeyVaultSecretAttribute*, *AzureKeyVaultKeyAttribute* and *AzureKeyVaultCertificateAttribute* as settings attributes to load secrets, keys and certificates respectively.

For more details, see examples in AzureKeyVaultSettings folder.

#####Use environment variables as settings

To add this ability, you need to add the **Advexp.EnvironmentVariables.Plugin** assembly to the references, and register the plugin:

    :::csharp
    SettingsBaseConfiguration.
        RegisterSettingsPlugin<IEnvironmentVariables, EnvironmentVariablesPlugin>();

Mark setting by *EnvironmentVariableAttribute* attribute.

For more details, see example in EnvironmentVariables folder.

#####Saving and loading settings by using JSON

To add this capability, you need to install the [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json) NuGet package in your project, add the **Advexp.JSONSettings.Plugin.Standard** assembly to the references, and register the plugin:

    :::csharp
	SettingsBaseConfiguration.
	    RegisterSettingsPlugin<IJSONSettingsPlugin, JSONSettingsPlugin>();

Next, obtain a copy of the plugin from the class or from an object from that class, depending on which one is being accessed - the static class elements or the object’s elements.

    :::csharp
	var settingsInstance = new SettingsClass();
	var jsonPlugin = settingsInstance.GetObjectPlugin<IJSONSettingsPlugin>();

or

    :::csharp
	var jsonPlugin = SettingsClass.GetPlugin<IJSONSettingsPlugin>();

You can load or save settings in JSON format by using the plugin object.

Save settings in JSON format:

    :::csharp
    var jsonSettings = jsonPlugin.SaveSettingsToJSON();

Load settings from JSON:

    :::csharp
    jsonPlugin.LoadSettingsFromJSON(jsonSettings);

The JSON parser parameters may be modified by using the parameter:
*JSONSettingsConfiguration.JsonSerializerSettings*

The JSON Settings plugin parameters may be modified by using the parameter:  
*JSONSettingsConfiguration.PluginSettings*

A way of using this plugin may be seen in the component examples: 
Sample.JSONSettings.Android and Sample.JSONSettings.iOS


#####Previous version support and settings naming

During the development process, you need to keep in mind that Advexp.Settings version 2.0 and higher save settings in a different format. Also, in the new version, a "v2" prefix is automatically added to the setting's name.

#####iOS: The relationship between the settings from Settings App and the settings from Advexp.Settings and using the InAppSettingsKit component

In order to link the settings from the Settings App with settings from Advexp.Settings, you need to ensure that the parameter name in the *SettingAttribute* attribute matches the corresponding identifier in Settings.bundle. In this case, it is not possible to set the parameter *Secure = true*, since the setting must be stored in *NSUserDefaults*. When using along with [InAppSettingsKit](https://www.nuget.org/packages/Xamarin.InAppSettingsKit/) component, it is enough to maintain identificator compliance.
This function is available only for local settings (settings marked by the *SettingAttribute*). 
Examples: Sample.LocalSettings.iOS and Sample.InAppSettingsKit.iOS.

#####Description of library classes

######Advexp.Settings library

***interface ISettingsSerializer*** (Advexp.Settings.Utils.Standard.dll)  
It describes the serializer interface.

***class MethodSerializerAttribute*** (Advexp.Settings.Utils.Standard.dll)  
Arbitrary class methods can be used as a serializer.

Method prototypes are:  
*bool Load(string settingName, bool secure, out object value);*  
Returns true if downloaded successfully, otherwise false  
*void Save(string settingName, bool secure, object value);*  
*void Delete(string settingName, bool secure);*  
*void Contains(string settingName, bool secure);*  
*void Synchronize();*  
This attribute can be applied to a class or to a member of a class.

***class SerializerAttribute*** (Advexp.Settings.Utils.Standard.dll)  
The attribute specifies the serializer type that must be applied to a setting or to a class with settings.

***class SettingAttribute*** (Advexp.Settings.Utils.Standard.dll)  
This attribute indicates that the current class element is a setting and can be loaded/saved/deleted.  
It is applied to a class member.

***class Settings<T>*** (Advexp.Settings.dll for iOS, Android and NetStandard)  
The class specifies that its inheritor is a settings class and adds the appropriate functionality to the methods set.

***class SettingsConfiguration*** (Advexp.Settings.dll for iOS and Android)  
Static class. It contains parameters that define the library configuration.

**iOS:** ***class UserDefaultsSerializer*** (Advexp.Settings.dll for iOS)  
A class that implements a serializer for *NSUserDefaults*. It can be used when it is necessary to gain access to parameters that are not saved in the *NSUserDefaults.StandardUserDefaults* container.  
Example - ExternalUserDefaultsTest in the iOS TDD project, which can be accessed at <https://bitbucket.org/advexp/component-advexp.settings>

**Android:** ***class SharedPreferencesSerializer*** (Advexp.Settings.dll for Android)  
The class that implements the serializer for *SharedPreferences*. It can be applied when it is necessary to access parameters that are not stored in SharedPreferences of the application context.
Example - ExternalSaredPreferencesTest in the Android TDD project, which can be accessed at <https://bitbucket.org/advexp/component-advexp.settings> 

***class AdvancedConfiguration*** (Advexp.Settings.Utils.Standard.dll)  
Extended library configuration parameters. Objects of this type belong to the *SettingsBaseConfiguration* class.

######Local Dynamic Settings plugin

***interface ILocalDynamicSettingsPlugin*** (Advexp.Settings.Utils.Standard.dll)  
Interface definition of the Local Dynamic Settings plugin

######JSON Settings plugin

***interface IJSONSettingsPlugin*** (Advexp.JSONSettings.Plugin.Standard)  
Interface definition of the JSON Settings plugin

***class JSONSettingsPlugin*** (Advexp.JSONSettings.Plugin.Standard)  
Implementation of the JSON Settings plugin

***class JSONSettingsConfiguration*** (Advexp.JSONSettings.Plugin.Standard)  
Static class. It contains parameters that define the JSON Settings plugin configuration

***class PluginSettings*** (Advexp.JSONSettings.Plugin.Standard)  
Class which contains the JSON Settings plugin parameters. Objects of this type belong to the JSONSettingsConfiguration class.

######Cognito Sync Settings plugin

***class CognitoSyncSettingsConfiguration*** (Advexp.CognitoSyncSettings.Plugin.PCL)  
Static class. It contains parameters that define the Cognito Sync Settings plugin configuration

***class CognitoSyncSettingAttribute*** (Advexp.CognitoSyncSettings.Plugin.PCL)  
This attribute indicates that the current class element is a setting and can be loaded/saved/deleted using Amazon Cognito Sync service. It is applied to a class member

***interface ICognitoSyncSettingsPlugin*** (Advexp.CognitoSyncSettings.Plugin.PCL)  
Interface definition of the Cognito Sync Settings plugin

***interface IDynamicSettingsPlugin*** (Advexp.Settings.Utils.Standard.dll)  
Interface definition of dynamic settings for the Cognito Sync Settings plugin

***class CognitoSyncSettingsPlugin*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Implementation of the Cognito Sync Settings plugin

***class CognitoSyncDatasetInfoAttribute*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Attribute which defines to which dataset the settings class belongs. It is applied to a class

***class CognitoSyncSerializer*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Settings serializer for the Amazon Cognito Sync service

######Firebase Remote Config plugin

***class FirebaseRemoteConfigConfiguration*** (Advexp.FirebaseRemoteConfig.Plugin.*)   
Static class. It contains parameters that define the Firebase Remote Config plugin configuration

***class FirebaseRemoteConfigAttribute*** (Advexp.FirebaseRemoteConfig.Plugin.Standard)   
This attribute indicates that the current class element is a setting and can be loaded from Google Firebase Remote Config service
It is applied to a class member

***interface IFirebaseRemoteConfigPlugin*** (Advexp.FirebaseRemoteConfig.Plugin.Standard)   
Interface definition of the Firebase Remote Config plugin

***interface IDynamicSettingsPlugin*** (Advexp.Settings.Utils.Standard.dll)   
Interface definition of dynamic settings for the Firebase Remote Config plugin

***class FirebaseRemoteConfigPluginStandard*** (Advexp.FirebaseRemoteConfig.Plugin.Standard)   
Implementation of the Firebase Remote Config plugin for NetStandard

***class FirebaseRemoteConfigPlugin*** (Advexp.FirebaseRemoteConfig.Plugin.(iOS | Android))   
Implementation of the Firebase Remote Config plugin for iOS and Android

***class FirebaseRemoteConfigSerializer*** (Advexp.FirebaseRemoteConfig.Plugin.(iOS | Android))   
Settings serializer for the  Firebase Remote Config service 

#####Library configuration

######Advexp.Settings library

All library parameters can be set through the *SettingsConfiguration* static class. These manipulations should be produced before the other library functions are first used.

- **Windows:** ***SecureSettingsAdditionalEntropy*** - an optional additional byte array used to increase the complexity of the encryption, or null for no additional complexity  
- **Windows:** ***SecureSettingsScope*** - one of the enumeration values that specifies the scope of encryption  
- **MacOS, iOS, Android:** ***EncryptionServiceID*** - for settings that will be stored in the Keychain or KeyStore. It defines the Service Name. The default value is "Advexp.Settings"
- **MacOS, iOS:** ***KeyChainSecAccessible*** - for settings that will be stored in the Keychain. It determines at what time the parameter can be read from the Keychain. The default value is *SecAccessible.Always*
- **Android:** ***KeyStoreFileProtectionPassword*** - the password used to protect KeyStore file. The default value is *null*
- **Android:** ***KeyStoreFileName*** - KeyStore file name. The default value is *null*. In this case file name is “.keystore”
- ***Serializer*** - determines the library level serializer. The default value is *null*  
- ***AdvancedConfiguration.SharpSerializerSettings*** - parameters of the *SharpSerializer* serializer which are used for saving settings
- ***RegisterSettingsAttribute*** - a method designed for registering the settings indicator attribute
- ***RegisterSettingsPlugin*** - a method designed for the registration of plugins, for example, *CognitoSyncSettingsPlugin*
- ***LogLevel*** - set log level

######JSON Settings plugin

All JSON Settings plugin parameters can be set through the *JSONSettingsConfiguration* static class. These manipulations should be produced before the other library functions are first used.  
***JsonSerializerSettings*** - The JSON parser parameters  
***PluginSettings*** - The JSON Settings plugin parameters

######Cognito Sync Settings plugin

All Cognito Sync Settings plugin parameters can be set through the *CognitoSyncSettingsConfiguration* static class. These manipulations should be produced before the other library functions are first used.  
***Config*** - Configuration for accessing Amazon Cognito Sync service  
***Credentials*** - Temporary, short-lived session credentials. Depending on configured Logins, credentials may be authenticated or unauthenticated.

######Firebase Remote Config plugin

All Firebase Remote Config plugin parameters can be set through the *FirebaseRemoteConfigConfiguration* static class. These manipulations should be produced before the other library functions are first used.   
***ExpirationDuration*** - Duration that defines how long fetched config data is available, in seconds   
**iOS**: ***RemoteConfigDefaultsPlistFileName*** - Sets default settings from plist file.   
**Android**: ***RemoteConfigDefaultsId*** - Sets default settings from xml resources.

####Supported platforms

.NET Framework  
.NET Core  
UWP  
Xamarin.Mac  
Xamarin.iOS (Unified)  
Xamarin.Android

NetStandard / PCL projects

Project home page:
<https://advexp.bitbucket.io>

NuGet package “Advexp.Settings Local” you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Local>
 
NuGet package “Advexp.Settings Cloud”, evaluation version, you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Cloud.Evaluation>

Samples, Unit Tests and additional information you can find here:  
<https://bitbucket.org/advexp/component-advexp.settings>

NuGet package "Advexp.Settings Cloud", full version, you can buy here:
<https://advexp.bitbucket.io>


Please send your questions, suggestions and impressions to <components@advexp.net> with the subject "Advexp.Settings"