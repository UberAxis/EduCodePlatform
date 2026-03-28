import { defineStore } from 'pinia'

export interface UnlockedAchievement {
  achievementId: number
  title: string
  description: string
  iconUrl: string
  xpReward: number
  unlockedAt: string
}

export interface AuthUser {
  id: string
  userName: string
  fullName?: string | null
  email?: string | null
  xp: number
  level: number
  role: string
  lessonsCompleted?: number
  tasksCompleted?: number
  quizzesPassed?: number
  coins?: number
  achievements?: UnlockedAchievement[]
}

export const useAuthStore = defineStore('auth', () => {
  const userCookie = useCookie<AuthUser | null>('auth_user', {
    maxAge: 60 * 60 * 24 * 7,
    path: '/'
  })

  const _user = ref<AuthUser | null>(userCookie.value || null)
  
  const isMock = ref(false)

  const config = useRuntimeConfig()
  const API_URL = config.public.apiBase

  const MOCK_DATA: AuthUser = {
    id: '00000000-0000-0000-0000-000000000000',
    userName: 'Junior_Coder',
    xp: 150,
    level: 2,
    role: 'User',
    lessonsCompleted: 0,
    tasksCompleted: 0,
    quizzesPassed: 0,
    coins: 0,
    achievements: [],
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
  const isAdmin = computed(() => _user.value?.role === 'Admin')

  async function fetchUser(): Promise<void> {
    if (isMock.value) return

    try {
      const data = await $fetch<any>(`${API_URL}/User/me`, {
        credentials: 'include',
      })
      const updatedUser: AuthUser = {
        id: data.id,
        userName: data.userName,
        fullName: data.fullName,
        email: data.email,
        xp: data.experiencePoints || 0,
        level: data.level || 1,
        role: data.role || 'User',
        lessonsCompleted: data.lessonsCompletedCount ?? 0,
        tasksCompleted: data.tasksCompletedCount ?? 0,
        quizzesPassed: data.quizzesPassedCount ?? 0,
        coins: data.coins ?? 0,
        achievements: Array.isArray(data.unlockedAchievements)
          ? data.unlockedAchievements.map((x: any) => ({
              achievementId: x.achievementId,
              title: x.title,
              description: x.description,
              iconUrl: x.iconUrl,
              xpReward: x.xpReward ?? 0,
              unlockedAt: x.unlockedAt,
            }))
          : [],
      }
      _user.value = updatedUser
      userCookie.value = updatedUser
    } catch (err) {
      console.error('Failed to fetch user', err)
      logout()
    }
  }

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
        credentials: 'include',
      })
      
      // Сначала сохраняем базовые данные из ответа логина
      const loggedUser: AuthUser = {
        id: data.id,
        userName: data.userName,
        xp: data.experiencePoints || 0,
        level: data.level || 1,
        role: data.role || 'User',
        lessonsCompleted: 0,
        tasksCompleted: 0,
        quizzesPassed: 0,
        coins: 0,
        achievements: [],
      }

      _user.value = loggedUser
      userCookie.value = loggedUser

      // Затем подгружаем полные данные (fullName, email и т.д.)
      await fetchUser()
    } catch (err: any) {
      if (err.status === 401) throw new Error('Неверный логин или пароль')
      throw new Error('Ошибка сервера')
    }
  }

  async function register(userName: string, password: string, fullName?: string, email?: string): Promise<void> {
    if (isMock.value) {
      await new Promise(res => setTimeout(res, 500))
      await login(userName, password)
      return
    }

    try {
      await $fetch(`${API_URL}/User/register`, {
        method: 'POST',
        body: { userName, password, fullName, email },
        credentials: 'include',
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
      $fetch(`${API_URL}/User/logout`, {
        method: 'POST',
        credentials: 'include',
      }).catch(() => {})
    }
    navigateTo('/login')
  }

  return { 
    user, 
    username, 
    isAuthenticated,
    isAdmin, 
    login, 
    register, 
    logout, 
    fetchUser,
    isMock 
    }
    })