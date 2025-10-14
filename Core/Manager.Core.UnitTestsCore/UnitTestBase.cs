using AutoFixture;
using NUnit.Framework;

namespace Manager.Core.UnitTestsCore;

[TestFixture]
[Category("UnitTests")]
public abstract class UnitTestBase
{
    [SetUp]
    protected virtual void SetUp() { }

    /// <summary>
    ///     Fixture для создания полностью заполненных моделей случайными значениями
    /// </summary>
    protected readonly Fixture Fixture = new();
}