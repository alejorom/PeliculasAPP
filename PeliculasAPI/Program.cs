using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PeliculasAPI.Data;
using PeliculasAPI.Mappers;
using PeliculasAPI.Repository;
using PeliculasAPI.Repository.IRepository;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddAutoMapper(typeof(Mappers));

/*Agregar dependencia del token*/
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("PeliculasAPI", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "API Películas",
        Version = "1"
    });

    var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
    options.IncludeXmlComments(rutaApiComentarios);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint("/swagger/PeliculasAPI/swagger.json", "API Películas");
            options.RoutePrefix = "";
        });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
