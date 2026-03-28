export const useCustomFetch = () => {
  const config = useRuntimeConfig()
  
  return $fetch.create({
    baseURL: config.public.apiBase,
    credentials: 'include'
  })
}
