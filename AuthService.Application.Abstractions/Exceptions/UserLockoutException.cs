﻿namespace AuthService.Application.Abstractions.Exceptions;

/// <summary>
/// Исключение, возникающее, если пользователь заблокирован.
/// </summary>
public class UserLockoutException() : Exception("User is locked.");
