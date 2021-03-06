﻿using System.Collections.Generic;

namespace Prometheus.WebUI.Models.ServiceRequestApproval
{
    /// <summary>
    /// Composite class for Service Request screen
    /// </summary>
    public class ServiceRequestApprovalModel
    {
        public ServiceRequestApprovalControls Controls { get; set; }
        public ICollection<ServiceRequestTableItemModel> ServiceRequests { get; set; }
        
    }
}