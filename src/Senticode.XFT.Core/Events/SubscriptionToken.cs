﻿using System;

namespace Senticode.Xamarin.Tools.Core.Events
{
    /// <summary>
    ///     Subscription token returned from <see cref = "T:Senticode.Xamarin.Tools.Core.Events.EventBase" /> on subscribe.
    /// </summary>
    public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
    {
        private readonly Guid _token;
        private Action<SubscriptionToken> _unsubscribeAction;

        /// <summary>
        ///     Initializes a new instance of <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" />.
        /// </summary>
        public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
        {
            _unsubscribeAction = unsubscribeAction;
            _token = Guid.NewGuid();
        }

        /// <summary>
        ///     Disposes the SubscriptionToken, removing the subscription from the corresponding
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.EventBase" />.
        /// </summary>
        public virtual void Dispose()
        {
            if (_unsubscribeAction != null)
            {
                _unsubscribeAction(this);
                _unsubscribeAction = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     <see langword = "true" /> if the current object is equal to the <paramref name = "other" /> parameter; otherwise,
        ///     <see langword = "false" />.
        /// </returns>
        /// <param name = "other">An object to compare with this object.</param>
        public bool Equals(SubscriptionToken other)
        {
            if (other == null)
            {
                return false;
            }

            return Equals(_token, other._token);
        }

        /// <summary>
        ///     Determines whether the specified <see cref = "T:System.Object" /> is equal to the current
        ///     <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. </param>
        /// <exception cref = "T:System.NullReferenceException">The <paramref name = "obj" /> parameter is null.</exception>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            return Equals(obj as SubscriptionToken);
        }

        /// <summary>Serves as a hash function for a particular type.</summary>
        /// <returns>
        ///     A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _token.GetHashCode();
        }
    }
}