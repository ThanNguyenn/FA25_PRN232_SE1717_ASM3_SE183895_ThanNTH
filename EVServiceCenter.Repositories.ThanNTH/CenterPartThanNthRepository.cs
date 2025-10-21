using EVServiceCenter.Repositories.ThanNTH.Basic;
using EVServiceCenter.Repositories.ThanNTH.DBContext;
using EVServiceCenter.Repositories.ThanNTH.ModelExtensions;
using EVServiceCenter.Repositories.ThanNTH.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Repositories.ThanNTH;
public class CenterPartThanNthRepository : GenericRepository<CenterPartThanNth>
{
    public CenterPartThanNthRepository() { }
    public CenterPartThanNthRepository(FA25_PRN232_SE1717_G3_EVServiceCenterContext context) => _context = context;
    public async Task<List<CenterPartThanNth>> GetAllAsync()
    {
        var items = await _context.CenterPartThanNths.Include(c => c.Part).ToListAsync();

        return items ?? new List<CenterPartThanNth>();
    }

    public async Task<CenterPartThanNth> GetByIdAsync(int id)
    {
        var item = await _context.CenterPartThanNths.Include(c => c.Part).FirstOrDefaultAsync(c => c.CenterPartThanNthid == id);

        return item ?? new CenterPartThanNth();
    }

    public async Task<List<CenterPartThanNth>> SearchAsync(string? partStatus, int? availableQuantity, string? partName)
    {
        var items = await _context.CenterPartThanNths
     .Include(c => c.Part)
     .Where(c =>
         (string.IsNullOrEmpty(partStatus) || c.PartStatus.Contains(partStatus)) &&
         (availableQuantity == null || availableQuantity == 0 || c.AvailableQuantity == availableQuantity) &&
         (string.IsNullOrEmpty(partName) || c.Part.PartName.Contains(partName) || string.IsNullOrEmpty(partName)
     ))
     .OrderBy(c => c.CenterPartThanNthid)
     .ToListAsync();
        return items ?? new List<CenterPartThanNth>();
    }

    public async Task<PaginationResult<List<CenterPartThanNth>>> SearchWithPagingAsync(CenterPartThanNthSearchRequest searchRequest)
    {
        var items = await this.SearchAsync(searchRequest.PartStatus, searchRequest.AvailableQuantity, searchRequest.PartName);
        var totalItems = items.Count();
        var totalPages = (int)Math.Ceiling((double)totalItems / searchRequest.PageSize.Value);

        items = items.Skip((searchRequest.CurrentPage.Value - 1) * searchRequest.PageSize.Value).Take(searchRequest.PageSize.Value).ToList();

        var result = new PaginationResult<List<CenterPartThanNth>>
        {
            TotaItems = totalItems,
            TotalPages = totalPages,
            CurrentPage = searchRequest.CurrentPage.Value,
            PageSize = searchRequest.PageSize.Value,
            Items = items
        };

        return result ?? new PaginationResult<List<CenterPartThanNth>>();
    }
}
