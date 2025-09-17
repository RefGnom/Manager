using System;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public class MatchCommandResult(
    IToolCommand toolCommand,
    float score
)
{
    public IToolCommand ToolCommand { get; } = toolCommand;

    /// <summary>
    /// Оценка, насколько подходит команда для выполнения. Находится в промежутке [0, 1]
    /// </summary>
    public float Score { get; } = score;

    public bool IsFullMath()
    {
        return Math.Abs(Score - ScoreConstants.MaxScore) < ScoreConstants.CompareEpsilon;
    }
}