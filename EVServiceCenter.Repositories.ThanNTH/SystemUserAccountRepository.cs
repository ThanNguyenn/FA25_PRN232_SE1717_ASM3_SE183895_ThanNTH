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
public class SystemUserAccountRepository : GenericRepository<SystemUserAccount>
{
    public SystemUserAccountRepository() { }
    public SystemUserAccountRepository(FA25_PRN232_SE1717_G3_EVServiceCenterContext context) => _context = context;

    public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
    {
        return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.Email == userName && u.Password == password && u.IsActive == true);
        //return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.Phone == userName && u.Password == password && u.IsActive == true);
        //return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.EmployeeCode == userName && u.Password == password && u.IsActive == true);
        //return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && u.IsActive == true);
    }
}
