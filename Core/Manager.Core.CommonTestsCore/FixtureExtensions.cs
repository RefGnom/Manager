using System;
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.Dsl;

namespace Manager.Core.CommonTestsCore;

public static class FixtureExtensions
{
    private static readonly Fixture localFixture = new();

    public static IPostprocessComposer<T> WithUtcDate<T>(
        this IPostprocessComposer<T> composer,
        Expression<Func<T, DateTime?>> property
    ) => composer.With(property, localFixture.Create<DateTime>().ToUniversalTime());
}