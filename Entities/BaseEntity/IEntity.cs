using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.BaseEntity
{
    public interface IEntity
    {
        object Id { get; set; }
        string Name { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
