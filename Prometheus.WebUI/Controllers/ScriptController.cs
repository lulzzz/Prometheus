﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Web;
using System.Web.Mvc;
using Common.Dto;
using Common.Enums.Entities;
using DataService.Models;
using Prometheus.WebUI.Models.Shared;
using Prometheus.WebUI.Helpers;
using Prometheus.WebUI.Infrastructure;
using Prometheus.WebUI.Models.Shared;
using RequestService.Controllers;

namespace Prometheus.WebUI.Controllers
{
	public class ScriptController : PrometheusController
	{

		private ScriptFileController _scriptFile = new ScriptFileController();

		/// <summary>
		/// Default page of Scripting
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			// TO DO:
			// retrieve all available scripts
			return View();
		}

		/// <summary>
		/// To get a specific script entry
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ActionResult GetScript(int id)
		{
			LinkListModel model = new LinkListModel();
			return View("PartialViews/_LinkList", model);
		}

		/// <summary>
		/// GET: Script/Add
		/// </summary>
		/// <returns></returns>
		public ActionResult Add()
		{
			return View(new ScriptDto());
		}

		[HttpPost]
		public ActionResult SaveScript(ScriptDto newScript, HttpPostedFileBase file)
		{

			if (!ModelState.IsValid) /* Server side validation */
			{
				TempData["MessageType"] = WebMessageType.Failure;
				TempData["Message"] = "Failed to save script due to invalid data";
				return RedirectToAction("Add");
			}

            try
            {
                if (Request.Files.Count > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    if (fileName != null)
                    {
                        Guid newFileName = Guid.NewGuid(); //to rename document			
                                                           //file path location comes from the Web.config file
                        try
                        {
                            var path = Path.Combine(ConfigHelper.GetScriptPath(), newFileName.ToString());
                            file.SaveAs(Server.MapPath(path));      /*create new doc and upload it */
                            _scriptFile.ModifyScript(UserId, new ScriptDto()
                            {
                                Id = newScript.Id,
                                Name = newScript.Name,
                                Description = newScript.Description,
                                Version = newScript.Version,
                                MimeType = file.ContentType,
                                ScriptFile = newFileName,
                                UploadDate = DateTime.Now,
                            }, EntityModification.Create);

                        }
                        catch (Exception e)
                        {
                            TempData["MessageType"] = WebMessageType.Failure;
                            TempData["Message"] = $"Failed to upload document, error: {e.Message}";
                        }
                    }
                }

                TempData["MessageType"] = WebMessageType.Success;
                TempData["Message"] = $"New script {newScript.Name} saved successfully";
            }
            catch (Exception e)
            {
                TempData["MessageType"] = WebMessageType.Failure;
                TempData["Message"] = $"Failed to save script {newScript.Name}, error: {e}";
                return RedirectToAction("Add");
            }

            //return to index
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Use for uploading scripts - DONT NEED THIS
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadScriptFile(HttpPostedFileBase file, int id)
        {
            if (Request.Files.Count > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

				if (fileName != null)
				{
					Guid newFileName = Guid.NewGuid(); //to rename document			
													   //file path location comes from the Web.config file
					try
					{
						var path = Path.Combine(ConfigHelper.GetScriptPath(), newFileName.ToString());
						file.SaveAs(Server.MapPath(path));      /*create new doc and upload it */
						_scriptFile.ModifyScript(UserId, new ScriptDto()
						{
							MimeType = file.ContentType,
							ScriptFile = newFileName,
							UploadDate = DateTime.Now,
						}, EntityModification.Create);
					}
					catch (Exception e)
					{
						TempData["MessageType"] = WebMessageType.Failure;
						TempData["Message"] = $"Failed to upload document, error: {e.Message}";
					}
				}
			}
			return View();
		}

		// GET: Script
		public JsonResult People()
		{
			var people = new List<string>();

			Runspace runspace = RunspaceFactory.CreateRunspace();

			// open it

			runspace.Open();
			Pipeline pipeline = runspace.CreatePipeline();
			pipeline.Commands.AddScript("[string[]] $run");
			string[] peeps = { "Brad", "Jamie" };
			runspace.SessionStateProxy.SetVariable("run", peeps);
			Collection<PSObject> results = pipeline.Invoke();
			runspace.Close();

			foreach (PSObject obj in results)
			{
				people.Add(obj.ToString());
			}

			return Json(people, JsonRequestBehavior.AllowGet);

		}

	}
}