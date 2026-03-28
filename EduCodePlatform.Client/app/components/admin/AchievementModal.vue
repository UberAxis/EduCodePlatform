<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4">
    <div class="bg-elevated rounded-lg shadow-xl max-w-2xl w-full border border-default">
      <!-- Header -->
      <div class="flex justify-between items-center p-6 border-b border-default">
        <h2 class="text-2xl font-bold text-highlighted">
          {{ isEditing ? 'Редактировать достижение' : 'Создать достижение' }}
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
          <label class="block text-sm font-semibold text-highlighted mb-2">Название *</label>
          <input
            v-model="form.title"
            type="text"
            placeholder="Название достижения"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Описание *</label>
          <textarea
            v-model="form.description"
            placeholder="Описание достижения"
            rows="3"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">URL иконки *</label>
            <input
              v-model="form.iconUrl"
              type="text"
              placeholder="https://example.com/icon.png"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Награда XP *</label>
            <input
              v-model.number="form.xpReward"
              type="number"
              placeholder="100"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>
        </div>

        <div v-if="form.iconUrl" class="flex items-center space-x-4">
          <img
            :src="form.iconUrl"
            :alt="form.title"
            class="w-16 h-16 rounded-lg"
            @error="form.iconUrl = ''"
          />
          <div class="text-sm text-muted">Предпросмотр</div>
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
          class="px-6 py-2 bg-yellow-600 hover:bg-yellow-700 disabled:bg-slate-600 text-highlighted rounded-lg transition font-semibold"
        >
          {{ isSaving ? 'Сохранение...' : isEditing ? 'Обновить' : 'Создать' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  achievement?: {
    id: number
    title: string
    description: string
    iconUrl: string
    xpReward: number
  } | null
  isEditing: boolean
}>()

const emit = defineEmits<{
  close: []
  save: [data: any]
}>()

const isSaving = ref(false)

const form = ref({
  title: props.achievement?.title || '',
  description: props.achievement?.description || '',
  iconUrl: props.achievement?.iconUrl || '',
  xpReward: props.achievement?.xpReward || 10
})

const submitForm = async () => {
  if (!form.value.title || !form.value.description || !form.value.iconUrl) {
    alert('Пожалуйста, заполните все обязательные поля')
    return
  }

  isSaving.value = true
  try {
    emit('save', {
      title: form.value.title,
      description: form.value.description,
      iconUrl: form.value.iconUrl,
      xpReward: form.value.xpReward
    })
  } finally {
    isSaving.value = false
  }
}
</script>
