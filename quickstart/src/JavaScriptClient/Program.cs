using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

builder.Services.AddBff()
                .AddRemoteApis();

builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";
        options.ClientId = "bff";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.Scope.Add("api1");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseBff();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapBffManagementEndpoints();
    // Uncomment this for Controller support
    // endpoints.MapControllers()
    //     .AsBffApiEndpoint();

    endpoints.MapGet("/local/identity", LocalIdentityHandler)
        .AsBffApiEndpoint();

    endpoints.MapRemoteBffApiEndpoint("/remote", "https://localhost:6001")
        .RequireAccessToken(Duende.Bff.TokenType.User);
});

app.Run();

[Authorize] 
static IResult LocalIdentityHandler(ClaimsPrincipal user)
{
    var name = user.FindFirst("name")?.Value ?? user.FindFirst("sub")?.Value;
    return Results.Json(new { message = "Local API Success!", user = name });
}