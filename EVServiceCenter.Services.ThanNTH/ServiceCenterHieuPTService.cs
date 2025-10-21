using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class ServiceCenterHieuPTService
{
    private readonly UnitOfWork _unitOfWork;

    public ServiceCenterHieuPTService()
    {
        _unitOfWork = new UnitOfWork();
    }

    public async Task<List<ServiceCenterHieupt>> GetAll()
    {
        return await _unitOfWork.ServiceCenterHieuPTRepository.GetAll();
    }
}
