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
    public class MauSacRespo:IMauSacRespo
    {
        private readonly BazaizaiContext _context;
        public MauSacRespo()
        {
            _context = new BazaizaiContext();
        }
        public async Task<bool> AddAsync(MauSac entity)
        {
            try
            {
                await _context.MauSacs.AddAsync(entity);
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
                _context.MauSacs.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<MauSac?> GetByKeyAsync(string id)
        {
            return await _context.MauSacs.FirstOrDefaultAsync(x => x.IdMauSac == id);
        }

        public async Task<IEnumerable<MauSac>> GetListAsync()
        {
            return await _context.MauSacs.ToListAsync();
        }

        public async Task<bool> UpdateAsync(MauSac entity)
        {
            try
            {
                _context.MauSacs.Update(entity);
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
