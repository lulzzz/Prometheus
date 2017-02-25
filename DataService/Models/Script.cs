﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models
{
	public class Script : IScript
	{
		/// <summary>
		/// Primary Key
		/// </summary>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        #region Fields

        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Filename { get; set; }
        public Guid ScriptFile { get; set; }
        public string MimeType { get; set; }
        public DateTime UploadDate { get; set; }

        #endregion

        public DateTime? DateCreated { get; set; }
		public DateTime? DateUpdated { get; set; }
		public int CreatedByUserId { get; set; }
		public int UpdatedByUserId { get; set; }

	}
}