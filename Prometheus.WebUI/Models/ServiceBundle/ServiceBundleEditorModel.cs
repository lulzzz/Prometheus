﻿using Prometheus.Domain.Abstract;
using System;
using System.Collections.Generic;

/// <summary>
/// Purpose is to send data to the ServicePortfolioEditor View
///	It needs to contain a list of all of the Service Bundle names & ids for display and the complete ServiceBundle of one to view
/// </summary>
namespace Prometheus.WebUI.Models.ServiceBundle
{
	public class ServiceBundleEditorModel : ServiceBundleModel
	{
		public IEnumerable<KeyValuePair<int, string>> Services { get; set; }
	}
}