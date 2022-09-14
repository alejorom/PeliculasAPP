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
    options.SwaggerDoc("PeliculasAPICategorias", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "API Categorías",
        Version = "1",
        Description = "Backend películas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "study@just4learn.com",
            Name = "J4L",
            Url = new Uri("https://just4learn.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
        }
    });

    options.SwaggerDoc("PeliculasAPIPeliculas", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "API Películas",
        Version = "1",
        Description = "Backend películas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "study@just4learn.com",
            Name = "J4L",
            Url = new Uri("https://just4learn.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
        }
    });

    options.SwaggerDoc("PeliculasAPIUsuarios", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "API Usuarios ",
        Version = "1",
        Description = "Backend películas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
        {
            Email = "study@just4learn.com",
            Name = "J4L",
            Url = new Uri("https://just4learn.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense()
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
        }
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
            options.SwaggerEndpoint("/swagger/PeliculasAPICategorias/swagger.json", "API Categorías ");
            options.SwaggerEndpoint("/swagger/PeliculasAPIPeliculas/swagger.json", "API Películas");
            options.SwaggerEndpoint("/swagger/PeliculasAPIUsuarios/swagger.json", "API Usuarios");
            options.RoutePrefix = "";
        });
}

app.UseHttpsRedirection();

/* Habilitar CORS para el API */
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
