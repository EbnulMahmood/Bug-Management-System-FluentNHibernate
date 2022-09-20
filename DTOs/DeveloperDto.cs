using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DeveloperDto
    {
        public virtual Guid Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual int Status { get; set; } = 1;
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

        public DeveloperDto() { }
        public DeveloperDto(Developer entity) =>
        (Id, Name, Status, CreatedAt) = (entity.Id, entity.Name, entity.Status, entity.CreatedAt);
    }
}
