<script setup lang="ts">
const route = useRoute()

const labels: Record<string, string> = {
  lessons: 'Уроки',
  profile: 'Профиль',
  login: 'Войти',
  register: 'Регистрация',
}

const items = computed(() => {
  const segments = route.path.split('/').filter(Boolean)
  const crumbs = [{ label: 'Главная', to: '/' }]

  segments.forEach((segment, index) => {
    const path = '/' + segments.slice(0, index + 1).join('/')

    let label = labels[segment] ?? segment

    // Если мы внутри /lessons и это динамический сегмент
    if (segments[0] === 'lessons' && !(segment in labels)) {
      if (index === 1) label = `Модуль ${segment}`
      if (index === 2) label = `Урок ${segment}`
    }

    crumbs.push({ label, to: path })
  })

  return crumbs
})
</script>
<template>
  <ClientOnly>
    <UContainer v-if="route.path !== '/'" class="py-6">
      <UBreadcrumb
        :items="items"
        :ui="{
          link: 'text-xl',
          separator: 'text-xl'
        }"
      />
    </UContainer>
  </ClientOnly>
</template>
