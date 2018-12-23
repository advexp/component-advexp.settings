using System;
using Amazon;

namespace Sample.CognitoSyncSettings.Windows
{
    static class Constants
    {
        public const string CognitoSyncIdentityPoolId = "us-east-1:1bfbb852-9b69-4d6a-8370-25ed484fd332";

        public static readonly RegionEndpoint CognitoSyncIdentityRegion = RegionEndpoint.USEast1;
        public static readonly RegionEndpoint CognitoSyncRegion = RegionEndpoint.USEast1;

        public const string FacebookAppId = "972960006158564";
        public const string FacebookAppName = "Advexp.Settings";
        public const string CognitoSyncProviderName_Facebook = "graph.facebook.com";

        public const string GoogleClientID = "534964568965-r6uem2jkghqk0gid5ejeojbhv70qu464.apps.googleusercontent.com";
        public const string GoogleClientSecret = "KrEn2pNaU_LaR0JH0XRIRE6T";
        public const string CognitoSyncProviderName_Google = "accounts.google.com";
    }
}

