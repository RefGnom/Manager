using System.Collections.Generic;
using FluentAssertions;
using Manager.AuthenticationService.Client.ServiceModels;
using Manager.Tool.Layers.Logic.Authentication;
using Manager.Tool.Layers.Logic.CommandsCore;
using NSubstitute;
using NUnit.Framework;

namespace Manager.Tool.UnitTests;

public class CommandContextFactoryTest
{
    private CommandContextFactory _commandContextFactory;
    private IUserService _userService;

    [SetUp]
    public void SetUp()
    {
        _userService = Substitute.For<IUserService>();
        _commandContextFactory = new CommandContextFactory(_userService);
    }

    [TestCaseSource(nameof(GetTestCases))]
    public void Test(CreateContextTestCase createContextTestCase)
    {
        _userService.FindUser().Returns((User?)null);

        var commandContext = _commandContextFactory.Create(createContextTestCase.RawString.Split(' '));
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

    private static CommandOption Flag(string arg, string? value = null)
    {
        return new CommandOption(arg, value);
    }

    public record CreateContextTestCase(
        string RawString,
        CommandContext ExpectedContext
    );
}