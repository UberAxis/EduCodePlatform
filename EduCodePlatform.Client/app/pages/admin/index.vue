<template>
  <div class="p-8">
    <!-- Header -->
    <div class="mb-8">
      <h1 class="text-3xl font-bold text-highlighted">Панель администратора</h1>
      <p class="text-muted mt-2">Обзор платформы и управление контентом</p>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      <USkeleton v-for="i in 4" :key="i" class="h-32 w-full" />
    </div>

    <!-- Stats Cards -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      <div class="bg-elevated rounded-lg p-6 border border-default">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-muted text-sm">Пользователи</p>
            <p class="text-3xl font-bold text-highlighted mt-1">{{ stats.totalUsers }}</p>
          </div>
          <div class="bg-primary/10 rounded-full p-3">
            <svg class="w-6 h-6 text-primary" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.856-1.487M15 10a3 3 0 11-6 0 3 3 0 016 0zM4 20h16a2 2 0 002-2v-2a3 3 0 00-5.856-1.487M13 16H4a2 2 0 00-2 2v2a2 2 0 002 2h9" />
            </svg>
          </div>
        </div>
        <div class="mt-3 flex items-center text-sm">
          <span class="text-muted">{{ stats.adminCount }} админов</span>
          <span class="mx-2 text-muted">|</span>
          <span class="text-muted">{{ stats.userCount }} учеников</span>
        </div>
      </div>

      <div class="bg-elevated rounded-lg p-6 border border-default">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-muted text-sm">Модули</p>
            <p class="text-3xl font-bold text-highlighted mt-1">{{ stats.totalModules }}</p>
          </div>
          <div class="bg-success/10 rounded-full p-3">
            <svg class="w-6 h-6 text-success" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C6.5 6.253 2 10.753 2 16.253s4.5 10 10 10 10-4.5 10-10c0-5.5-4.5-10-10-10z" />
            </svg>
          </div>
        </div>
        <div class="mt-3 flex items-center text-sm">
          <span class="text-muted">{{ stats.totalLessons }} уроков</span>
          <span class="mx-2 text-muted">|</span>
          <span class="text-muted">{{ stats.totalTasks }} заданий</span>
        </div>
      </div>

      <div class="bg-elevated rounded-lg p-6 border border-default">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-muted text-sm">Достижения</p>
            <p class="text-3xl font-bold text-highlighted mt-1">{{ stats.totalAchievements }}</p>
          </div>
          <div class="bg-warning/10 rounded-full p-3">
            <svg class="w-6 h-6 text-warning" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z" />
            </svg>
          </div>
        </div>
        <div class="mt-3 flex items-center text-sm">
          <span class="text-muted">{{ stats.totalTriggers }} триггеров</span>
        </div>
      </div>

      <div class="bg-elevated rounded-lg p-6 border border-default">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-muted text-sm">Активность</p>
            <p class="text-3xl font-bold text-highlighted mt-1">{{ stats.totalSubmissions }}</p>
          </div>
          <div class="bg-purple-900/50 rounded-full p-3">
            <svg class="w-6 h-6 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
            </svg>
          </div>
        </div>
        <div class="mt-3 flex items-center text-sm">
          <span class="text-muted">Отправок заданий</span>
        </div>
      </div>
    </div>

    <!-- Detailed Stats -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
      <!-- Tasks by Type -->
      <div class="bg-elevated rounded-lg p-6 border border-default">
        <h3 class="text-lg font-semibold text-highlighted mb-4">Задания по типам</h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-3">
              <div class="w-3 h-3 rounded-full bg-green-500"></div>
              <span class="text-default">JavaScript</span>
            </div>
            <div class="flex items-center gap-3">
              <div class="w-32 bg-accented rounded-full h-2">
                <div class="bg-green-500 h-2 rounded-full" :style="{ width: getTaskPercent('JavaScript') + '%' }"></div>
              </div>
              <span class="text-highlighted font-semibold w-8 text-right">{{ stats.tasksByType.JavaScript }}</span>
            </div>
          </div>
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-3">
              <div class="w-3 h-3 rounded-full bg-yellow-500"></div>
              <span class="text-default">Python</span>
            </div>
            <div class="flex items-center gap-3">
              <div class="w-32 bg-accented rounded-full h-2">
                <div class="bg-yellow-500 h-2 rounded-full" :style="{ width: getTaskPercent('Python') + '%' }"></div>
              </div>
              <span class="text-highlighted font-semibold w-8 text-right">{{ stats.tasksByType.Python }}</span>
            </div>
          </div>
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-3">
              <div class="w-3 h-3 rounded-full bg-blue-500"></div>
              <span class="text-default">Тесты (Quiz)</span>
            </div>
            <div class="flex items-center gap-3">
              <div class="w-32 bg-accented rounded-full h-2">
                <div class="bg-blue-500 h-2 rounded-full" :style="{ width: getTaskPercent('Quiz') + '%' }"></div>
              </div>
              <span class="text-highlighted font-semibold w-8 text-right">{{ stats.tasksByType.Quiz }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- User Activity -->
      <div class="bg-elevated rounded-lg p-6 border border-default">
        <h3 class="text-lg font-semibold text-highlighted mb-4">Активность пользователей</h3>
        <div class="space-y-4">
          <div class="flex items-center justify-between">
            <span class="text-default">Средний уровень</span>
            <span class="text-highlighted font-semibold">{{ stats.avgLevel }}</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-default">Средний XP</span>
            <span class="text-highlighted font-semibold">{{ stats.avgXp }}</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-default">Всего монет в системе</span>
            <span class="text-highlighted font-semibold">{{ stats.totalCoins }}</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-default">Заданий выполнено (всего)</span>
            <span class="text-highlighted font-semibold">{{ stats.totalTasksCompleted }}</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-default">Тестов пройдено (всего)</span>
            <span class="text-highlighted font-semibold">{{ stats.totalQuizzesPassed }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Top Users -->
    <div class="bg-elevated rounded-lg p-6 border border-default mb-8">
      <h3 class="text-lg font-semibold text-highlighted mb-4">Топ пользователей по опыту</h3>
      <div v-if="topUsers.length === 0" class="text-muted text-sm">Нет данных</div>
      <div v-else class="space-y-3">
        <div
          v-for="(user, index) in topUsers"
          :key="user.id"
          class="flex items-center gap-4 p-3 rounded-lg bg-accented/50"
        >
          <div
            class="size-8 rounded-full flex items-center justify-center text-sm font-bold shrink-0"
            :class="index === 0 ? 'bg-yellow-500/20 text-warning' : index === 1 ? 'bg-slate-400/20 text-default' : index === 2 ? 'bg-orange-500/20 text-orange-400' : 'bg-slate-600 text-muted'"
          >
            {{ index + 1 }}
          </div>
          <div class="flex-1 min-w-0">
            <p class="text-highlighted font-medium truncate">{{ user.fullName || user.userName }}</p>
            <p class="text-xs text-muted">Уровень {{ user.level }} | {{ user.tasksCompletedCount }} заданий | {{ user.quizzesPassedCount }} тестов</p>
          </div>
          <div class="text-right shrink-0">
            <p class="text-highlighted font-bold">{{ user.experiencePoints }} XP</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Quick Navigation -->
    <h3 class="text-lg font-semibold text-highlighted mb-4">Быстрый доступ</h3>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-5 gap-4">
      <NuxtLink
        v-for="link in navLinks"
        :key="link.to"
        :to="link.to"
        class="bg-elevated rounded-lg p-4 border border-default hover:border-slate-500 transition-colors"
      >
        <div class="flex items-center gap-3">
          <div :class="[link.bgClass, 'rounded-lg p-2']">
            <svg :class="[link.iconClass, 'w-5 h-5']" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="link.icon" />
            </svg>
          </div>
          <div>
            <p class="text-highlighted font-medium text-sm">{{ link.label }}</p>
            <p class="text-muted text-xs">{{ link.count }} записей</p>
          </div>
        </div>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: 'admin',
  middleware: 'admin-only',
})

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

const loading = ref(true)

interface User {
  id: string
  fullName: string | null
  userName: string
  level: number
  experiencePoints: number
  coins: number
  role: string
  tasksCompletedCount: number
  quizzesPassedCount: number
}

interface Module { id: number }
interface Lesson { id: number }
interface LessonTask { id: number; type: string }
interface Achievement { id: number }
interface AchievementTrigger { id: number }
interface Submission { id: number }

const stats = reactive({
  totalUsers: 0,
  adminCount: 0,
  userCount: 0,
  totalModules: 0,
  totalLessons: 0,
  totalTasks: 0,
  totalAchievements: 0,
  totalTriggers: 0,
  totalSubmissions: 0,
  tasksByType: { JavaScript: 0, Python: 0, Quiz: 0 } as Record<string, number>,
  avgLevel: '0',
  avgXp: '0',
  totalCoins: 0,
  totalTasksCompleted: 0,
  totalQuizzesPassed: 0,
})

const topUsers = ref<User[]>([])

const navLinks = computed(() => [
  { to: '/admin/modules', label: 'Модули', count: stats.totalModules, bgClass: 'bg-primary/10', iconClass: 'text-primary', icon: 'M12 6.253v13m0-13C6.5 6.253 2 10.753 2 16.253s4.5 10 10 10 10-4.5 10-10c0-5.5-4.5-10-10-10z' },
  { to: '/admin/lessons', label: 'Уроки', count: stats.totalLessons, bgClass: 'bg-success/10', iconClass: 'text-success', icon: 'M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z' },
  { to: '/admin/tasks', label: 'Задания', count: stats.totalTasks, bgClass: 'bg-purple-900/50', iconClass: 'text-purple-400', icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2' },
  { to: '/admin/achievements', label: 'Достижения', count: stats.totalAchievements, bgClass: 'bg-warning/10', iconClass: 'text-warning', icon: 'M5 3v4M3 5h4M6 17v4m-2-2h4m5-16l2.286 6.857L21 12l-5.714 2.143L13 21l-2.286-6.857L5 12l5.714-2.143L13 3z' },
  { to: '/admin/users', label: 'Пользователи', count: stats.totalUsers, bgClass: 'bg-error/10', iconClass: 'text-error', icon: 'M17 20h5v-2a3 3 0 00-5.856-1.487M15 10a3 3 0 11-6 0 3 3 0 016 0zM4 20h16a2 2 0 002-2v-2a3 3 0 00-5.856-1.487M13 16H4a2 2 0 00-2 2v2a2 2 0 002 2h9' },
])

function getTaskPercent(type: string): number {
  if (stats.totalTasks === 0) return 0
  return Math.round((stats.tasksByType[type] || 0) / stats.totalTasks * 100)
}

async function fetchAll() {
  loading.value = true
  try {
    const [users, modules, lessons, tasks, achievements, triggers, submissions] = await Promise.all([
      $fetch<User[]>(`${API_URL}/User`, { credentials: 'include' }).catch(() => []),
      $fetch<Module[]>(`${API_URL}/Module`, { credentials: 'include' }).catch(() => []),
      $fetch<Lesson[]>(`${API_URL}/Lesson`, { credentials: 'include' }).catch(() => []),
      $fetch<LessonTask[]>(`${API_URL}/Task`, { credentials: 'include' }).catch(() => []),
      $fetch<Achievement[]>(`${API_URL}/Achievement`, { credentials: 'include' }).catch(() => []),
      $fetch<AchievementTrigger[]>(`${API_URL}/Achievement/triggers`, { credentials: 'include' }).catch(() => []),
      $fetch<Submission[]>(`${API_URL}/Submission`, { credentials: 'include' }).catch(() => []),
    ])

    // Users
    stats.totalUsers = users.length
    stats.adminCount = users.filter(u => u.role === 'Admin').length
    stats.userCount = users.filter(u => u.role !== 'Admin').length
    stats.avgLevel = users.length ? (users.reduce((s, u) => s + u.level, 0) / users.length).toFixed(1) : '0'
    stats.avgXp = users.length ? Math.round(users.reduce((s, u) => s + u.experiencePoints, 0) / users.length).toString() : '0'
    stats.totalCoins = users.reduce((s, u) => s + (u.coins || 0), 0)
    stats.totalTasksCompleted = users.reduce((s, u) => s + u.tasksCompletedCount, 0)
    stats.totalQuizzesPassed = users.reduce((s, u) => s + u.quizzesPassedCount, 0)

    // Top 5 users by XP
    topUsers.value = [...users].sort((a, b) => b.experiencePoints - a.experiencePoints).slice(0, 5)

    // Modules / Lessons / Tasks
    stats.totalModules = modules.length
    stats.totalLessons = lessons.length
    stats.totalTasks = tasks.length
    stats.tasksByType = {
      JavaScript: tasks.filter(t => t.type === 'JavaScript').length,
      Python: tasks.filter(t => t.type === 'Python').length,
      Quiz: tasks.filter(t => t.type === 'Quiz').length,
    }

    // Achievements
    stats.totalAchievements = achievements.length
    stats.totalTriggers = triggers.length

    // Submissions
    stats.totalSubmissions = submissions.length
  } catch {
    // silently fail
  } finally {
    loading.value = false
  }
}

onMounted(fetchAll)
</script>
