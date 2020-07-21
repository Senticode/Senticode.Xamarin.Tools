using System;
using Senticode.Base.Interfaces;

namespace Senticode.Database.Tools.Tests.Entities
{
    public class Project : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
