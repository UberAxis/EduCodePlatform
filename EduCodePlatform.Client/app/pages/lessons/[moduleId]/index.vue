<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-3xl mx-auto flex flex-col gap-8">

      <!-- Назад -->
      <UButton
        label="Все модули"
        icon="i-lucide-arrow-left"
        variant="ghost"
        color="neutral"
        to="/lessons"
        class="self-start"
      />

      <!-- Загрузка -->
      <div v-if="study.loading" class="flex flex-col gap-4">
        <USkeleton class="h-10 w-1/2" />
        <USkeleton class="h-5 w-3/4" />
        <USkeleton v-for="i in 3" :key="i" class="h-24 w-full" />
      </div>

      <!-- Модуль не найден -->
      <UCard v-else-if="!currentModule">
        <div class="flex flex-col items-center gap-3 py-8 text-center">
          <UIcon name="i-lucide-frown" class="size-12 text-muted" />
          <p class="text-lg font-bold text-highlighted">Модуль не найден</p>
          <UButton label="Вернуться к модулям" to="/lessons" />
        </div>
      </UCard>

      <template v-else>
        <!-- Заголовок модуля -->
        <div class="flex items-start gap-4">
          <div class="size-14 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
            <UIcon :name="moduleIcon(currentModule)" class="size-8 text-primary" />
          </div>
          <div class="flex flex-col gap-1">
            <h1 class="text-3xl font-extrabold text-highlighted">{{ currentModule.title }}</h1>
            <p class="text-lg text-muted">{{ currentModule.description }}</p>
          </div>
        </div>

        <!-- Прогресс модуля -->
        <UCard>
          <div class="flex flex-col gap-3">
            <div class="flex items-center justify-between text-sm">
              <span class="font-bold">Прогресс модуля</span>
              <span class="text-muted">
                {{ currentModule.lessons.filter(l => l.isCompleted).length }} / {{ currentModule.lessons.length }}
              </span>
            </div>
            <UProgress :model-value="moduleProgress" color="primary" size="md" />
          </div>
        </UCard>

        <!-- Список уроков -->
        <div class="flex flex-col gap-4">
          <UCard
            v-for="(lesson, index) in currentModule.lessons"
            :key="lesson.id"
            class="cursor-pointer hover:ring-2 hover:ring-primary transition-all"
            @click="navigateTo(`/lessons/${moduleId}/${lesson.id}`)"
          >
            <div class="flex items-center gap-4">
              <!-- Номер / галочка -->
              <div
                class="size-10 rounded-full flex items-center justify-center shrink-0 font-bold text-base"
                :class="lesson.isCompleted
                  ? 'bg-success/10 text-success'
                  : 'bg-primary/10 text-primary'"
              >
                <UIcon v-if="lesson.isCompleted" name="i-lucide-check" class="size-5" />
                <span v-else>{{ index + 1 }}</span>
              </div>

              <div class="flex flex-col gap-0.5 flex-1">
                <h3 class="font-bold text-highlighted">{{ lesson.title }}</h3>
                <p v-if="lessonListSubtitle(lesson)" class="text-sm text-muted">
                  {{ lessonListSubtitle(lesson) }}
                </p>
              </div>

              <div class="flex items-center gap-2 shrink-0">
                <UBadge
                  v-if="hasPracticeForLesson(lesson.id)"
                  label="Практика"
                  icon="i-lucide-code-2"
                  variant="subtle"
                  color="secondary"
                  size="sm"
                />
                <UBadge
                  v-if="hasQuizForLesson(lesson.id)"
                  label="Тест"
                  icon="i-lucide-list-checks"
                  variant="subtle"
                  color="primary"
                  size="sm"
                />
                <UIcon name="i-lucide-chevron-right" class="size-5 text-muted" />
              </div>
            </div>
          </UCard>
        </div>
      </template>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useStudyStore } from '~/stores/study'
import type { Lesson, Module } from '~/stores/study'

definePageMeta({ middleware: 'auth' })

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

function moduleIcon(m: Module | null) {
  return m?.icon?.trim() || 'i-lucide-book-marked'
}

/** API не шлёт description у урока — кратко из Markdown */
function lessonListSubtitle(lesson: Lesson) {
  if (lesson.description?.trim()) return lesson.description
  const raw = (lesson.markdownContent ?? lesson.content ?? '').trim()
  if (!raw) return ''
  const plain = raw
    .replace(/```[\s\S]*?```/g, ' ')
    .replace(/#+\s*/g, '')
    .replace(/[*_[\]`]+/g, '')
    .replace(/\s+/g, ' ')
    .trim()
  if (!plain) return ''
  return plain.length > 140 ? `${plain.slice(0, 140)}…` : plain
}

const route = useRoute()
const study = useStudyStore()
const moduleId = computed(() => String(route.params.moduleId))
const currentModule = computed(() => study.getModuleById(moduleId.value))
const moduleProgress = computed(() => {
  const m = currentModule.value
  if (!m || !m.lessons.length) return 0
  return Math.round((m.lessons.filter(l => l.isCompleted).length / m.lessons.length) * 100)
})

// Задания уроков модуля
interface LessonTask {
  id: number
  lessonId: number
  type: string
}

const allTasks = ref<LessonTask[]>([])

function hasPracticeForLesson(lessonId: string | number) {
  // Проверяем из списка задач API
  const fromApi = allTasks.value.some(
    t => t.lessonId === Number(lessonId) && (t.type?.toLowerCase() === 'javascript' || t.type?.toLowerCase() === 'python')
  )
  // Проверяем задачу из модуля API (GetLessonDTO.Task)
  const lesson = currentModule.value?.lessons.find(l => String(l.id) === String(lessonId))
  const fromModule = lesson?.task?.type ? (lesson.task.type.toLowerCase() === 'javascript' || lesson.task.type.toLowerCase() === 'python') : false
  return fromApi || fromModule
}

function hasQuizForLesson(lessonId: string | number) {
  // Проверяем из списка задач API
  const fromApi = allTasks.value.some(
    t => t.lessonId === Number(lessonId) && t.type?.toLowerCase() === 'quiz'
  )
  // Проверяем задачу из модуля API (GetLessonDTO.Task)
  const lesson = currentModule.value?.lessons.find(l => String(l.id) === String(lessonId))
  const fromModule = lesson?.task?.type?.toLowerCase() === 'quiz'
  return fromApi || fromModule
}

async function fetchModuleTasks() {
  try {
    const tasks = await $fetch<LessonTask[]>(`${API_URL}/Task`, { credentials: 'include' })
    allTasks.value = tasks
  } catch {
    allTasks.value = []
  }
}

onMounted(async () => {
  if (!study.modules.length) await study.fetchModules()
  await fetchModuleTasks()
})
</script>