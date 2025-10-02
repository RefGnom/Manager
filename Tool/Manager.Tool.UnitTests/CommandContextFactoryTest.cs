using System.Collections.Generic;
using FluentAssertions;
using Manager.Tool.BusinessObjects;
using Manager.Tool.Layers.Logic.Authentication;
using Manager.Tool.Layers.Logic.CommandsCore;
using NSubstitute;
using NUnit.Framework;

namespace Manager.Tool.UnitTests;

public class CommandContextFactoryTest
{
    private CommandContextFactory commandContextFactory;
    private IUserService userService;

    [SetUp]
    public void SetUp()
    {
        userService = Substitute.For<IUserService>();
        commandContextFactory = new CommandContextFactory(userService);
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Test(CreateContextTestCase createContextTestCase)
    {
        userService.FindUser().Returns((LocalRecipient?)null);

        var commandContext = commandContextFactory.Create(createContextTestCase.RawString.Split(' '));
        commandContext.Should().BeEquivalentTo(createContextTestCase.ExpectedContext);
    }

    private static IEnumerable<CreateContextTestCase> GetTestCases()
    {
        yield return new CreateContextTestCase(
            "command",
            new CommandContext(
                null,
                ["command"],
                []
            )
        );
        yield return new CreateContextTestCase(
            "command-space command-name",
            new CommandContext(
                null,
                ["command-space", "command-name"],
                []
            )
        );
        yield return new CreateContextTestCase(
            "first-space second-space third_space viu0viu",
            new CommandContext(
                null,
                ["first-space", "second-space", "third_space", "viu0viu"],
                []
            )
        );
        yield return new CreateContextTestCase(
            "space command -d",
            new CommandContext(
                null,
                ["space", "command"],
                [Flag("-d")]
            )
        );
        yield return new CreateContextTestCase(
            "space command -d --name",
            new CommandContext(
                null,
                ["space", "command"],
                [Flag("-d"), Flag("--name")]
            )
        );
        // Норм кейс, но в тесте надо умный сплит делать
        // yield return new CreateContextTestCase(
        //     "space command --name \"my name is\" -d --ping 8:0:0",
        //     new CommandContext(
        //         new User(),
        //         true,
        //         new CommandSpace("space"),
        //         "command",
        //         [Flag("--name", "my name is"), Flag("-d"), Flag("--ping", "8:0:0")]
        //     )
        // );
        yield return new CreateContextTestCase(
            "space command --name my_name-is -d --ping 8:0:0",
            new CommandContext(
                null,
                ["space", "command"],
                [Flag("--name", "my_name-is"), Flag("-d"), Flag("--ping", "8:0:0")]
            )
        );
    }

    private static CommandOption Flag(string arg, string? value = null) => new(arg, value);

    public record CreateContextTestCase(
        string RawString,
        CommandContext ExpectedContext
    );
}