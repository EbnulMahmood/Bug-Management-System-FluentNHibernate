using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAOs.QADao
{
    public interface IQADao
    {
        Task<IEnumerable<QA>> ListQAsDescExclude404();
        Task<bool> CreateQA(QA qAToCreate);
        Task<bool> UpdateQA(QA qAToUpdate);
        Task<QA?> GetQAExclude404(Guid? qAToGetId);
        Task<bool> DeleteQAInclude404(Guid? qAToGetId);
    }
}
