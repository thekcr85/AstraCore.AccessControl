using AstraCore.AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AstraCore.AccessControl.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Employee> Employees => Set<Employee>();
	public DbSet<AccessCard> AccessCards => Set<AccessCard>();
	public DbSet<AccessPoint> AccessPoints => Set<AccessPoint>();
	public DbSet<AccessLog> AccessLogs => Set<AccessLog>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}