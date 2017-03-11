﻿using System.Collections.Generic;
using System.Web.Http;
using Common.Dto;
using Common.Enums.Entities;
using RequestService;
using RequestService.Controllers;
using RESTAPI.Models;
using UserManager;


namespace RESTAPI.Controllers
{
	public class RequestController : ApiController
	{
		private IUserManager _userManager;
		private IRequestManager _requestManager;
		private IServiceRequestController _serviceRequestController;

		public RequestController(IUserManager userManager, IRequestManager requestManager, IServiceRequestController serviceRequestController)
		{
			_userManager = userManager;
			_requestManager = requestManager;
			_serviceRequestController = serviceRequestController;
		}

		// GET: api/Request
		public IEnumerable<string> Get()
		{
			//Need to determine what we want
			return new string[] { "value1", "value2" };
		}

		// GET: api/Request/5
		public IServiceRequestDto Get(int id)
		{
			//Need to change userId
			return _serviceRequestController.GetServiceRequest(_userManager.GuestId, id);
		}

		// POST: api/Request
		public void Post([FromBody]Request value)
		{
			//var um = new UserManagerService(
			//	new PermissionController(),
			//	new UserController(),
			//	new RoleController(new PermissionController())
			//);
			//var srController = new ServiceRequestController(um);

			////Need to change userId
			//srController.ModifyServiceRequest(um.GuestId, value, EntityModification.Create);
		}

		// PUT: api/Request/5
		public void Put(int id, [FromBody]string value)
		{
			//Convert the string to a Request
			var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Request>(value);

			//Need to change userId
			_requestManager.ChangeRequestState(_userManager.GuestId, id, request.State);
		}

		// DELETE: api/Request/5
		public void Delete(int id)
		{
			//Convert the string to a Request
			var request = _serviceRequestController.GetServiceRequest(_userManager.GuestId, id);

			//Need to change userId
			_serviceRequestController.ModifyServiceRequest(_userManager.GuestId, request, EntityModification.Delete);
		}
	}
}
