using BookStoreApp.API;
using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using BookStoreApp.API.Middleware;
using BookStoreApp.API.Repository;
using BookStoreApp.API.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// AddAsync services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Host.UseSerilog((ctx, config) =>
    config.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsOptions =>
    {
        corsOptions.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    // cfg.AddProfile<GlobalMappingConfiguration>();
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddDbContext<BookStoreDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("BookStoreDbConnection")));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<BookStoreDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorReportingMiddleware>();

app.MapControllers();

app.Run();
