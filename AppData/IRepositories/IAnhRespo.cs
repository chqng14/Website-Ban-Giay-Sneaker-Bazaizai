using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IAnhRespo
    {

        Task<IEnumerable<Anh>> GetListAsync();
        Task<Anh?> GetByKeyAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(Anh entity);
        Task<bool> AddAsync(Anh entity);

    }
}
