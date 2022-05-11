using DevTrackR.API.Entities;

namespace DevTrackR.API.Persistance
{
  public class DevTrackRContext
  {
    public DevTrackRContext()
    {
      Packages = new List<Package>();
    }

    public List<Package> Packages { get; set; }
  }
}