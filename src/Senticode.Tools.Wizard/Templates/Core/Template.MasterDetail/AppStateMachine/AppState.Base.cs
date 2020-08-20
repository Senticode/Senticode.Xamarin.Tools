using System;
using System.Collections.Generic;
using Senticode.Xamarin.Tools.Core.Abstractions.StateMachine;

namespace _template.MasterDetail.AppStateMachine
{
    public partial class AppState : AppStateBase, IComparable<AppState>
    {
        public int CompareTo(AppState other) => CompareTo((object) other);

        public override int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return obj is AppState other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(AppState)}");
        }

        public override object Clone() =>
            new AppState
            {
                Index = Index,
                DateTime = DateTime
            };

        public static bool operator <(AppState left, AppState right) =>
            Comparer<AppState>.Default.Compare(left, right) < 0;

        public static bool operator >(AppState left, AppState right) =>
            Comparer<AppState>.Default.Compare(left, right) > 0;

        public static bool operator <=(AppState left, AppState right) =>
            Comparer<AppState>.Default.Compare(left, right) <= 0;

        public static bool operator >=(AppState left, AppState right) =>
            Comparer<AppState>.Default.Compare(left, right) >= 0;
    }
}