using System;

namespace Manager.Core.DependencyInjection.AutoRegistration;

public class AutoRegistrationException(string message) : Exception(message);