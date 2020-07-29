﻿using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Xamarin.Essentials;

namespace Template.MasterDetail.AppStateMachine
{
    public partial class AppState
    {
        public NetworkAccess NetworkAccess { get; set; }
        public ConnectionProfile NetworkProfile { get; set; }
        public AppLifeTimeState AppLifeTimeState { get; set; }
    }
}