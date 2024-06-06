using HW_11.Interfaces;
using HW_11.Models;
using HW_11.Models.Form;
using HW_11.Models.Rest;

namespace HW_11.Services;

public class SkillService
{
	private readonly ISkillRepository _skillRepository;
	private readonly IUserSkillRepository _userSkillRepository;
	private readonly IImageRepository _imageRepository;
	private readonly ImageStorage _storage;

	public SkillService(ISkillRepository skillRepository, ImageStorage storage, IUserSkillRepository userSkillRepository, IImageRepository imageRepository)
	{
		_storage = storage;
		_skillRepository = skillRepository;
		_userSkillRepository = userSkillRepository;
		_imageRepository = imageRepository;
	}

	public Skill Get(int id)
	{
		var skill = _skillRepository.FindInclude(x => x.Id == id, x => x.Images);

		return skill;
	}

	public async Task<List<Skill>> GetAll()
	{
		var list = await _skillRepository.GetAllWithInclude();
		return list.ToList();
	}

	private async Task<bool> ExistsAsync(string name, int? id = null)
	{
		var listSkill = await GetAll();
		if (listSkill != null)
		{
			var exists = listSkill.Exists(x => x.Name == name && x.Id != id);

			if (exists)
			{
				return true;
			}
		}
		return false;
	}

	public async Task<bool> CreateAsync(SkillForm form)
	{
		if (await ExistsAsync(form.Name!))
		{
			return false;
		}

		var skill = new Skill();
		skill.Name = form.Name!;

		if (form.Image != null)
		{
			var listImage = await _storage.SaveFile(form.Image);
			foreach (var image in listImage)
			{
				skill.Images.Add(image);
			}
		}

		await _skillRepository.AddAsync(skill);
		await _skillRepository.SaveAsync();

		return true;
	}

	public async Task<int> GetCountSkill(int id)
	{
		var userSkill = await _userSkillRepository.FindAsync(x => x.UserId == id) as List<UserSkill>;

		return userSkill != null ? userSkill.Count : 0;
	}

	public async Task<bool> UpdateSkill(int id, SkillForm form)
	{
		if (await ExistsAsync(form.Name!, id))
		{
			return false;
		}

		var skill = _skillRepository.FindInclude(x => x.Id == id, x => x.Images);

		if (skill != null)
		{
			skill.Name = form.Name!;

			if (form.Image != null)
			{
				var listImage = await _storage.SaveFile(form.Image);
				foreach (var image in listImage)
				{
					skill.Images.Clear();
					skill.Images.Add(image);
				}
			}

			await _skillRepository.UpdateAsync(skill);
		}
		await _skillRepository.SaveAsync();

		return true;
	}

	public async Task DeleteSkill(int id)
	{
		var skill = _skillRepository.FindInclude(x => x.Id == id, x => x.Images);

		if (skill != null)
		{
			foreach (var item in skill.Images)
			{
				await _imageRepository.RemoveAsync(item);
				_storage.RemoveFile(item);
			}

			await _skillRepository.RemoveAsync(skill);
		}
		await _skillRepository.SaveAsync();
	}

	public async Task DeleteImage(ImageDeleteModel model)
	{
		var skill = _skillRepository.FindInclude(x => x.Id == model.Id, x => x.Images);
		var img = skill.Images.First(x => x.GetImage() == model.Src);

		if (img != null)
		{
			skill.Images.Remove(img);
			await _imageRepository.RemoveAsync(img);
			_storage.RemoveFile(img);

		}
		await _skillRepository.SaveAsync();
	}
}