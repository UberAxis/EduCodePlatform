<template>
  <div class="p-8">
    <!-- Header -->
    <div class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-highlighted">Управление заданиями</h1>
        <p class="text-muted mt-2">Создавайте и управляйте заданиями уроков (тесты и задачи кодирования)</p>
      </div>
      <button
        @click="showCreateModal = true"
        class="bg-purple-600 hover:bg-purple-700 text-highlighted px-6 py-3 rounded-lg transition font-semibold"
      >
        + Новое задание
      </button>
    </div>

    <!-- Filters -->
    <div class="mb-6 grid grid-cols-2 gap-4">
      <div>
        <label class="block text-sm font-semibold text-highlighted mb-2">Фильтр по уроку:</label>
        <select
          v-model="selectedLessonId"
          class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500"
        >
          <option :value="null">Все уроки</option>
          <option v-for="lesson in lessons" :key="lesson.id" :value="lesson.id">
            {{ lesson.title }}
          </option>
        </select>
      </div>

      <div>
        <label class="block text-sm font-semibold text-highlighted mb-2">Фильтр по типу:</label>
        <select
          v-model="selectedTaskType"
          class="w-full px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500"
        >
          <option :value="null">Все типы</option>
          <option value="Quiz">Тест</option>
          <option value="JavaScript">JavaScript</option>
          <option value="Python">Python</option>
        </select>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-12">
      <div class="text-muted">Загрузка заданий...</div>
    </div>

    <!-- Error -->
    <div v-if="error" class="bg-error/10 border border-error/30 text-error px-4 py-3 rounded-lg mb-6">
      {{ error }}
    </div>

    <!-- Tasks Table -->
    <div v-if="!loading && filteredTasks.length > 0" class="overflow-x-auto">
      <table class="w-full">
        <thead class="bg-elevated border-b border-default">
          <tr>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Название</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Тип</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Урок</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Предпросмотр ответа</th>
            <th class="px-6 py-3 text-right text-highlighted font-semibold">Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="task in filteredTasks" :key="task.id" class="border-b border-default hover:bg-elevated transition">
            <td class="px-6 py-4 text-highlighted font-medium">{{ task.title }}</td>
            <td class="px-6 py-4">
              <span
                :class="[
                  'px-3 py-1 rounded-full text-sm font-semibold',
                  task.type === 'Quiz'
                    ? 'bg-primary/15 text-primary'
                    : task.type === 'JavaScript'
                    ? 'bg-warning/15 text-warning'
                    : 'bg-success/15 text-success'
                ]"
              >
                {{ task.type }}
              </span>
            </td>
            <td class="px-6 py-4 text-default">{{ getLessonTitle(task.lessonId) }}</td>
            <td class="px-6 py-4 text-muted text-sm max-w-xs truncate">
              <span v-if="task.type === 'Quiz'">
                {{ task.quizOptions?.length || 0 }} вариантов
              </span>
              <span v-else>
                {{ task.expectedAnswer.substring(0, 30) }}{{ task.expectedAnswer.length > 30 ? '...' : '' }}
              </span>
            </td>
            <td class="px-6 py-4 text-right space-x-3">
              <button
                @click="editTask(task)"
                class="text-primary hover:text-blue-300 transition text-sm font-semibold"
              >
                Редактировать
              </button>
              <button
                @click="deleteTask(task.id)"
                class="text-error hover:text-red-300 transition text-sm font-semibold"
              >
                Удалить
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Empty State -->
    <div v-if="!loading && filteredTasks.length === 0" class="text-center py-12">
      <div class="text-muted">Задания не найдены.</div>
    </div>

    <!-- Create/Edit Modal -->
    <AdminTaskModal
      v-if="showCreateModal || showEditModal"
      :task="editingTask"
      :lessons="lessons"
      :is-editing="showEditModal"
      @close="closeModal"
      @save="saveTask"
    />
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: "admin",
  middleware: "admin-only"
})

interface Lesson {
  id: number
  title: string
}

interface QuizOption {
  text: string
}

interface Task {
  id: number
  title: string
  type: 'Quiz' | 'JavaScript' | 'Python'
  lessonId: number
  expectedAnswer: string
  markdownContent: string
  initialCode?: string
  quizOptions?: QuizOption[]
  correctAnswerIndex?: number
}

const tasks = ref<Task[]>([])
const lessons = ref<Lesson[]>([])
const loading = ref(false)
const error = ref('')
const selectedLessonId = ref<number | null>(null)
const selectedTaskType = ref<string | null>(null)
const showCreateModal = ref(false)
const showEditModal = ref(false)
const editingTask = ref<Task | null>(null)

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

const filteredTasks = computed(() => {
  return tasks.value.filter(t => {
    const lessonMatch = !selectedLessonId.value || t.lessonId === selectedLessonId.value
    const typeMatch = !selectedTaskType.value || t.type === selectedTaskType.value
    return lessonMatch && typeMatch
  })
})

onMounted(async () => {
  await fetchTasks()
  await fetchLessons()
})

const fetchTasks = async () => {
  loading.value = true
  error.value = ''
  try {
    tasks.value = await $fetch<Task[]>(`${API_URL}/task`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load tasks'
  } finally {
    loading.value = false
  }
}

const fetchLessons = async () => {
  try {
    lessons.value = await $fetch<Lesson[]>(`${API_URL}/lesson`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load lessons'
  }
}

const getLessonTitle = (lessonId: number) => {
  return lessons.value.find(l => l.id === lessonId)?.title || 'Unknown'
}

const editTask = (task: Task) => {
  editingTask.value = task
  showEditModal.value = true
}

const closeModal = () => {
  showCreateModal.value = false
  showEditModal.value = false
  editingTask.value = null
}

const saveTask = async (data: any) => {
  try {
    if (showEditModal.value && editingTask.value) {
      await $fetch(`${API_URL}/task/${editingTask.value.id}`, {
        method: 'PUT',
        body: data,
        credentials: 'include',
      })
    } else {
      await $fetch(`${API_URL}/task`, {
        method: 'POST',
        body: data,
        credentials: 'include',
      })
    }
    await fetchTasks()
    closeModal()
  } catch (e: any) {
    error.value = e.message || 'Failed to save task'
  }
}

const deleteTask = async (id: number) => {
  if (confirm('Are you sure you want to delete this task?')) {
    try {
      await $fetch(`${API_URL}/task/${id}`, {
        method: 'DELETE',
        credentials: 'include',
      })
      await fetchTasks()
    } catch (e: any) {
      error.value = e.message || 'Failed to delete task'
    }
  }
}
</script>
