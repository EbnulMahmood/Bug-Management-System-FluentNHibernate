using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Entities.BaseEntity
{
    public abstract class BaseEntity<T> : IEntity
    {
        [Required]
        public virtual T Id { get; set; }

        object IEntity.Id
        {
            get { return Id; }
            set { }
        }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
