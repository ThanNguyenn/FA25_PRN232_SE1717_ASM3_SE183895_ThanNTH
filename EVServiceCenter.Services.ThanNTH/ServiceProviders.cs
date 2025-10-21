using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public class ServiceProviders : IServiceProviders
{
    ICenterPartThanNthService _centerPartThanNthService;
    PartThanNthService _partThanNthService;
    ServiceCenterHieuPTService _serviceCenterHieuPTService;
    SystemUserAccountService _systemUserAccountService;

    public SystemUserAccountService SystemUserAccountService 
    {
        get
        {
            return _systemUserAccountService ?? new SystemUserAccountService();
        }
    }
    public ICenterPartThanNthService CenterPartThanNthService 
    {
        get
        {
            return _centerPartThanNthService ?? new CenterPartThanNthService();
        }
    }

    public PartThanNthService PartThanNthService 
    {
        get
        {
            return _partThanNthService ?? new PartThanNthService();
        }
    }

    public ServiceCenterHieuPTService ServiceCenterHieuPTService 
    {
        get
        {
            return _serviceCenterHieuPTService ?? new ServiceCenterHieuPTService();
        }
    }

}
