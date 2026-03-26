import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as any | null,
    loading: false
  }),

  getters: {
    isLoggedIn: (state) => !!state.user,
  },

  actions: {
    // Регистрация
    async register(credentials: any) {
      this.loading = true
      try {
        await $fetch('http://localhost:5145/api/User/register', {
          method: 'POST',
          body: credentials
        })
        // После регистрации сразу логиним
        return await this.login({ 
          name: credentials.name, 
          password: credentials.password 
        })
      } finally {
        this.loading = false
      }
    },

    // Вход
    async login(credentials: any) {
      this.loading = true
      try {
        // Бэкенд установит HttpOnly куку автоматически
        await $fetch('http://localhost:5145/api/User/login', {
          method: 'POST',
          body: credentials
        })
        
        // Получаем данные пользователя (нужен эндпоинт GET /api/User/me или аналогичный)
        // Если его нет, пока захардкодим данные из формы
        this.user = { name: credentials.name, xp: 0, level: 1, avatar: '👤' }
        navigateTo('/')
      } catch (e) {
        console.error('Ошибка входа:', e)
        throw e
      } finally {
        this.loading = false
      }
    },

    // Выход
    logout() {
      this.user = null
      // Здесь нужно вызвать эндпоинт бэка для удаления куки
      navigateTo('/login')
    }
  }
})