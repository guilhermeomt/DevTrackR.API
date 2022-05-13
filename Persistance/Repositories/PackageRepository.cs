using DevTrackR.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrackR.API.Persistance.Repositories
{
  public class PackageRepository : IPackageRepository
  {
    private readonly DevTrackRContext _context;
    public PackageRepository(DevTrackRContext context)
    {
      _context = context;
    }
    public async Task Add(Package package)
    {
      _context.Packages.Add(package);
      await _context.SaveChangesAsync();
    }

    public async Task<List<Package>> GetAll()
    {
      return await _context.Packages.ToListAsync();
    }

    public async Task<Package> GetByCode(string code)
    {
      return await _context.Packages
                      .Include(p => p.Updates)
                      .SingleOrDefaultAsync(p => p.Code == code);
    }

    public async Task Update(Package package)
    {
      _context.Entry(package).State = EntityState.Modified;
      await _context.SaveChangesAsync();
    }
  }
}