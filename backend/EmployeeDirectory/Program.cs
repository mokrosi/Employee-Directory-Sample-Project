using EmployeeDirectory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeDirectory.Domain.Interfaces;
using EmployeeDirectory.Infrastructure.Repositories;
using EmployeeDirectory.Application.Interfaces;
using EmployeeDirectory.Infrastructure.Services;
using EmployeeDirectory.Application.Features.Auth.Commands.RegisterUser;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
