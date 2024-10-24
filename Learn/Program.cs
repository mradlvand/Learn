using Learn.Common;
using Data.Context;
using Learn.Models.Aut;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//var connectionString = builder.Configuration.GetConnectionString("LearnContext");
//builder.Services.AddDbContext<DBLearnContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddDbContext<DBLearnContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString("LearnContext")), ServiceLifetime.Singleton);


builder.Services.AddScoped<IUserService, UserService>();

#region "JWT Token For Authentication Login"    
SiteKeys.Configure(builder.Configuration.GetSection("AppSettings"));
var key = Encoding.ASCII.GetBytes(SiteKeys.Token);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});


builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(token =>
 {
     token.RequireHttpsMetadata = false;
     token.SaveToken = true;
     token.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(key),
         ValidateIssuer = true,
         ValidIssuer = SiteKeys.WebSiteDomain,
         ValidateAudience = true,
         ValidAudience = SiteKeys.WebSiteDomain,
         RequireExpirationTime = true,
         ValidateLifetime = true,
         ClockSkew = TimeSpan.Zero
     };
 });

#endregion

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = 4L * 1024L * 1024L * 1024L;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.BufferBodyLengthLimit = 4L * 1024L * 1024L * 1024L;
    o.BufferBody = true;
    o.ValueCountLimit = int.MaxValue;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = Int64.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = Int64.MaxValue;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseDatabaseErrorPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.    
    app.UseHsts();
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<DBLearnContext>();
//    context.Database.EnsureCreated();
//   context.Database.in DbInitializer.Initialize(context);
//}

app.UseStaticFiles();

app.UseRouting();

#region "JWT Token For Authentication Login"    

app.UseCookiePolicy();
app.UseSession();
app.Use(async (context, next) =>
{
    var JWToken = context.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(JWToken))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
    }
    await next();
});
app.UseAuthentication();
app.UseAuthorization();


#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
