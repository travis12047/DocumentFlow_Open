using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// 2023-04-10 Add
// AutoMapper対応
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 2023-05-16 Add
// クッキー認証対応
builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.Secure = CookieSecurePolicy.Always;
	options.HttpOnly = HttpOnlyPolicy.Always;
});
// 2023-06-04 Add
// クッキー認証対応
builder.Services.Configure<RouteOptions>(options =>
{
	// URLは小文字にする
	options.LowercaseUrls = true;
});
// 2023-06-04 Mod
// クッキー認証対応
builder.Services
		//.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		//.AddAuthentication(options =>
		//{
		//	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		//})
		//.AddCookie();.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(options =>
		{
			// クッキーの名前を変える
			options.Cookie.Name = "auth";

			// リダイレクトするログインURLも小文字に変える
			// ~/Account/Login =＞ ~/account/login
			options.LoginPath = CookieAuthenticationDefaults.LoginPath.ToString().ToLower();
		});
// 2023-05-25 Add
// HTML文字化け対応
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

// 2023-05-16 Add
// Authorize(承認機能)対応
builder.Services.AddControllers(options =>
{
	// デフォルトで[Authorize]属性を付与
	var authFilter = new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
	options.Filters.Add(authFilter);
});

// 2023-06-16 Add
// EC2ヘルスチェック対応
builder.Services.AddHealthChecks();

var app = builder.Build();

// 2023-06-13 Mod
// Apache 搭載の Linux での ASP.NET Core ホスト対応
// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//	app.UseExceptionHandler("/Home/Error");
//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//	app.UseHsts();
//}

//app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

// 2023-06-13 Add
// Apache 搭載の Linux での ASP.NET Core ホスト対応
// using Microsoft.AspNetCore.HttpOverrides;
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseStaticFiles();

app.UseRouting();

// 2023-05-16 Add
// クッキー認証対応
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Account}/{action=Login}/{id?}");

// 2023-06-16 Add
// EC2ヘルスチェック対応
app.MapHealthChecks("/healthz", new HealthCheckOptions
{
	ResultStatusCodes =
		{
			[HealthStatus.Healthy] = StatusCodes.Status200OK,
			[HealthStatus.Degraded] = StatusCodes.Status200OK,
			[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
		}
}).RequireHost("*:5001");

app.UseEndpoints(endpoints =>
{
	endpoints.MapHealthChecks("/healthz");
	endpoints.MapControllers();
});

app.Run();
