﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataService.Models
{
	public class ScriptedSelectionInput : IScriptedSelectionInput
	{
		public int ScriptId { get; set; }
		public bool ExecutionEnabled { get; set; }
		public int Id { get; set; }
		[Display(Order = 1, Name = "Display Name")]
		[Required(ErrorMessage = "Display Name is required")]
		public string DisplayName { get; set; }
		[Display(Order = 2, Name = "Help Tool Tip")]
		[AllowHtml]
		public string HelpToolTip { get; set; }

		public bool AvailableOnAdd { get; set; }
		public bool AvailableOnRemove { get; set; }
		public int NumberToSelect { get; set; }

		public virtual ICollection<ServiceOption> ServiceOptions { get; set; }
	}
}