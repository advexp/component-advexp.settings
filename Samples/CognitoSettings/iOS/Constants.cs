using System;
using Amazon;

namespace Sample.CognitoSyncSettings.iOS
{
    static class Constants
    {
        public static readonly string FacebookAppId = "972960006158564";
        public static readonly string FacebookAppName = "Advexp.Settings";

		public static readonly string CognitoSyncIdentityPoolId = "us-east-1:e7ba3086-94b6-4ff2-bc87-30523372862f";
        public static readonly string CognitoSyncProviderName = "graph.facebook.com";
        public static readonly RegionEndpoint CognitoSyncIdentityRegion = RegionEndpoint.USEast1;
        public static readonly RegionEndpoint CognitoSyncRegion = RegionEndpoint.USEast1;
    }
}

