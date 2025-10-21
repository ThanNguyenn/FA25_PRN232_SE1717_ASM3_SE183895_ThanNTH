using EVServiceCenter.Repositories.ThanNTH.ModelExtensions;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public interface ICenterPartThanNthService
{
    Task<List<CenterPartThanNth>> GetAllAsync();
    Task<CenterPartThanNth> GetByIdAsync(int id);
    Task<PaginationResult<List<CenterPartThanNth>>> SaerchWithPagingAsync(CenterPartThanNthSearchRequest searchRequest);

    Task<int> CreateAsync(CenterPartThanNth centerPart);
    Task<int> UpdateAsync(CenterPartThanNth centerPart);
    Task<bool> DeleteAsync(int id);

    Task<List<CenterPartThanNth>> SearchAsync(string? partStatus, int? availableQuantity, string? partName);
    Task<PaginationResult<List<CenterPartThanNth>>> SearchWithPagingAsync(CenterPartThanNthSearchRequest centerPartThanNthSearchRequest);
}
