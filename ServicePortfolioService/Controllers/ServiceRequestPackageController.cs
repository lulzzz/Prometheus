using System;
using System.Collections.Generic;
using System.Linq;
using Common.Controllers;
using Common.Dto;
using Common.Enums;
using Common.Enums.Entities;
using Common.Exceptions;
using DataService;
using DataService.DataAccessLayer;
using DataService.Models;

namespace ServicePortfolioService.Controllers
{
	public class ServiceRequestPackageController : EntityController<IServiceRequestPackageDto>, IServiceRequestPackageController
	{
		/// <summary>
		/// Finds service package with identifier provided and returns its DTO
		/// </summary>
		/// <param name="performingUserId"></param>
		/// <param name="servicePackageId"></param>
		/// <returns></returns>
		public IServiceRequestPackageDto GetServiceRequestPackage(int performingUserId, int servicePackageId)
		{
			using (var context = new PrometheusContext())
			{
				var servicePackage = context.ServiceRequestPackages.Find(servicePackageId);
				if (servicePackage != null)
					return ManualMapper.MapServiceRequestPackageToDto(servicePackage);
				return null;
			}
		}

		/// <summary>
		/// Modifies the service Package in the database
		/// </summary>
		/// <param name="performingUserId"></param>
		/// <param name="servicePackage"></param>
		/// <param name="modification">Type of modification to make</param>
		/// <returns>Modified Service Request Package</returns>
		public IServiceRequestPackageDto ModifyServiceRequestPackage(int performingUserId, IServiceRequestPackageDto servicePackage, EntityModification modification)
		{
			return base.ModifyEntity(performingUserId, servicePackage, modification);
		}

		protected override IServiceRequestPackageDto Create(int performingUserId, IServiceRequestPackageDto entity)
		{
			using (var context = new PrometheusContext())
			{
				var servicePackage = context.ServiceRequestPackages.Find(entity.Id);
				if (servicePackage != null)
				{
					throw new InvalidOperationException(string.Format("Service Request Package with ID {0} already exists.", entity.Id));
				}
				var savedPackage = context.ServiceRequestPackages.Add(ManualMapper.MapDtoToServiceRequestPackage(entity));

				//Set tags to match DTO tags
				var categoryTags = new List<ServiceOptionCategoryTag>();
				foreach (var tag in entity.ServiceOptionCategoryTags)
				{
					categoryTags.Add(ManualMapper.MapDtoToServiceOptionCategoryTag(tag));
				}
				savedPackage.ServiceOptionCategoryTags = categoryTags;

				var serviceTags = new List<ServiceTag>();
				foreach (var tag in entity.ServiceTags)
				{
					serviceTags.Add((ManualMapper.MapDtoToServiceTag(tag)));
				}
				savedPackage.ServiceTags = serviceTags;

				context.SaveChanges(performingUserId);

				return ManualMapper.MapServiceRequestPackageToDto(savedPackage);
			}
		}

		protected override IServiceRequestPackageDto Update(int performingUserId, IServiceRequestPackageDto entity)
		{
			throw new ModificationException(string.Format("Modification {0} cannot be performed on Service Request Packages.", EntityModification.Update));
		}

		protected override IServiceRequestPackageDto Delete(int performingUserId, IServiceRequestPackageDto entity)
		{
			using (var context = new PrometheusContext())
			{
				var categoryTagsToDelete = context.ServiceOptionCategoryTags.Where(x => x.ServiceRequestPackageId == entity.Id);
				context.ServiceOptionCategoryTags.RemoveRange(categoryTagsToDelete);
				context.SaveChanges(performingUserId);

				var serviceTagsToDelete = context.ServiceTags.Where(x => x.ServiceRequestPackageId == entity.Id);
				context.ServiceTags.RemoveRange(serviceTagsToDelete);
				context.SaveChanges(performingUserId);

				var toDelete = context.ServiceRequestPackages.FirstOrDefault(x => x.Id == entity.Id);
				context.ServiceRequestPackages.Remove(toDelete);
				context.SaveChanges(performingUserId);
			}
			return null;
		}

		/// <summary>
		/// All of the Service Request Packages in the database
		/// </summary>
		public IEnumerable<IServiceRequestPackageDto> AllServiceRequestPackages
		{
			get
			{
				using (var context = new PrometheusContext())
				{
					foreach (var contextServiceRequestPackage in context.ServiceRequestPackages)
					{
						yield return ManualMapper.MapServiceRequestPackageToDto(contextServiceRequestPackage);
					}
				}
			}
		}

		/// <summary>
		/// Retrieves the service packages that the service option id exists in
		/// </summary>
		/// <param name="performingUserId"></param>
		/// <param name="serviceOptionId"></param>
		/// <returns></returns>
		public IEnumerable<IServiceRequestPackageDto> GetServiceRequestPackagesForServiceOption(int performingUserId, int serviceOptionId, ServiceRequestAction action)
		{
			using (var context = new PrometheusContext())
			{
				var option = context.ServiceOptions.Find(serviceOptionId);
				if (option == null)
					throw new InvalidOperationException(string.Format("Service Option with ID {0} does not exist. Cannot retrieve service package with option identifier {0}.", serviceOptionId));

				//All packages where the service option exists in the first category of the package
				// OR the service option exists in the first service of the package
				var packages = context.ServiceRequestPackages.Where(
					x => x.Action == action
					 && (x.ServiceOptionCategoryTags.Any(
						y => y.Order == 1 && y.ServiceOptionCategory.ServiceOptions.Any(
							z => z.Id == serviceOptionId))
					 || x.ServiceTags.Any(
							y => y.Order == 1 && y.Service.ServiceOptionCategories.Any(
								z => z.Id == serviceOptionId))));
				//Sweet baby jesus

				if (!packages.Any())
					throw new InvalidOperationException(string.Format("Service Request Package with Service Option ID {0} does not exist.", serviceOptionId));

				foreach (var package in packages)
				{
					yield return ManualMapper.MapServiceRequestPackageToDto(package);
				}
			}
		}
	}
}