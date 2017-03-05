﻿using System;
using System.Web.Mvc;
using System.Web.Security;
using Common.Dto;
using Prometheus.WebUI.Helpers;
using Prometheus.WebUI.Infrastructure;
using Prometheus.WebUI.Models.UserAccount;
using UserManager;

namespace Prometheus.WebUI.Controllers
{
    public class UserAccountController :PrometheusController
    {
        /// <summary>
        /// user manager functions for security
        /// </summary>
	    private readonly IUserManager _userManager;

        public UserAccountController()
	    {
		    _userManager = InterfaceFactory.CreateUserManagerService();
	    }

        /// <summary>
        /// Index page is the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }


        /// <summary>
        /// Login Authorization and builds session cookie
        /// </summary>
        /// <param name="userLogin">Contains user credentials</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(UserAccountModel userLogin)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "UserAccount");

            //validate login, create session cookie
            
	        IUserDto user;
	        try
	        {
		        user = (UserDto) _userManager.Login(userLogin.Username, userLogin.Password);    //get the user object
	        }
	        catch (Exception exception)
	        {
		        TempData["MessageType"] = WebMessageType.Failure;
		        TempData["Message"] = $"Login failure, error: {exception.Message}";
				return RedirectToAction("Index", "UserAccount");
			}
	        if (user != null && user.Id > 0)
            {
                FormsAuthentication.SetAuthCookie(user.Name, true);                             //enter data in session cookie
				
                Session["DisplayName"] = user.Name;
               // Session["Guid"] = user.AdGuid;
                Session["Id"] = user.Id;
	            Session["Department"] = user.DepartmentId;
                return RedirectToAction("Index", "Home");
            }

            TempData["MessageType"] = WebMessageType.Failure;
            TempData["Message"] = "Failed to login, please check username and password.";

            return RedirectToAction("Index", "UserAccount");
        }

        /// <summary>
        /// Login as Guest
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginGuest()
        {		
            FormsAuthentication.SetAuthCookie("Guest", true);
            Session["DisplayName"] = "Guest";
	        Session["Id"] = _userManager.GuestId;
            return RedirectToAction("Index", "Home");
        }

		[HttpPost]
		public ActionResult LoginAdmin()
		{
			FormsAuthentication.SetAuthCookie("Admin", true);
			Session["DisplayName"] = "Administrator";
			Session["Id"] = _userManager.AdministratorId;
			return RedirectToAction("Index", "Home");
		}


		/// <summary>
		/// Destroys the session
		/// </summary>
		/// <returns></returns>
		public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index", "UserAccount");
        }
    }
}