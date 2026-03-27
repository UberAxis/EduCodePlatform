<template>
  <UHeader :ui="{ root: 'border-b-0 shadow-sm' }">
    <template #title>
      <NuxtLink to="/" class="flex items-center gap-2">
        <img src="/logo.png" alt="logo" class="size-10" />
        <span class="text-2xl font-extrabold text-primary">Кодо<span class="text-secondary">Сфера</span></span>
      </NuxtLink>
    </template>

    <template #default>
      <nav class="hidden md:flex items-center gap-1">
        <UButton
          v-for="link in links"
          :key="link.to"
          :label="link.label"
          :icon="link.icon"
          :to="link.to"
          variant="ghost"
          color="neutral"
          size="xl"
        />
      </nav>
    </template>

    <template #right>
      <div class="flex items-center gap-2">
        <!-- Авторизован -->
        <template v-if="auth.isAuthenticated">
          <span class="text-xl hidden md:block">{{ auth.user?.name }}</span>
          <UButton
            label="Выйти"
            icon="i-lucide-log-out"
            variant="ghost"
            color="neutral"
            size="xl"
            @click="handleLogout"
          />
        </template>

        <!-- Гость -->
        <template v-else>
          <UButton label="Войти" variant="ghost" color="neutral" size="xl" to="/login" />
          <UButton label="Регистрация" size="xl" to="/register" />
        </template>
      </div>
    </template>

    <template #body>
      <div class="flex flex-col gap-1 p-4">
        <UButton
          v-for="link in links"
          :key="link.to"
          :label="link.label"
          :icon="link.icon"
          :to="link.to"
          variant="ghost"
          color="neutral"
          size="xl"
          class="justify-start"
        />
        <USeparator class="my-2" />

        <!-- Авторизован -->
        <template v-if="auth.isAuthenticated">
          <UButton
            label="Выйти"
            icon="i-lucide-log-out"
            variant="ghost"
            color="neutral"
            size="xl"
            class="w-full justify-start"
            @click="handleLogout"
          />
        </template>

        <!-- Гость -->
        <template v-else>
          <UButton label="Войти" variant="ghost" color="neutral" size="xl" to="/login" class="w-full" />
          <UButton label="Регистрация" size="xl" to="/register" class="w-full" />
        </template>
      </div>
    </template>
  </UHeader>
</template>

<script setup lang="ts">
const auth = useAuthStore()
const router = useRouter()
const toast = useToast()

const links = [
  { label: 'Главная', icon: 'i-lucide-house', to: '/' },
  { label: 'Модули', icon: 'i-lucide-book-open', to: '/lessons' },
  { label: 'Профиль', icon: 'i-lucide-user', to: '/profile' },
]

async function handleLogout() {
  await auth.logout()
  toast.add({ title: 'Вы вышли из аккаунта', color: 'neutral', icon: 'i-lucide-log-out' })
  await router.push('/')
}
</script>
