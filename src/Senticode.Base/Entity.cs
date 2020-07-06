using System;
using System.ComponentModel.DataAnnotations;
using Senticode.Base.Interfaces;

namespace Senticode.Base
{
    public abstract class Entity<T> : IEntity<T> where T : struct
    {
        [Key] public T Id { get; set; }
    }

    [Obsolete("Should use Entity<T>.")]
    public abstract class Entity : IEntity
    {
        [Key] public int Id { get; set; }
    }
    
    [Obsolete("Should use Entity<T>.")]
    public abstract class UniqueEntity : IUniqueEntity
    {
        [Key] public Guid Id { get; set; }
    }
}
