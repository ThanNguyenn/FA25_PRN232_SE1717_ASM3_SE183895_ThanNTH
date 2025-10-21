using EVServiceCenter.Repositories.ThanNTH.Basic;
using EVServiceCenter.Repositories.ThanNTH.DBContext;
using EVServiceCenter.Repositories.ThanNTH.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Repositories.ThanNTH;
public class ServiceCenterHieuPTRepository : GenericRepository<ServiceCenterHieupt>
{
    public ServiceCenterHieuPTRepository()
    {

    }

    public ServiceCenterHieuPTRepository(FA25_PRN232_SE1717_G3_EVServiceCenterContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceCenterHieupt>> GetAll()
    {
        return await _context.ServiceCenterHieupts.ToListAsync();
    }
}
