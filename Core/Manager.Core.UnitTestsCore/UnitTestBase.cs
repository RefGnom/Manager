using AutoFixture;
using NUnit.Framework;

namespace Manager.Core.UnitTestsCore;

[TestFixture]
[Category("UnitTests")]
public abstract class UnitTestBase
{
    /// <summary>
    ///     Fixture для создания полностью заполненных моделей случайными значениями
    /// </summary>
    protected readonly Fixture Fixture = new();

    [SetUp]
    protected virtual void SetUp() { }
}