using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.Swagger;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.Host;

var builder = WebApplication.CreateBuilder(args);

builder.InstallModule<HostModuleInstaller>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHttpsRedirection()
    .UseCors(bld =>
        bld
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
    .UseAuthentication()
    .UseRouting()
    .UseAuthorization()

    .UseEndpoints(endpoint =>
    {
        endpoint.MapFastEndpoints();
    });

await app.RunModulesBootstrapperAsync();

app.Run();
