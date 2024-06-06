using HW_11.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Infrastructure.DbContextApp;

public class ApplicationDBContext : IdentityDbContext<Account, IdentityRole<int>, int>
{
	public ApplicationDBContext()
	{
	}

	public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
	{
	}

	public DbSet<UserInfo> UserInfos { get; set; }

	public DbSet<Position> Positions { get; set; }

	public DbSet<Image> Images { get; set; }

	public DbSet<Skill> Skills { get; set; }

	public DbSet<UserSkill> UserSkills { get; set; }

	public DbSet<MapMarker> MapMarkers { get; set; }

	public DbSet<UserMarker> UserMarkers { get; set; }
}