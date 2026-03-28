<template>
  <div class="min-h-screen bg-default">
    <!-- Header -->
    <div class="bg-elevated border-b border-default shadow-lg sticky top-0 z-40">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center h-16">
          <div class="flex items-center">
            <NuxtLink to="/admin" class="font-bold text-xl text-highlighted hover:text-primary transition">
              Панель администратора
            </NuxtLink>
          </div>

          <div class="flex items-center space-x-4">
            <NuxtLink to="/" class="text-muted hover:text-default transition text-sm">
              На сайт
            </NuxtLink>
            <button
              @click="logout"
              class="text-muted hover:text-error transition text-sm"
            >
              Выход
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Navigation Sidebar -->
    <div class="grid grid-cols-1 lg:grid-cols-5 min-h-[calc(100vh-64px)]">
      <div class="col-span-1 bg-elevated border-r border-default p-4">
        <nav class="space-y-2">
          <NuxtLink
            v-for="link in navLinks"
            :key="link.to"
            :to="link.to"
            class="block px-4 py-2 rounded-lg text-muted hover:text-highlighted hover:bg-accented transition"
            active-class="bg-primary text-white"
          >
            {{ link.label }}
          </NuxtLink>
        </nav>
      </div>

      <!-- Main Content -->
      <div class="col-span-4 overflow-auto">
        <slot />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const router = useRouter()
const authStore = useAuthStore()

const navLinks = [
  { to: '/admin', label: 'Главная' },
  { to: '/admin/modules', label: 'Модули' },
  { to: '/admin/lessons', label: 'Уроки' },
  { to: '/admin/tasks', label: 'Задания' },
  { to: '/admin/achievements', label: 'Достижения' },
  { to: '/admin/users', label: 'Пользователи' },
]

const logout = async () => {
  await authStore.logout()
  router.push('/login')
}
</script>
