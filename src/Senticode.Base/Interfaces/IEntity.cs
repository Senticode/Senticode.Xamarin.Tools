using System;
using System.ComponentModel.DataAnnotations;

namespace Senticode.Base.Interfaces
{

    public interface IEntity<T> where T : struct
    {
        [Key] T Id { get; set; }
    }

    [Obsolete("Should use IEntity<T>.")]
    public interface IEntity: IEntity<int>
    {

    }

    [Obsolete("Should use IEntity<T>.")]
    public interface IUniqueEntity: IEntity<Guid>
    {

    }
}