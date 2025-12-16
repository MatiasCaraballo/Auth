using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
                                     
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

//Add Services
builder.Services.AddScoped<IRegisterUserRoleService, RegisterUserRoleService>();

builder.Services.AddScoped<ILoginJWTService, LoginJWTService>();

builder.Services.AddScoped<ILogoutService, LogoutService>();


// Add Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insert 'Bearer' followed by the token JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});

//Connection to the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Add the Endpoints from Microsoft.AspNetCore.Identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()                    
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Configuration for users login 
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

//Add Roles in the Authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", p => p.RequireRole("Admin"));
    options.AddPolicy("Client", p => p.RequireRole("Client"));
    options.AddPolicy("AdminOrClient", p => p.RequireRole("Admin", "Client"));
});

//Adds the Authentication policy services
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey)) { throw new InvalidOperationException("No JWT:Key was specified in the configuration"); }

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrEmpty(jwtIssuer)) { throw new InvalidOperationException("No JWT:issuer was specified in the configuration"); }

var jwtAudience = builder.Configuration["Jwt:Audience"];
if (string.IsNullOrEmpty(jwtAudience)) { throw new InvalidOperationException("No JWT:audience was specified in the configuration"); }

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = builder.Configuration["Jwt:RoleClaimType"],
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure Swagger environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<IdentityUser>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();

    await RoleInitializer.SeedRoles(services);
}

app.Run();
