<template>
  <UContainer class="py-8">
    <div class="mb-6">
      <p class="text-3xl font-extrabold text-highlighted">Каталог модулей</p>
      <p class="text-lg text-muted mt-2">Выбери модуль и начни учиться</p>
    </div>

    <div class="flex flex-col gap-3">
      <UCard
        v-for="module in modules"
        :key="module.id"
        class="cursor-pointer hover:shadow-md transition-shadow"
        @click="navigateTo(`/lessons/${module.id}`)"
      >
        <div class="flex items-center gap-4">
          <div class="flex items-center justify-center size-12 rounded-full bg-primary/10 text-primary font-extrabold text-lg shrink-0">
            {{ module.order }}
          </div>
          <div class="flex-1">
            <p class="text-base font-bold text-highlighted">{{ module.title }}</p>
            <p class="text-sm text-muted">{{ module.description }}</p>
          </div>
          <UIcon name="i-lucide-chevron-right" class="size-5 text-muted shrink-0" />
        </div>
      </UCard>
    </div>
  </UContainer>
</template>

<script setup lang="ts">
// TODO: заменить на запрос к бэку
// const { data: modules } = await useFetch('http://localhost:5145/api/modules')

const modules = [
  { id: 1, order: 1, title: 'Основы программирования', description: 'Переменные, типы данных, условия и циклы' },
  { id: 2, order: 2, title: 'Функции и алгоритмы', description: 'Пишем переиспользуемый код' },
  { id: 3, order: 3, title: 'Работа с данными', description: 'Массивы, объекты и структуры данных' },
]
</script>
