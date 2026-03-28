using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            string[] roles = { "Admin", "User" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            var adminExists = userManager.Users.Any(u => u.UserName == "admin");
            if (!adminExists)
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@educode.com",
                    EmailConfirmed = true,
                    Role = UserRole.Admin,
                    FullName = "System Administrator"
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            if (!context.Achievements.Any())
            {
                context.Achievements.AddRange(
                    new Achievement("Первые шаги", "Реши свою первую задачу", "i-lucide-footprints", 50),
                    new Achievement("Кодер на JS", "Заверши вводный курс по JavaScript", "i-lucide-code-2", 100),
                    new Achievement("Заклинатель Змей", "Заверши вводный курс по Python", "i-lucide-zap", 100),
                    new Achievement("Мастер Логики", "Реши 5 задач без ошибок", "i-lucide-brain", 200)
                );
                await context.SaveChangesAsync();
            }

            // сидим учебный контент
            if (!context.Modules.Any())
            {
                // --- МОДУЛЬ JS ---
                var jsModule = new Module(
                    "Магия JavaScript",
                    "module-js.png",
                    "Изучи язык, который оживляет интернет. От переменных до простых игр!",
                    1
                );
                context.Modules.Add(jsModule);
                await context.SaveChangesAsync();

                // Урок 1 JS
                var jsLesson1 = new Lesson(
                    "Твоя первая переменная",
                    "js-lesson-1.png",
                    "### Что такое переменная?\nПредставь, что это **коробочка** с наклейкой. Внутри лежит значение, а на наклейке — его имя.\n\n```javascript\nlet score = 100;\n```\nТеперь программа знает, что `score` — это 100.",
                    1,
                    jsModule.Id
                );
                context.Lessons.Add(jsLesson1);
                await context.SaveChangesAsync();

                context.LessonTasks.Add(new LessonTask(
                    "Имя для робота",
                    "Создай переменную **robotName** и присвой ей строку **'Bumblebee'** (в одинарных кавычках). Выведи значение в консоль: **console.log(robotName)**.",
                    "Bumblebee",
                    jsLesson1.Id,
                    TaskType.JavaScript,
                    "// Напиши код здесь\nlet robotName = '';\nconsole.log(robotName);"
                ));

                // Урок 2 JS (Тест)
                var jsLesson2 = new Lesson(
                    "Типы данных",
                    "js-lesson-2.png",
                    "### Строки и Числа\nВ JS есть разные типы данных:\n- **String** (Строка) — текст в кавычках, например **«Привет»**.\n- **Number** (Число) — числа без кавычек, например **42**.",
                    2,
                    jsModule.Id
                );
                context.Lessons.Add(jsLesson2);
                await context.SaveChangesAsync();

                context.LessonTasks.Add(new LessonTask(
                    "Викторина по типам",
                    "Как правильно написать строку в JavaScript?",
                    "В кавычках",
                    jsLesson2.Id,
                    TaskType.Quiz
                ));

                // --- МОДУЛЬ PYTHON ---
                var pyModule = new Module(
                    "Приключения с Python",
                    "module-py.png",
                    "Python — это как суперсила. На нем пишут ИИ, игры и серьезные программы!",
                    2
                );
                context.Modules.Add(pyModule);
                await context.SaveChangesAsync();

                // Урок 1 Python
                var pyLesson1 = new Lesson(
                    "Привет, Мир Python!",
                    "py-lesson-1.png",
                    "### Вывод данных\nВ Python всё просто. Чтобы что-то сказать миру, используй команду `print()`.\n\n```python\nprint(\"Привет, Кодер!\")\n```",
                    1,
                    pyModule.Id
                );
                context.Lessons.Add(pyLesson1);
                await context.SaveChangesAsync();

                context.LessonTasks.Add(new LessonTask(
                    "Первая программа",
                    "Выведи на экран фразу `Python is cool`. Будь внимателен к регистру!",
                    "Python is cool",
                    pyLesson1.Id,
                    TaskType.Python,
                    "# Твой код на Python ниже\nprint('')"
                ));

                // Урок 2 Python
                var pyLesson2 = new Lesson(
                    "Математика в Python",
                    "py-lesson-2.png",
                    "### Считаем как профи\nPython обожает математику:\n- `+` сложение\n- `-` вычитание\n- `*` умножение\n- `/` деление",
                    2,
                    pyModule.Id
                );
                context.Lessons.Add(pyLesson2);
                await context.SaveChangesAsync();

                context.LessonTasks.Add(new LessonTask(
                    "Сумматор",
                    "Создай две переменные `a = 5` и `b = 10`. Выведи их сумму через `print(a + b)`.",
                    "15",
                    pyLesson2.Id,
                    TaskType.Python,
                    "a = 0\nb = 0\n# Выведи сумму ниже\n"
                ));

                await context.SaveChangesAsync();
            }
        }
    }
}
