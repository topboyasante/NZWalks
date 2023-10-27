using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Dependency Injection
builder.Services.AddDbContext<NZWalksDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));
});

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
builder.Services.AddScoped<IWalksDifficultyRepository, WalksDifficultyRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
