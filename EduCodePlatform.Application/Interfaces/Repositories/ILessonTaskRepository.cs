using EduCodePlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCodePlatform.Application.Interfaces.Repositories
{
    public interface ILessonTaskRepository
    {
        Task<IEnumerable<LessonTask>> GetAllAsync();

        Task<LessonTask?> GetByIdAsync(int id);

        Task<LessonTask?> GetByTitleAsync(string title);

        void Add(LessonTask LessonTask);

        void Delete(LessonTask LessonTask);

        Task<bool> ExistsByTitleAsync(string title);

        Task<bool> ExistsByTitleAsync(string title, int excludeId);

        Task<bool> ExistsByIdAsync(int id);
    }
}
