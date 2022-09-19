using DataAccess.IRepository;
using DataAccess.Repository;
using Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FluentNHibernateSession _session;

        public IDeveloperRepository Developers { get; private set; }
        public UnitOfWork(FluentNHibernateSession session)
        {
            _session = session;
            Developers = new DeveloperRepository(_session);
        }
    }
}
