<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-2xl mx-auto flex flex-col gap-6">
      <div class="flex flex-col gap-2">
        <UButton
          label="Назад к модулям"
          icon="i-lucide-arrow-left"
          variant="ghost"
          color="neutral"
          to="/lessons"
          class="self-start"
        />
        <h1 class="text-3xl font-extrabold text-highlighted">Рейтинг учеников</h1>
        <p class="text-muted">Сравни свои успехи с другими — опыт обновляется после заданий.</p>
      </div>

      <UCard>
        <div v-if="pending" class="flex flex-col gap-3 py-4">
          <USkeleton v-for="i in 6" :key="i" class="h-12 w-full" />
        </div>
        <UAlert
          v-else-if="error"
          color="error"
          variant="soft"
          icon="i-lucide-circle-x"
          title="Не удалось загрузить рейтинг"
          :description="error"
        />
        <ol v-else class="flex flex-col divide-y divide-default">
          <li
            v-for="(row, idx) in rows"
            :key="row.id"
            class="flex items-center gap-4 py-4 first:pt-0"
          >
            <span
              class="size-10 rounded-full flex items-center justify-center font-extrabold text-lg shrink-0"
              :class="medalClass(idx)"
            >
              {{ idx + 1 }}
            </span>
            <div class="flex-1 min-w-0">
              <p class="font-bold text-highlighted truncate">{{ row.userName }}</p>
              <p class="text-sm text-muted">
                Ур. {{ row.level }} · уроков {{ row.lessonsCompleted ?? 0 }}
              </p>
            </div>
            <span class="font-extrabold text-primary tabular-nums">{{ row.experiencePoints }} XP</span>
          </li>
        </ol>
      </UCard>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({ middleware: 'auth' })

const config = useRuntimeConfig()

type Row = {
  id: string
  userName: string
  experiencePoints: number
  level: number
  lessonsCompleted?: number
}

const rows = ref<Row[]>([])
const pending = ref(true)
const error = ref<string | null>(null)

function medalClass(index: number) {
  if (index === 0) return 'bg-amber-400/20 text-amber-600 dark:text-amber-400'
  if (index === 1) return 'bg-slate-300/30 text-slate-700 dark:text-slate-300'
  if (index === 2) return 'bg-orange-400/20 text-orange-700 dark:text-orange-400'
  return 'bg-muted text-muted'
}

onMounted(async () => {
  pending.value = true
  error.value = null
  try {
    const data = await $fetch<any[]>(`${config.public.apiBase}/User/leaderboard?count=25`, {
      credentials: 'include',
    })
    rows.value = data.map((u) => ({
      id: u.id,
      userName: u.userName,
      experiencePoints: u.experiencePoints ?? 0,
      level: u.level ?? 1,
      lessonsCompleted: u.lessonsCompletedCount ?? 0,
    }))
  } catch {
    error.value = 'Попробуй обновить страницу или зайти позже.'
  } finally {
    pending.value = false
  }
})
</script>
