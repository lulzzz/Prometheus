﻿namespace Common.Dto
{
	public interface IServiceRequestOptionDto : IUserCreatedEntityDto
	{
		int Id { get; set; }
		int ServiceId { get; set; }
	}
}