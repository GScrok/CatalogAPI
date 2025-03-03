using AutoMapper;
using Microsoft.EntityFrameworkCore;

using CatalogAPI.mapper;
using CatalogAPI.Models.Context;
using CatalogAPI.Models.Seeder;
using CatalogAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

# region DataBase
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CatalogoDb"));

# endregion 


# region Registrando Repositorios
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
# endregion

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

# region DbSeeder
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    DbSeeder.Initialize(dbContext);
}
# endregion

app.Run();
