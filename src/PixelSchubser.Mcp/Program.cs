using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PixelSchubser.Application.DependencyInjection;
using PixelSchubser.Mcp.Contracts;
using PixelSchubser.Mcp.Errors;
using PixelSchubser.Mcp.Tools;
using ModelContextProtocol.Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddPixelSchubserApplicationCore();
builder.Services.AddSingleton<McpErrorMapper>();
builder.Services.AddSingleton<ProjectTools>();
builder.Services.AddSingleton<SpriteEditTools>();
builder.Services.AddSingleton<RenderExportTools>();
builder.Services.AddSingleton<AnimationTools>();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<ProjectTools>()
    .WithTools<SpriteEditTools>()
    .WithTools<RenderExportTools>()
    .WithTools<AnimationTools>();

var contract = McpContractVersion.Current;
Console.WriteLine($"PixelSchubser MCP host ready: {contract.Namespace} v{contract.Version}");
await builder.Build().RunAsync();
