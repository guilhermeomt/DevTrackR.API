using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistance.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevTrackR.API.Controllers
{
  [ApiController]
  [Route("api/packages")]
  public class PackagesController : ControllerBase
  {
    private readonly IPackageRepository _packageRepository;
    public PackagesController(IPackageRepository packageRepository)
    {
      _packageRepository = packageRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
      var packages = _packageRepository.GetAll();

      return Ok(packages);
    }

    [HttpGet("{code}")]
    public IActionResult GetByCode(string code)
    {
      var package = _packageRepository.GetByCode(code);

      if (package == null)
      {
        return NotFound();
      }

      return Ok(package);
    }

    [HttpPost]
    public IActionResult Post(AddPackageInputModel model)
    {
      if (model.Title.Length < 10)
      {
        return BadRequest("Title must be at least 10 characters long");
      }

      var package = new Package(model.Title, model.Weight);

      _packageRepository.Add(package);

      return CreatedAtAction("GetByCode", new { code = package.Code }, package);
    }

    [HttpPost("{code}/updates")]
    public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model)
    {
      var package = _packageRepository.GetByCode(code);

      if (package == null)
      {
        return NotFound();
      }

      package.AddUpdate(model.Status, model.Delivered);
      _packageRepository.Update(package);

      return NoContent();
    }
  }
}