using DataAccess.IRepository;
using Entities;
using Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DeveloperRepository : Repository<Developer>, IDeveloperRepository
    {
        private readonly FluentNHibernateSession _session;

        public DeveloperRepository(FluentNHibernateSession session) : base(session)
        {
            _session = session;
        }

        public void Update(Developer entity)
        {
            using (var session = _session.Instance.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }
    }
}
