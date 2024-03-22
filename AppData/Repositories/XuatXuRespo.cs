using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Data.Repositories
{
    public class XuatXuRespo : IXuatXuRespo
    {

        private readonly BazaizaiContext _context;
        public XuatXuRespo()
        {
            _context = new BazaizaiContext();
        }
        public async Task<bool> AddAsync(XuatXu entity)
        {
            try
            {
                await _context.XuatXus.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var entity = await GetByKeyAsync(id);
                if (entity == null) return false;
                _context.XuatXus.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<XuatXu?> GetByKeyAsync(string id)
        {
            return await _context.XuatXus.FirstOrDefaultAsync(x => x.IdXuatXu == id);
        }

        public async Task<IEnumerable<XuatXu>> GetListAsync()
        {
            return await _context.XuatXus.ToListAsync();
        }

        public async Task<bool> UpdateAsync(XuatXu entity)
        {
            try
            {
                _context.XuatXus.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
