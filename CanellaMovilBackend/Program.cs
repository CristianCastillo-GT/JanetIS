using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Middleware;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Service.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMvc();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ISAPService, SAPService>();
builder.Services.AddScoped<SAPConnectionFilter>();
builder.Services.AddScoped<RoleFilter>();
builder.Services.AddScoped<SAPConnectionFilter>();
builder.Services.AddScoped<BlockEndpointFilter>();
builder.Services.AddScoped<STODFilter>();
builder.Services.AddScoped<ResultAllFilter>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
          builder =>
          {
              builder.AllowAnyOrigin();
              builder.AllowAnyMethod();
              builder.AllowAnyHeader();
          });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder?.Configuration?.GetSection("JWT:Key")?.Value ?? "")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Documentación",
        Version= "v1",
        Description = ""
    });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Ingrese el Bearer Authorization de la siguiente manera: `Bearer Token-Generado`",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.DocInclusionPredicate((name, api) => true);
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                e => e.Key,
                e => e.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
            );

        var result = new
        {
            status = 400,
            result = "Fail",
            message = "Se produjeron uno o más errores de validación.",
            errors
        };

        context.HttpContext.Response.StatusCode = 400;
        context.HttpContext.Response.ContentType = "application/json";

        return new JsonResult(result)
        {
            StatusCode = 400
        };
    };
});

var app = builder.Build();

var swagger_url = builder?.Configuration?.GetSection("URL:SWAGGER")?.Value?.ToString();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"{swagger_url}","Web Services - General");
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseRateLimitingMiddleware();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
