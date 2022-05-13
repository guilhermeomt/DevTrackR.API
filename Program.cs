using DevTrackR.API.Persistance;
using DevTrackR.API.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Development");

var sengGridApiKey = builder.Configuration.GetSection("SendGridApiKey").Value;

// Add services to the container.
builder.Services.AddSendGrid(o => o.ApiKey = sengGridApiKey);
// builder.Services
//        .AddDbContext<DevTrackRContext>(o => o.UseSqlServer(connectionString));
builder.Services
       .AddDbContext<DevTrackRContext>(o => o.UseInMemoryDatabase("DevTrackRDb"));
builder.Services.AddScoped<IPackageRepository, PackageRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
  o.SwaggerDoc("v1", new OpenApiInfo { Title = "DevTrackR API", Version = "v1" });
  var xmlPath = Path.Combine(AppContext.BaseDirectory, "DevTrackR.API.xml");
  o.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
