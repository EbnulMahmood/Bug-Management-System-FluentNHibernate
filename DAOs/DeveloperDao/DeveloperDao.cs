using Entities;
using NHibernate;
using DAOs.BaseDao;

namespace DAOs.DeveloperDao
{
    public class DeveloperDao : BaseDao<Developer>, IDeveloperDao
    {
        public DeveloperDao(ISession session) : base(session)
        {
        }
    }
}
