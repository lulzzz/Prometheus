﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Common.Dto
{
	public class ScriptDto : IScriptDto
	{
		[HiddenInput]
		public int Id { get; set; }

        /// <summary>
        /// General name for the script file
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }

        /// <summary>
        /// Version number of the script
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Original name of the script file
        /// </summary>
        [Required(ErrorMessage = "Filename is required")]
        public string Filename { get; set; }

        /// <summary>
        /// The replacement name used in the file system
        /// </summary>
        public Guid ScriptFile { get; set; }
        public string MimeType { get; set; }

        /// <summary>
        /// Date when file was uploaded
        /// </summary>
        public DateTime UploadDate { get; set; }
		/*
		 * don't forget to annotate
		 */


		public DateTime? DateCreated { get; set; }
		public DateTime? DateUpdated { get; set; }
		public int CreatedByUserId { get; set; }
		public int UpdatedByUserId { get; set; }
	}
}
