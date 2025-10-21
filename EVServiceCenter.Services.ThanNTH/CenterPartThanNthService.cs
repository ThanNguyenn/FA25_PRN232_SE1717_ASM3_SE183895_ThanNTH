using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.ModelExtensions;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class CenterPartThanNthService : ICenterPartThanNthService
{
    private readonly IUnitOfWork _unitOfWork;
    public CenterPartThanNthService()
    {
        _unitOfWork ??= new UnitOfWork();
    }

    public async Task<int> CreateAsync(CenterPartThanNth centerPart)
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.CreateAsync(centerPart);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var item = await _unitOfWork.CenterPartThanNthRepository.GetByIdAsync(id);
            if (item != null)
            {
                return await _unitOfWork.CenterPartThanNthRepository.RemoveAsync(item);
            }
        }
        catch (Exception ex)
        {
        }
        return false;
    }

    public async Task<List<CenterPartThanNth>> GetAllAsync()
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        //return new List<CenterPartThanNth>();
    }

    public async Task<CenterPartThanNth> GetByIdAsync(int id)
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.GetByIdAsync(id);

        }
        catch (Exception ex)
        {

        }
        return new CenterPartThanNth();
    }

    public async Task<PaginationResult<List<CenterPartThanNth>>> SaerchWithPagingAsync(CenterPartThanNthSearchRequest searchRequest)
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.SearchWithPagingAsync(searchRequest);
        }
        catch (Exception ex)
        {

        }
        return new PaginationResult<List<CenterPartThanNth>>();
    }

    public async Task<List<CenterPartThanNth>> SearchAsync(string? partStatus, int? availableQuantity, string? partName)
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.SearchAsync(partStatus, availableQuantity, partName);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<PaginationResult<List<CenterPartThanNth>>> SearchWithPagingAsync(CenterPartThanNthSearchRequest centerPartThanNthSearchRequest)
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.SearchWithPagingAsync(centerPartThanNthSearchRequest);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<int> UpdateAsync(CenterPartThanNth centerPart)
    {
        try
        {
            return await _unitOfWork.CenterPartThanNthRepository.UpdateAsync(centerPart);
        }
        catch (Exception ex)
        {

        }
        return 0;
    }
}
