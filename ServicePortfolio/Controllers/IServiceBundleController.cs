﻿using Common.Dto;
using System;
using System.Collections.Generic;

namespace ServicePortfolio.Controllers
{
	public interface IServiceBundleController
	{
		/// <summary>
		/// Finds service bundle with identifier provided and returns its DTO
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceBundleId"></param>
		/// <returns></returns>
		IServiceBundleDto GetServiceBundle(int userId, int serviceBundleId);

		/// <summary>
		/// KVP of all service bundle IDs and names in ascending order by name
		/// </summary>
		/// <returns></returns>
		IEnumerable<Tuple<int, string>> GetServiceBundleNames(int userId);

		/// <summary>
		/// Returns all service bundles
		/// </summary>
		/// <returns></returns>
		IEnumerable<IServiceBundleDto> GetServiceBundles(int userId);

		/// <summary>
		/// Saves the service bundle to the database
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceBundle"></param>
		/// <returns>Saved entity DTO</returns>
		IServiceBundleDto SaveServiceBundle(int userId, IServiceBundleDto serviceBundle);

		/// <summary>
		/// Deletes the service bundle from the database
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="serviceBundleId"></param>
		/// <returns>True if successful</returns>
		bool DeleteServiceBundle(int userId, int serviceBundleId);
	}
}