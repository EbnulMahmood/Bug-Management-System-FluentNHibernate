using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class DeveloperDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DeveloperDto() { }
        public DeveloperDto(Developer entity) =>
        (Id, Name, Status, CreatedAt) = (entity.Id, entity.Name, entity.Status, entity.CreatedAt);
    }
}
