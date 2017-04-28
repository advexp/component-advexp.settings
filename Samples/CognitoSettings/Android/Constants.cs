using System;
using Amazon;

namespace Sample.CognitoSyncSettings.Android
{
    static class Constants
    {
        public static readonly string FacebookAppId = "972960006158564";
        public static readonly string FacebookAppName = "Advexp.Settings";

        public static readonly string CognitoSyncIdentityPoolId = "us-east-1:7f768ed9-ab1a-4d3f-8fe0-acf1306f6de9";
        public static readonly string CognitoSyncProviderName = "graph.facebook.com";
        public static readonly RegionEndpoint CognitoSyncIdentityRegion = RegionEndpoint.USEast1;
        public static readonly RegionEndpoint CognitoSyncRegion = RegionEndpoint.USEast1;
    }
}

