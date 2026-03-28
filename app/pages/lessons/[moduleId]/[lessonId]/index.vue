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
        <!-- Шапка урока -->
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
              v-if="currentLesson.task"
              label="Есть практика"
              icon="i-lucide-code-2"
              color="secondary"
              variant="subtle"
            />
            <UBadge
              v-if="currentLesson.quiz"
              label="Есть тест"
              icon="i-lucide-list-checks"
              color="warning"
              variant="subtle"
            />
          </div>
          <h1 class="text-3xl font-extrabold text-highlighted">{{ currentLesson.title }}</h1>
          <p class="text-lg text-muted">{{ currentLesson.description }}</p>
        </div>

        <!-- Контент -->
        <UCard>
          <div class="prose dark:prose-invert max-w-none" v-html="renderedContent" />
        </UCard>

        <!-- Навигация -->
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

          <div class="flex items-center gap-3 flex-wrap">
            <UButton
              v-if="currentLesson.task"
              label="Перейти к заданию"
              icon="i-lucide-code-2"
              size="lg"
              :to="`/lessons/${moduleId}/${lessonId}/task`"
            />
            <UButton
              v-if="currentLesson.quiz"
              label="Пройти тест"
              icon="i-lucide-list-checks"
              size="lg"
              variant="outline"
              :to="`/lessons/${moduleId}/${lessonId}/quiz`"
            />
            <UButton
              v-if="!currentLesson.task && !currentLesson.quiz && !currentLesson.isCompleted"
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
import { marked } from 'marked'
import { useStudyStore } from '~/stores/study'

definePageMeta({ middleware: 'auth' })

const route = useRoute()
const study = useStudyStore()
const toast = useToast()

const moduleId = computed(() => String(route.params.moduleId))
const lessonId = computed(() => String(route.params.lessonId))
const currentModule = computed(() => study.getModuleById(moduleId.value))
const currentLesson = computed(() => study.getLessonById(moduleId.value, lessonId.value))

const renderedContent = ref('')

watch(currentLesson, async (lesson) => {
  renderedContent.value = lesson?.content
    ? await marked.parse(lesson.content)
    : ''
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
    toast.add({ title: 'Урок пройден! 🎉', color: 'success', icon: 'i-lucide-check-circle' })
  } catch {
    toast.add({ title: 'Не удалось сохранить прогресс', color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    completing.value = false
  }
}

onMounted(() => {
  if (!study.modules.length) study.fetchModules()
})
</script>
