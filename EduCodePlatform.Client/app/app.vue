<script setup lang="ts">
import AppBreadcrumb from './components/layout/AppBreadcrumb.vue';
import AppFooter from './components/layout/AppFooter.vue';
import AppHeader from './components/layout/AppHeader.vue';

const auth = useAuthStore()

onMounted(() => {
  if (auth.isAuthenticated) {
    auth.fetchUser()
  }
})
</script>

<template>
  <UApp>
    <AppHeader />
    <AppBreadcrumb />
    <NuxtPage />
    <AppFooter />
    <UToaster />
  </UApp>
</template>

<style>
@import "tailwindcss";
@import "@nuxt/ui";
@plugin "@tailwindcss/typography";

@theme {
  --font-sans: 'Nunito', sans-serif;
}

:root {
  --ui-radius: 0.5rem;
}

:root, .light {
  --ui-primary: #AACA1C;
  --ui-secondary: var(--color-amber-500);
  --ui-success: var(--color-lime-500);
  --ui-warning: var(--color-amber-500);
  --ui-error: var(--color-rose-500);
  --ui-info: var(--color-cyan-500);
}

.dark {
  --ui-primary: #AACA1C;
  --ui-secondary: var(--color-amber-400);
  --ui-success: var(--color-lime-400);
  --ui-warning: var(--color-amber-400);
  --ui-error: var(--color-rose-400);
  --ui-info: var(--color-cyan-400);
}
</style>
