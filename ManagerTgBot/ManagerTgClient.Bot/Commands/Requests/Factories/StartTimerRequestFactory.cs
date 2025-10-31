﻿namespace Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

public class StartTimerRequestFactory : ICommandRequestFactory
{
    public ICommandRequest Create(long telegramId, string userInput)
    {
        return new  StartTimerRequest(telegramId, userInput);
    }

    public string CommandName => "/startTimer";
}