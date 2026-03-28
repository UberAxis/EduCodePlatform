<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4">
    <div class="bg-elevated rounded-lg shadow-xl max-w-2xl w-full border border-default">
      <!-- Header -->
      <div class="flex justify-between items-center p-6 border-b border-default">
        <h2 class="text-2xl font-bold text-highlighted">
          {{ isEditing ? 'Редактировать триггер' : 'Создать триггер' }}
        </h2>
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
        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Достижение *</label>
          <select
            v-model.number="form.achievementId"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          >
            <option value="" disabled>Выберите достижение</option>
            <option v-for="achievement in achievements" :key="achievement.id" :value="achievement.id">
              {{ achievement.title }}
            </option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Тип триггера *</label>
          <select
            v-model="form.triggerType"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          >
            <option value="">Выберите тип триггера</option>
            <option value="XpThreshold">Порог XP</option>
            <option value="TasksCompletedCount">Завершенные задания</option>
            <option value="QuizzesPassedCount">Пройденные тесты</option>
            <option value="LessonsCompletedCount">Завершенные уроки</option>
            <option value="ConsecutiveCorrectAnswers">Правильные ответы подряд</option>
            <option value="ModuleCompletionCount">Завершенные модули</option>
            <option value="SpecificModuleCompletion">Конкретный модуль</option>
            <option value="SpecificLessonCompletion">Конкретный урок</option>
            <option value="LevelThreshold">Уровень</option>
            <option value="CoinsThreshold">Монеты</option>
            <option value="AccuracyPercentage">Процент точности</option>
            <option value="DaysActive">Дни активности</option>
            <option value="FirstTaskCompletion">Первое задание</option>
            <option value="FirstQuizPassed">Первый пройденный тест</option>
          </select>
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Необходимое значение *</label>
          <input
            v-model.number="form.requiredValue"
            type="number"
            placeholder="Например, 100"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
          <p class="text-xs text-muted mt-1">
            Для XP/Level/Coins: пороговое значение. Для подсчета: количество завершений.
          </p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Целевой модуль (Опционально)</label>
            <input
              v-model.number="form.targetModuleId"
              type="number"
              placeholder="ID модуля"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Целевой урок (Опционально)</label>
            <input
              v-model.number="form.targetLessonId"
              type="number"
              placeholder="ID урока"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>
        </div>

        <div v-if="isEditing" class="flex items-center">
          <input
            v-model="form.isActive"
            type="checkbox"
            id="isActive"
            class="w-4 h-4 text-blue-600 rounded"
          />
          <label for="isActive" class="ml-3 text-sm text-highlighted">Активен</label>
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
          class="px-6 py-2 bg-purple-600 hover:bg-purple-700 disabled:bg-slate-600 text-highlighted rounded-lg transition font-semibold"
        >
          {{ isSaving ? 'Сохранение...' : isEditing ? 'Обновить' : 'Создать' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  trigger?: {
    id: number
    achievementId: number
    triggerType: string
    requiredValue: number
    targetModuleId?: number | null
    targetLessonId?: number | null
    isActive: boolean
  } | null
  achievements: Array<{
    id: number
    title: string
  }>
  isEditing: boolean
}>()

const emit = defineEmits<{
  close: []
  save: [data: any]
}>()

const isSaving = ref(false)

const form = ref({
  achievementId: props.trigger?.achievementId || null,
  triggerType: props.trigger?.triggerType || '',
  requiredValue: props.trigger?.requiredValue || 1,
  targetModuleId: props.trigger?.targetModuleId || null,
  targetLessonId: props.trigger?.targetLessonId || null,
  isActive: props.trigger?.isActive ?? true
})

const submitForm = async () => {
  if (!form.value.achievementId || !form.value.triggerType) {
    alert('Пожалуйста, заполните обязательные поля')
    return
  }

  isSaving.value = true
  try {
    emit('save', {
      achievementId: form.value.achievementId,
      triggerType: form.value.triggerType,
      requiredValue: form.value.requiredValue,
      targetModuleId: form.value.targetModuleId || undefined,
      targetLessonId: form.value.targetLessonId || undefined,
      isActive: form.value.isActive
    })
  } finally {
    isSaving.value = false
  }
}
</script>
