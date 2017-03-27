﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using ServiceFulfillmentEngineWebJob.Api.Controllers;
using ServiceFulfillmentEngineWebJob.Api.Models;
using ServiceFulfillmentEngineWebJob.Api.Models.Enums;
using ServiceFulfillmentEngineWebJob.EntityFramework.DataAccessLayer;
using ServiceFulfillmentEngineWebJob.EntityFramework.Models;

namespace ServiceFulfillmentEngineWebJob
{
	public class FulfillmentManager
	{
		private int _userId;
		private string _apiKey;

		public FulfillmentManager(int userId, string apiKey)
		{
			_userId = userId;
			_apiKey = apiKey;
		}

		public void FulfillNewRequests()
		{
			var newRequests = GetNewServiceRequests();
			foreach (var request in newRequests)
			{
				ProcessRequest(request);
			}
		}

		/// <summary>
		/// Returns if the request is already processed or not
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		private bool IsProcessedRequest(IServiceRequest request)
		{
			return request.State != ServiceRequestState.Approved;
		}

		/// <summary>
		/// Processes Fulfillment of a Prometheus Service Request
		/// </summary>
		/// <param name="request"></param>
		private void ProcessRequest(IServiceRequest request)
		{
			Console.WriteLine("\n");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Processing new Request {request.Name}");
			Console.ForegroundColor = ConsoleColor.White;

			if (request.ServiceRequestOptions.Any(r => r.Code == "ACCT"))
			{
				Script script;
				using (var context = new ServiceFulfillmentEngineContext())
				{
					script = context.Scripts.FirstOrDefault(x => x.ApplicableCode == "ACCT");
				}
				if (script != null)
				{
					Console.WriteLine("Identified as executable on ACCT");

					Runspace runspace = RunspaceFactory.CreateRunspace();
					runspace.Open();

					// create a pipeline and feed it the script
					Pipeline pipeline = runspace.CreatePipeline();
					
					
					try
					{
						pipeline.Commands.AddScript(@"Scripts\NewUserScript.ps1");
					}
					catch (Exception exception)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine(exception.Message);
						Console.ForegroundColor = ConsoleColor.White;
					} 

					foreach (var userInput in request.ServiceRequestUserInputs) //just add everything
					{
						runspace.SessionStateProxy.SetVariable(userInput.Name, userInput.Value);
						Console.WriteLine($"Added parameter: label {userInput.Name}, value: {userInput.Value} ");
					}
					try
					{
						Collection<PSObject> results = pipeline.Invoke();
						Console.WriteLine("Script results: ");
						
						foreach (var psObject in results)
						{
							Console.WriteLine(psObject);
						}

					}
					catch (Exception exception)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"Error in executing script: {exception.Message}");
						Console.ForegroundColor = ConsoleColor.White;
					}
				}
			}




			/* Just chill for now
			var processedServiceOptions = new List<IServiceRequestOptionDto>();
			foreach (var option in request.ServiceRequestOptions)
			{
				if (IsProcessableOption(option))
				{
					ProcessServiceOption(option);
					processedServiceOptions.Add(option);
				}
			}
			*/
			ForwardRequest(request);

			FulfillPrometheusRequest(request);
		}

		/// <summary>
		/// Fulfills the request in Prometheus
		/// </summary>
		private void FulfillPrometheusRequest(IServiceRequest request)
		{
			Console.WriteLine($"Set Request {request.Name} to fulfilled");
			request.State = ServiceRequestState.Fulfilled;
			try
			{
				var controller = new PrometheusApiController(_userId, _apiKey);
				controller.UpdateRequestById(request.Id, request);
			}
			catch (Exception exception)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error fulfilling request: {exception.Message}");
				Console.ForegroundColor = ConsoleColor.White;
			}		
		}

		/// <summary>
		/// Sends the request and relevant data to the ticketing system
		/// Also Saves the fulfillment record in the database
		/// </summary>
		/// <param name="request"></param>

		private void ForwardRequest(IServiceRequest request)
		{
			Console.WriteLine($"Forward Request to Ticketing System {request.Name}");
		}

		/// <summary>
		/// Runs the script relevant to the SRO
		/// </summary>
		/// <param name="option"></param>
		private void ProcessServiceOption(IServiceRequestOption option)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns if the SRO has a script
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		private bool IsProcessableOption(IServiceRequestOption option)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets all of the service requests from the API and then filters for new ones
		/// </summary>
		/// <returns></returns>
		private IEnumerable<IServiceRequest> GetNewServiceRequests()
		{
			var controller = new PrometheusApiController(_userId, _apiKey);
			var requests = controller.GetServiceRequests();
			if (requests != null)
			{
				foreach (var request in requests)
				{
					if (!IsProcessedRequest(request))
					{
						yield return request;
					}
				}
			}
		}
	}
}
