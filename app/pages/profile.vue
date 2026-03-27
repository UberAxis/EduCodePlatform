<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-2xl mx-auto flex flex-col gap-6">

      <!-- ⚠️ MOCK-баннер — удалить когда бэк готов -->
      <UAlert
        color="warning"
        variant="soft"
        icon="i-lucide-flask-conical"
        title="Моковый режим"
        description="Данные берутся из стора, редактирование только локальное"
      />

      <!-- Карточка профиля -->
      <UCard>
        <template #header>
          <div class="flex items-center gap-5 py-2">
            <UAvatar
              :alt="auth.user?.name"
              size="3xl"
              icon="i-lucide-user"
            />
            <div class="flex flex-col gap-1">
              <h1 class="text-2xl font-extrabold text-highlighted">{{ auth.user?.name }}</h1>
              <UBadge
                :label="roleLabel"
                variant="subtle"
                color="primary"
                size="lg"
              />
            </div>
          </div>
        </template>

        <!-- XP и уровень -->
        <div class="flex flex-col gap-3">
          <div class="flex items-center justify-between text-sm">
            <span class="font-bold text-base">Уровень {{ auth.user?.level }}</span>
            <span class="text-muted">{{ auth.user?.xp }} / {{ xpForNextLevel }} XP</span>
          </div>
          <UProgress :model-value="xpProgress" size="md" color="primary" />
          <p class="text-sm text-muted">До следующего уровня: {{ xpForNextLevel - (auth.user?.xp ?? 0) }} XP</p>
        </div>

        <USeparator class="my-4" />

        <!-- Статистика -->
        <div class="grid grid-cols-3 gap-4 text-center">
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.xp }}</span>
            <span class="text-sm text-muted">Очки опыта</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.level }}</span>
            <span class="text-sm text-muted">Уровень</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">0</span>
            <span class="text-sm text-muted">Уроков пройдено</span>
          </div>
        </div>
      </UCard>

      <!-- Карточка редактирования -->
      <UCard>
        <template #header>
          <h2 class="text-lg font-bold text-highlighted">Редактировать профиль</h2>
        </template>

        <form class="flex flex-col gap-5" @submit.prevent="handleUpdate">
          <UFormField
            label="Новое имя пользователя"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.name"
              type="text"
              :placeholder="auth.user?.name"
              icon="i-lucide-user"
              size="xl"
              class="w-full"
              autocomplete="username"
            />
          </UFormField>

          <UFormField
            label="Новый пароль"
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
            label="Повторите новый пароль"
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

          <div class="flex gap-3 justify-end">
            <UButton
              label="Сбросить"
              variant="ghost"
              color="neutral"
              size="xl"
              :disabled="loading"
              @click="resetForm"
            />
            <UButton
              type="submit"
              label="Сохранить"
              icon="i-lucide-save"
              size="xl"
              :loading="loading"
            />
          </div>
        </form>
      </UCard>

      <!-- Опасная зона -->
      <UCard variant="subtle">
        <template #header>
          <h2 class="text-lg font-bold text-error">Опасная зона</h2>
        </template>
        <div class="flex items-center justify-between">
          <div>
            <p class="font-medium text-highlighted">Выйти из аккаунта</p>
            <p class="text-sm text-muted">Вы будете перенаправлены на страницу входа</p>
          </div>
          <UButton
            label="Выйти"
            icon="i-lucide-log-out"
            color="error"
            variant="soft"
            size="xl"
            @click="handleLogout"
          />
        </div>
      </UCard>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '~/stores/auth'

definePageMeta({ middleware: 'auth' })

const auth = useAuthStore()
const router = useRouter()
const toast = useToast()

// XP до следующего уровня (100 XP на уровень)
const xpForNextLevel = computed(() => (auth.user?.level ?? 1) * 100)
const xpProgress = computed(() => {
  const xp = auth.user?.xp ?? 0
  return Math.min(Math.round((xp / xpForNextLevel.value) * 100), 100)
})

const roleLabel = computed(() => {
  const map: Record<string, string> = {
    student: 'Ученик',
    teacher: 'Учитель',
    admin: 'Администратор',
  }
  return map[auth.user?.role ?? ''] ?? auth.user?.role ?? 'Пользователь'
})

const form = reactive({ name: '', password: '', passwordConfirm: '' })
const showPassword = ref(false)
const loading = ref(false)

function resetForm() {
  form.name = ''
  form.password = ''
  form.passwordConfirm = ''
}

async function handleUpdate() {
  if (!form.name.trim() && !form.password) {
    toast.add({ title: 'Нечего сохранять', description: 'Заполните хотя бы одно поле', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }
  if (form.password && form.password !== form.passwordConfirm) {
    toast.add({ title: 'Пароли не совпадают', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }

  loading.value = true
  try {
    // ========================= MOCK =========================
    await new Promise(resolve => setTimeout(resolve, 600))
    if (form.name.trim() && auth.user) {
      auth.user.name = form.name.trim()
    }
    toast.add({ title: 'Профиль обновлён!', color: 'success', icon: 'i-lucide-circle-check' })
    resetForm()
    // ========================================================

    // ----- РЕАЛЬНЫЙ update (раскомментировать когда бэк готов) -----
    // const body: Record<string, string> = {}
    // if (form.name.trim()) body.name = form.name.trim()
    // if (form.password) body.password = form.password
    // const res = await fetch(`http://localhost:5145/api/User/${auth.user?.id}`, {
    //   method: 'PUT',
    //   headers: { 'Content-Type': 'application/json' },
    //   credentials: 'include',
    //   body: JSON.stringify(body),
    // })
    // if (!res.ok) throw new Error('Ошибка сервера. Попробуйте позже')
    // if (form.name.trim() && auth.user) auth.user.name = form.name.trim()
    // toast.add({ title: 'Профиль обновлён!', color: 'success', icon: 'i-lucide-circle-check' })
    // resetForm()
    // ---------------------------------------------------------------
  } catch (e: unknown) {
    const message = e instanceof Error ? e.message : 'Что-то пошло не так'
    toast.add({ title: 'Ошибка обновления', description: message, color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    loading.value = false
  }
}

async function handleLogout() {
  await auth.logout()
  toast.add({ title: 'Вы вышли из аккаунта', color: 'neutral', icon: 'i-lucide-log-out' })
  await router.push('/')
}
</script>
