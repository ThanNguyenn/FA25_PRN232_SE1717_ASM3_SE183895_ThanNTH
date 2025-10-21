using EVServiceCenter.Repositories.ThanNTH.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVServiceCenter.Repositories.ThanNTH;


public class UnitOfWork : IUnitOfWork
{
    private readonly FA25_PRN232_SE1717_G3_EVServiceCenterContext _context;
    private readonly CenterPartThanNthRepository _centerPartThanNthRepository;
    private readonly PartThanNthRepository _partThanNthRepository;
    private readonly ServiceCenterHieuPTRepository _serviceCenterHieuPTRepository;
    public UnitOfWork()
    {
        _context ??= new FA25_PRN232_SE1717_G3_EVServiceCenterContext();
    }
    public CenterPartThanNthRepository CenterPartThanNthRepository 
    { 
        get
        {
            return _centerPartThanNthRepository ?? new CenterPartThanNthRepository(_context);
        }
    }

    public PartThanNthRepository PartThanNthRepository 
    { 
        get
        {
            return _partThanNthRepository ?? new PartThanNthRepository(_context);
        }
    }

    public ServiceCenterHieuPTRepository ServiceCenterHieuPTRepository 
    { 
        get
        {
            return _serviceCenterHieuPTRepository ?? new ServiceCenterHieuPTRepository(_context);
        }
    }


    public int SaveChangesWithTransaction()
    {
        int result = -1;

        using (var dbContextTransaction = _context.Database.BeginTransaction())
        {
            try
            {
                result = _context.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                result = -1;
                dbContextTransaction.Rollback();
            }
        }

        return result;
    }

    public async Task<int> SaveChangesWithTransactionAsync()
    {
        int result = -1;

        using (var dbContextTransaction = _context.Database.BeginTransaction())
        {
            try
            {
                result = await _context.SaveChangesAsync();
                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                result = -1;
                dbContextTransaction.Rollback();
            }
        }

        return result;
    }
}
