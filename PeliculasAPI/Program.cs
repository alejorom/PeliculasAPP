using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeliculasAPI.Data;
using PeliculasAPI.Helpers;
using PeliculasAPI.Mappers;
using PeliculasAPI.Repository;
using PeliculasAPI.Repository.IRepository;
using System.Net;
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
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

    //Primero definir el esquema de seguridad
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT (Bearer)",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        }, new List<string>()
    }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(builder => {
        builder.Run(async context => {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error = context.Features.Get<IExceptionHandlerFeature>();

            if (error != null)
            {
                context.Response.AddApplicationError(error.Error.Message);
                await context.Response.WriteAsync(error.Error.Message);
            }
        });
    });
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/PeliculasAPICategorias/swagger.json", "API Categorías ");
    options.SwaggerEndpoint("/swagger/PeliculasAPIPeliculas/swagger.json", "API Películas");
    options.SwaggerEndpoint("/swagger/PeliculasAPIUsuarios/swagger.json", "API Usuarios");
    options.RoutePrefix = "";
});

app.UseHttpsRedirection();

/* Habilitar CORS para el API */
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
