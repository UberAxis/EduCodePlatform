// stores/auth.ts
import { defineStore } from 'pinia'

// ========================= MOCK =========================
// Тестовый пользователь для моковой авторизации
const MOCK_USER = { id: 1, name: 'testuser', password: 'test123', xp: 120, level: 2, role: 'student' }
// ========================================================

export interface AuthUser {
  id: number
  name: string
  xp: number
  role: string
  level: number
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<AuthUser | null>(null)
  const isAuthenticated = computed(() => !!user.value)

  // ========================= MOCK =========================
  async function login(name: string, password: string): Promise<void> {
    // Имитация задержки сети
    await new Promise(resolve => setTimeout(resolve, 600))

    if (name === MOCK_USER.name && password === MOCK_USER.password) {
      user.value = { id: MOCK_USER.id, name: MOCK_USER.name, xp: MOCK_USER.xp, level: MOCK_USER.level, role: MOCK_USER.role }
      return
    }
    throw new Error('Неверное имя пользователя или пароль')
  }
  // ========================================================

  // ----- РЕАЛЬНЫЙ login (раскомментировать когда бэк готов) -----
  // async function login(name: string, password: string): Promise<void> {
  //   const res = await fetch('http://localhost:5145/api/User/login', {
  //     method: 'POST',
  //     headers: { 'Content-Type': 'application/json' },
  //     credentials: 'include', // важно для HttpOnly куки
  //     body: JSON.stringify({ name, password }),
  //   })
  //   if (res.status === 401) throw new Error('Неверное имя пользователя или пароль')
  //   if (!res.ok) throw new Error('Ошибка сервера. Попробуйте позже')
  //   const data = await res.json()
  //   // user.value = { id: data.id, name: data.name, role: data.role, xp: data.xp, level: data.level }

  // }
  // ---------------------------------------------------------------

  // ========================= MOCK =========================
  async function register(name: string, password: string): Promise<void> {
    await new Promise(resolve => setTimeout(resolve, 600))

    if (name === MOCK_USER.name) {
      throw new Error('Пользователь с таким именем уже существует')
    }
    // После "регистрации" сразу логиним
    user.value = { id: 0, name, xp: 0, level: 1, role: 'student' }
  }
  // ========================================================

  // ----- РЕАЛЬНЫЙ register (раскомментировать когда бэк готов) -----
  // async function register(name: string, password: string): Promise<void> {
  //   const res = await fetch('http://localhost:5145/api/User/register', {
  //     method: 'POST',
  //     headers: { 'Content-Type': 'application/json' },
  //     body: JSON.stringify({ name, password }),
  //   })
  //   if (res.status === 409) throw new Error('Пользователь с таким именем уже существует')
  //   if (!res.ok) throw new Error('Ошибка сервера. Попробуйте позже')
  //   // После регистрации сразу логиним
  //   await login(name, password)
  // }
  // -----------------------------------------------------------------

  async function logout(): Promise<void> {
    // ========================= MOCK =========================
    user.value = null
    // ========================================================

    // ----- РЕАЛЬНЫЙ logout (раскомментировать когда бэк готов) -----
    // await fetch('http://localhost:5145/api/User/logout', {
    //   method: 'POST',
    //   credentials: 'include',
    // })
    // user.value = null
    // ---------------------------------------------------------------
  }

  return { user, isAuthenticated, login, register, logout }
})
