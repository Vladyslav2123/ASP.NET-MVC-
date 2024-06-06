using HW_11.Interfaces;
using HW_11.Models;
using HW_11.Models.Rest;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Services;

public class AccountService
{
	private readonly IAccountRepository _accountRepository;
	private readonly IImageRepository _imageRepository;
	private readonly ImageStorage _storage;

	public AccountService(IAccountRepository accountRepository, IImageRepository imageRepository, ImageStorage storage)
	{
		_accountRepository = accountRepository;
		_imageRepository = imageRepository;
		_storage = storage;
	}

	public async Task DeleteImage(ImageDeleteModel model)
	{
		var account = _accountRepository.FindInclude(x => x.Id == model.Id, x => x.Image!);
		var img = account.Image;

		if (img != null)
		{
			account.Image = null;
			await _imageRepository.RemoveAsync(img);
			await _imageRepository.SaveAsync();
			_storage.RemoveFile(img);
		}
		await _accountRepository.SaveAsync();
	}

	public Account GetAsync(int id)
	{
		return _accountRepository.FindInclude(x => x.Id == id, x => x.Image!);
	}

	public async Task<List<Account>> GetAccounts()
	{
		var list = await _accountRepository.GetAllAccount();
		return await list.ToListAsync();
	}
	public async Task<Image?> GetImageAsync(IFormFile? image)
	{
		var img = await _storage.SaveUploadedFileAsync(image!);

		if (img == null)
		{
			return null;
		}

		await _imageRepository.AddAsync(img);
		await _imageRepository.SaveAsync();

		return img;
	}
}