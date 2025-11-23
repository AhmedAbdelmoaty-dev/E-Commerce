
using Application.Contracts.Seeders;
using Application.Extensions;
using Application.RequestHelpers;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using API.Middlewares;
using API.Registrations;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

var app = builder.Build();

using (var scope =app.Services.CreateScope())
{
    var services= scope.ServiceProvider;
    var productSeeder = services.GetRequiredService<IProductSeeder>();
    await productSeeder.SeedDataAsync();
    var deliveryMethodSeeder = services.GetRequiredService<IDeliveryMethodSeeder>();
    await deliveryMethodSeeder.SeedDataAsync();
}


app.UseMiddleware<ErrorHandelingMiddleware>();

app.UseHangfireDashboard();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(policy => policy
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
