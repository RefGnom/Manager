using System;

namespace Manager.Core.AppConfiguration.DependencyInjection.AutoRegistration;

public class AutoRegistrationException(string message) : Exception(message);