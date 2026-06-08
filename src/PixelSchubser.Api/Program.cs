using PixelSchubser.Api.Configuration;
using PixelSchubser.Api.Endpoints;
using PixelSchubser.Api.Errors;
using PixelSchubser.Application.DependencyInjection;
using PixelSchubser.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddPixelSchubserApplicationCore();
builder.Services.AddPixelSchubserExtensionRegistries();
builder.Services.AddSingleton<ProblemDetailsMapper>();
builder.Services.Configure<ApiHostOptions>(builder.Configuration.GetSection(ApiHostOptions.SectionName));
builder.Services.Configure<ApiVersioningOptions>(builder.Configuration.GetSection(ApiVersioningOptions.SectionName));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var apiV1 = app.MapGroup("/api/v1");
apiV1.MapProjectEndpoints();
apiV1.MapSpriteEditEndpoints();
apiV1.MapPreviewEndpoints();
apiV1.MapFormatEndpoints();
apiV1.MapAnimationEndpoints();
apiV1.MapProfileEndpoints();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Api-Version", "v1");
    await next();
});

app.Run();
