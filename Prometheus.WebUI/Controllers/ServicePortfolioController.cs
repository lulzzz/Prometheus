﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Common.Dto;
using Prometheus.WebUI.Models.ServicePortfolio;
using ServicePortfolio;
using ServicePortfolio.Controllers;

namespace Prometheus.WebUI.Controllers
{
    public class ServicePortfolioController : Controller
    {
        /// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public ActionResult Index()
        {
            /* create interface to service portfolio */
        var sps = new ServicePortfolioService(new ServiceBundleController(), new global::ServicePortfolio.Controllers.ServiceController(), new LifecycleStatusController());
        var portfolioBundles = sps.GetServiceBundles(0);
        /* IEnumerable<IServiceBundleDto> portfolioBundles = new List<IServiceBundleDto> {new ServiceBundleDto
        {
            Name = "Employee Productivity Services",
            Description = "Enable secure, anytime, anywhere, stable work capabilities and access to required information to meet personal computing requirements and increase customer satisfaction",
            BusinessValue = "This service will provide you with <ul><li>Increased employee productivity</li><li>Value created through enterprise procurement with standard offerings in order to reduce cost</li></ul>",
            Services = new List<IServiceDto> {new ServiceDto { Name = "Identity and Access Management"}, new ServiceDto { Name="Hardware Services"} },
            Measures = "Customer satisfaction surveys, Customer reports"
        } };
        */
            return View(portfolioBundles);
        }
		/// <summary>
        /// 
        /// </summary>
        /// <param name="serviceBundle"></param>
        /// <returns></returns>
		[HttpPost]
		public ActionResult Save(ServiceBundleDto serviceBundle)
		{
		    if (ModelState.IsValid)
		    {
		        var sps = new ServicePortfolioService(new ServiceBundleController(),
		            new global::ServicePortfolio.Controllers.ServiceController(), new LifecycleStatusController());
		        serviceBundle.Id = 0;
		        sps.SaveServiceBundle(0, serviceBundle);
		        TempData["messageType"] = "success";
		        TempData["message"] = "service bundle saved successfully";
		        return RedirectToAction("Show");
		    }

		    TempData["messageType"] = "failure";                /* invalid model state */
		    TempData["message"] = "could not save due to invalid information";
            return RedirectToAction("Add", serviceBundle);
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
            ServiceBundleModel model = new ServiceBundleModel(new ServiceBundleDto());
            
			return View(model);
		}

		/// <summary>
		/// Last chance before deleting a record
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult ConfirmDelete(int id)
		{
			return View();
		}


		/// <summary>
		/// Delete a service bundle
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Delete(int id)
		{

			return null;
		}

    }
}