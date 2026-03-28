<template>
  <div class="p-8">
    <!-- Header -->
    <div class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-highlighted">Управление уроками</h1>
        <p class="text-muted mt-2">Создавайте и управляйте уроками в модулях</p>
      </div>
      <button
        @click="showCreateModal = true"
        class="bg-green-600 hover:bg-green-700 text-highlighted px-6 py-3 rounded-lg transition font-semibold"
      >
        + Новый урок
      </button>
    </div>

    <!-- Module Filter -->
    <div class="mb-6 flex space-x-4">
      <div>
        <label class="block text-sm font-semibold text-highlighted mb-2">Фильтр по модулю:</label>
        <select
          v-model="selectedModuleId"
          class="px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500"
        >
          <option :value="null">Все модули</option>
          <option v-for="module in modules" :key="module.id" :value="module.id">
            {{ module.title }}
          </option>
        </select>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-12">
      <div class="text-muted">Загрузка уроков...</div>
    </div>

    <!-- Error -->
    <div v-if="error" class="bg-error/10 border border-error/30 text-error px-4 py-3 rounded-lg mb-6">
      {{ error }}
    </div>

    <!-- Lessons Table -->
    <div v-if="!loading && filteredLessons.length > 0" class="overflow-x-auto">
      <table class="w-full">
        <thead class="bg-elevated border-b border-default">
          <tr>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Название</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Модуль</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Задания</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Порядок</th>
            <th class="px-6 py-3 text-right text-highlighted font-semibold">Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="lesson in filteredLessons" :key="lesson.id" class="border-b border-default hover:bg-elevated transition">
            <td class="px-6 py-4 text-highlighted font-medium">{{ lesson.title }}</td>
            <td class="px-6 py-4 text-default">{{ getModuleTitle(lesson.moduleId) }}</td>
            <td class="px-6 py-4 text-default">{{ lesson.lessonTasks?.length || 0 }}</td>
            <td class="px-6 py-4 text-default">{{ lesson.orderIndex }}</td>
            <td class="px-6 py-4 text-right space-x-3">
              <button
                @click="editLesson(lesson)"
                class="text-primary hover:text-blue-300 transition text-sm font-semibold"
              >
                Редактировать
              </button>
              <button
                @click="deleteLesson(lesson.id)"
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
    <div v-if="!loading && filteredLessons.length === 0" class="text-center py-12">
      <div class="text-muted">Уроки не найдены.</div>
    </div>

    <!-- Create/Edit Modal -->
    <AdminLessonModal
      v-if="showCreateModal || showEditModal"
      :lesson="editingLesson"
      :modules="modules"
      :is-editing="showEditModal"
      @close="closeModal"
      @save="saveLesson"
    />
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: "admin",
  middleware: "admin-only"
})

interface Module {
  id: number
  title: string
}

interface Lesson {
  id: number
  title: string
  moduleId: number
  orderIndex: number
  markdownContent: string
  coverImage: string
  lessonTasks?: Array<{
    id: number
    title: string
  }>
}

const lessons = ref<Lesson[]>([])
const modules = ref<Module[]>([])
const loading = ref(false)
const error = ref('')
const selectedModuleId = ref<number | null>(null)
const showCreateModal = ref(false)
const showEditModal = ref(false)
const editingLesson = ref<Lesson | null>(null)

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

const filteredLessons = computed(() => {
  if (!selectedModuleId.value) return lessons.value
  return lessons.value.filter(l => l.moduleId === selectedModuleId.value)
})

onMounted(async () => {
  await fetchLessons()
  await fetchModules()
})

const fetchLessons = async () => {
  loading.value = true
  error.value = ''
  try {
    lessons.value = await $fetch<Lesson[]>(`${API_URL}/lesson`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load lessons'
  } finally {
    loading.value = false
  }
}

const fetchModules = async () => {
  try {
    modules.value = await $fetch<Module[]>(`${API_URL}/module`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load modules'
  }
}

const getModuleTitle = (moduleId: number) => {
  return modules.value.find(m => m.id === moduleId)?.title || 'Unknown'
}

const editLesson = (lesson: Lesson) => {
  editingLesson.value = lesson
  showEditModal.value = true
}

const closeModal = () => {
  showCreateModal.value = false
  showEditModal.value = false
  editingLesson.value = null
}

const saveLesson = async (data: any) => {
  try {
    if (showEditModal.value && editingLesson.value) {
      await $fetch(`${API_URL}/lesson/${editingLesson.value.id}`, {
        method: 'PUT',
        body: data,
        credentials: 'include',
      })
    } else {
      await $fetch(`${API_URL}/lesson`, {
        method: 'POST',
        body: data,
        credentials: 'include',
      })
    }
    await fetchLessons()
    closeModal()
  } catch (e: any) {
    error.value = e.message || 'Failed to save lesson'
  }
}

const deleteLesson = async (id: number) => {
  if (confirm('Are you sure you want to delete this lesson?')) {
    try {
      await $fetch(`${API_URL}/lesson/${id}`, {
        method: 'DELETE',
        credentials: 'include',
      })
      await fetchLessons()
    } catch (e: any) {
      error.value = e.message || 'Failed to delete lesson'
    }
  }
}
</script>
