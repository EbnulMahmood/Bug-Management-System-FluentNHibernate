using Entities;
using Services.BaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.QAService
{
    public interface IQAService : IBaseService<QA>
    {
        Task<IEnumerable<QA>> ListEntities();
        Task<bool> DeleteEntity(Guid id);
    }
}
