<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4 overflow-y-auto">
    <div class="bg-elevated rounded-lg shadow-xl max-w-4xl w-full border border-default my-8">
      <!-- Header -->
      <div class="flex justify-between items-center p-6 border-b border-default sticky top-0 bg-elevated">
        <h2 class="text-2xl font-bold text-highlighted">
          {{ isEditing ? 'Редактировать задание' : 'Создать задание' }}
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
      <div class="p-6 space-y-6 max-h-[calc(100vh-220px)] overflow-y-auto">
        <div class="grid grid-cols-2 gap-4">
          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Урок *</label>
            <select
              v-model.number="form.lessonId"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            >
              <option value="" disabled>Выберите урок</option>
              <option v-for="lesson in lessons" :key="lesson.id" :value="lesson.id">
                {{ lesson.title }}
              </option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-semibold text-highlighted mb-2">Тип задания *</label>
            <select
              v-model="form.type"
              @change="form.type === 'Quiz' ? (form.quizOptions = form.quizOptions || []) : null"
              class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
            >
              <option value="">Выберите тип</option>
              <option value="Quiz">Тест (множественный выбор)</option>
              <option value="JavaScript">JavaScript Задача</option>
              <option value="Python">Python Задача</option>
            </select>
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Название *</label>
          <input
            v-model="form.title"
            type="text"
            placeholder="Название задания"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
          />
        </div>

        <div>
          <label class="block text-sm font-semibold text-highlighted mb-2">Содержание (Markdown) *</label>
          <AdminMarkdownEditor
            v-model="form.markdownContent"
            label=""
            placeholder="# Описание задания&#10;Что нужно сделать пользователю?"
          />
        </div>

        <!-- Quiz Options Section -->
        <div v-if="form.type === 'Quiz'" class="bg-accented rounded-lg p-6 border border-default">
          <div class="flex justify-between items-center mb-4">
            <h3 class="text-lg font-bold text-highlighted">Варианты ответов</h3>
            <button
              @click="addQuizOption"
              class="px-4 py-2 bg-green-600 hover:bg-green-700 text-highlighted rounded-lg transition font-semibold text-sm"
            >
              + Добавить вариант
            </button>
          </div>

          <div class="space-y-4">
            <div v-for="(option, index) in form.quizOptions" :key="index" class="bg-elevated p-4 rounded-lg border border-default">
              <div class="grid grid-cols-3 gap-4 items-end">
                <div class="col-span-2">
                  <label class="block text-xs font-semibold text-default mb-2">Вариант {{ index + 1 }}</label>
                  <input
                    v-model="form.quizOptions[index].text"
                    type="text"
                    placeholder="Текст варианта ответа"
                    class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition"
                  />
                </div>

                <div class="flex items-center space-x-2">
                  <label class="flex items-center space-x-2 cursor-pointer">
                    <input
                      type="radio"
                      :value="index"
                      v-model.number="form.correctAnswerIndex"
                      class="w-4 h-4"
                    />
                    <span class="text-sm text-highlighted font-semibold">Правильный</span>
                  </label>

                  <button
                    v-if="form.quizOptions.length > 2"
                    @click="removeQuizOption(index)"
                    class="ml-auto px-3 py-1 bg-red-600 hover:bg-red-700 text-highlighted rounded text-sm transition"
                  >
                    Удалить
                  </button>
                </div>
              </div>
            </div>

            <div v-if="form.quizOptions.length < 2" class="text-muted text-sm italic">
              Добавьте минимум 2 варианта ответа
            </div>
          </div>
        </div>

        <!-- Code Challenge Section -->
        <div v-if="form.type === 'JavaScript' || form.type === 'Python'">
          <label class="block text-sm font-semibold text-highlighted mb-2">Ожидаемый ответ/вывод *</label>
          <textarea
            v-model="form.expectedAnswer"
            placeholder="Для задач на код: ожидаемый вывод или код"
            rows="4"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition font-mono text-sm"
          />
        </div>

        <div v-if="form.type === 'JavaScript' || form.type === 'Python'">
          <label class="block text-sm font-semibold text-highlighted mb-2">Начальный шаблон кода (опционально)</label>
          <textarea
            v-model="form.initialCode"
            :placeholder="form.type === 'JavaScript' ? '// Начните писать код здесь...' : '# Начните писать код здесь...'"
            rows="4"
            class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition font-mono text-sm"
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
          class="px-6 py-2 bg-purple-600 hover:bg-purple-700 disabled:bg-slate-600 text-highlighted rounded-lg transition font-semibold"
        >
          {{ isSaving ? 'Сохранение...' : isEditing ? 'Обновить' : 'Создать' }}
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface QuizOption {
  text: string
}

const props = defineProps<{
  task?: {
    id: number
    title: string
    type: 'Quiz' | 'JavaScript' | 'Python'
    lessonId: number
    markdownContent: string
    expectedAnswer: string
    initialCode?: string
    quizOptions?: QuizOption[]
    correctAnswerIndex?: number
  } | null
  lessons: Array<{
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
  lessonId: props.task?.lessonId || null,
  type: props.task?.type || '',
  title: props.task?.title || '',
  markdownContent: props.task?.markdownContent || '',
  expectedAnswer: props.task?.expectedAnswer || '',
  initialCode: props.task?.initialCode || '',
  quizOptions: props.task?.quizOptions && props.task.quizOptions.length > 0 
    ? props.task.quizOptions.map(opt => ({ text: opt.text }))
    : [
        { text: '' },
        { text: '' }
      ],
  correctAnswerIndex: props.task?.correctAnswerIndex !== undefined ? props.task.correctAnswerIndex : 0
})

const addQuizOption = () => {
  form.value.quizOptions.push({ text: '' })
}

const removeQuizOption = (index: number) => {
  form.value.quizOptions.splice(index, 1)
  if (form.value.correctAnswerIndex >= form.value.quizOptions.length) {
    form.value.correctAnswerIndex = form.value.quizOptions.length - 1
  }
}

const submitForm = async () => {
  const requiredFields = [
    form.value.lessonId,
    form.value.type,
    form.value.title,
    form.value.markdownContent
  ]

  // Специальная проверка для Quiz
  if (form.value.type === 'Quiz') {
    const hasValidOptions = form.value.quizOptions.every(opt => opt.text.trim())
    if (!hasValidOptions || form.value.quizOptions.length < 2) {
      alert('Для теста необходимо минимум 2 варианта ответов')
      return
    }
    if (form.value.correctAnswerIndex === undefined || form.value.correctAnswerIndex < 0) {
      alert('Выберите правильный ответ')
      return
    }
  } else {
    // Для кода обязателен ожидаемый результат
    requiredFields.push(form.value.expectedAnswer)
  }

  if (!requiredFields.every(field => field)) {
    alert('Пожалуйста, заполните все обязательные поля')
    return
  }

  isSaving.value = true
  try {
    const data: any = {
      lessonId: form.value.lessonId,
      type: form.value.type,
      title: form.value.title,
      markdownContent: form.value.markdownContent,
      initialCode: form.value.initialCode || undefined
    }

    if (form.value.type === 'Quiz') {
      data.quizOptions = form.value.quizOptions
      data.correctAnswerIndex = form.value.correctAnswerIndex
      // Для теста expectedAnswer = правильный ответ
      data.expectedAnswer = form.value.quizOptions[form.value.correctAnswerIndex].text
    } else {
      data.expectedAnswer = form.value.expectedAnswer
    }

    emit('save', data)
  } finally {
    isSaving.value = false
  }
}
</script>
