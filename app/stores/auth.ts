import { defineStore } from 'pinia'

export interface AuthUser {
  id: number
  userName: string
  xp: number
  level: number
  role: string
}

export const useAuthStore = defineStore('auth', () => {
  const userCookie = useCookie<AuthUser | null>('auth_user', {
    maxAge: 60 * 60 * 24 * 7,
    path: '/'
  })

  const _user = ref<AuthUser | null>(userCookie.value || null)
  
  const isMock = ref(true) // ЯДЕРНАЯ КНОПКА переключает мока на реальный АПИ

  const API_URL = 'http://localhost:5145/api'

  const MOCK_DATA: AuthUser = { 
    id: 1, 
    userName: 'Junior_Coder', 
    xp: 150, 
    level: 2, 
    role: 'User' 
  }

  const user = computed(() => {
    if (!_user.value) return null
    return {
      ..._user.value,
      name: _user.value.userName 
    }
  })

  const username = computed(() => _user.value?.userName)
  const isAuthenticated = computed(() => !!_user.value)

  async function login(userName: string, password: string): Promise<void> {
    if (isMock.value) {
      await new Promise(res => setTimeout(res, 500))
      const mockUser = { ...MOCK_DATA, userName }
      _user.value = mockUser
      userCookie.value = mockUser
      return
    }

    try {
      const data = await $fetch<any>(`${API_URL}/User/login`, {
        method: 'POST',
        body: { userName, password },
      })
      
      const loggedUser = {
        id: data.id,
        userName: data.userName,
        xp: data.xp || 0,
        level: data.level || 1,
        role: data.role || 'User'
      }

      _user.value = loggedUser
      userCookie.value = loggedUser
    } catch (err: any) {
      if (err.status === 401) throw new Error('Неверный логин или пароль')
      throw new Error('Ошибка сервера')
    }
  }

  async function register(userName: string, password: string): Promise<void> {
    if (isMock.value) {
      await new Promise(res => setTimeout(res, 500))
      await login(userName, password)
      return
    }

    try {
      await $fetch(`${API_URL}/User/register`, {
        method: 'POST',
        body: { userName, password },
      })
      await login(userName, password)
    } catch (err: any) {
      if (err.status === 409) throw new Error('Логин уже занят')
      throw new Error('Ошибка регистрации')
    }
  }

  function logout() {
    _user.value = null
    userCookie.value = null
    if (!isMock.value) {
      $fetch(`${API_URL}/User/logout`, { method: 'POST' }).catch(() => {})
    }
    navigateTo('/login')
  }

  return { 
    user, 
    username, 
    isAuthenticated, 
    login, 
    register, 
    logout, 
    isMock 
  }
})