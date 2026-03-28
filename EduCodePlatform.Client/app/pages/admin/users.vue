<template>
  <div class="p-8">
    <!-- Header -->
    <div class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-highlighted">Управление пользователями</h1>
        <p class="text-muted mt-2">Просмотрите и управляйте пользователями платформы</p>
      </div>
    </div>

    <!-- Search & Filter -->
    <div class="mb-6 flex space-x-4">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Поиск по имени или email..."
        class="flex-1 px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500"
      />
      <select
        v-model="selectedRole"
        class="px-4 py-2 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500"
      >
        <option :value="null">Все роли</option>
        <option value="User">Пользователь</option>
        <option value="Admin">Админ</option>
      </select>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="text-center py-12">
      <div class="text-muted">Загрузка пользователей...</div>
    </div>

    <!-- Error -->
    <div v-if="error" class="bg-error/10 border border-error/30 text-error px-4 py-3 rounded-lg mb-6">
      {{ error }}
    </div>

    <!-- Users Table -->
    <div v-if="!loading && filteredUsers.length > 0" class="overflow-x-auto">
      <table class="w-full text-sm">
        <thead class="bg-elevated border-b border-default">
          <tr>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Полное имя</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Email</th>
            <th class="px-6 py-3 text-left text-highlighted font-semibold">Роль</th>
            <th class="px-6 py-3 text-center text-highlighted font-semibold">Уровень</th>
            <th class="px-6 py-3 text-center text-highlighted font-semibold">XP</th>
            <th class="px-6 py-3 text-center text-highlighted font-semibold">Задания</th>
            <th class="px-6 py-3 text-center text-highlighted font-semibold">Тесты</th>
            <th class="px-6 py-3 text-right text-highlighted font-semibold">Действия</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in filteredUsers" :key="user.id" class="border-b border-default hover:bg-elevated transition">
            <td class="px-6 py-4 text-highlighted font-medium">{{ user.fullName || user.email }}</td>
            <td class="px-6 py-4 text-default truncate">{{ user.email }}</td>
            <td class="px-6 py-4">
              <span
                :class="[
                  'px-3 py-1 rounded-full text-xs font-semibold',
                  user.role === 'Admin'
                    ? 'bg-error/15 text-error'
                    : 'bg-primary/15 text-primary'
                ]"
              >
                {{ user.role || 'Пользователь' }}
              </span>
            </td>
            <td class="px-6 py-4 text-center text-default">{{ user.level }}</td>
            <td class="px-6 py-4 text-center text-default">{{ user.experiencePoints }}</td>
            <td class="px-6 py-4 text-center text-default">{{ user.tasksCompletedCount }}</td>
            <td class="px-6 py-4 text-center text-default">{{ user.quizzesPassedCount }}</td>
            <td class="px-6 py-4 text-right space-x-2">
              <button
                @click="editUser(user)"
                class="text-primary hover:text-blue-300 transition text-xs font-semibold"
              >
                Редактировать
              </button>
              <button
                @click="deleteUser(user.id)"
                class="text-error hover:text-red-300 transition text-xs font-semibold"
              >
                Удалить
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Empty State -->
    <div v-if="!loading && filteredUsers.length === 0" class="text-center py-12">
      <div class="text-muted">Пользователи не найдены.</div>
    </div>

    <!-- Edit User Modal -->
    <AdminUserModal
      v-if="showEditModal"
      :user="editingUser"
      @close="closeModal"
      @save="saveUser"
    />
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: "admin",
  middleware: "admin-only"
})

interface User {
  id: string
  fullName: string | null
  email: string | null
  userName?: string
  role?: string
  level: number
  experiencePoints: number
  coins?: number
  tasksCompletedCount: number
  quizzesPassedCount: number
}

const users = ref<User[]>([])
const loading = ref(false)
const error = ref('')
const searchQuery = ref('')
const selectedRole = ref<string | null>(null)
const showEditModal = ref(false)
const editingUser = ref<User | null>(null)

const config = useRuntimeConfig()
const API_URL = config.public.apiBase

const filteredUsers = computed(() => {
  const q = searchQuery.value.toLowerCase()
  return users.value.filter(u => {
    const name = (u.fullName || '').toLowerCase()
    const email = (u.email || '').toLowerCase()
    const searchMatch = name.includes(q) || email.includes(q)
    const roleMatch = !selectedRole.value || u.role === selectedRole.value
    return searchMatch && roleMatch
  })
})

onMounted(async () => {
  await fetchUsers()
})

const fetchUsers = async () => {
  loading.value = true
  error.value = ''
  try {
    users.value = await $fetch<User[]>(`${API_URL}/user`, {
      credentials: 'include',
    })
  } catch (e: any) {
    error.value = e.message || 'Не удалось загрузить пользователей'
  } finally {
    loading.value = false
  }
}

const editUser = (user: User) => {
  editingUser.value = user
  showEditModal.value = true
}

const closeModal = () => {
  showEditModal.value = false
  editingUser.value = null
}

const saveUser = async (data: any) => {
  if (!editingUser.value) return

  try {
    await $fetch(`${API_URL}/user/admin/${editingUser.value.id}`, {
      method: 'PUT',
      body: data,
      credentials: 'include',
    })
    await fetchUsers()
    closeModal()
  } catch (e: any) {
    error.value = e.message || 'Не удалось сохранить пользователя'
  }
}

const deleteUser = async (id: string) => {
  if (confirm('Вы уверены, что хотите удалить этого пользователя?')) {
    try {
      await $fetch(`${API_URL}/user/${id}`, {
        method: 'DELETE',
        credentials: 'include',
      })
      await fetchUsers()
    } catch (e: any) {
      error.value = e.message || 'Не удалось удалить пользователя'
    }
  }
}
</script>
