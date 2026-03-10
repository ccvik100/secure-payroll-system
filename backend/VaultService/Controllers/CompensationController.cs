using VaultService.Models;
using VaultService.Services;
using Microsoft.AspNetCore.Mvc;

namespace VaultService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompensationController : ControllerBase
{
    private readonly CompensationService _compensationService;

    public CompensationController(CompensationService compensationService)
    {
        _compensationService = compensationService;
    }

    [HttpGet]
    public async Task<List<Compensation>> Get() => await _compensationService.GetAsync();

    // Ezt a végpontot fogjuk a legtöbbször használni: lekérjük Gipsz Jakab bérét az ID-ja alapján
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<Compensation>> GetByEmployeeId(string employeeId)
    {
        var compensation = await _compensationService.GetByEmployeeIdAsync(employeeId);
        if (compensation is null) return NotFound();
        return compensation;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Compensation newCompensation)
    {
        await _compensationService.CreateAsync(newCompensation);
        return CreatedAtAction(nameof(GetByEmployeeId), new { employeeId = newCompensation.EmployeeId }, newCompensation);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Compensation updatedCompensation)
    {
        updatedCompensation.Id = id;
        await _compensationService.UpdateAsync(id, updatedCompensation);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _compensationService.RemoveAsync(id);
        return NoContent();
    }
}