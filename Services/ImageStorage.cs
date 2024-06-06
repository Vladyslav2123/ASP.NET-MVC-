using HW_11.Models;

namespace HW_11.Services;

public class ImageStorage
{
	private readonly IWebHostEnvironment _environment;

	public ImageStorage(IWebHostEnvironment environment)
	{
		_environment = environment;
	}

	public async Task<List<Image>> SaveFile(List<IFormFile> file)
	{
		var list = new List<Image>();
		for (int i = 0; i < file.Count; i++)
		{
			var guid = Guid.NewGuid().ToString().ToLower();
			var filename = guid + Path.GetExtension(file[i].FileName);
			var fullname = Path.Combine(_environment.WebRootPath, "uploads", filename);

			using (var localFile = File.OpenWrite(fullname))
			{
				await file[i].CopyToAsync(localFile);
			}
			list.Add(new Image() { FileName = filename });
		}

		return list;
	}

	public async Task<Image> SaveUploadedFileAsync(IFormFile file)
	{
		var guid = Guid.NewGuid().ToString().ToLower();

		var filename = guid + Path.GetExtension(file.FileName);
		var fullFilename = Path.Combine(_environment.WebRootPath, "uploads", filename);

		using (var localFile = File.OpenWrite(fullFilename))
		{
			await file.CopyToAsync(localFile);
		}

		return new Image() { FileName = filename };
	}

	public void RemoveFile(Image image)
	{
		var fullname = Path.Combine(_environment.WebRootPath, "uploads", image.FileName!);
		File.Delete(fullname);
	}
}