﻿using System;
using System.Collections.Generic;
using Common.Dto;
using Common.Enums.Entities;

namespace UserManager.Controllers
{
	public interface IUserController
	{
		/// <summary>
		/// Adds the roles to the specified user if permission allows it
		/// </summary>
		/// <param name="performingUserId">User performing the role addition</param>
		/// <param name="adjustedUserId">User having roles added</param>
		/// <param name="rolesToAdd">Roles to be added to the user</param>
		/// <returns></returns>
		IEnumerable<IRoleDto> AddRolesToUser(int performingUserId, int adjustedUserId, IEnumerable<IRoleDto> rolesToAdd);

		/// <summary>
		/// Modifies the User in the database
		/// </summary>
		/// <param name="performingUserId">User ID for the user perfomring the modification</param>
		/// <param name="userDto"></param>
		/// <param name="modification">Type of modification to make</param>
		/// <returns></returns>
		IUserDto ModifyUser(int performingUserId, IUserDto userDto, EntityModification modification);

		/// <summary>
		/// Get a list of all users
		/// </summary>
		/// <param name="performingUserId">user id of requestor</param>
		/// <returns></returns>
		IEnumerable<UserDto> GetUsers(int performingUserId);

		/// <summary>
		/// get a specific user
		/// </summary>
		/// <param name="performingUserId">user making the request</param>
		/// <param name="userId">user requested</param>
		/// <returns></returns>
		UserDto GetUser(int performingUserId, int userId);
	}
}