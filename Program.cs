using HW_11.Infrastructure.DbContextApp;
using HW_11.Infrastructure.Repositories;
using HW_11.Interfaces;
using HW_11.Models;
using HW_11.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HW_11
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

			builder.Services.AddDbContext<ApplicationDBContext>(o => o.UseSqlite(connection));

			builder.Services.AddHttpClient();
			builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IPositionRepository, PositionRepository>();
			builder.Services.AddScoped<IImageRepository, ImageRepository>();
			builder.Services.AddScoped<ISkillRepository, SkillRepository>();
			builder.Services.AddScoped<IUserSkillRepository, UserSkillRepository>();
			builder.Services.AddScoped<IAccountRepository, AccountRepository>();
			builder.Services.AddScoped<IMapMarkerRepository, MapMarkerRepository>();
			builder.Services.AddScoped<IUserMarkerRepository, UserMarkerRepository>();

			builder.Services.AddScoped<ImageStorage>();
			builder.Services.AddScoped<UserService>();
			builder.Services.AddScoped<PositionService>();
			builder.Services.AddScoped<SkillService>();
			builder.Services.AddScoped<UserSkillService>();
			builder.Services.AddScoped<AccountService>();
			builder.Services.AddScoped<MarkerService>();


			builder.Services.AddIdentity<Account, IdentityRole<int>>(options =>
			{
				options.SignIn.RequireConfirmedPhoneNumber = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedAccount = false;

				options.Password.RequiredLength = 3;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredUniqueChars = 0;

			})
			.AddRoles<IdentityRole<int>>()
			.AddEntityFrameworkStores<ApplicationDBContext>()
			.AddDefaultTokenProviders();

			builder.Services.ConfigureApplicationCookie(o =>
			{
				o.LoginPath = new PathString("/Auth/Home/Login");
				o.LogoutPath = new PathString("/Auth/Home/Logout");
				o.AccessDeniedPath = new PathString("/Auth/Home/AccesDenied");
			});

			builder.Services.AddAntiforgery(o =>
			{
				o.HeaderName = "XSRF-TOKEN";
			});

			builder.Services.AddControllersWithViews();

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

			app.UseAuthentication();
			app.UseAuthorization();

			try
			{
				var scope = app.Services.CreateScope();

				var services = scope.ServiceProvider;

				var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

				if (!roleManager.RoleExistsAsync("Admin").Result)
				{
					roleManager.CreateAsync(new IdentityRole<int> { Name = "Admin" }).Wait();
				}

				if (!roleManager.RoleExistsAsync("User").Result)
				{
					roleManager.CreateAsync(new IdentityRole<int> { Name = "User" }).Wait();
				}
			}
			catch (Exception ex) { Console.WriteLine(ex.Message); }



			app.MapControllerRoute(
				name: "areas",
				pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}