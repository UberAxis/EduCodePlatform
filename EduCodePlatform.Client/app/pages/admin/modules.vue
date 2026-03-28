<template>
  <div class="p-8">
    <!-- Header -->
    <div class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-highlighted">Управление модулями</h1>
        <p class="text-muted mt-2">Создавайте и управляйте модулями курса</p>
      </div>
      <button
        @click="showCreateModal = true"
        class="bg-blue-600 hover:bg-blue-700 text-highlighted px-6 py-3 rounded-lg transition font-semibold"
      >
        + Новый модуль
      </button>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-12">
      <div class="text-muted">Загрузка модулей...</div>
    </div>

    <!-- Error -->
    <div v-if="error" class="bg-error/10 border border-error/30 text-error px-4 py-3 rounded-lg mb-6">
      {{ error }}
    </div>

    <!-- Modules Table -->
    <div v-if="!loading && modules.length > 0" class="overflow-x-auto">
      <table class="w-full">
        <thead class="bg-elevated border-b border-default">
          <tr>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Название</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Описание</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Уроки</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Порядок</th>
            <th class="px-6 py-3 text-right text-highlighted font-semibold">Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="module in modules" :key="module.id" class="border-b border-default hover:bg-elevated transition">
            <td class="px-6 py-4 text-highlighted font-medium">{{ module.title }}</td>
            <td class="px-6 py-4 text-default max-w-xs truncate">{{ module.description }}</td>
            <td class="px-6 py-4 text-default">{{ module.lessons?.length || 0 }}</td>
            <td class="px-6 py-4 text-default">{{ module.orderIndex }}</td>
            <td class="px-6 py-4 text-right space-x-3">
              <button
                @click="editModule(module)"
                class="text-primary hover:text-blue-300 transition text-sm font-semibold"
              >
                Редактировать
              </button>
              <button
                @click="deleteModule(module.id)"
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
    <div v-if="!loading && modules.length === 0" class="text-center py-12">
      <div class="text-muted">Модули еще не созданы. Создайте первый модуль для начала.</div>
    </div>

    <!-- Create/Edit Modal -->
    <AdminModuleModal
      v-if="showCreateModal || showEditModal"
      :module="editingModule"
      :is-editing="showEditModal"
      @close="closeModal"
      @save="saveModule"
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
  description: string
  coverImage: string
  orderIndex: number
  lessons?: Array<{
    id: number
    title: string
  }>
}

const modules = ref<Module[]>([])
const loading = ref(false)
const error = ref('')
const showCreateModal = ref(false)
const showEditModal = ref(false)
const editingModule = ref<Module | null>(null)

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

onMounted(async () => {
  await fetchModules()
})

const fetchModules = async () => {
  loading.value = true
  error.value = ''
  try {
    modules.value = await $fetch<Module[]>(`${API_URL}/module`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Failed to load modules'
  } finally {
    loading.value = false
  }
}

const editModule = (module: Module) => {
  editingModule.value = module
  showEditModal.value = true
}

const closeModal = () => {
  showCreateModal.value = false
  showEditModal.value = false
  editingModule.value = null
}

const saveModule = async (data: any) => {
  try {
    if (showEditModal.value && editingModule.value) {
      await $fetch(`${API_URL}/module/${editingModule.value.id}`, {
        method: 'PUT',
        body: data,
        credentials: 'include',
      })
    } else {
      await $fetch(`${API_URL}/module`, {
        method: 'POST',
        body: data,
        credentials: 'include',
      })
    }
    await fetchModules()
    closeModal()
  } catch (e: any) {
    error.value = e.message || 'Failed to save module'
  }
}

const deleteModule = async (id: number) => {
  if (confirm('Are you sure you want to delete this module?')) {
    try {
      await $fetch(`${API_URL}/module/${id}`, {
        method: 'DELETE',
        credentials: 'include',
      })
      await fetchModules()
    } catch (e: any) {
      error.value = e.message || 'Failed to delete module'
    }
  }
}
</script>
