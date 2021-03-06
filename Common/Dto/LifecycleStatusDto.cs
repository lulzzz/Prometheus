﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Common.Dto
{
	/// <summary>
	/// ITIL Status that a Service can be in
	/// </summary>
	public class LifecycleStatusDto : ILifecycleStatusDto
	{
		//PK
		[HiddenInput(DisplayValue = false)]
		public int Id { get; set; }

		//FK
		#region Fields
		public DateTime? DateCreated { get; set; }
		public DateTime? DateUpdated { get; set; }
		public int CreatedByUserId { get; set; }
		public int UpdatedByUserId { get; set; }

		/// <summary>
		/// Unique name of each status
		/// </summary>
		[Required(ErrorMessage = "Name required")]
		public string Name { get; set; }

		/// <summary>
		/// Used for the sort order when displaying statuses
		/// </summary>
		[Required(ErrorMessage = "Position is required")]
		public int Position { get; set; }

		/// <summary>
		/// Attribute to decide if services with this status will be 
		/// visible in business / support catalog
		/// </summary>
		[Display(Name = "Catalog Visible")]
		[Required(ErrorMessage = "Catalog visibility selection required")]
		public bool CatalogVisible { get; set; }
		#endregion

		#region Navigation Properties
		#endregion

	}
}
