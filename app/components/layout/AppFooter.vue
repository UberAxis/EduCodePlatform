<template>
  <UFooter :ui="{ root: 'border-t-0 shadow-[0_-2px_10px_rgba(0,0,0,0.05)]' }">

    <template #top>
      <UContainer>
        <div class="grid grid-cols-2 md:grid-cols-3 gap-4 pb-4">

          <div class="col-span-2 md:col-span-1 flex flex-col gap-3">
            <div class="flex items-center gap-2">
              <img src="/logo.png" alt="logo" class="size-10" />
              <span class="text-xl font-extrabold text-primary">Кодо<span class="text-secondary">Сфера</span></span>
            </div>
            <p class="text-md text-muted leading-relaxed">
              Учимся программировать весело и легко!
            </p>
          </div>

          <div class="flex flex-col">
            <p class="font-extrabold text-highlighted text-lg">Обучение</p>
            <UButton v-for="link in study" :key="link.to" :label="link.label" :to="link.to"
              variant="link" color="neutral" size="xl" class="justify-start px-0" :ui="{ base: 'py-1' }"/>
          </div>

          <div class="flex flex-col">
            <p class="font-extrabold text-highlighted text-lg">Аккаунт</p>
            <UButton v-for="link in account" :key="link.to" :label="link.label" :to="link.to"
              variant="link" color="neutral" size="xl" class="justify-start px-0" :ui="{ base: 'py-1' }"/>
          </div>

        </div>
      </UContainer>
    </template>

    <template #left>
      <p class="text-sm text-muted">© 2026 КодоСфера. Все права защищены</p>
    </template>

    <template #right>
      <div class="flex gap-4">
        <UButton label="Конфиденциальность" variant="link" color="neutral" size="sm" />
        <UButton label="Условия использования" variant="link" color="neutral" size="sm" />
      </div>
    </template>

  </UFooter>
</template>

<script setup lang="ts">
const study = [
  { label: 'Каталог модулей', to: '/lessons' },
]

const account = [
  { label: 'Профиль', to: '/profile' },
  { label: 'Войти', to: '/login' },
  { label: 'Регистрация', to: '/register' },
]
</script>
