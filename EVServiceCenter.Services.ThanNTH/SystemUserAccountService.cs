using EVServiceCenter.Repositories.ThanNTH;
using EVServiceCenter.Repositories.ThanNTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class SystemUserAccountService
{
    private readonly UnitOfWork _unitOfWork;
    public SystemUserAccountService()
    {
        _unitOfWork = new UnitOfWork();
    }

    public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
    {
        try
        {
            return await _unitOfWork.SystemUserAccountRepository.GetUserAccount(userName, password);
        }
        catch (Exception ex)
        {

        }
        return null;
    }
}
