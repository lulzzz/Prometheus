﻿using Common.Enums;

namespace Common.Dto
{
	public interface IServiceRequestUserInputDto
	{
		int Id { get; set; }
		int ServiceRequestId { get; set; }
		int InputId { get; set; }
		UserInputType UserInputType { get; set; }
		string Name { get; set; } 
		string Value { get; set; }
	}
}