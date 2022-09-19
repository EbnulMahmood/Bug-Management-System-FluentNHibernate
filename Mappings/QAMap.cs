using Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappings
{
    public class QAMap : ClassMap<QA>
    {
        public QAMap()
        {
            Table("QAs");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Status);
            Map(x => x.CreatedAt);
        }
    }
}
