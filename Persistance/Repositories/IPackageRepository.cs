using DevTrackR.API.Entities;

namespace DevTrackR.API.Persistance.Repositories
{

  public interface IPackageRepository
  {
    Task<List<Package>> GetAll();
    Task<Package> GetByCode(string code);
    Task Add(Package package);
    Task Update(Package package);
  }
}