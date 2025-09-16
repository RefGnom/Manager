using System;

namespace Manager.Core.Common.DependencyInjection.AutoRegistration;

public class AutoRegistrationException(string message) : Exception(message);