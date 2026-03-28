<!-- pages/lessons/[moduleId]/[lessonId]/index.vue -->
<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-3xl mx-auto flex flex-col gap-8">

      <UButton
        :label="currentModule?.title ?? 'Назад'"
        icon="i-lucide-arrow-left"
        variant="ghost"
        color="neutral"
        :to="`/lessons/${moduleId}`"
        class="self-start"
      />

      <UCard v-if="!currentLesson">
        <div class="flex flex-col items-center gap-3 py-8 text-center">
          <UIcon name="i-lucide-frown" class="size-12 text-muted" />
          <p class="text-lg font-bold text-highlighted">Урок не найден</p>
          <UButton label="Вернуться к модулю" :to="`/lessons/${moduleId}`" />
        </div>
      </UCard>

      <template v-else>
        <div class="flex flex-col gap-3">
          <div class="flex items-center gap-2 flex-wrap">
            <UBadge
              v-if="currentLesson.isCompleted"
              label="Пройден"
              icon="i-lucide-check-circle"
              color="success"
              variant="subtle"
            />
            <UBadge
              v-if="hasPracticeTask"
              label="Практика"
              icon="i-lucide-code-2"
              color="secondary"
              variant="subtle"
            />
            <UBadge
              v-if="hasQuizTask"
              label="Тест"
              icon="i-lucide-list-checks"
              color="primary"
              variant="subtle"
            />
          </div>
          <h1 class="text-3xl font-extrabold text-highlighted">{{ currentLesson.title }}</h1>
          <p v-if="lessonLead" class="text-lg text-muted">{{ lessonLead }}</p>
        </div>

        <UCard>
          <div class="prose dark:prose-invert max-w-none" v-html="renderedContent" />
        </UCard>

        <div class="flex items-center justify-between gap-4 flex-wrap">
          <UButton
            v-if="prevLesson"
            :label="prevLesson.title"
            icon="i-lucide-arrow-left"
            variant="outline"
            color="neutral"
            :to="`/lessons/${moduleId}/${prevLesson.id}`"
          />
          <div v-else />

          <div class="flex items-center gap-3">
            <UButton
              v-if="hasPracticeTask"
              label="Перейти к заданию"
              icon="i-lucide-code-2"
              size="lg"
              :to="`/lessons/${moduleId}/${lessonId}/task`"
            />
            <UButton
              v-if="hasQuizTask"
              label="Пройти тест"
              icon="i-lucide-list-checks"
              size="lg"
              color="primary"
              :to="`/lessons/${moduleId}/${lessonId}/quiz`"
            />
            <UButton
              v-if="!hasPracticeTask && !hasQuizTask && !currentLesson.isCompleted"
              label="Отметить пройденным"
              icon="i-lucide-check"
              size="lg"
              :loading="completing"
              @click="handleComplete"
            />
            <UButton
              v-if="nextLesson"
              :label="nextLesson.title"
              trailing-icon="i-lucide-arrow-right"
              variant="outline"
              :to="`/lessons/${moduleId}/${nextLesson.id}`"
            />
          </div>
        </div>
      </template>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useStudyStore } from '~/stores/study'
import type { Lesson } from '~/stores/study'
import { markdownToHtml } from '~/composables/useMarkdownHtml'

definePageMeta({ middleware: 'auth' })

const route = useRoute()
const study = useStudyStore()
const toast = useToast()

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

const moduleId = computed(() => String(route.params.moduleId))
const lessonId = computed(() => String(route.params.lessonId))
const currentModule = computed(() => study.getModuleById(moduleId.value))
const currentLesson = computed(() => study.getLessonById(moduleId.value, lessonId.value))

interface LessonTask {
  id: number
  lessonId: number
  type: string
}

const allLessonTasks = ref<LessonTask[]>([])
const tasksLoaded = ref(false)

// Задача из модуля API (одна)
const moduleTask = computed(() => currentLesson.value?.task ?? null)

// Проверка типа: нормализуем регистр на случай несовпадений
function isPracticeType(t: string | undefined) {
  if (!t) return false
  const lower = t.toLowerCase()
  return lower === 'javascript' || lower === 'python'
}

function isQuizType(t: string | undefined) {
  if (!t) return false
  return t.toLowerCase() === 'quiz'
}

const hasPracticeTask = computed(() => {
  // Сначала проверяем задачу из модуля API
  if (moduleTask.value && isPracticeType(moduleTask.value.type)) return true
  // Потом из списка всех задач
  return allLessonTasks.value.some(t => isPracticeType(t.type))
})

const hasQuizTask = computed(() => {
  // Сначала проверяем задачу из модуля API
  if (moduleTask.value && isQuizType(moduleTask.value.type)) return true
  // Потом из списка всех задач
  return allLessonTasks.value.some(t => isQuizType(t.type))
})

async function fetchLessonTasks() {
  try {
    const tasks = await $fetch<LessonTask[]>(`${API_URL}/Task`, {
      credentials: 'include',
    })
    allLessonTasks.value = tasks.filter(t => t.lessonId === Number(lessonId.value))
  } catch {
    allLessonTasks.value = []
  } finally {
    tasksLoaded.value = true
  }
}

/** Текст урока: API отдаёт markdownContent, мок — content */
function lessonMarkdownSource(lesson: Lesson | undefined) {
  if (!lesson) return ''
  return (lesson.markdownContent ?? lesson.content ?? '').trim()
}

const lessonLead = computed(() => currentLesson.value?.description?.trim() ?? '')

const renderedContent = ref('')

watch(currentLesson, async (lesson) => {
  const md = lessonMarkdownSource(lesson)
  renderedContent.value = md ? await markdownToHtml(md) : ''
}, { immediate: true })

const prevLesson = computed(() => {
  const lessons = currentModule.value?.lessons ?? []
  const idx = lessons.findIndex(l => String(l.id) === lessonId.value)
  return idx > 0 ? lessons[idx - 1] : null
})

const nextLesson = computed(() => {
  const lessons = currentModule.value?.lessons ?? []
  const idx = lessons.findIndex(l => String(l.id) === lessonId.value)
  return idx < lessons.length - 1 ? lessons[idx + 1] : null
})

const completing = ref(false)

async function handleComplete() {
  completing.value = true
  try {
    await study.markLessonCompleted(moduleId.value, lessonId.value)
    toast.add({ title: 'Урок пройден!', color: 'success', icon: 'i-lucide-check-circle' })
  } catch {
    toast.add({ title: 'Не удалось сохранить прогресс', color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    completing.value = false
  }
}

onMounted(async () => {
  if (!study.modules.length) await study.fetchModules()
  await fetchLessonTasks()
})

watch([lessonId], () => {
  fetchLessonTasks()
})
</script>
