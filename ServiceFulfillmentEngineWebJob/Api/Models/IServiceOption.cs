﻿using System;

namespace ServiceFulfillmentEngineWebJob.Api.Models
{
	public interface IServiceOption
	{
		int Id { get; set; }
		string Name { get; set; }
		int ServiceOptionCategoryId { get; set; }
		Guid? Picture { get; set; }
		string PictureMimeType { get; set; }
		double PriceUpFront { get; set; }
		double PriceMonthly { get; set; }
		double Cost { get; set; }
		string Utilization { get; set; }
		string Included { get; set; }
		string Procurement { get; set; }
		string Description { get; set; }
		string Details { get; set; }
		string BusinessValue { get; set; }
		int Popularity { get; set; }
		bool BasicRequest { get; set; }
	}
}