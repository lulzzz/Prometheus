﻿namespace Prometheus.WebUI.Infrastructure
{
	public interface IUserInput
	{
		int Id { get; set; }
		string DisplayName { get; set; }
		string HelpToolTip { get; set; }
	}
}