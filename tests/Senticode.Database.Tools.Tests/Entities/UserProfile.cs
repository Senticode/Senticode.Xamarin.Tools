using System;
using System.ComponentModel.DataAnnotations.Schema;
using Senticode.Base.Interfaces;

namespace Senticode.Database.Tools.Tests.Entities
{
   public class UserProfile : IEntity<Guid>
   {
       [ForeignKey("User")] public string UserId { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public Guid Id { get; set; }
        
    }
}
