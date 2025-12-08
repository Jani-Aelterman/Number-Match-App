using NumberMatch.Data;

namespace NumberMatch.Services;

public interface ISettingsService
{
    int NumbersMatched { get; set; } /*NumbersMatched*/
    int Stage { get; set; } /*Stage*/
    List<List<int>> GameGrid { get; set; } /*GameGrid*/
    bool OledDarkmode { get; set; } /*OledDarkmode*/
    bool Vibration { get; set; } /*HapticFeedbackEnabled*/
    bool DeveloperOptions { get; set; } /*DeveloperOptions*/
}