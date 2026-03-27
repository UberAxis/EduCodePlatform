<template>
  <div class="min-h-screen flex items-center justify-center bg-muted">
    <UCard class="w-full max-w-md">
      <template #header>
        <div class="flex flex-col items-center gap-3 text-center py-4">
          <div class="flex items-center gap-2">
            <img src="/logo.png" alt="logo" class="size-16" />
            <span class="text-4xl font-extrabold text-primary">Кодо<span class="text-secondary">Сфера</span></span>
          </div>
          <p class="text-lg text-muted">Создайте аккаунт</p>
        </div>

        <!-- ⚠️ MOCK-баннер — удалить когда бэк готов -->
        <UAlert
          color="warning"
          variant="soft"
          icon="i-lucide-flask-conical"
          title="Моковый режим"
          description="Любое имя (кроме testuser) и пароль"
          class="mt-2"
        />
      </template>

      <form class="space-y-6" @submit.prevent="handleRegister">
        <UFormField
          label="Имя пользователя"
          required
          :ui="{ label: 'text-base font-bold mb-1' }"
        >
          <UInput
            v-model="form.name"
            type="text"
            placeholder="Придумайте имя"
            icon="i-lucide-user"
            size="xl"
            class="w-full"
            autocomplete="username"
            autofocus
          />
        </UFormField>

        <UFormField
          label="Пароль"
          required
          :ui="{ label: 'text-base font-bold mb-1' }"
        >
          <UInput
            v-model="form.password"
            :type="showPassword ? 'text' : 'password'"
            placeholder="••••••••"
            icon="i-lucide-lock"
            size="xl"
            class="w-full"
            autocomplete="new-password"
          >
            <template #trailing>
              <UButton
                :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                variant="ghost"
                color="neutral"
                size="xl"
                @click="showPassword = !showPassword"
              />
            </template>
          </UInput>
        </UFormField>

        <UFormField
          label="Повторите пароль"
          required
          :ui="{ label: 'text-base font-bold mb-1' }"
        >
          <UInput
            v-model="form.passwordConfirm"
            :type="showPassword ? 'text' : 'password'"
            placeholder="••••••••"
            icon="i-lucide-lock-keyhole"
            size="xl"
            class="w-full"
            autocomplete="new-password"
          >
            <template #trailing>
              <UButton
                :icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
                variant="ghost"
                color="neutral"
                size="xl"
                @click="showPassword = !showPassword"
              />
            </template>
          </UInput>
        </UFormField>


        <UButton
          type="submit"
          label="Зарегистрироваться"
          icon="i-lucide-user-plus"
          size="xl"
          class="w-full"
          :loading="loading"
        />
      </form>

      <template #footer>
        <p class="text-center text-base text-muted py-2">
          Уже есть аккаунт?
          <UButton label="Войти" variant="link" size="md" to="/login" />
        </p>
      </template>
    </UCard>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth'

definePageMeta({ middleware: 'guest' })

const auth = useAuthStore()
const router = useRouter()
const toast = useToast()

const form = reactive({
  name: '',
  password: '',
  passwordConfirm: '',
})

const showPassword = ref(false)
const loading = ref(false)

async function handleRegister() {
  if (!form.name.trim()) {
    toast.add({ title: 'Укажите имя пользователя', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }
  if (!form.password) {
    toast.add({ title: 'Введите пароль', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }
  if (form.password !== form.passwordConfirm) {
    toast.add({ title: 'Пароли не совпадают', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }

  loading.value = true
  try {
    await auth.register(form.name.trim(), form.password)
    toast.add({
      title: 'Аккаунт создан!',
      description: `Добро пожаловать, ${auth.user?.name} 🎉`,
      color: 'success',
      icon: 'i-lucide-circle-check',
    })
    await router.push('/')
  } catch (e: unknown) {
    const message = e instanceof Error ? e.message : 'Что-то пошло не так'
    toast.add({ title: 'Ошибка регистрации', description: message, color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    loading.value = false
  }
}
</script>
