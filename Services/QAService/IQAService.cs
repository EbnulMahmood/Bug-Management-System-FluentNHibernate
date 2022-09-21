using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.QAService
{
    public interface IQAService
    {
        Task<IEnumerable<QA>> ListEntities();
        Task<bool> CreateEntity(QA entity);
        Task<bool> UpdateEntity(QA entity);
        Task<QA?> GetEntity(Guid? id);
        Task<bool> DeleteEntity(Guid? id);
    }
}
