<template>
  <UContainer class="py-8">
    <template v-if="currentModule">
      <div class="mb-6">
        <p class="text-3xl font-extrabold text-highlighted">{{ currentModule.title }}</p>
        <p class="text-lg text-muted mt-2">{{ currentModule.description }}</p>
      </div>

      <div class="flex flex-col gap-3">
        <UCard
          v-for="lesson in currentModule.lessons"
          :key="lesson.id"
          class="cursor-pointer hover:shadow-md transition-shadow"
          @click="navigateTo(`/lessons/${currentModule.id}/${lesson.id}`)"
        >
          <div class="flex items-center gap-4">
            <div class="flex items-center justify-center size-12 rounded-full bg-primary/10 text-primary font-extrabold text-lg shrink-0">
              {{ lesson.order }}
            </div>
            <div class="flex-1">
              <p class="text-base font-bold text-highlighted">{{ lesson.title }}</p>
              <p class="text-sm text-muted">{{ lesson.description }}</p>
            </div>
            <UIcon name="i-lucide-chevron-right" class="size-5 text-muted shrink-0" />
          </div>
        </UCard>
      </div>
    </template>

    <div v-else class="text-center py-16">
      <p class="text-2xl font-extrabold text-highlighted mb-2">Модуль не найден</p>
      <UButton label="Вернуться к урокам" to="/lessons" class="mt-4" size="xl" />
    </div>
  </UContainer>
</template>

<script setup lang="ts">
const route = useRoute()

const modules = [
  {
    id: 1,
    title: 'Основы программирования',
    description: 'Знакомство с базовыми понятиями: переменные, типы данных, условия и циклы.',
    lessons: [
      { id: 1, order: 1, title: 'Что такое программирование?', description: 'Введение в мир кода' },
      { id: 2, order: 2, title: 'Переменные и типы данных', description: 'Как хранить информацию' },
      { id: 3, order: 3, title: 'Условия if/else', description: 'Учим программу принимать решения' },
    ]
  },
  {
    id: 2,
    title: 'Функции и алгоритмы',
    description: 'Пишем переиспользуемый код.',
    lessons: [
      { id: 1, order: 1, title: 'Что такое функция?', description: 'Основы' },
      { id: 2, order: 2, title: 'Аргументы и возврат', description: 'Передаём данные в функцию' },
    ]
  }
]

const currentModule = computed(() =>
  modules.find(m => m.id === Number(route.params.moduleId))
)
</script>
