import { defineStore } from 'pinia'

export interface QuizOption {
  text: string
}

export interface Task {
  id: string
  title: string
  /** Условие задания (API: markdownContent) */
  markdownContent?: string
  /** Локальный мок */
  description?: string
  initialCode?: string | null
  type?: string
  expectedAnswer?: string
  xpReward?: number
  correctAnswerIndex?: number
  quizOptions?: QuizOption[]
}

export interface Lesson {
  id: string
  title: string
  description?: string
  /** Локальный мок: тело урока */
  content?: string
  /** API: тело урока в Markdown */
  markdownContent?: string
  task?: Task
  isCompleted: boolean
}

export interface Module {
  id: string
  title: string
  description: string
  /** Локальный мок: имя иконки Nuxt UI */
  icon?: string
  /** API: файл обложки модуля */
  coverImage?: string
  lessons: Lesson[]
}

// ========================= MOCK DATA =========================
const MOCK_MODULES: Module[] = [
  {
    id: '1',
    title: 'Основы JavaScript',
    description: 'Базовые концепции языка: переменные, типы данных и операторы.',
    icon: 'i-lucide-code-2',
    lessons: [
      {
        id: '1',
        title: 'Переменные и константы',
        description: 'Учимся хранить данные в памяти.',
        content: `
### Что такое переменная?

Представь, что переменная — это **коробочка**, в которую можно положить данные. На наклейке написано имя, а внутри лежит значение.

\`\`\`js
let box = "Игрушка";
\`\`\`

Теперь, когда мы скажем \`box\`, компьютер поймет, что мы имеем в виду **"Игрушку"**.

### Виды переменных

- \`let\` — можно менять значение
- \`const\` — значение менять нельзя
- \`var\` — старый способ, лучше не использовать
        `,
        isCompleted: false,
        task: {
          id: 'task-1',
          title: 'Робо-имя',
          description: 'Объяви переменную robotName и присвой ей любое имя в кавычках.',
          initialCode: '// Напиши код здесь\nlet robotName = "";',
          xpReward: 50,
        },
      },
      {
        id: '2',
        title: 'Базовая математика',
        description: 'Сложение, вычитание и другие операторы.',
        content: `
### Математика в JavaScript

JavaScript умеет считать как калькулятор:

\`\`\`js
let sum = 10 + 5;   // 15
let diff = 10 - 3;  // 7
let mult = 4 * 3;   // 12
let div = 10 / 2;   // 5
\`\`\`
        `,
        isCompleted: false,
        task: {
          id: 'task-2',
          title: 'Счётчик деталей',
          description: 'Создай переменную totalParts равную сумме 10 и 15.',
          initialCode: '// Напиши код здесь\n',
          xpReward: 50,
        },
      },
    ],
  },
  {
    id: '2',
    title: 'Логика и циклы',
    description: 'Управление потоком выполнения программы.',
    icon: 'i-lucide-repeat',
    lessons: [
      {
        id: '1',
        title: 'Условия if/else',
        description: 'Учим программу принимать решения.',
        content: `
### Условия в JavaScript

Конструкция \`if\` позволяет выполнять код только при определённом условии:

\`\`\`js
let age = 12;

if (age >= 10) {
  console.log("Можно играть!");
} else {
  console.log("Ещё маловат");
}
\`\`\`
        `,
        isCompleted: false,
      },
    ],
  },
]

// =============================================================

export const useStudyStore = defineStore('study', () => {
  const modules = ref<Module[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const config = useRuntimeConfig()
  const API_URL = config.public.apiBase

  async function fetchModules(): Promise<void> {
    loading.value = true
    error.value = null
    try {
      const res = await fetch(`${API_URL}/Module`, {
        credentials: 'include',
      })
      if (!res.ok) throw new Error('Не удалось загрузить модули')
      modules.value = await res.json()
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Ошибка загрузки'
    } finally {
      loading.value = false
    }
  }

  const getModuleById = (moduleId: string) =>
    modules.value.find(m => String(m.id) === String(moduleId)) ?? null

  const getLessonById = (moduleId: string, lessonId: string) =>
    getModuleById(moduleId)?.lessons.find(l => String(l.id) === String(lessonId)) ?? null

  const totalProgress = computed(() => {
    const total = modules.value.reduce((sum, m) => sum + m.lessons.length, 0)
    const done = modules.value.reduce((sum, m) => sum + m.lessons.filter(l => l.isCompleted).length, 0)
    return total === 0 ? 0 : Math.round((done / total) * 100)
  })

  async function markLessonCompleted(moduleId: string, lessonId: string): Promise<void> {
    // В нашей реализации мы отправляем решение задачи, которое автоматически завершает урок.
    // Если в уроке нет задачи, нам нужен отдельный эндпоинт. 
    const lesson = getLessonById(moduleId, lessonId)
    if (lesson) lesson.isCompleted = true
  }

  async function submitTask(taskId: number, answer: string): Promise<boolean> {
    try {
      const res = await fetch(`${API_URL}/Submission`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ lessonTaskId: taskId, answer }),
        credentials: 'include',
      })
      if (!res.ok) return false
      const data = await res.json()
      return data.result // возвращаем результат проверки (true/false)
    } catch {
      return false
    }
  }

  return { modules, loading, error, fetchModules, getModuleById, getLessonById, totalProgress, markLessonCompleted, submitTask }
})
