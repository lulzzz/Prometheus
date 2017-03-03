﻿using System.Web.Mvc;
using Common.Dto;
using Prometheus.WebUI.Helpers;
using Prometheus.WebUI.Infrastructure;
using Prometheus.WebUI.Models.ServicePortfolio;
using Prometheus.WebUI.Models.Shared;

namespace Prometheus.WebUI.Controllers
{
	public class ServicePortfolioController : PrometheusController
	{
		/// <summary>
		/// Main page to display all Service Bundles
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			var ps = InterfaceFactory.CreatePortfolioService();

			return View(ps.GetServiceBundles());
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceBundle"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Save(ServiceBundleDto serviceBundle)
		{
			if (!ModelState.IsValid)
			{
				TempData["MessageType"] = WebMessageType.Failure;
				TempData["Message"] = $"Failed to save service bundle due to invalid data";

				return RedirectToAction("Update", serviceBundle.Id);
			}

			var ps = InterfaceFactory.CreatePortfolioService();

			if (serviceBundle.Id == 0)
				ps.SaveServiceBundle(serviceBundle);
			else
				ps.UpdateServiceBundle(serviceBundle);

			TempData["MessageType"] = WebMessageType.Success;
			TempData["Message"] = $"{serviceBundle.Name} saved successfully";

			return RedirectToAction("Show", new { id = serviceBundle.Id });
		}

		/// <summary>
		/// Returns the Service Portfolio Editor with a model with id = 0;
		/// </summary>
		/// <returns></returns>
		public ActionResult Add()
		{
			ServiceBundleModel model = new ServiceBundleModel(new ServiceBundleDto());

			return View(model);
		}


		/// <summary>
		/// Show the initial service portfolio editor and if an item is selected, otherwise 
		///   currentSelection is null
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult Show(int id = 0)
		{
			ServiceBundleDto serviceBundle = new ServiceBundleDto();

			if (id > 0)
			{
				var ps = InterfaceFactory.CreatePortfolioService();
				serviceBundle = (ServiceBundleDto)ps.GetServiceBundle(id);
			}
			else
			{
				serviceBundle.Id = 0;
			}
			return View(serviceBundle);
		}

		public ActionResult Update(int id = 0)
		{

			var ps = InterfaceFactory.CreatePortfolioService();
			ServiceBundleDto serviceBundle = (ServiceBundleDto)ps.GetServiceBundle(id);
			return View("Update", serviceBundle);
		}

		/// <summary>
		/// Last chance before deleting a record
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ConfirmDelete(int id)
		{
			var ps = InterfaceFactory.CreatePortfolioService();
			ServiceBundleDto serviceBundle = (ServiceBundleDto)ps.GetServiceBundle(id);

			return View(serviceBundle);
		}


		/// <summary>
		/// Delete a service bundle
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Delete(DeleteModel item)
		{
			if (!ModelState.IsValid)
			{
				TempData["MessageType"] = WebMessageType.Failure;
				TempData["Message"] = $"Failed to delete {item.Name}";
				return RedirectToAction("Show");
			}

			var ps = InterfaceFactory.CreatePortfolioService();
			ps.DeleteServiceBundle(item.Id);
			TempData["MessageType"] = WebMessageType.Success;
			TempData["Message"] = $"Successfully deleted {item.Name}";
			return RedirectToAction("Show");
		}

		[ChildActionOnly]
		public ActionResult ShowServiceBundleList(int id = 0)
		{
			var ps = InterfaceFactory.CreatePortfolioService();

			LinkListModel serviceBundleModel = new LinkListModel
			{
				AddAction = "Add",
				SelectAction = "Show",
				Controller = "ServicePortfolio",
				Title = "Service Bundles",
				SelectedItemId = id,
				ListItems = ps.GetServiceBundleNames()
			};

			return PartialView("PartialViews/_LinkList", serviceBundleModel);
		}
	}
}