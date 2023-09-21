using App_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.IRepositories
{
    public interface IMauSacRespo
    {

        Task<IEnumerable<MauSac>> GetListAsync();
        Task<MauSac?> GetByKeyAsync(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(MauSac entity);
        Task<bool> AddAsync(MauSac entity);

    }
}
