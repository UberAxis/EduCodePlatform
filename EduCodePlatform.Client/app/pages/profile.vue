<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-2xl mx-auto flex flex-col gap-6">

      <!-- Карточка профиля -->
      <UCard>
        <template #header>
          <div class="flex items-center gap-5 py-2">
            <UAvatar
              :alt="auth.user?.fullName || auth.user?.name"
              size="3xl"
              icon="i-lucide-user"
            />
            <div class="flex flex-col gap-1">
              <h1 class="text-2xl font-extrabold text-highlighted">{{ auth.user?.fullName || auth.user?.name }}</h1>
              <p v-if="auth.user?.fullName" class="text-sm text-muted">@{{ auth.user?.userName || auth.user?.name }}</p>
              <p v-if="auth.user?.email" class="text-sm text-muted">{{ auth.user?.email }}</p>
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
        <div class="grid grid-cols-2 sm:grid-cols-3 gap-4 text-center">
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.xp }}</span>
            <span class="text-sm text-muted">Очки опыта</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.level }}</span>
            <span class="text-sm text-muted">Уровень</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.lessonsCompleted ?? 0 }}</span>
            <span class="text-sm text-muted">Уроков пройдено</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.tasksCompleted ?? 0 }}</span>
            <span class="text-sm text-muted">Задач решено</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.quizzesPassed ?? 0 }}</span>
            <span class="text-sm text-muted">Викторин пройдено</span>
          </div>
          <div class="flex flex-col gap-1">
            <span class="text-2xl font-extrabold text-primary">{{ auth.user?.coins ?? 0 }}</span>
            <span class="text-sm text-muted">Монеты</span>
          </div>
        </div>
      </UCard>

      <UCard>
        <template #header>
          <div class="flex items-center gap-2">
            <UIcon name="i-lucide-trophy" class="size-5 text-primary" />
            <h2 class="text-lg font-bold text-highlighted">Достижения</h2>
          </div>
        </template>
        <ul v-if="(auth.user?.achievements?.length ?? 0) > 0" class="flex flex-col gap-4">
          <li
            v-for="a in auth.user?.achievements"
            :key="a.achievementId"
            class="flex gap-4 items-start"
          >
            <div class="size-12 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
              <UIcon :name="a.iconUrl" class="size-6 text-primary" />
            </div>
            <div class="flex flex-col gap-0.5 min-w-0">
              <span class="font-bold text-highlighted">{{ a.title }}</span>
              <span class="text-sm text-muted">{{ a.description }}</span>
              <span class="text-xs text-primary">+{{ a.xpReward }} XP при получении</span>
            </div>
          </li>
        </ul>
        <p v-else class="text-sm text-muted">
          Решай задания и викторины — откроются достижения и бонусный опыт.
        </p>
      </UCard>

      <!-- Карточка редактирования -->
      <UCard>
        <template #header>
          <h2 class="text-lg font-bold text-highlighted">Редактировать профиль</h2>
        </template>

        <form class="flex flex-col gap-5" @submit.prevent="handleUpdate">
          <UFormField
            label="Имя пользователя"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.userName"
              type="text"
              :placeholder="auth.user?.userName || auth.user?.name"
              icon="i-lucide-user"
              size="xl"
              class="w-full"
              autocomplete="username"
            />
          </UFormField>

          <UFormField
            label="Полное имя"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.fullName"
              type="text"
              placeholder="Иван Иванов"
              icon="i-lucide-user-circle"
              size="xl"
              class="w-full"
              autocomplete="name"
            />
          </UFormField>

          <UFormField
            label="Email"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.email"
              type="email"
              placeholder="ivan@example.com"
              icon="i-lucide-mail"
              size="xl"
              class="w-full"
              autocomplete="email"
            />
          </UFormField>

          <USeparator label="Смена пароля" />

          <UFormField
            label="Текущий пароль"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.currentPassword"
              :type="showPassword ? 'text' : 'password'"
              placeholder="••••••••"
              icon="i-lucide-lock"
              size="xl"
              class="w-full"
              autocomplete="current-password"
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
            label="Новый пароль"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.newPassword"
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

          <UFormField
            label="Повторите новый пароль"
            :ui="{ label: 'text-base font-bold mb-1' }"
          >
            <UInput
              v-model="form.newPasswordConfirm"
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

      <!-- Привязка детей / код для родителя -->
      <UCard>
        <template #header>
          <div class="flex items-center gap-2">
            <UIcon name="i-lucide-link" class="size-5 text-primary" />
            <h2 class="text-lg font-bold text-highlighted">Связь аккаунтов</h2>
          </div>
        </template>

        <div class="flex flex-col gap-6">
          <!-- Генерация кода (для ребёнка) -->
          <div>
            <p class="text-sm text-muted mb-3">
              Сгенерируйте 6-значный код и передайте его родителю для привязки. Код действителен 10 минут.
            </p>
            <div class="flex items-center gap-3">
              <UButton
                label="Сгенерировать код"
                icon="i-lucide-key-round"
                :loading="generatingCode"
                @click="handleGenerateCode"
              />
              <div v-if="linkCode" class="flex items-center gap-2">
                <span class="text-3xl font-mono font-bold text-primary tracking-widest">{{ linkCode }}</span>
                <UButton
                  icon="i-lucide-copy"
                  variant="ghost"
                  size="sm"
                  @click="copyCode"
                />
              </div>
            </div>
            <p v-if="linkCode" class="text-xs text-muted mt-2">Код expires через 10 минут</p>
          </div>

          <USeparator />

          <!-- Ввод кода (для родителя) -->
          <div>
            <p class="text-sm text-muted mb-3">
              Введите код, который сгенерировал ребёнок, чтобы привязать его аккаунт.
            </p>
            <form class="flex items-center gap-3" @submit.prevent="handleLinkChild">
              <UInput
                v-model="childCode"
                placeholder="000000"
                maxlength="6"
                size="xl"
                class="w-40"
              />
              <UButton
                type="submit"
                label="Привязать"
                icon="i-lucide-link"
                :loading="linkingChild"
                :disabled="childCode.length !== 6"
              />
            </form>
          </div>

          <USeparator />

          <!-- Список привязанных детей -->
          <div>
            <div class="flex items-center justify-between mb-3">
              <p class="text-sm font-bold text-highlighted">Мои дети</p>
              <UButton
                label="Обновить"
                variant="ghost"
                size="xs"
                icon="i-lucide-refresh-cw"
                @click="fetchChildren"
              />
            </div>
            <div v-if="children.length === 0" class="text-sm text-muted">
              Нет привязанных аккаунтов
            </div>
            <div v-else class="space-y-2">
              <div
                v-for="child in children"
                :key="child.id"
                class="flex items-center justify-between p-3 rounded-lg bg-elevated"
              >
                <div class="flex items-center gap-3">
                  <UAvatar :alt="child.fullName || child.userName" size="sm" icon="i-lucide-user" />
                  <div>
                    <p class="font-medium text-highlighted">{{ child.fullName || child.userName }}</p>
                    <p class="text-xs text-muted">Уровень {{ child.level }} | {{ child.experiencePoints }} XP</p>
                  </div>
                </div>
                <UButton
                  icon="i-lucide-unlink"
                  color="error"
                  variant="ghost"
                  size="sm"
                  @click="handleUnlink(child.id)"
                />
              </div>
            </div>
          </div>
        </div>
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
    User: 'Ученик',
    teacher: 'Учитель',
    admin: 'Администратор',
  }
  return map[auth.user?.role ?? ''] ?? auth.user?.role ?? 'Пользователь'
})

const form = reactive({
  userName: '',
  fullName: '',
  email: '',
  currentPassword: '',
  newPassword: '',
  newPasswordConfirm: '',
})
const showPassword = ref(false)
const loading = ref(false)

function resetForm() {
  form.userName = ''
  form.fullName = ''
  form.email = ''
  form.currentPassword = ''
  form.newPassword = ''
  form.newPasswordConfirm = ''
}

async function handleUpdate() {
  const hasProfileChanges = form.userName.trim() || form.fullName.trim() || form.email.trim()
  const hasPasswordChange = form.newPassword

  if (!hasProfileChanges && !hasPasswordChange) {
    toast.add({ title: 'Нечего сохранять', description: 'Заполните хотя бы одно поле', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }
  if (hasPasswordChange && form.newPassword !== form.newPasswordConfirm) {
    toast.add({ title: 'Пароли не совпадают', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }
  if (hasPasswordChange && !form.currentPassword) {
    toast.add({ title: 'Введите текущий пароль', description: 'Для смены пароля нужен текущий пароль', color: 'warning', icon: 'i-lucide-alert-triangle' })
    return
  }

  loading.value = true
  try {
    const body: any = {}
    if (form.userName.trim()) body.userName = form.userName.trim()
    if (form.fullName.trim()) body.fullName = form.fullName.trim()
    if (form.email.trim()) body.email = form.email.trim()
    if (hasPasswordChange) {
      body.currentPassword = form.currentPassword
      body.newPassword = form.newPassword
    }

    const config = useRuntimeConfig()
    const res = await fetch(`${config.public.apiBase}/User/profile`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body),
      credentials: 'include',
    })

    if (!res.ok) {
      const errData = await res.json().catch(() => null)
      throw new Error(errData?.detail || errData?.title || 'Ошибка сервера')
    }

    await auth.fetchUser()
    toast.add({ title: 'Профиль обновлён!', color: 'success', icon: 'i-lucide-circle-check' })
    resetForm()
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

// Привязка детей
const config = useRuntimeConfig()
const API_URL = config.public.apiBase

const linkCode = ref('')
const generatingCode = ref(false)
const childCode = ref('')
const linkingChild = ref(false)

interface ChildInfo {
  id: string
  fullName: string | null
  userName: string
  level: number
  experiencePoints: number
}

const children = ref<ChildInfo[]>([])

async function handleGenerateCode() {
  generatingCode.value = true
  try {
    const data = await $fetch<{ code: string }>(`${API_URL}/User/generate-link-code`, {
      method: 'POST',
      credentials: 'include',
    })
    linkCode.value = data.code
    toast.add({ title: 'Код сгенерирован', color: 'success', icon: 'i-lucide-key-round' })
  } catch (e: any) {
    const msg = e?.data?.detail || e?.data?.title || 'Не удалось сгенерировать код'
    toast.add({ title: 'Ошибка', description: msg, color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    generatingCode.value = false
  }
}

async function copyCode() {
  try {
    await navigator.clipboard.writeText(linkCode.value)
    toast.add({ title: 'Скопировано', color: 'success', icon: 'i-lucide-copy' })
  } catch {
    toast.add({ title: 'Не удалось скопировать', color: 'error' })
  }
}

async function handleLinkChild() {
  if (childCode.value.length !== 6) return
  linkingChild.value = true
  try {
    await $fetch(`${API_URL}/User/link-child-by-code`, {
      method: 'POST',
      body: { code: childCode.value },
      credentials: 'include',
    })
    childCode.value = ''
    toast.add({ title: 'Ребёнок привязан!', color: 'success', icon: 'i-lucide-link' })
    await fetchChildren()
  } catch (e: any) {
    const msg = e?.data?.detail || e?.data?.title || 'Не удалось привязать'
    toast.add({ title: 'Ошибка', description: msg, color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    linkingChild.value = false
  }
}

async function fetchChildren() {
  try {
    children.value = await $fetch<ChildInfo[]>(`${API_URL}/User/my-children`, {
      credentials: 'include',
    })
  } catch {
    children.value = []
  }
}

async function handleUnlink(childId: string) {
  if (!confirm('Отвязать ребёнка?')) return
  try {
    await $fetch(`${API_URL}/User/unlink-child/${childId}`, {
      method: 'DELETE',
      credentials: 'include',
    })
    toast.add({ title: 'Ребёнок отвязан', color: 'success', icon: 'i-lucide-unlink' })
    await fetchChildren()
  } catch (e: any) {
    const msg = e?.data?.detail || e?.data?.title || 'Не удалось отвязать'
    toast.add({ title: 'Ошибка', description: msg, color: 'error', icon: 'i-lucide-circle-x' })
  }
}

onMounted(() => {
  auth.fetchUser()
  fetchChildren()
})
</script>
