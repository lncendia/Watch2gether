using System;

namespace PJMS.AuthService.Exceptions;

/// <summary>
/// Исключение, возникающее при отсутствии значения в конфигурации.
/// </summary>
/// <param name="key">Путь к отсутствующему значению в конфигурации.</param>
public class ConfigurationException(string key) : Exception($"Configuration value not specified: {key}");