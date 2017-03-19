﻿using System;
using System.Data.Entity;
using System.Linq;
using Common.Controllers;
using Common.Dto;
using Common.Enums.Entities;
using DataService;
using DataService.DataAccessLayer;

namespace RequestService.Controllers
{
	public class ApprovalController : EntityController<IApprovalDto>, IApprovalController
	{
		public IApprovalDto GetApproval(int performingUserId, int approvalId)
		{
			using (var context = new PrometheusContext())
			{
				var approval = context.Approvals.Find(approvalId);
				if (approval != null)
					return ManualMapper.MapApprovalToDto(approval);
				return null;
			}
		}

		public IApprovalDto GetApprovalForServiceRequest(int performingUserId, int serviceRequestId)
		{
			using (var context = new PrometheusContext())
			{
				var approval = context.Approvals.FirstOrDefault(x => x.ServiceRequestId == serviceRequestId);
				if (approval != null)
					return ManualMapper.MapApprovalToDto(approval);
				return null;
			}
		}

		public IApprovalDto ModifyApproval(int performingUserId, IApprovalDto approval, EntityModification modification)
		{
			return base.ModifyEntity(performingUserId, approval, modification);
		}

		protected override IApprovalDto Create(int performingUserId, IApprovalDto approvalDto)
		{
			using (var context = new PrometheusContext())
			{
				var approval = context.Approvals.Find(approvalDto.Id);
				if (approval != null)
				{
					throw new InvalidOperationException(string.Format("Approval with ID {0} already exists.", approvalDto.Id));
				}
				var saved = context.Approvals.Add(ManualMapper.MapDtoToApproval(approvalDto));
				context.SaveChanges(performingUserId);
				return ManualMapper.MapApprovalToDto(saved);
			}
		}

		protected override IApprovalDto Update(int performingUserId, IApprovalDto approvalDto)
		{
			using (var context = new PrometheusContext())
			{
				if (!context.Approvals.Any(x => x.Id == approvalDto.Id))
				{
					throw new InvalidOperationException(string.Format("Approval with ID {0} cannot be updated since it does not exist.", approvalDto.Id));
				}
				var updated = ManualMapper.MapDtoToApproval(approvalDto);
				context.Approvals.Attach(updated);
				context.Entry(updated).State = EntityState.Modified;
				context.SaveChanges(performingUserId);
				return ManualMapper.MapApprovalToDto(updated);
			}
		}

		protected override IApprovalDto Delete(int performingUserId, IApprovalDto entity)
		{
			using (var context = new PrometheusContext())
			{
				var toDelete = context.Approvals.Find(entity.Id);
				context.Approvals.Remove(toDelete);
				context.SaveChanges(performingUserId);
			}
			return null;
		}


	}
}