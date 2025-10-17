using System;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Manager.Core.CommonTestsCore;

public static class FluentAssertionsExtensions
{
    public static EquivalencyOptions<TExpectation> WithDateTimeCloseTo<TExpectation>(
        this EquivalencyOptions<TExpectation> equivalencyOptions,
        TimeSpan? precision = null
    ) => equivalencyOptions
        .Using<DateTime>(x => x.Expectation.Should()
            .BeCloseTo(x.Subject, precision ?? TimeSpan.FromMilliseconds(10))
        )
        .WhenTypeIs<DateTime>();

    public static EquivalencyOptions<TExpectation> WithTimeSpanCloseTo<TExpectation>(
        this EquivalencyOptions<TExpectation> equivalencyOptions,
        TimeSpan? precision = null
    ) => equivalencyOptions
        .Using<TimeSpan>(x => x.Expectation.Should()
            .BeCloseTo(x.Subject, precision ?? TimeSpan.FromMilliseconds(10))
        )
        .WhenTypeIs<TimeSpan>();
}