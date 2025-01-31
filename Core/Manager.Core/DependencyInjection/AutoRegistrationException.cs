using System;

namespace Manager.Core.DependencyInjection;

public class AutoRegistrationException(string message) : Exception(message);