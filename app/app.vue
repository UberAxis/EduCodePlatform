<template>
  <UApp>
    <NuxtPage />
    <UToaster />
  </UApp>
</template>

<style>
@import "tailwindcss";
@import "@nuxt/ui";

@theme {
  --font-sans: 'Nunito', sans-serif;
}

:root {
  --ui-radius: 0.5rem;
  --ui-primary: var(--color-emerald-500);
  --ui-secondary: var(--color-amber-500);
  --ui-success: var(--color-lime-500);
  --ui-warning: var(--color-amber-500);
  --ui-error: var(--color-rose-500);
  --ui-info: var(--color-cyan-500);
}

.dark {
  --ui-primary: var(--color-emerald-400);
  --ui-secondary: var(--color-amber-400);
  --ui-success: var(--color-lime-400);
  --ui-warning: var(--color-amber-400);
  --ui-error: var(--color-rose-400);
  --ui-info: var(--color-cyan-400);
}
</style>

