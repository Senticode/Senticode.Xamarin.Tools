using System;
using Senticode.Base.Interfaces;

namespace Senticode.Base
{
    public abstract class UniqueEntity : IUniqueEntity
    {
        public Guid Id { get; set; }
    }
}