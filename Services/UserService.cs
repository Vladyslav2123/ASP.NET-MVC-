using HW_11.Interfaces;
using HW_11.Models;
using HW_11.Models.Form;
using HW_11.Models.Rest;
using Microsoft.EntityFrameworkCore;

namespace HW_11.Services;

public class UserService
{
	private readonly IUserRepository _userRepository;
	private readonly ImageStorage _storage;
	private readonly IPositionRepository _positionRepository;
	private readonly IUserSkillRepository _userSkillRepository;
	private readonly IImageRepository _imageRepository;

	public UserService(IUserRepository context,
		IPositionRepository positionRepository,
		IImageRepository imageRepository,
		IUserSkillRepository userSkillRepository,
		ImageStorage storage)
	{
		_userRepository = context;
		_storage = storage;
		_positionRepository = positionRepository;
		_userSkillRepository = userSkillRepository;
		_imageRepository = imageRepository;
	}

	public async Task Add(UserForm form)
	{
		var user = await CreateNewUser(form);

		await _userRepository.AddAsync(user);
		await _userRepository.SaveAsync();
	}

	public UserForm GetAsync(int id)
	{
		var user = _userRepository.FindInclude(x => x.Id == id, x => x.Images);

		if (user == null)
		{
			return null!;
		}

		return new UserForm(user);
	}

	public async Task Update(int id, UserForm form)
	{
		var user = await _userRepository.GetByIdAsync(id);
		if (user != null)
		{
			user.FirstName = form.FirstName;
			user.LastName = form.LastName;
			user.Description = form.Description;
			user.IsActive = form.IsActive;

			if (form.Birthday != null)
			{
				user.Birthday = (DateTime)form.Birthday;
			}

			if (form.Image != null)
			{
				var listImage = await _storage.SaveFile(form.Image);
				foreach (var image in listImage)
				{
					user.Images.Add(image);
				}
			}

			user.PositionId = form.PositionId;

			await _userRepository.UpdateAsync(user);
			await _userRepository.SaveAsync();
		}
	}

	public async Task<List<Position>> GetAllPosition()
	{
		var list = await _positionRepository.GetAllAsync();
		return await list.ToListAsync();
	}

	public async Task<Position> GetPosition(int id)
	{
		var position = await _positionRepository.GetByIdAsync(id);
		return position;
	}

	public async Task<List<UserInfo>> GetAll()
	{
		var list = await _userRepository.GetAllWithInclude();
		return await list.ToListAsync();
	}

	public async Task Delete(int id)
	{
		var user = _userRepository.FindInclude(x => x.Id == id, x => x.Images);

		if (user != null)
		{
			foreach (var item in user.Images)
			{
				await _imageRepository.RemoveAsync(item);
				_storage.RemoveFile(item);
			}
			await _userRepository.RemoveAsync(user);
		}
		await _userRepository.SaveAsync();
	}

	public async Task DeleteImage(ImageDeleteModel model)
	{
		var user = _userRepository.FindInclude(x => x.Id == model.Id, x => x.Images);

		var img = user.Images.FirstOrDefault(x => x.GetImage() == model.Src);

		if (img != null)
		{
			user.Images.Remove(img);
			await _imageRepository.RemoveAsync(img);
			_storage.RemoveFile(img);
		}
		await _userRepository.SaveAsync();
	}

	public async Task<UserInfo> CreateNewUser(UserForm form)
	{
		var user = new UserInfo();

		user.FirstName = form.FirstName;
		user.LastName = form.LastName;
		user.Description = form.Description;
		user.IsActive = form.IsActive;
		if (form.Birthday != null)
		{
			user.Birthday = (DateTime)form.Birthday;
		}

		user.PositionId = form.PositionId;

		if (form.Image != null)
		{
			var listImage = await _storage.SaveFile(form.Image);
			foreach (var image in listImage)
			{
				user.Images.Add(image);
			}
		}

		return user;
	}

	public async Task<List<UserInfo>> Search(string searchString)
	{
		var list = await _userRepository
			.FindAsync(u =>
				u.FirstName.ToLower().Contains(searchString.ToLower()) ||
				u.LastName.ToLower().Contains(searchString.ToLower()) ||
				u.Description.ToLower().Contains(searchString.ToLower()) ||
				u.Position!.Title!.ToLower().Contains(searchString.ToLower()));

		return await list.ToListAsync();
	}

	public async Task<IEnumerable<UserSkill>> GetUserSkill(int id)
	{
		var userskills = await _userSkillRepository.GetUserSkills(id);

		return await userskills.ToListAsync();
	}
}