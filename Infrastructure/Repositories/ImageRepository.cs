using HW_11.Infrastructure.DbContextApp;
using HW_11.Interfaces;
using HW_11.Models;

namespace HW_11.Infrastructure.Repositories;

public class ImageRepository : Repository<Image>, IImageRepository
{
	public ImageRepository(ApplicationDBContext context) : base(context) { }
}
