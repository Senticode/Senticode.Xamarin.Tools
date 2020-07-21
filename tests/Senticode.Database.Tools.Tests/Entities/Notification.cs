using System;
using Senticode.Base.Interfaces;

namespace Senticode.Database.Tools.Tests.Entities
{
    public class Notification : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}
