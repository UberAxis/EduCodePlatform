<template>
  <div class="min-h-screen flex items-center justify-center bg-muted">
    <UCard class="w-full max-w-sm">
      <template #header>
        <div class="flex flex-col items-center gap-2 text-center">
          <UIcon name="i-lucide-code-2" class="size-10 text-primary" />
          <p class="text-xl font-bold text-highlighted">Добро пожаловать</p>
          <p class="text-sm text-muted">Войдите в свой аккаунт</p>
        </div>
      </template>

      <form class="space-y-4" @submit.prevent="handleLogin">
        <UFormField label="Email" required>
          <UInput
            v-model="form.email"
            type="email"
            placeholder="you@example.com"
            icon="i-lucide-mail"
            class="w-full"
            autofocus
          />
        </UFormField>

        <UFormField label="Пароль" required>
          <UInput
            v-model="form.password"
            :type="showPassword ? 'text' : 'password'"
            placeholder="••••••••"
            icon="i-lucide-lock"
            :trailing-icon="showPassword ? 'i-lucide-eye-off' : 'i-lucide-eye'"
            class="w-full"
            @click:trailing="showPassword = !showPassword"
          />
        </UFormField>

        <div class="flex items-center justify-between">
          <UCheckbox v-model="form.remember" label="Запомнить меня" />
          <UButton label="Забыли пароль?" variant="link" size="sm" />
        </div>

        <UButton
          type="submit"
          label="Войти"
          icon="i-lucide-log-in"
          class="w-full"
          :loading="loading"
        />
      </form>

      <template #footer>
        <p class="text-center text-sm text-muted">
          Нет аккаунта?
          <UButton label="Зарегистрироваться" variant="link" size="sm" />
        </p>
      </template>
    </UCard>
  </div>
</template>

<script setup lang="ts">
const toast = useToast()

const form = reactive({
  email: '',
  password: '',
  remember: false
})

const showPassword = ref(false)
const loading = ref(false)

async function handleLogin() {
  if (!form.email || !form.password) {
    toast.add({ title: 'Заполните все поля', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }

  loading.value = true

  // имитация запроса к API
  await new Promise(resolve => setTimeout(resolve, 1500))

  loading.value = false

  toast.add({ title: 'Успешный вход!', description: `Привет, ${form.email}`, color: 'success' })

  // navigateTo('/dashboard') // переход на другую страницу после успешного входа
}
</script>
