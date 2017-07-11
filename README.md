###Advexp.Settings

####Details

Create cross-platform settings and make them accessible in your iOS or Android application. Ability to save the settings locally or to the cloud and sync them across the different devices by using the [Amazon Cognito Sync](http://docs.aws.amazon.com/cognito/latest/developerguide/cognito-sync.html) service

- **iOS**: Storing settings in a normal form using *NSUserDefaults*
- **iOS**: Storing settings in an encrypted form using Keychain
- **Android**: Using *SharedPreferences* to store settings in a normal form
- **Android**: Using KeyStore to save confidential settings in an encrypted form
- Using the [Amazon Cognito Sync](http://docs.aws.amazon.com/cognito/latest/developerguide/cognito-sync.html) service to save the settings to the cloud and sync them across the different devices
- Using user storage for settings
- Using any build-in or user-defined types which can be saved as a setting
- **iOS**: Ability to link settings from Advexp.Settings with settings from the Settings App
- **iOS**: The possibility of using [InAppSettingsKit](https://components.xamarin.com/view/InAppSettingsKit) along with Advexp.Settings. Both for creating fully functional GUI of the app settings and for locating them in the Settings App and accessing them from C# code.
- Using library in PCL projects
- Saving or loading settings by using JSON. In this case, the additional component [Json.NET](https://components.xamarin.com/view/json.net) is used

NuGet package “Advexp.Settings Local” you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Local>
 
NuGet package “Advexp.Settings Cloud”, evaluation version, you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Cloud.Evaluation>
 
"Advexp.Settings Local" component for Xamarin you can find here:  
<https://components.xamarin.com/view/advexp-settings-local>

"Advexp.Settings Cloud" full version component for Xamarin you can find here:  
<https://components.xamarin.com/view/advexp-settings-cloud>

Samples and Unit Tests can be downloaded from this site.

#####Example of a settings declaration

    :::csharp
    [Advexp.CognitoSyncSettings.Plugin.
     CognitoSyncDatasetInfo(Name = "MyCognitoSyncDatasetName")]
    class Settings : Advexp.Settings<Settings>
    {
        [Advexp.CognitoSyncSettings.Plugin.
         CognitoSyncSetting(Name = "CognitoSyncSettings.Boolean", Default = false)]
        public static Boolean CognitoSyncBoolean {get; set;}

        [Advexp.
         Setting(Name = "IntSetting", Default = 3)]
        public static Int32 IntSetting {get; set;}

        [Advexp.
         Setting(Name = "NonStaticStringSetting", Default = "default string value")]
        public String NonStaticStringSetting {get; set;}

        [Advexp.
         Setting(Name = "SecureDateTimeSetting", 
                     	Secure = true, 
                     	Default = "2009-06-15T13:45:30.0000000Z")]
        public static DateTime SecureDateTimeSetting {get; set;}

        // In this case, the automatic setting name in storage will be
        // "{NamespaceName}.{ClassName}.{FieldName}"
        // The name pattern can be changed 
        // using the SettingsConfiguration.SettingsNamePattern property
        // The default pattern name is: "{NamespaceName}.{ClassName}.{FieldName}"
        [Advexp.
         Setting]
        public static String SettingWithAutoName {get; set;}
    }


#####Example of settings usage

    :::csharp
    class Application
    {
        static void Main(string[] args)
        {
            Advexp.
                SettingsBaseConfiguration.RegisterSettingsPlugin
                <
                    Advexp.CognitoSyncSettings.Plugin.ICognitoSyncSettingsPlugin,
                    Advexp.CognitoSyncSettings.Plugin.CognitoSyncSettingsPlugin
                >();
                    
            Advexp.CognitoSyncSettings.Plugin.
                CognitoSyncSettingsConfiguration.Config = new AmazonCognitoSyncConfig()
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast1
                };
            
            Advexp.CognitoSyncSettings.Plugin.
                CognitoSyncSettingsConfiguration.Credentials = 
                    new CognitoAWSCredentials("MyIdentityPoolId", Amazon.RegionEndpoint.USEast1);

            Advexp.CognitoSyncSettings.Plugin.
                CognitoSyncSettingsConfiguration.Credentials.AddLogin(
                    "MySyncProviderName", "MyAccessToken");

            Settings.LoadSettings();

            // Will be saved to Amazon Cognito Sync
            // and will be available for syncing to another device
            Settings.CognitoSyncBoolean = true;
            // Will be saved to NSUserDefaults for iOS 
            // and to SharedPreferences for Android
            Settings.IntSetting = 5;
            Settings.Instance.NonStaticStringSetting = "Data1";
            Settings.SettingWithAutoName = "Data2";
            // Will be saved to Keychain for iOS and to KeyStore for Android
            Settings.SecureDateTimeSetting = DateTime.Now;

            Settings.SaveSettings();

            // If needed, you can force the Cognito Sync container 
            // to perform synchronization
            var plugin = Settings.GetPlugin<ICognitoSyncSettingsPlugin>();
            plugin.SynchronizeDataset();
        }
    }

The evaluation version of the component does not allow specifying the name of the Cognito Sync dataset and uses the name "Advexp.Settings.Evaluation"

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
    [CognitoSyncSetting(Name = "CognitoSyncSettings.Boolean", Default = false)]
    public static Boolean CognitoSyncBoolean {get; set;}

    [Setting(Name = "IntSetting", Default = 3)]
    public static Int32 IntSetting {get; set;}

    [Setting(Name = "StringSetting")]
    public static String StringSetting {get; set;}
    
    [Setting(Name = "SecureDateTimeSetting", 
                 Secure = true, 
                 Default = "2009-06-15T13:45:30.0000000Z")]
    public static DateTime SecureDateTimeSetting {get; set;}


You can use the class fields instead of properties. For example:

    :::csharp
    [CognitoSyncSetting(Name = "StringSetting")]
    public static String StringSetting;

The settings do not have to be static. In that case, they can be accessed through the *Instance* static property or through the class object which you manage independently.

Call the appropriate method in order to perform the desired actions.


#####Using user-defined types as settings

The library allows the use of any user-defined types which can be saved as settings. User-defined types do not require modification and addition of special attributes. Usage case - CustomObjectTest in TDD projects on the site 
<https://bitbucket.org/advexp/component-advexp.settings>. 
Serializing settings is done via the [SharpSerializer](http://sharpserializer.com/en/index.html) library.
The serializer settings can be modified by using the following parameter:
*SettingsBaseConfiguration.AdvancedConfiguration.SharpSerializerSettings*


#####Settings serialization

Library users can specify their own method for storing/loading/deleting settings by using the *ISettingsSerializer* interface and the *SerializerAttribute* attribute or the *MethodSerializerAttribute* attribute.

#####Priorities for using serializers

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


#####Syncing settings by using Amazon Cognito Sync

To add this ability, you need to install the [AWSSDK - Amazon Cognito Sync](https://components.xamarin.com/view/aws-cognitosync-sdk) component in your project, add the Advexp.CognitoSyncSettings.Plugin.PCL assembly to the dependencies, and register the plugin:

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

The evaluation version of the component does not allow specifying the name of the Cognito Sync dataset and uses the name "Advexp.Settings.Evaluation"

#####Saving and loading settings by using JSON

To add this capability, you need to install the [Json.NET](https://components.xamarin.com/view/json.net) component in your project, add the **Advexp.JSONSettings.Plugin.PCL** assembly to the dependencies, and register the plugin:

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
	var jsonSettings = jsonPlugin.Settings;

Load settings from JSON:

    :::csharp
	jsonPlugin.Settings = jsonSettings;

The JSON parser parameters may be modified by using the parameter:  
*JSONSettingsConfiguration.JsonSerializerSettings*

The JSON Settings plugin parameters may be modified by using the parameter:  
*JSONSettingsConfiguration.PluginSettings*

A way of using this plugin may be seen in the component examples:  
Sample.JSONSettings.iOS and Sample.JSONSettings.Android

#####Previous version support and settings naming

During the development process, you need to keep in mind that Advexp.Settings version 2.0 and higher, records settings in a different format. Reading the parameters formed by the library's previous version is done by utilizing the migration mechanism. Also, in the new version, a "v2" prefix is automatically added to the setting's name. Encrypted parameters (Secure) in the library's Android version are not subject to migration. Consequently, the library does not have the capability to read the confidential settings and perform migration from the old version (encrypted SharedPreferences) to the new one (KeyStore). You can do that yourself for the simple settings type by reading them from *SharedPreferences* and decrypting them with the help of the *Cryptography* class located in the TDD library project ([here](https://bitbucket.org/advexp/component-advexp.settings/src/833b7ffdcd37244a9082d692117324c32f3b0793/TDD.Android/Cryptography.cs?at=default&fileviewer=file-view-default)). For the iOS version, all settings are readable without additional manipulations by the user. Also, for the iOS library version, the *SettingsConfiguration.KeyChainPrefixName* configuration parameter is no longer used. Previous version support can be turned on by using *SettingsBaseConfiguration.EnableFormatMigration* property.

#####iOS: The relationship between the settings from Settings App and the settings from Advexp.Settings and using the InAppSettingsKit component

In order to link the settings from the Settings app with settings from Advexp.Settings, you must ensure that the parameter *Name* in the *SettingAttribute* attribute matches the corresponding identifier in Settings.bundle. In this case, it is not possible to set the parameter *Secure = true*, since the setting must be stored in *NSUserDefaults*. When using along with [InAppSettingsKit](https://components.xamarin.com/view/InAppSettingsKit) component, it is enough to maintain identificator compliance.  
This function is available only for local settings (settings marked by the *SettingAttribute*).  
For a more detailed usage option for this scenario, see the component example.

#####Description of library classes

######Advexp.Settings library

***interface ISettingsSerializer*** (Advexp.Settings.Utils.PCL.dll)  
It describes the serializer interface.

***class MethodSerializerAttribute*** (Advexp.Settings.Utils.PCL.dll)  
Arbitrary class methods can be used as a serializer.

Method prototypes are:  
*bool Load(string settingName, bool secure, out object value);*  
Returns true if downloaded successfully, otherwise false  
*void Save(string settingName, bool secure, object value);*  
*void Delete(string settingName, bool secure);*  
*void Synchronize();*  
This attribute can be applied to a class or to a member of a class.

***class SerializerAttribute*** (Advexp.Settings.Utils.PCL.dll)  
The attribute specifies the serializer type that must be applied to a setting or to a class with settings.

***class SettingAttribute*** (Advexp.Settings.Utils.PCL.dll)  
This attribute indicates that the current class element is a setting and can be loaded/saved/deleted.  
It is applied to a class member.

***class Settings\<T\>*** (Advexp.Settings.dll for iOS, Android and PCL)  
The class specifies that its inheritor is a settings class and adds the appropriate functionality to the methods set.

***class SettingsConfiguration*** (Advexp.Settings.dll for iOS and Android)  
Static class. It contains parameters that define the library configuration.

**iOS:** ***class UserDefaultsSerializer*** (Advexp.Settings.dll for iOS)  
A class that implements a serializer for *NSUserDefaults*. It can be used when it is necessary to gain access to parameters that are not saved in the *NSUserDefaults.StandardUserDefaults* container.  
Example - ExternalUserDefaultsTest in the iOS TDD project, which can be accessed at <https://bitbucket.org/advexp/component-advexp.settings>

**Android:** ***class SharedPreferencesSerializer*** (Advexp.Settings.dll for Android)  
The class that implements the serializer for *SharedPreferences*. It can be applied when it is necessary to access parameters that are not stored in SharedPreferences of the application context.
Example - ExternalSaredPreferencesTest in the Android TDD project, which can be accessed at <https://bitbucket.org/advexp/component-advexp.settings> 

***class AdvancedConfiguration*** (Advexp.Settings.Utils.PCL.dll)  
Extended library configuration parameters. Objects of this type belong to the *SettingsBaseConfiguration* class.

######JSON Settings plugin

***interface IJSONSettingsPlugin*** (Advexp.JSONSettings.Plugin.PCL)  
Interface definition of the JSON Settings plugin

***class JSONSettingsPlugin*** (Advexp.JSONSettings.Plugin.PCL)  
Implementation of the JSON Settings plugin

***class JSONSettingsConfiguration*** (Advexp.JSONSettings.Plugin.PCL)  
Static class. It contains parameters that define the JSON Settings plugin configuration

***class PluginSettings*** (Advexp.JSONSettings.Plugin.PCL)  
Class which contains the JSON Settings plugin parameters. Objects of this type belong to the JSONSettingsConfiguration class.

######Cognito Sync Settings plugin

***class CognitoSyncSettingsConfiguration*** (Advexp.CognitoSyncSettings.Plugin.PCL)  
Static class. It contains parameters that define the Cognito Sync Settings plugin configuration

***class CognitoSyncSettingAttribute*** (Advexp.CognitoSyncSettings.Plugin.PCL)  
This attribute indicates that the current class element is a setting and can be loaded/saved/deleted using Amazon Cognito Sync service. It is applied to a class member

***interface ICognitoSyncSettingsPlugin*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Interface definition of the Cognito Sync Settings plugin

***class CognitoSyncSettingsPlugin*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Implementation of the Cognito Sync Settings plugin

***class CognitoSyncDatasetInfoAttribute*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Attribute which defines to which dataset the settings class belongs. It is applied to a class

***class CognitoSyncSerializer*** (Advexp.CognitoSyncSettings.Plugin.PCL)
Settings serializer for the Amazon Cognito Sync service

#####Library configuration

######Advexp.Settings library

All library parameters can be set through the *SettingsConfiguration* static class. These manipulations should be produced before the other library functions are first used.

- ***SettingsNamePattern***  - specifies the name pattern under which the settings are saved to the storage. The default value is "{NamespaceName}.{ClassName}.{FieldName}". It supports only those substitution parameters that are used by default
- ***EncryptionServiceID*** - for settings that will be stored in the Keychain or KeyStore. It defines the Service Name. The default value is "Advexp.Settings"
- **iOS:** ***KeyChainSecAccessible*** - for settings that will be stored in the Keychain. It determines at what time the parameter can be read from the Keychain. The default value is *SecAccessible.Always*
- **Android:** ***KeyStoreFileProtectionPassword*** - the password used to protect KeyStore file. The default value is *null*
- **Android:** ***KeyStoreFileName*** - KeyStore file name. The default value is *null*. In this case file name is “.keystore”
- ***Serializer*** - determines the library level serializer. The default value is *null*  
- ***AdvancedConfiguration.SharpSerializerSettings*** - parameters of the *SharpSerializer* serializer which are used for saving settings
- ***RegisterSettingsAttribute*** - a method designed for registering the settings indicator attribute
- ***RegisterSettingsPlugin*** - a method designed for the registration of plugins, for example, *CognitoSyncSettingsPlugin*
- **EnableFormatMigration** - enable or disable previous version support. The default value is *false*

######JSON Settings plugin

All JSON Settings plugin parameters can be set through the JSONSettingsConfiguration static class. These manipulations should be produced before the other library functions are first used.  
***JsonSerializerSettings*** - The JSON parser parameters  
***PluginSettings*** - The JSON Settings plugin parameters

######Cognito Sync Settings plugin

All Cognito Sync Settings plugin parameters can be set through the *CognitoSyncSettingsConfiguration* static class. These manipulations should be produced before the other library functions are first used.  
***Config*** - Configuration for accessing Amazon Cognito Sync service  
***Credentials*** - Temporary, short-lived session credentials. Depending on configured Logins, credentials may be authenticated or unauthenticated.

####Supported platforms

Xamarin.iOS (Unified)  
Xamarin.Android

PCL projects

NuGet package “Advexp.Settings Local” you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Local>
 
NuGet package “Advexp.Settings Cloud”, evaluation version, you can download from the site:  
<https://www.nuget.org/packages/Advexp.Settings.Cloud.Evaluation>
 
"Advexp.Settings Local" component for Xamarin you can find here:  
<https://components.xamarin.com/view/advexp-settings-local>

"Advexp.Settings Cloud" full version component for Xamarin you can find here:  
<https://components.xamarin.com/view/advexp-settings-cloud>

Samples and Unit Tests can be downloaded from this site.

Please send your questions, suggestions and impressions to <components@advexp.net> with the subject "Advexp.Settings"
