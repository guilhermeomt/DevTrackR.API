using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistance.Repositories;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DevTrackR.API.Controllers
{
  [ApiController]
  [Route("api/packages")]
  public class PackagesController : ControllerBase
  {
    private readonly IPackageRepository _packageRepository;
    private readonly ISendGridClient _sendGridClient;

    public PackagesController(IPackageRepository packageRepository, ISendGridClient sendGridClient)
    {
      _packageRepository = packageRepository;
      _sendGridClient = sendGridClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var packages = await _packageRepository.GetAll();

      return Ok(packages);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
      var package = await _packageRepository.GetByCode(code);

      if (package == null)
      {
        return NotFound();
      }

      return Ok(package);
    }

    /// <summary>
    /// Cadastro de um pacote
    /// </summary>
    /// <remarks>
    /// {
    ///   "title": "Coleção de Cartas",
    ///   "weight": 1,
    ///   "senderName": "Guilherme",
    ///   "senderEmail": "tidaf28332@roxoas.com"
    /// }
    /// </remarks>
    /// <param name="model">Dados de um pacote</param>
    /// <returns>Objeto recém-criado</returns>
    /// <response code="201">Objeto recém-criado</response>
    /// <response code="400">Objeto inválido</response>
    [HttpPost]
    public async Task<IActionResult> Post(AddPackageInputModel model)
    {
      if (model.Title.Length < 10)
      {
        return BadRequest("Title must be at least 10 characters long");
      }

      var package = new Package(model.Title, model.Weight);

      await _packageRepository.Add(package);

      var message = new SendGridMessage()
      {
        From = new EmailAddress("tidaf28332@roxoas.com", "Guilherme"),
        Subject = "Your package was dispatched",
        PlainTextContent = $"Your package with code {package.Code} was dispatched",
      };

      message.AddTo(model.SenderEmail, model.SenderName);

      await _sendGridClient.SendEmailAsync(message);

      return CreatedAtAction("GetByCode", new { code = package.Code }, package);
    }

    [HttpPost("{code}/updates")]
    public async Task<IActionResult> PostUpdate(string code, AddPackageUpdateInputModel model)
    {
      var package = await _packageRepository.GetByCode(code);

      if (package == null)
      {
        return NotFound();
      }

      package.AddUpdate(model.Status, model.Delivered);
      await _packageRepository.Update(package);

      return NoContent();
    }
  }
}