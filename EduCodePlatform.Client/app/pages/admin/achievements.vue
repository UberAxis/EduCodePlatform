<template>
  <div class="p-8">
    <!-- Header -->
    <div class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-highlighted">Управление достижениями</h1>
        <p class="text-muted mt-2">Создавайте достижения и настраивайте условия их разблокировки</p>
      </div>
      <button
        @click="showCreateAchievementModal = true"
        class="bg-yellow-600 hover:bg-yellow-700 text-highlighted px-6 py-3 rounded-lg transition font-semibold"
      >
        + Новое достижение
      </button>
    </div>

    <!-- Tabs -->
    <div class="flex space-x-4 mb-8 border-b border-default">
      <button
        @click="activeTab = 'achievements'"
        :class="[
          'px-4 py-3 font-semibold border-b-2 transition',
          activeTab === 'achievements'
            ? 'text-warning border-yellow-400'
            : 'text-muted border-transparent hover:text-highlighted'
        ]"
      >
        Достижения
      </button>
      <button
        @click="activeTab = 'triggers'"
        :class="[
          'px-4 py-3 font-semibold border-b-2 transition',
          activeTab === 'triggers'
            ? 'text-warning border-yellow-400'
            : 'text-muted border-transparent hover:text-highlighted'
        ]"
      >
        Триггеры
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-12">
      <div class="text-muted">Загрузка достижений...</div>
    </div>

    <!-- Error -->
    <div v-if="error" class="bg-error/10 border border-error/30 text-error px-4 py-3 rounded-lg mb-6">
      {{ error }}
    </div>

    <!-- Achievements Tab -->
    <div v-if="activeTab === 'achievements' && !loading">
      <div v-if="achievements.length > 0" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="achievement in achievements"
          :key="achievement.id"
          class="bg-elevated rounded-lg border border-default p-6 hover:border-yellow-500 transition"
        >
          <div class="flex items-start justify-between mb-4">
            <div>
              <h3 class="text-lg font-bold text-highlighted">{{ achievement.title }}</h3>
              <p class="text-warning text-sm font-semibold">+{{ achievement.xpReward }} XP</p>
            </div>
            <img
              v-if="achievement.iconUrl"
              :src="achievement.iconUrl"
              :alt="achievement.title"
              class="w-12 h-12 rounded-lg"
            />
          </div>
          <p class="text-muted text-sm mb-6">{{ achievement.description }}</p>
          <div class="flex space-x-2">
            <button
              @click="editAchievement(achievement)"
              class="flex-1 text-primary hover:text-primary/80 text-sm font-semibold py-2 bg-primary/10 hover:bg-primary/20 rounded transition"
            >
              Редактировать
            </button>
            <button
              @click="deleteAchievement(achievement.id)"
              class="flex-1 text-error hover:text-error/80 text-sm font-semibold py-2 bg-error/10 hover:bg-error/20 rounded transition"
            >
              Удалить
            </button>
          </div>
        </div>
      </div>

      <div v-if="achievements.length === 0" class="text-center py-12">
        <div class="text-muted">Достижения еще не созданы.</div>
      </div>
    </div>

    <!-- Triggers Tab -->
    <div v-if="activeTab === 'triggers' && !loading">
      <div class="flex justify-between items-center mb-6">
        <h3 class="text-xl font-bold text-highlighted">Триггеры достижений</h3>
        <button
          @click="showCreateTriggerModal = true"
          class="bg-purple-600 hover:bg-purple-700 text-highlighted px-4 py-2 rounded-lg transition font-semibold text-sm"
        >
          + Новый триггер
        </button>
      </div>

      <div v-if="triggers.length > 0" class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-elevated border-b border-default">
            <tr>
              <th class="px-6 py-3 text-left text-highlighted font-semibold">Достижение</th>
              <th class="px-6 py-3 text-left text-highlighted font-semibold">Тип триггера</th>
              <th class="px-6 py-3 text-left text-highlighted font-semibold">Необходимое значение</th>
              <th class="px-6 py-3 text-left text-highlighted font-semibold">Статус</th>
              <th class="px-6 py-3 text-right text-highlighted font-semibold">Действия</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="trigger in triggers"
              :key="trigger.id"
              class="border-b border-default hover:bg-elevated transition"
            >
              <td class="px-6 py-4 text-highlighted font-medium">{{ getTriggerAchievementTitle(trigger.achievementId) }}</td>
              <td class="px-6 py-4 text-default">{{ getTriggerTypeLabel(trigger.triggerType) }}</td>
              <td class="px-6 py-4 text-default">{{ trigger.requiredValue }}</td>
              <td class="px-6 py-4">
                <span
                  :class="[
                    'px-3 py-1 rounded-full text-sm font-semibold',
                    trigger.isActive
                      ? 'bg-success/15 text-success'
                      : 'bg-error/15 text-error'
                  ]"
                >
                  {{ trigger.isActive ? 'Активен' : 'Неактивен' }}
                </span>
              </td>
              <td class="px-6 py-4 text-right space-x-2">
                <button
                  @click="editTrigger(trigger)"
                  class="text-primary hover:text-blue-300 text-sm font-semibold"
                >
                  Редактировать
                </button>
                <button
                  @click="deleteTrigger(trigger.id)"
                  class="text-error hover:text-red-300 text-sm font-semibold"
                >
                  Удалить
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="triggers.length === 0" class="text-center py-12">
        <div class="text-muted">Триггеры еще не настроены.</div>
      </div>
    </div>

    <!-- Achievement Modal -->
    <AdminAchievementModal
      v-if="showCreateAchievementModal || showEditAchievementModal"
      :achievement="editingAchievement"
      :is-editing="showEditAchievementModal"
      @close="closeAchievementModal"
      @save="saveAchievement"
    />

    <!-- Trigger Modal -->
    <AdminTriggerModal
      v-if="showCreateTriggerModal || showEditTriggerModal"
      :trigger="editingTrigger"
      :achievements="achievements"
      :is-editing="showEditTriggerModal"
      @close="closeTriggerModal"
      @save="saveTrigger"
    />
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: "admin",
  middleware: "admin-only"
})

interface Achievement {
  id: number
  title: string
  description: string
  iconUrl: string
  xpReward: number
  triggers: AchievementTrigger[]
}

interface AchievementTrigger {
  id: number
  achievementId: number
  triggerType: string
  requiredValue: number
  targetModuleId?: number
  targetLessonId?: number
  isActive: boolean
}

const achievements = ref<Achievement[]>([])
const triggers = ref<AchievementTrigger[]>([])
const loading = ref(false)
const error = ref('')
const activeTab = ref<'achievements' | 'triggers'>('achievements')
const showCreateAchievementModal = ref(false)
const showEditAchievementModal = ref(false)
const showCreateTriggerModal = ref(false)
const showEditTriggerModal = ref(false)
const editingAchievement = ref<Achievement | null>(null)
const editingTrigger = ref<AchievementTrigger | null>(null)

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

onMounted(async () => {
  await fetchAchievements()
  await fetchTriggers()
})

const fetchAchievements = async () => {
  loading.value = true
  error.value = ''
  try {
    achievements.value = await $fetch<Achievement[]>(`${API_URL}/achievement`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load achievements'
  } finally {
    loading.value = false
  }
}

const fetchTriggers = async () => {
  try {
    triggers.value = await $fetch<AchievementTrigger[]>(`${API_URL}/achievement/triggers`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load triggers'
  }
}

const getTriggerAchievementTitle = (achievementId: number) => {
  return achievements.value.find(a => a.id === achievementId)?.title || 'Unknown'
}

const getTriggerTypeLabel = (triggerType: string): string => {
  const triggerLabels: Record<string, string> = {
    'XpThreshold': 'Порог XP',
    'TasksCompletedCount': 'Завершенные задания',
    'QuizzesPassedCount': 'Пройденные тесты',
    'LessonsCompletedCount': 'Завершенные уроки',
    'ConsecutiveCorrectAnswers': 'Правильные ответы подряд',
    'ModuleCompletionCount': 'Завершенные модули',
    'SpecificModuleCompletion': 'Конкретный модуль',
    'SpecificLessonCompletion': 'Конкретный урок',
    'LevelThreshold': 'Уровень',
    'CoinsThreshold': 'Монеты',
    'AccuracyPercentage': 'Процент точности',
    'DaysActive': 'Дни активности',
    'FirstTaskCompletion': 'Первое задание',
    'FirstQuizPassed': 'Первый пройденный тест'
  }
  return triggerLabels[triggerType] || triggerType
}

const editAchievement = (achievement: Achievement) => {
  editingAchievement.value = achievement
  showEditAchievementModal.value = true
}

const editTrigger = (trigger: AchievementTrigger) => {
  editingTrigger.value = trigger
  showEditTriggerModal.value = true
}

const closeAchievementModal = () => {
  showCreateAchievementModal.value = false
  showEditAchievementModal.value = false
  editingAchievement.value = null
}

const closeTriggerModal = () => {
  showCreateTriggerModal.value = false
  showEditTriggerModal.value = false
  editingTrigger.value = null
}

const saveAchievement = async (data: any) => {
  try {
    if (showEditAchievementModal.value && editingAchievement.value) {
      await $fetch(`${API_URL}/achievement/${editingAchievement.value.id}`, {
        method: 'PUT',
        body: data,
        credentials: 'include',
      })
    } else {
      await $fetch(`${API_URL}/achievement`, {
        method: 'POST',
        body: data,
        credentials: 'include',
      })
    }
    await fetchAchievements()
    closeAchievementModal()
  } catch (e: any) {
    error.value = e.message || 'Failed to save achievement'
  }
}

const saveTrigger = async (data: any) => {
  try {
    if (showEditTriggerModal.value && editingTrigger.value) {
      await $fetch(`${API_URL}/achievement/triggers/${editingTrigger.value.id}`, {
        method: 'PUT',
        body: data,
        credentials: 'include',
      })
    } else {
      await $fetch(`${API_URL}/achievement/triggers`, {
        method: 'POST',
        body: data,
        credentials: 'include',
      })
    }
    await fetchTriggers()
    closeTriggerModal()
  } catch (e: any) {
    error.value = e.message || 'Failed to save trigger'
  }
}

const deleteAchievement = async (id: number) => {
  if (confirm('Вы уверены, что хотите удалить это достижение?')) {
    try {
      await $fetch(`${API_URL}/achievement/${id}`, {
        method: 'DELETE',
        credentials: 'include',
      })
      await fetchAchievements()
    } catch (e: any) {
      error.value = e.message || 'Failed to delete achievement'
    }
  }
}

const deleteTrigger = async (id: number) => {
  if (confirm('Вы уверены, что хотите удалить этот триггер?')) {
    try {
      await $fetch(`${API_URL}/achievement/triggers/${id}`, {
        method: 'DELETE',
        credentials: 'include',
      })
      await fetchTriggers()
    } catch (e: any) {
      error.value = e.message || 'Failed to delete trigger'
    }
  }
}
</script>
