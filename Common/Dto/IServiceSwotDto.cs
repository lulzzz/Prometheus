﻿using Common.Enums;
using System.Collections.Generic;

namespace Common.Dto
{
	public interface IServiceSwotDto : IUserCreatedEntityDto
	{
		string Description { get; set; }
		string Item { get; set; }
		int Id { get; set; }
		ICollection<ISwotActivityDto> SwotActivities { get; set; }
		ServiceSwotType Type { get; set; }
	}
}