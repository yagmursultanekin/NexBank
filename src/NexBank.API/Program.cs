using Microsoft.EntityFrameworkCore;
using NexBank.Application.Interfaces;
using NexBank.Application.Services;
using NexBank.Domain.Interfaces;
using NexBank.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Veritabanı
builder.Services.AddDbContext<NexBankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository kayıtları
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// Service kayıtları
builder.Services.AddScoped<IAccountService, AccountService>();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<NexBank.Application.Mappings.MappingProfile>();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();