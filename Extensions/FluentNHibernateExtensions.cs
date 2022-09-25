using DAOs.DeveloperDao;
using DAOs.QADao;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Mappings;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Services.DeveloperService;
using Services.QAService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class FluentNHibernateExtensions
    {
        public static IServiceCollection AddFluentNHibernate(this IServiceCollection services,
            string connectionString)
        {
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<DeveloperMap>();
                    m.FluentMappings.AddFromAssemblyOf<QAMap>();
                })
                .BuildSessionFactory();

            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
            services.AddScoped<IDeveloperDao, DeveloperDao>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<IQADao, QADao>();
            services.AddScoped<IQAService, QAService>();

            return services;
        }
    }
}
