// using Microsoft.EntityFrameworkCore;
// using server.Application.UseCases;
// using server.Core.Interfaces;
// using server.Infrastructure.Data;
// using server.Infrastructure.Repositories;

// var builder = WebApplication.CreateBuilder(args);

// // DbContext (SQL Server)
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddControllers();
// builder.Services.AddAuthorization();

// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IRoleRepository, RoleRepository>();
// builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
// builder.Services.AddScoped<CreateUserUseCase>();

// // Swagger for API documentation
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// // create app
// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // routing and middleware
// app.UseHttpsRedirection();
// app.UseRouting();
// app.UseAuthorization();
// app.MapControllers();

// app.Run();

using Microsoft.EntityFrameworkCore;
using server.Application.UseCases;
using server.Core.Interfaces;
using server.Infrastructure.Data;
using server.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// CORS setup for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular app origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<CreateUserUseCase>();

// Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware configuration
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
