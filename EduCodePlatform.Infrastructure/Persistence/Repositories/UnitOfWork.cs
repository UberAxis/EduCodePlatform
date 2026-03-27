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

        public UnitOfWork(
            AppDbContext context,
            IUserRepository userRepository,
            IModuleRepository modules,
            ILessonRepository lessons,
            ILessonTaskRepository lessonTasks,
            ITaskSubmissionRepository taskSubmissions)
        {
            _context = context;
            Users = userRepository;
            Modules = modules;
            Lessons = lessons;
            LessonTasks = lessonTasks;
            TaskSubmissions = taskSubmissions;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
