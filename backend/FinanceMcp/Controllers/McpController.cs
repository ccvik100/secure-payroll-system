using FinanceMcp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinanceMcp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class McpController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public McpController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // 1. Eszközök (Tools) kiajánlása az AI számára
    [HttpGet("tools")]
    public ActionResult<List<McpTool>> GetTools()
    {
        var tools = new List<McpTool>
        {
            new McpTool
            {
                Name = "get_employee_financial_summary",
                Description = "Lekérdezi egy dolgozó alapadatait és béradatait az ID-ja alapján.",
                InputSchema = new { type = "object", properties = new { employeeId = new { type = "string" } }, required = new[] { "employeeId" } }
            }
        };
        return Ok(tools);
    }

    // 2. Az eszköz végrehajtása (Execute)
    [HttpPost("execute")]
    public async Task<ActionResult<McpExecuteResponse>> ExecuteTool([FromBody] McpExecuteRequest request)
    {
        if (request.ToolName == "get_employee_financial_summary")
        {
            if (!request.Arguments.TryGetValue("employeeId", out var empIdObj))
                return BadRequest(new McpExecuteResponse { IsSuccess = false, Error = "Missing employeeId parameter." });

            string employeeId = empIdObj.ToString()!;
            
            try
            {
                // 1. Lekérdezzük a nevet a Directory Service-ből
                var directoryClient = _httpClientFactory.CreateClient("DirectoryService");
                var dirResponse = await directoryClient.GetAsync($"/api/employee/{employeeId}");
                var directoryData = dirResponse.IsSuccessStatusCode ? await dirResponse.Content.ReadAsStringAsync() : "N/A";

                // 2. Lekérdezzük a fizetést a Vault Service-ből
                var vaultClient = _httpClientFactory.CreateClient("VaultService");
                var vaultResponse = await vaultClient.GetAsync($"/api/compensation/employee/{employeeId}");
                var vaultData = vaultResponse.IsSuccessStatusCode ? await vaultResponse.Content.ReadAsStringAsync() : "N/A";

                // 3. Összefűzzük a választ az AI-nak
                var result = $"Directory Data: {directoryData} | Vault Data: {vaultData}";
                return Ok(new McpExecuteResponse { IsSuccess = true, Result = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new McpExecuteResponse { IsSuccess = false, Error = ex.Message });
            }
        }

        return NotFound(new McpExecuteResponse { IsSuccess = false, Error = "Tool not found." });
    }
}