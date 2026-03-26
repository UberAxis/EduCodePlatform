<template>
  <div class="flex flex-col items-center justify-center min-h-screen gap-4 text-2xl">
    <UChatShimmer text="Введите вашу пенсию..." />
    <UInputNumber v-model="pension" :min="0" :step="42" placeholder="0 ₽" />
  </div>
</template>

<script setup lang="ts">
const pension = ref(0)
</script>
