using EduCodePlatform.Application.Interfaces.Repositories;

namespace EduCodePlatform.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; set; }
        public IModuleRepository Modules { get; }
        public ILessonRepository Lessons { get; }
        public ILessonTaskRepository LessonTasks { get; }
        public ITaskSubmissionRepository TaskSubmissions { get; }
        public IAchievementRepository Achievements { get; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            IModuleRepository modules,
            ILessonRepository lessons,
            ILessonTaskRepository lessonTasks,
            ITaskSubmissionRepository taskSubmissions,
            IAchievementRepository achievements)
        {
            _context = context;
            Users = userRepository;
            Modules = modules;
            Lessons = lessons;
            LessonTasks = lessonTasks;
            TaskSubmissions = taskSubmissions;
            Achievements = achievements;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
