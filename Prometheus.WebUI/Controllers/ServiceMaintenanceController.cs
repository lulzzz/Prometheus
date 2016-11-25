﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Common.Dto;
using Prometheus.WebUI.Helpers;
using Prometheus.WebUI.Models.Shared;
using ServicePortfolioService;
using ServicePortfolioService.Controllers;

namespace Prometheus.WebUI.Controllers
{
    public class ServiceMaintenanceController : Controller
    {
        private int dummyId = 0;

        /// <summary>
        /// Returns main scree, this is the menu options
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Basic list of services
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
	    public ActionResult ShowServices(int id = 0)
	    {
            IPortfolioService sps = new PortfolioService(0, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            var model = (ServiceDto) sps.GetService(id);
            if (model == null)
              model = new ServiceDto {Id = 0};

            return View(model);
	    }

        public ActionResult ShowServiceList(int id = 0)
        {
            var model = new LinkListModel
            {
                Title = "Services",
                ListItems = new List<Tuple<int, string>>(),
                SelectAction = "ShowServices",
                Controller = "ServiceMaintenance"
            };

            return PartialView("PartialViews/_LinkList", model);
        }


        /// <summary>
        /// Show details of selected lifecycle or none if no id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowLifecycle(int id=0)
        {
            IPortfolioService sps = new PortfolioService(dummyId, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            var model = (LifecycleStatusDto) sps.GetLifecycleStatus(id);
            if (model == null)
               model = new LifecycleStatusDto {Id = 0};
            return View(model);
        }

        public ActionResult ShowLifeCycleList(int id = 0)
        {
            IPortfolioService sps = new PortfolioService(0, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            
            LinkListModel servicesModel = new LinkListModel();
            servicesModel.SelectedItemId = id;

            servicesModel.ListItems = sps.GetLifecycleStatusNames();

            servicesModel.AddAction = "AddLifecycle";
            servicesModel.SelectAction = "ShowLifecycle";
            servicesModel.Controller = "ServiceMaintenance";
            servicesModel.Title = "Lifecycle Statuses";

            return PartialView("PartialViews/_LinkList", servicesModel);
        }

        public ActionResult AddLifecycle()
        {
            return View();
        }


        /// <summary>
        /// Save or update Lifecycle Statuses
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveLifecycle(LifecycleStatusDto model)
        {
            if (ModelState.IsValid)
            {
                IPortfolioService sps = new PortfolioService(0, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());

                sps.SaveLifecycleStatus(model);

                TempData["messageType"] = WebMessageType.Success;
                TempData["message"] = "successfully saved lifecycle status";
                return RedirectToAction("ShowLifecycle");
            }
            TempData["messageType"] = WebMessageType.Failure;
                TempData["message"] = "failed to save lifecycle status";
                return RedirectToAction("AddLifecycle");
        }

        public ActionResult UpdateLifecycle(int id=0)
        {
            IPortfolioService sps = new PortfolioService(dummyId, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            return View((LifecycleStatusDto)sps.GetLifecycleStatus(id));
        }

        /// <summary>
        /// Confirm the deletion prior to deleting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ConfirmDeleteLifecycle(int id=0)
        {
            IPortfolioService sps = new PortfolioService(dummyId, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            if (id == 0)
                return RedirectToAction("ShowLifecycle");


            return View((LifecycleStatusDto)sps.GetLifecycleStatus(id));
        }

    
        [HttpPost]
        public ActionResult DeleteLifecycle(int id=0)
        {
            IPortfolioService sps = new PortfolioService(0, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            sps.DeleteLifecycleStatus(id);
            TempData["messageType"] = WebMessageType.Success;
            TempData["message"] = "successfully deleted lifecycle status";

            return RedirectToAction("ShowLifecycle");
        }

        /// <summary>
        /// Sends View for delete confirmation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
	    public ActionResult ConfirmDeleteService(int id = 0)
	    {
            IPortfolioService sps = new PortfolioService(0, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());

            return View(sps.GetService(id));
	    }

        /// <summary>
        
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteService(int id)
        {
            IPortfolioService sps = new PortfolioService(0, new ServiceBundleController(), new ServicePortfolioService.Controllers.ServiceController(), new LifecycleStatusController());
            sps.DeleteService(id);

            TempData["messageType"] = WebMessageType.Success;
            TempData["message"] = "Successfully deleted Service";

            return RedirectToAction("ShowServices");
        }
    }
}