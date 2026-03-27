export default defineNuxtConfig({
  future: {
    compatibilityVersion: 4,
  },


  imports: {
    dirs: ['stores'], // ← авто-импорт сторов
  },



  // css: ['./app/assets/css/main.css'],
  modules: ['@nuxt/ui', '@pinia/nuxt'],
  
  ssr: true,
  devtools: { enabled: true },
  
  compatibilityDate: '2024-11-01'
})
