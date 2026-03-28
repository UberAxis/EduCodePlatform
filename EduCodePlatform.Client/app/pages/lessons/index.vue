<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-4xl mx-auto flex flex-col gap-8">

      <!-- Шапка -->
      <div class="flex flex-col gap-2">
        <h1 class="text-3xl font-extrabold text-highlighted">📚 Модули</h1>
        <p class="text-lg text-muted">Выбери модуль и начни учиться</p>
      </div>

      <!-- Общий прогресс -->
      <UCard>
        <div class="flex flex-col gap-3">
          <div class="flex items-center justify-between">
            <span class="font-bold text-base">Общий прогресс</span>
            <span class="text-muted text-sm">{{ study.totalProgress }}%</span>
          </div>
          <UProgress :model-value="study.totalProgress" color="primary" size="md" />
        </div>
      </UCard>

      <!-- Загрузка -->
      <div v-if="study.loading" class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <UCard v-for="i in 4" :key="i">
          <div class="flex flex-col gap-3 py-2">
            <USkeleton class="h-8 w-3/4" />
            <USkeleton class="h-4 w-full" />
            <USkeleton class="h-4 w-1/2" />
          </div>
        </UCard>
      </div>

      <!-- Ошибка -->
      <UAlert
        v-else-if="study.error"
        color="error"
        variant="soft"
        icon="i-lucide-circle-x"
        :title="study.error"
      />

      <!-- Список модулей -->
      <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <UCard
          v-for="module in study.modules"
          :key="module.id"
          class="cursor-pointer hover:ring-2 hover:ring-primary transition-all"
          @click="navigateTo(`/lessons/${module.id}`)"
        >
          <div class="flex flex-col gap-4 py-2">
            <div class="flex items-start gap-4">
              <div class="size-12 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
                <UIcon :name="moduleIcon(module)" class="size-6 text-primary" />
              </div>
              <div class="flex flex-col gap-1 flex-1">
                <h2 class="text-lg font-bold text-highlighted">{{ module.title }}</h2>
                <p class="text-sm text-muted">{{ module.description }}</p>
              </div>
            </div>

            <div class="flex items-center justify-between text-sm text-muted">
              <span class="flex items-center gap-1">
                <UIcon name="i-lucide-book-open" class="size-4" />
                {{ module.lessons.length }} уроков
              </span>
              <span class="flex items-center gap-1">
                <UIcon name="i-lucide-check-circle" class="size-4 text-success" />
                {{ module.lessons.filter(l => l.isCompleted).length }} пройдено
              </span>
            </div>

            <UProgress
              :model-value="moduleProgress(module)"
              color="primary"
              size="sm"
            />
          </div>
        </UCard>
      </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useStudyStore } from '~/stores/study'
import type { Module } from '~/stores/study'

definePageMeta({ middleware: 'auth' })

const study = useStudyStore()

function moduleIcon(m: Module) {
  return m.icon?.trim() || 'i-lucide-book-marked'
}

const moduleProgress = (module: Module) => {
  if (!module.lessons.length) return 0
  return Math.round((module.lessons.filter(l => l.isCompleted).length / module.lessons.length) * 100)
}

onMounted(() => {
  if (!study.modules.length) study.fetchModules()
})
</script>