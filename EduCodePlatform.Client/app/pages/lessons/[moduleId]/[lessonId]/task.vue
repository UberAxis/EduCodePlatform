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
                    <div
                      class="prose dark:prose-invert prose-sm max-w-none mt-1 text-muted"
                      v-html="renderedTaskMarkdown"
                    />
                  </div>
                </div>

                <USeparator />

                <div class="flex items-center gap-2 text-sm text-muted">
                  <UIcon name="i-lucide-zap" class="size-4 text-primary" />
                  <span>Награда: <strong class="text-primary">+{{ task.xpReward ?? 10 }} XP</strong></span>
                </div>
              </div>
            </UCard>

            <!-- Результат выполнения (только для задач с кодом) -->
            <UCard v-if="!isQuizTask && result !== null">
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

          <!-- Правая колонка — ответ -->
          <div class="flex flex-col gap-4">
            <!-- Викторина: обычный текст -->
            <template v-if="isQuizTask">
              <UFormField
                label="Твой ответ"
                hint="Один текст; лишние пробелы по краям игнорируются, регистр не важен."
              >
                <UTextarea
                  v-model="code"
                  :rows="5"
                  autoresize
                  placeholder="Напиши ответ сюда…"
                  size="xl"
                  class="w-full"
                />
              </UFormField>
              <UButton
                v-if="!isCompleted"
                label="Отправить ответ"
                icon="i-lucide-send"
                size="xl"
                class="w-full"
                color="success"
                :loading="submitting"
                :disabled="!code.trim()"
                @click="handleSubmit"
              />
            </template>

            <!-- Код: Monaco + запуск -->
            <template v-else>
              <UiCodeEditor
                :key="editorMountKey"
                v-model="code"
                :language="monacoLanguage"
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
            </template>

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
import { markdownToHtml } from '~/composables/useMarkdownHtml'
import { useAuthStore } from '~/stores/auth'

definePageMeta({ middleware: 'auth' })

// Подключаем Pyodide для Python
useHead({
  script: [
    { src: 'https://cdn.jsdelivr.net/pyodide/v0.26.4/full/pyodide.js', defer: true }
  ]
})

const route = useRoute()
const study = useStudyStore()
const auth = useAuthStore()
const toast = useToast()

const moduleId = computed(() => String(route.params.moduleId))
const lessonId = computed(() => String(route.params.lessonId))
const currentLesson = computed(() => study.getLessonById(moduleId.value, lessonId.value))
const task = computed(() => currentLesson.value?.task ?? null)
const isCompleted = computed(() => currentLesson.value?.isCompleted ?? false)

// Если тип задачи Quiz — перенаправляем на страницу теста
watch(task, (t) => {
  if (t?.type === 'Quiz') {
    navigateTo(`/lessons/${moduleId.value}/${lessonId.value}/quiz`)
  }
}, { immediate: true })

const isQuizTask = computed(() => task.value?.type === 'Quiz')
const monacoLanguage = computed(() =>
  task.value?.type === 'Python' ? 'python' : 'javascript'
)
/** Пересоздаём редактор при смене урока или языка, иначе Monaco иногда не подхватывает language */
const editorMountKey = computed(
  () => `${task.value?.id ?? 'x'}-${monacoLanguage.value}`
)

const renderedTaskMarkdown = ref('')

watch(task, async (t) => {
  const md = (t?.markdownContent ?? t?.description ?? '').trim()
  renderedTaskMarkdown.value = md ? await markdownToHtml(md) : ''
}, { immediate: true })

const code = ref('')
const running = ref(false)
const submitting = ref(false)
const result = ref<{ success: boolean; output: string } | null>(null)

function resetCode() {
  code.value = task.value?.initialCode ?? ''
  result.value = null
}

/** Плейсхолдер, если не было console.log — должен совпадать с runJS */
const MSG_JS_NO_CONSOLE_OUTPUT = '✓ Код выполнен без ошибок'
/** Плейсхолдер, если не было print — должен совпадать с runPython */
const MSG_PY_NO_STDOUT = '✓ Python код выполнен'

// Раннер для JavaScript
async function runJS(userCode: string) {
  const logs: string[] = []
  const fakeConsole = { log: (...args: any[]) => logs.push(args.map(a => String(a)).join(' ')) }
  
  try {
    // Pass fakeConsole as a parameter so it is not referenced inside a string
    // (eval of that string can run in global scope and lose the lexical binding).
    const runner = new Function('console', userCode)
    runner(fakeConsole)
    return {
      success: true,
      output: logs.join('\n') || MSG_JS_NO_CONSOLE_OUTPUT
    }
  } catch (e: any) {
    return { success: false, output: e.message }
  }
}

// Раннер для Python
async function runPython(userCode: string) {
  try {
    // @ts-ignore
    if (!window.loadPyodide) throw new Error('Интерпретатор Python еще загружается...')
    
    // @ts-ignore
    const pyodide = await window.loadPyodide()
    
    // Перехват стандартного вывода Python
    let output = ''
    pyodide.setStdout({ batched: (str: string) => { output += str + '\n' } })
    
    await pyodide.runPythonAsync(userCode)
    
    return {
      success: true,
      output: output.trim() || MSG_PY_NO_STDOUT
    }
  } catch (e: any) {
    return { success: false, output: e.message }
  }
}

async function runCode() {
  if (!code.value.trim() || !task.value) return
  running.value = true
  result.value = null

  const taskType = task.value.type // JavaScript или Python

  if (taskType === 'JavaScript') {
    result.value = await runJS(code.value)
  } else if (taskType === 'Python') {
    result.value = await runPython(code.value)
  } else {
    result.value = { success: true, output: 'Эта задача не требует запуска кода' }
  }

  running.value = false
}

async function handleSubmit() {
  if (!task.value) return
  submitting.value = true
  try {
    const taskType = task.value.type
    // На сервере ExpectedAnswer — это текстовый ответ (вывод консоли / print или ответ викторины),
    // а не весь исходный код. Раньше отправлялся code.value целиком — сравнение всегда проваливалось.
    let answer: string

    if (taskType === 'JavaScript') {
      const run = await runJS(code.value)
      if (!run.success) {
        toast.add({
          title: 'Сначала исправь ошибки',
          description: run.output,
          color: 'error',
          icon: 'i-lucide-circle-x',
        })
        return
      }
      answer =
        run.output === MSG_JS_NO_CONSOLE_OUTPUT ? '' : run.output.trim()
    } else if (taskType === 'Python') {
      const run = await runPython(code.value)
      if (!run.success) {
        toast.add({
          title: 'Сначала исправь ошибки',
          description: run.output,
          color: 'error',
          icon: 'i-lucide-circle-x',
        })
        return
      }
      answer = run.output === MSG_PY_NO_STDOUT ? '' : run.output.trim()
    } else {
      answer = code.value.trim()
    }

    if (!answer) {
      toast.add({
        title: 'Нет ответа для проверки',
        description:
          taskType === 'Quiz'
            ? 'Введи ответ в поле ниже.'
            : 'Сделай вывод через console.log или print — по нему проверяется задание.',
        color: 'warning',
        icon: 'i-lucide-alert-triangle',
      })
      return
    }

    const success = await study.submitTask(Number(task.value.id), answer)

    if (success) {
      await study.markLessonCompleted(moduleId.value, lessonId.value)
      await auth.fetchUser()

      toast.add({
        title: `Задание выполнено! 🎉`,
        description: 'XP начислен, продолжай в том же духе!',
        color: 'success',
        icon: 'i-lucide-trophy',
      })
    } else {
      toast.add({
        title: 'Не совсем верно',
        description:
          taskType === 'Quiz'
            ? 'Ответ не совпадает с правильным.'
            : 'То, что выводит твой код, не совпадает с ожидаемым в условии (регистр не важен).',
        color: 'error',
      })
    }
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

watch([moduleId, lessonId], () => {
  resetCode()
})
</script>
