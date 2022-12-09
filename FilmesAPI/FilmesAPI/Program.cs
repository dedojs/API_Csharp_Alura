using FilmesAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


#region Conexão ao banco Alura
//criamos uma variavel de conexão
var connectionString = builder.Configuration.GetConnectionString("FilmeConnection");

//Adicionamos um services utilizando o UseMysql para conectar ao banco
builder.Services.AddDbContext<FilmeContext>(opts =>
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#endregion


#region Adicionando Mapper
// Adicionado o automapper para converter um filmedto em filme
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(); // Adicionado o newtonsoftJson para efetuar patch
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Adicionando a documentação ao swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilmesAPI", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
#endregion
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
