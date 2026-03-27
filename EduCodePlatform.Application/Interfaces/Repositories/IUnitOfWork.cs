namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IModuleRepository Modules { get; }
        ILessonRepository Lessons { get; }
        ILessonTaskRepository LessonTasks { get; }
        ITaskSubmissionRepository TaskSubmissions { get; }
        Task<int> SaveChangesAsync();
    }
}
