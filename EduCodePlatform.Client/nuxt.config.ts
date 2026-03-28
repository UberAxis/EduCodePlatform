export default defineNuxtConfig({
  future: {
    compatibilityVersion: 4,
  },


  imports: {
    dirs: ['stores'], // ← авто-импорт сторов
  },



  // css: ['./app/assets/css/main.css'],
  css: ['./app/assets/css/prose-markdown.css'],
  modules: ['@nuxt/ui', '@pinia/nuxt'],
  
  ssr: true,
  devtools: { enabled: true },

  runtimeConfig: {
    public: {
      apiBase: 'http://localhost:8888/api'
    }
  },
  
  compatibilityDate: '2024-11-01'
})
