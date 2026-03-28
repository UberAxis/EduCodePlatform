export default defineNuxtRouteMiddleware(async (to, from) => {
  const authStore = useAuthStore()

  // Ensure user is authenticated
  if (!authStore.isAuthenticated) {
    return navigateTo('/login')
  }

  // Ensure user is admin
  if (!authStore.isAdmin) {
    return navigateTo('/')
  }
})
