<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4">
    <div class="bg-elevated rounded-lg shadow-xl max-w-2xl w-full border border-default">
      <!-- Header -->
      <div class="flex justify-between items-center p-6 border-b border-default">
        <h2 class="text-2xl font-bold text-highlighted">
          {{ isEditing ? 'Редактировать модуль' : 'Создать модуль' }}
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
          <label class="block text-sm font-semibold text-highlighted mb-2">Название</label>
          <input
            v-model="form.title"
            type="text"
            placeholder="Название модуля"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Описание</label>
          <textarea
            v-model="form.description"
            placeholder="Описание модуля"
            rows="4"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">URL обложки</label>
          <input
            v-model="form.coverImage"
            type="text"
            placeholder="https://example.com/image.jpg"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Порядковый номер</label>
          <input
            v-model.number="form.orderIndex"
            type="number"
            placeholder="0"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
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
          {{ isSaving ? 'Сохранение...' : isEditing ? 'Обновить' : 'Создать' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  module?: {
    id: number
    title: string
    description: string
    coverImage: string
    orderIndex: number
  } | null
  isEditing: boolean
}>()

const emit = defineEmits<{
  close: []
  save: [data: any]
}>()

const isSaving = ref(false)

const form = ref({
  title: props.module?.title || '',
  description: props.module?.description || '',
  coverImage: props.module?.coverImage || '',
  orderIndex: props.module?.orderIndex || 0
})

const submitForm = async () => {
  if (!form.value.title || !form.value.description) {
    alert('Пожалуйста, заполните все обязательные поля')
    return
  }

  isSaving.value = true
  try {
    emit('save', {
      title: form.value.title,
      description: form.value.description,
      coverImage: form.value.coverImage,
      orderIndex: form.value.orderIndex
    })
  } finally {
    isSaving.value = false
  }
}
</script>
