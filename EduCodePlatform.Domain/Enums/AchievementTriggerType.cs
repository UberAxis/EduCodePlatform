namespace EduCodePlatform.Domain.Enums
{
    /// <summary>
    /// Defines the types of triggers that unlock achievements
    /// </summary>
    public enum AchievementTriggerType
    {
        // XP-based triggers
        XpThreshold = 0,           // Unlock when user reaches X XP

        // Task/Quiz completion triggers
        TasksCompletedCount = 1,   // Unlock after completing X tasks
        QuizzesPassedCount = 2,    // Unlock after passing X quizzes
        LessonsCompletedCount = 3, // Unlock after completing X lessons

        // Streak triggers
        ConsecutiveCorrectAnswers = 4, // Unlock after X correct answers in a row

        // Module/Lesson completion
        ModuleCompletionCount = 5, // Unlock after completing X modules
        SpecificModuleCompletion = 6, // Unlock after completing a specific module
        SpecificLessonCompletion = 7,  // Unlock after completing a specific lesson

        // Level-based
        LevelThreshold = 8,        // Unlock when reaching level X

        // Coins-based
        CoinsThreshold = 9,        // Unlock when accumulating X coins

        // Accuracy-based
        AccuracyPercentage = 10,   // Unlock when task accuracy is X%

        // Time-based
        DaysActive = 11,           // Unlock after being active for X days
        FirstTaskCompletion = 12,  // Unlock on first task completion
        FirstQuizPassed = 13       // Unlock on first quiz passed
    }
}
