using System;
using Senticode.Base.Interfaces;

namespace Senticode.Database.Tools.Tests.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public virtual UserProfile UserProfile { get; set; }

    }
}
