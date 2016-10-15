﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DataService.Models
{
	public class ServiceBundle : IServiceBundle
	{
		//PK
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public Guid? Id { get; set; }
		public DateTime? DateCreated { get; set; }
		public DateTime? DateUpdated { get; set; }
		public Guid CreatedByUserId { get; set; }
		public Guid UpdatedByUserId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

        
        [DataType(DataType.MultilineText)]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }

		[DataType(DataType.MultilineText)]
		public string BusinessValue { get; set; }
		public string Measures { get; set; }
		
		//Navigation properties
		public virtual ICollection<IService> Services { get; set; }
	}
}
