<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4 overflow-y-auto">
    <div class="bg-elevated rounded-lg shadow-xl max-w-4xl w-full border border-default my-8">
      <!-- Header -->
      <div class="flex justify-between items-center p-6 border-b border-default sticky top-0 bg-elevated">
        <h2 class="text-2xl font-bold text-highlighted">
          {{ isEditing ? 'Редактировать урок' : 'Создать урок' }}
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
      <div class="p-6 space-y-6 max-h-[calc(100vh-200px)] overflow-y-auto">
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Модуль *</label>
            <select
              v-model.number="form.moduleId"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            >
              <option value="" disabled>Выберите модуль</option>
              <option v-for="module in modules" :key="module.id" :value="module.id">
                {{ module.title }}
              </option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Название *</label>
            <input
              v-model="form.title"
              type="text"
              placeholder="Название урока"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            />
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">URL обложки (опционально)</label>
          <input
            v-model="form.coverImage"
            type="text"
            placeholder="https://example.com/image.jpg"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div>
          <AdminMarkdownEditor
            v-model="form.markdownContent"
            label="Содержание урока *"
            placeholder="# Заголовок&#10;&#10;Начните писать содержание урока..."
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
      <div class="flex justify-end space-x-3 p-6 border-t border-default sticky bottom-0 bg-elevated">
        <button
          @click="$emit('close')"
          class="px-4 py-2 text-muted hover:text-highlighted transition"
        >
          Отмена
        </button>
        <button
          @click="submitForm"
          :disabled="isSaving"
          class="px-6 py-2 bg-green-600 hover:bg-green-700 disabled:bg-slate-600 text-highlighted rounded-lg transition font-semibold"
        >
          {{ isSaving ? 'Сохранение...' : isEditing ? 'Обновить' : 'Создать' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  lesson?: {
    id: number
    title: string
    moduleId: number
    coverImage: string
    markdownContent: string
    orderIndex: number
  } | null
  modules: Array<{
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
  moduleId: props.lesson?.moduleId || null,
  title: props.lesson?.title || '',
  coverImage: props.lesson?.coverImage || '',
  markdownContent: props.lesson?.markdownContent || '',
  orderIndex: props.lesson?.orderIndex || 0
})

const submitForm = async () => {
  if (!form.value.moduleId || !form.value.title || !form.value.markdownContent) {
    alert('Пожалуйста, заполните все обязательные поля')
    return
  }

  isSaving.value = true
  try {
    const payload: any = {
      moduleId: form.value.moduleId,
      title: form.value.title,
      markdownContent: form.value.markdownContent,
      orderIndex: form.value.orderIndex
    }
    
    if (form.value.coverImage && form.value.coverImage.trim()) {
      payload.coverImage = form.value.coverImage
    }
    
    emit('save', payload)
  } finally {
    isSaving.value = false
  }
}
</script>
