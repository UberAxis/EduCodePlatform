<script setup lang="ts">
import { useStudyStore } from '~/stores/study'

const route = useRoute()
const study = useStudyStore()

const staticLabels: Record<string, string> = {
  lessons: 'Модули',
  leaderboard: 'Рейтинг',
  parent: 'Родителям',
  profile: 'Профиль',
  login: 'Войти',
  register: 'Регистрация',
  task: 'Практическое задание',
  quiz: 'Тест',
  admin: 'Админка',
  modules: 'Модули',
  tasks: 'Задания',
  achievements: 'Достижения',
  users: 'Пользователи',
}

const items = computed(() => {
  const segments = route.path.split('/').filter(Boolean)
  const crumbs: { label: string; to: string }[] = [{ label: 'Главная', to: '/' }]

  let accumulated = ''
  segments.forEach((segment, index) => {
    accumulated += '/' + segment
    let label = staticLabels[segment]

    if (!label) {
      if (segments[0] === 'admin') {
        label = segment
      } else if (segments[0] === 'lessons') {
        if (index === 1) {
          const mod = study.getModuleById(segment)
          label = mod?.title || `Модуль ${segment}`
        } else if (index === 2) {
          const moduleId = segments[1]
          const lesson = study.getLessonById(moduleId, segment)
          label = lesson?.title || `Урок ${segment}`
        }
      } else {
        label = segment
      }
    }

    crumbs.push({ label, to: accumulated })
  })

  return crumbs
})
</script>

<template>
  <ClientOnly>
    <UContainer v-if="route.path !== '/' && !route.path.startsWith('/admin')" class="py-6">
      <UBreadcrumb
        :items="items"
        :ui="{
          link: 'text-xl',
          separator: 'text-xl',
        }"
      />
    </UContainer>
  </ClientOnly>
</template>
