using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class PartThanNthService
{
    private readonly UnitOfWork _unitOfWork;

    public PartThanNthService()
    {
        _unitOfWork = new UnitOfWork();
    }

    public async Task<List<PartThanNth>> GetAllPartsAsync()
    {
        try
        {
            return await _unitOfWork.PartThanNthRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
        
        }
        return new List<PartThanNth>();
    }
}
