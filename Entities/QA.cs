using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class QA
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual int Status { get; set; } = 1;
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
