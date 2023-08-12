using DotNetArchitecture.Repository.Base;
using DotNetArchitecture.Repository.Interfaces;
using DotNetArchitecture.Repository.Services;
using DotNetArchitecture.Validator.Interfaces;
using DotNetArchitecture.Validator.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Logging
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
#endregion
#region DB Connection
builder.Services.AddDbContext<DBContext>(option=>option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion
#region Service Registration
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
#endregion
#region Validation Service Registration
builder.Services.AddTransient<IProductValidation, ProductValidation>();
#endregion


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
