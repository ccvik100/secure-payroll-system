namespace FinanceMcp.Models;

public class McpTool
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public object InputSchema { get; set; } = null!; // JSON Schema a paraméterekhez
}

public class McpExecuteRequest
{
    public string ToolName { get; set; } = null!;
    public Dictionary<string, object> Arguments { get; set; } = new();
}

public class McpExecuteResponse
{
    public bool IsSuccess { get; set; }
    public string? Result { get; set; }
    public string? Error { get; set; }
}