<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4">
    <div class="bg-elevated rounded-lg shadow-xl max-w-2xl w-full border border-default">
      <!-- Header -->
      <div class="flex justify-between items-center p-6 border-b border-default">
        <h2 class="text-2xl font-bold text-highlighted">Редактировать пользователя</h2>
        <button
          @click="$emit('close')"
          class="text-muted hover:text-highlighted transition"
        >
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      <!-- Form -->
      <div class="p-6 space-y-6">
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Полное имя</label>
            <input
              v-model="form.fullName"
              type="text"
              placeholder="Иван Иванов"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Email</label>
            <input
              v-model="form.email"
              type="email"
              placeholder="user@example.com"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Уровень</label>
            <input
              v-model.number="form.level"
              type="number"
              min="1"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Опыт (XP)</label>
            <input
              v-model.number="form.experiencePoints"
              type="number"
              min="0"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Монеты</label>
            <input
              v-model.number="form.coins"
              type="number"
              min="0"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Роль</label>
            <select
              v-model="form.role"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            >
              <option value="User">Пользователь</option>
              <option value="Admin">Админ</option>
            </select>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Задания выполнено</label>
            <input
              v-model.number="form.tasksCompletedCount"
              type="number"
              min="0"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Тесты пройдены</label>
            <input
              v-model.number="form.quizzesPassedCount"
              type="number"
              min="0"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>
        </div>
      </div>

      <!-- Actions -->
      <div class="flex justify-end space-x-3 p-6 border-t border-default">
        <button
          @click="$emit('close')"
          class="px-4 py-2 text-muted hover:text-highlighted transition"
        >
          Отмена
        </button>
        <button
          @click="submitForm"
          :disabled="isSaving"
          class="px-6 py-2 bg-blue-600 hover:bg-blue-700 disabled:bg-slate-600 text-highlighted rounded-lg transition font-semibold"
        >
          {{ isSaving ? 'Сохранение...' : 'Обновить' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  user: {
    id: string
    fullName: string | null
    email: string | null
    level: number
    experiencePoints: number
    coins: number
    role?: string
    tasksCompletedCount: number
    quizzesPassedCount: number
  }
}>()

const emit = defineEmits<{
  close: []
  save: [data: any]
}>()

const isSaving = ref(false)

const form = ref({
  fullName: props.user.fullName || '',
  email: props.user.email || '',
  level: props.user.level,
  experiencePoints: props.user.experiencePoints,
  coins: props.user.coins || 0,
  role: props.user.role || 'User',
  tasksCompletedCount: props.user.tasksCompletedCount,
  quizzesPassedCount: props.user.quizzesPassedCount
})

const submitForm = async () => {
  isSaving.value = true
  try {
    emit('save', {
      fullName: form.value.fullName || undefined,
      email: form.value.email || undefined,
      level: form.value.level,
      experiencePoints: form.value.experiencePoints,
      coins: form.value.coins,
      role: form.value.role,
      tasksCompletedCount: form.value.tasksCompletedCount,
      quizzesPassedCount: form.value.quizzesPassedCount
    })
  } finally {
    isSaving.value = false
  }
}
</script>
