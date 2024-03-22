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
    public class AnhRespo:IAnhRespo
    {

        private readonly BazaizaiContext _context;
        public AnhRespo()
        {
            _context = new BazaizaiContext();
        }
        public async Task<bool> AddAsync(Anh entity)
        {
            try
            {
                await _context.Anh.AddAsync(entity);
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
                _context.Anh.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<Anh?> GetByKeyAsync(string id)
        {
            return await _context.Anh.FirstOrDefaultAsync(x => x.IdAnh == id);
        }

        public async Task<IEnumerable<Anh>> GetListAsync()
        {
            return await _context.Anh.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Anh entity)
        {
            try
            {
                _context.Anh.Update(entity);
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
