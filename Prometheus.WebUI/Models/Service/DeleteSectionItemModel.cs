﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prometheus.WebUI.Models.Service
{
    public class DeleteSectionItemModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Used for redirection after deletion
        /// </summary>
        public int Serviceid { get; set; }
        /// <summary>
        /// Section to return to after deletion
        /// </summary>
        public string Section { get; set; }

        public string FriendlyName { get; set; }
    }
}