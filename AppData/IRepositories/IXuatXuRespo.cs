using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IXuatXuRespo
    {

        Task<IEnumerable<XuatXu>> GetListAsync();
        Task<XuatXu?> GetByKeyAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(XuatXu entity);
        Task<bool> AddAsync(XuatXu entity);

    }
}
