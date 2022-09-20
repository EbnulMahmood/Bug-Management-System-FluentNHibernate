using Entities;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sessions
{
    public static class FluentNHibernateSession
    {
        private static ISessionFactory _sessionFactory;
        // static string defaultConnection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public static ISessionFactory Instance
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = CreateSessionFactory();
                }

                return _sessionFactory;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(
                    "Server=DESKTOP-S9A8FPH\\SQLEXPRESS;Database=FluentTaskManagerDbTest;Trusted_Connection=True;"
                    ))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<DeveloperMap>();
                    m.FluentMappings.AddFromAssemblyOf<QAMap>();
                })
                .BuildSessionFactory();
        }
    }
}
