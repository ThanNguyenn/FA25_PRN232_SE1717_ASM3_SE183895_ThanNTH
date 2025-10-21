using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Services.ThanNTH;
public interface IServiceProviders
{
    ICenterPartThanNthService CenterPartThanNthService { get; }
    PartThanNthService PartThanNthService { get; }
    ServiceCenterHieuPTService ServiceCenterHieuPTService { get; }

    SystemUserAccountService SystemUserAccountService { get; }
}
