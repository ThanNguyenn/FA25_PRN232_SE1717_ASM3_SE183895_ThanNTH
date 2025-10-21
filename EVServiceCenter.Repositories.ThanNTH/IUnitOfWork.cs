using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Repositories.ThanNTH;
public interface IUnitOfWork
{
    CenterPartThanNthRepository CenterPartThanNthRepository { get; }
    PartThanNthRepository PartThanNthRepository { get; }
    ServiceCenterHieuPTRepository ServiceCenterHieuPTRepository { get; }

    int SaveChangesWithTransaction();
    Task<int> SaveChangesWithTransactionAsync();
}
