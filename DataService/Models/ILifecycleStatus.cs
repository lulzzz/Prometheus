﻿using System;

namespace DataService.Models
{
	public interface ILifecycleStatus
	{
		bool CatalogVisible { get; set; }
		string Comment { get; set; }
		Guid? Id { get; set; }
		string Name { get; set; }
		int Position { get; set; }
	}
}