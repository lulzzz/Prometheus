﻿namespace Common.Dto
{
	public interface ILifecycleStatusDto : IUserCreatedEntityDto
	{
		bool CatalogVisible { get; set; }
		int Id { get; set; }
		string Name { get; set; }
		int Position { get; set; }
		//int ServiceId { get; set; }
	}
}