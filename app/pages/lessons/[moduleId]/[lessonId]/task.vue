<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-5xl mx-auto flex flex-col gap-6">

      <!-- Назад -->
      <UButton
        label="К уроку"
        icon="i-lucide-arrow-left"
        variant="ghost"
        color="neutral"
        :to="`/lessons/${moduleId}/${lessonId}`"
        class="self-start"
      />

      <!-- Не найдено -->
      <UCard v-if="!task">
        <div class="flex flex-col items-center gap-3 py-8 text-center">
          <UIcon name="i-lucide-frown" class="size-12 text-muted" />
          <p class="text-lg font-bold text-highlighted">Задание не найдено</p>
          <UButton label="Вернуться к уроку" :to="`/lessons/${moduleId}/${lessonId}`" />
        </div>
      </UCard>

      <template v-else>
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">

          <!-- Левая колонка — условие -->
          <div class="flex flex-col gap-4">
            <UCard>
              <div class="flex flex-col gap-4">
                <div class="flex items-start gap-3">
                  <div class="size-10 rounded-xl bg-primary/10 flex items-center justify-center shrink-0">
                    <UIcon name="i-lucide-target" class="size-5 text-primary" />
                  </div>
                  <div>
                    <h1 class="text-xl font-extrabold text-highlighted">{{ task.title }}</h1>
                    <p class="text-muted mt-1">{{ task.description }}</p>
                  </div>
                </div>

                <USeparator />

                <div class="flex items-center gap-2 text-sm text-muted">
                  <UIcon name="i-lucide-zap" class="size-4 text-primary" />
                  <span>Награда: <strong class="text-primary">+{{ task.xpReward }} XP</strong></span>
                </div>
              </div>
            </UCard>

            <!-- Результат выполнения -->
            <UCard v-if="result !== null">
              <template #header>
                <div class="flex items-center gap-2">
                  <UIcon
                    :name="result.success ? 'i-lucide-check-circle' : 'i-lucide-circle-x'"
                    :class="result.success ? 'text-success' : 'text-error'"
                    class="size-5"
                  />
                  <span class="font-bold" :class="result.success ? 'text-success' : 'text-error'">
                    {{ result.success ? 'Отлично! Всё работает' : 'Есть ошибка' }}
                  </span>
                </div>
              </template>
              <pre class="text-sm font-mono whitespace-pre-wrap break-all"
                :class="result.success ? 'text-success' : 'text-error'"
              >{{ result.output }}</pre>
            </UCard>
          </div>

          <!-- Правая колонка — редактор -->
          <div class="flex flex-col gap-4">
            <UiCodeEditor
              v-model="code"
              language="javascript"
              height="400px"
            />

            <div class="flex gap-3">
              <UButton
                label="Сбросить"
                icon="i-lucide-rotate-ccw"
                variant="ghost"
                color="neutral"
                size="xl"
                class="flex-1"
                @click="resetCode"
              />
              <UButton
                label="Запустить"
                icon="i-lucide-play"
                size="xl"
                class="flex-1"
                :loading="running"
                @click="runCode"
              />
            </div>

            <!-- Сдать задание -->
            <UButton
              v-if="result?.success && !isCompleted"
              label="Сдать задание и получить XP"
              icon="i-lucide-trophy"
              size="xl"
              class="w-full"
              color="success"
              :loading="submitting"
              @click="handleSubmit"
            />

            <UAlert
              v-if="isCompleted"
              color="success"
              variant="soft"
              icon="i-lucide-check-circle"
              title="Задание уже выполнено!"
              description="XP начислен. Можешь перейти к следующему уроку."
            />
          </div>

        </div>
      </template>

    </div>
  </div>
</template>

<script setup lang="ts">
import { useStudyStore } from '~/stores/study'
import { useAuthStore } from '~/stores/auth'

definePageMeta({ middleware: 'auth' })

const route = useRoute()
const study = useStudyStore()
const auth = useAuthStore()
const toast = useToast()

const moduleId = computed(() => String(route.params.moduleId))
const lessonId = computed(() => String(route.params.lessonId))
const currentLesson = computed(() => study.getLessonById(moduleId.value, lessonId.value))
const task = computed(() => currentLesson.value?.task ?? null)
const isCompleted = computed(() => currentLesson.value?.isCompleted ?? false)

const code = ref('')
const running = ref(false)
const submitting = ref(false)
const result = ref<{ success: boolean; output: string } | null>(null)

function resetCode() {
  code.value = task.value?.initialCode ?? ''
  result.value = null
}

// ========================= MOCK =========================
// Запуск кода через Function() — только для мока, небезопасно в продакшне
function runCode() {
  if (!code.value.trim()) return
  running.value = true
  result.value = null

  setTimeout(() => {
    try {
      const logs: string[] = []
      const fakeConsole = { log: (...args: unknown[]) => logs.push(args.join(' ')) }
      // eslint-disable-next-line no-new-func
      new Function('console', code.value)(fakeConsole)
      result.value = {
        success: true,
        output: logs.length ? logs.join('\n') : '✓ Код выполнен без ошибок',
      }
    } catch (e: unknown) {
      result.value = {
        success: false,
        output: e instanceof Error ? e.message : 'Неизвестная ошибка',
      }
    } finally {
      running.value = false
    }
  }, 500)
}
// ========================================================

// ----- РЕАЛЬНЫЙ runCode (раскомментировать когда бэк готов) -----
// async function runCode() {
//   if (!code.value.trim()) return
//   running.value = true
//   result.value = null
//   try {
//     const res = await fetch('http://localhost:5145/api/Task/run', {
//       method: 'POST',
//       headers: { 'Content-Type': 'application/json' },
//       credentials: 'include',
//       body: JSON.stringify({ code: code.value, taskId: task.value?.id }),
//     })
//     const data = await res.json()
//     result.value = { success: data.success, output: data.output }
//   } catch {
//     result.value = { success: false, output: 'Ошибка соединения с сервером' }
//   } finally {
//     running.value = false
//   }
// }
// ----------------------------------------------------------------

async function handleSubmit() {
  if (!task.value) return
  submitting.value = true
  try {
    await study.markLessonCompleted(moduleId.value, lessonId.value)

    // ========================= MOCK =========================
    if (auth.user) auth.user.xp += task.value.xpReward
    // ========================================================

    // ----- РЕАЛЬНЫЙ XP (раскомментировать когда бэк готов) -----
    // const res = await fetch(`http://localhost:5145/api/User/${auth.user?.id}`, {
    //   credentials: 'include',
    // })
    // const data = await res.json()
    // if (auth.user) { auth.user.xp = data.xp; auth.user.level = data.level }
    // ------------------------------------------------------------

    toast.add({
      title: `+${task.value.xpReward} XP получено! 🎉`,
      description: 'Задание выполнено, продолжай в том же духе!',
      color: 'success',
      icon: 'i-lucide-trophy',
    })
  } catch {
    toast.add({ title: 'Не удалось сохранить прогресс', color: 'error', icon: 'i-lucide-circle-x' })
  } finally {
    submitting.value = false
  }
}

onMounted(async () => {
  if (!study.modules.length) await study.fetchModules()
  resetCode()
})
</script>
