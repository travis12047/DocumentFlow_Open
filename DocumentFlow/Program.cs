using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// 2023-04-10 iwai Add
// AutoMapper�Ή�
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 2023-05-16 iwai Add
// �N�b�L�[�F�ؑΉ�
builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.Secure = CookieSecurePolicy.Always;
	options.HttpOnly = HttpOnlyPolicy.Always;
});
// 2023-06-04 iwai Add
// �N�b�L�[�F�ؑΉ�
builder.Services.Configure<RouteOptions>(options => {
	// URL�͏������ɂ���
	options.LowercaseUrls = true;
});
// 2023-06-04 iwai Mod
// �N�b�L�[�F�ؑΉ�
builder.Services
		//.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		//.AddAuthentication(options =>
		//{
		//	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		//})
		//.AddCookie();.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie(options => {
			// �N�b�L�[�̖��O��ς���
			options.Cookie.Name = "auth";

			// ���_�C���N�g���郍�O�C��URL���������ɕς���
			// ~/Account/Login =�� ~/account/login
			options.LoginPath = CookieAuthenticationDefaults.LoginPath.ToString().ToLower();
		});
// 2023-05-25 iwai Add
// HTML���������Ή�
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

// 2023-05-16 iwai Add
// Authorize(���F�@�\)�Ή�
builder.Services.AddControllers(options =>
{
	// �f�t�H���g��[Authorize]������t�^
	var authFilter = new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
	options.Filters.Add(authFilter);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 2023-05-16 iwai Add
// �N�b�L�[�F�ؑΉ�
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	//pattern: "{controller=Home}/{action=Index}/{id?}");
	//pattern: "{controller=Login}/{action=Login}/{id?}");
	pattern: "{controller=Account}/{action=Login}/{id?}");
//pattern: "{controller=DocCreate}/{action=Create}/{id?}");

app.Run();
