﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Controllers;
using Common.Dto;
using Common.Enums.Entities;
using Common.Exceptions;
using DataService;
using DataService.DataAccessLayer;
using DataService.Models;

namespace UserManager.Controllers
{
	public class UserController : EntityController<IUserDto>, IUserController
	{
		/// <summary>
		/// Modifies the User in the database
		/// </summary>
		/// <param name="performingUserId">User ID for the user perfomring the modification</param>
		/// <param name="userDto"></param>
		/// <param name="modification">Type of modification to make</param>
		/// <returns></returns>
		public IUserDto ModifyUser(int performingUserId, IUserDto userDto, EntityModification modification)
		{
			return base.ModifyEntity(performingUserId, userDto, modification);
		}

		/// <summary>
		/// Get a list of all users
		/// </summary>
		/// <param name="performingUserId">user id of requestor</param>
		/// <returns></returns>
		public IEnumerable<IUserDto> GetUsers(int performingUserId)
		{
			{

				using (var context = new PrometheusContext())
				{
					var users = context.Users;
					foreach (var user in users)
					{
						yield return ManualMapper.MapUserToDto(user);
					}
				}
			}
		}

		/// <summary>
		/// get a specific user
		/// </summary>
		/// <param name="performingUserId">user making the request</param>
		/// <param name="userId">user requested</param>
		/// <returns></returns>
		public IUserDto GetUser(int performingUserId, int userId)
		{
			using (var context = new PrometheusContext())
			{
				var user = (from u in context.Users
							where u.Id == userId
							select u).FirstOrDefault();

				return ManualMapper.MapUserToDto(user);             //will return null if user not found
			}
		}

		/// <summary>
		/// Gets a specific user from their AD identifier
		/// </summary>
		/// <param name="userGuid"></param>
		/// <returns></returns>
		public IUserDto GetUser(Guid userGuid)
		{
			using (var context = new PrometheusContext())
			{
				var user = (from u in context.Users
							where u.AdGuid == userGuid
							select u).FirstOrDefault();

				return ManualMapper.MapUserToDto(user);             //will return null if user not found
			}
		}

		private int _guestId;

		/// <summary>
		/// ID of the Guest User of the system
		/// </summary>
		public int GuestId
		{
			get
			{
				if (_guestId == 0)
				{
					using (var context = new PrometheusContext())
					{
						var guest = context.Users.FirstOrDefault(x => x.Name == "Guest");
						if (guest != null)
							_guestId = guest.Id;
					}
				}
				return _guestId;
			}
		}

		private int _administratorId;

		/// <summary>
		/// ID of the defaulted Administrator of the system
		/// </summary>
		public int AdministratorId
		{
			get
			{
				if (_administratorId == 0)
				{
					using (var context = new PrometheusContext())
					{
						var administrator = context.Users.FirstOrDefault(x => x.Name == "Administrator");
						if (administrator != null)
							_administratorId = administrator.Id;
					}
				}
				return _administratorId;
			}
		}

		protected override IUserDto Create(int performingUserId, IUserDto userDto)
		{
			using (var context = new PrometheusContext())
			{
				var user = context.Users.Find(userDto.Id);
				if (user != null)
				{
					throw new InvalidOperationException(string.Format("User with ID {0} already exists.", userDto.Id));
				}
				var savedUser = context.Users.Add(ManualMapper.MapDtoToUser(userDto));
				context.SaveChanges();
				return ManualMapper.MapUserToDto(savedUser);
			}
		}

		protected override IUserDto Update(int performingUserId, IUserDto userDto)
		{
			if (userDto.Id == AdministratorId)
			{
				throw new InvalidOperationException("Administrator account cannot be updated.");
			}

			if (userDto.Id == GuestId)
			{
				throw new InvalidOperationException("Guest account cannot be updated.");
			}

			using (var context = new PrometheusContext())
			{
				if (!context.Users.Any(x => x.Id == userDto.Id))
				{
					throw new InvalidOperationException(string.Format("User with ID {0} cannot be updated since it does not exist.", userDto.Id));
				}

				var updatedUser = ManualMapper.MapDtoToUser(userDto);
				context.Users.Attach(updatedUser);
				context.Entry(updatedUser).State = EntityState.Modified;
				context.SaveChanges();
				return ManualMapper.MapUserToDto(updatedUser);
			}
		}

		protected override IUserDto Delete(int performingUserId, IUserDto userDto)
		{
			if (userDto.Id == GuestId)
			{
				throw new InvalidOperationException("Guest account cannot be deleted.");
			}

			using (var context = new PrometheusContext())
			{
				var userToDelete = context.Users.Find(userDto.Id);
				if (userToDelete.Roles.Any(x => x.Name == "Administrator"))
				{
					//If there is only one admin
					if (context.Users.Count(x => x.Roles.Any(y => y.Name == "Administrator")) <= 1)
					{
						throw new InvalidOperationException("Prometheus must have an Administrator account active. " +
															"Cannot delete this user as it is the only Administrator account.");
					}
				}
				context.Users.Remove(userToDelete);
				context.SaveChanges();
			}
			return null;
		}

		/// <summary>
		/// Adds the roles to the specified user if permission allows it
		/// </summary>
		/// <param name="performingUserId">User performing the role addition</param>
		/// <param name="adjustedUserId">User having roles added</param>
		/// <param name="rolesToAdd">Roles to be added to the user</param>
		/// <returns></returns>
		public IEnumerable<IRoleDto> AddRolesToUser(int performingUserId, int adjustedUserId, IEnumerable<IRoleDto> rolesToAdd)
		{
			using (var context = new PrometheusContext())
			{

				List<Role> newRoles = new List<Role>();
				foreach (var role in rolesToAdd)
				{
					newRoles.Add((from r in context.Roles where r.Id == role.Id select r).First()); /* attach context objects */
				}

				if (!context.Users.Any(x => x.Id == adjustedUserId))
					throw new EntityNotFoundException("Could not add Roles to User.", typeof(User), adjustedUserId);

				var updatedUser = context.Users.Find(adjustedUserId);
				updatedUser.Roles = new List<Role>();
				context.Users.Attach(updatedUser);

				foreach (var role in newRoles)
				{
					updatedUser.Roles.Add(role);
				}

				context.Entry(updatedUser).State = EntityState.Modified;
				context.SaveChanges();

				foreach (var updatedUserRole in updatedUser.Roles)
				{
					yield return ManualMapper.MapRoleToDto(updatedUserRole);
				}
			}
		}

		protected override bool UserHasPermissionToModify(int performingUserId, IUserDto user, EntityModification modification, out object permission)
		{
			permission = null;
			return true;
		}
	}
}
