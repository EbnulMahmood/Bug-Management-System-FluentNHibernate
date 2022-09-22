using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.BaseEntity;

namespace Entities
{
    public class Developer : BaseEntity<Guid>, IEntity
    {
        public virtual int Status { get; set; } = 1;
    }
}
