<!-- pages/lessons/[moduleId]/[lessonId]/quiz.vue -->
<template>
  <div class="min-h-screen bg-muted py-10 px-4">
    <div class="max-w-2xl mx-auto flex flex-col gap-6">

      <!-- Назад -->
      <UButton
        label="К уроку"
        icon="i-lucide-arrow-left"
        variant="ghost"
        color="neutral"
        :to="`/lessons/${moduleId}/${lessonId}`"
        class="self-start"
      />

      <!-- Загрузка -->
      <div v-if="study.loading" class="flex flex-col gap-4">
        <USkeleton class="h-8 w-1/2" />
        <USkeleton class="h-5 w-1/3" />
        <USkeleton class="h-64 w-full" />
      </div>

      <!-- Не найдено — только после загрузки -->
      <UCard v-else-if="!quiz">
        <div class="flex flex-col items-center gap-3 py-8 text-center">
          <UIcon name="i-lucide-frown" class="size-12 text-muted" />
          <p class="text-lg font-bold text-highlighted">Тест не найден</p>
          <UButton label="Вернуться к уроку" :to="`/lessons/${moduleId}/${lessonId}`" />
        </div>
      </UCard>

      <template v-else>

        <!-- Шапка -->
        <div class="flex flex-col gap-2">
          <h1 class="text-2xl font-extrabold text-highlighted">🧠 {{ quiz.title }}</h1>
          <div class="flex items-center gap-4 text-sm text-muted">
            <span class="flex items-center gap-1">
              <UIcon name="i-lucide-list-checks" class="size-4" />
              {{ quiz.questions.length }} вопросов
            </span>
            <span class="flex items-center gap-1">
              <UIcon name="i-lucide-zap" class="size-4 text-primary" />
              +{{ quiz.xpReward }} XP за прохождение
            </span>
          </div>
        </div>

        <!-- Прогресс -->
        <div v-if="!isFinished" class="flex flex-col gap-2">
          <div class="flex justify-between text-sm text-muted">
            <span>Вопрос {{ currentIndex + 1 }} из {{ quiz.questions.length }}</span>
            <span>{{ correctCount }} правильных</span>
          </div>
          <UProgress
            :model-value="Math.round((currentIndex / quiz.questions.length) * 100)"
            color="primary"
            size="sm"
          />
        </div>

        <!-- ВОПРОС -->
        <template v-if="!isFinished">
          <UCard>
            <div class="flex flex-col gap-6 py-2">
              <p class="text-lg font-bold text-highlighted leading-snug">
                {{ currentQuestion.question }}
              </p>

              <div class="flex flex-col gap-3">
                <button
                  v-for="(option, idx) in currentQuestion.options"
                  :key="idx"
                  class="w-full text-left px-4 py-3 rounded-lg border-2 font-medium transition-all"
                  :class="optionClass(idx)"
                  :disabled="answered"
                  @click="selectAnswer(idx)"
                >
                  <div class="flex items-center gap-3">
                    <span
                      class="size-7 rounded-full flex items-center justify-center text-sm font-bold shrink-0"
                      :class="optionBadgeClass(idx)"
                    >
                      {{ ['A', 'B', 'C', 'D'][idx] }}
                    </span>
                    {{ option }}
                  </div>
                </button>
              </div>

              <!-- Фидбек после ответа -->
              <Transition name="fade">
                <div v-if="answered">
                  <UAlert
                    v-if="selectedIndex === currentQuestion.correctIndex"
                    color="success"
                    variant="soft"
                    icon="i-lucide-party-popper"
                    title="Правильно! 🎉"
                  />
                  <UAlert
                    v-else
                    color="error"
                    variant="soft"
                    icon="i-lucide-x-circle"
                    :title="`Не совсем... Правильный ответ: «${currentQuestion.options[currentQuestion.correctIndex]}»`"
                  />

                  <UButton
                    :label="isLastQuestion ? 'Посмотреть результат' : 'Следующий вопрос'"
                    :trailing-icon="isLastQuestion ? 'i-lucide-flag' : 'i-lucide-arrow-right'"
                    size="lg"
                    class="w-full mt-4"
                    @click="nextQuestion"
                  />
                </div>
              </Transition>
            </div>
          </UCard>
        </template>

        <!-- РЕЗУЛЬТАТ -->
        <template v-else>
          <UCard>
            <div class="flex flex-col items-center gap-6 py-6 text-center">
              <span class="text-6xl">{{ resultEmoji }}</span>

              <div class="flex flex-col gap-2">
                <h2 class="text-2xl font-extrabold text-highlighted">{{ resultTitle }}</h2>
                <p class="text-muted">{{ resultDescription }}</p>
              </div>

              <!-- Счёт -->
              <div class="flex items-center gap-8">
                <div class="flex flex-col items-center gap-1">
                  <span class="text-3xl font-extrabold text-success">{{ correctCount }}</span>
                  <span class="text-sm text-muted">правильных</span>
                </div>
                <div class="flex flex-col items-center gap-1">
                  <span class="text-3xl font-extrabold text-error">{{ quiz.questions.length - correctCount }}</span>
                  <span class="text-sm text-muted">ошибок</span>
                </div>
                <div class="flex flex-col items-center gap-1">
                  <span class="text-3xl font-extrabold text-primary">{{ score }}%</span>
                  <span class="text-sm text-muted">результат</span>
                </div>
              </div>

              <!-- XP -->
              <UAlert
                v-if="xpEarned > 0 && !alreadyRewarded"
                color="success"
                variant="soft"
                icon="i-lucide-zap"
                :title="`+${xpEarned} XP получено!`"
              />
              <UAlert
                v-else-if="alreadyRewarded"
                color="neutral"
                variant="soft"
                icon="i-lucide-info"
                title="XP за этот тест уже начислен"
              />

              <!-- Кнопки -->
              <div class="flex flex-col gap-3 w-full">
                <UButton
                  label="Начать заново"
                  icon="i-lucide-rotate-ccw"
                  size="xl"
                  variant="outline"
                  color="neutral"
                  class="w-full"
                  @click="restart"
                />
                <UButton
                  label="Вернуться к уроку"
                  icon="i-lucide-book-open"
                  size="xl"
                  class="w-full"
                  :to="`/lessons/${moduleId}/${lessonId}`"
                />
              </div>
            </div>
          </UCard>
        </template>

      </template>
    </div>
  </div>
</template>


<script setup lang="ts">
import confetti from 'canvas-confetti'
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
const quiz = computed(() => currentLesson.value?.quiz ?? null)

// Состояние квиза
const currentIndex = ref(0)
const selectedIndex = ref<number | null>(null)
const answered = ref(false)
const correctCount = ref(0)
const isFinished = ref(false)
const alreadyRewarded = ref(false)
const xpEarned = ref(0)

const currentQuestion = computed(() => quiz.value!.questions[currentIndex.value]!)
const isLastQuestion = computed(() => currentIndex.value === (quiz.value?.questions.length ?? 0) - 1)
const score = computed(() => Math.round((correctCount.value / (quiz.value?.questions.length ?? 1)) * 100))

// Стили вариантов ответа
function optionClass(idx: number) {
  if (!answered.value) {
    return 'border-default hover:border-primary hover:bg-primary/5 text-default cursor-pointer'
  }
  if (idx === currentQuestion.value.correctIndex) {
    return 'border-success bg-success/10 text-success cursor-default'
  }
  if (idx === selectedIndex.value) {
    return 'border-error bg-error/10 text-error cursor-default'
  }
  return 'border-default text-muted cursor-default opacity-50'
}

function optionBadgeClass(idx: number) {
  if (!answered.value) return 'bg-elevated text-muted'
  if (idx === currentQuestion.value.correctIndex) return 'bg-success/20 text-success'
  if (idx === selectedIndex.value) return 'bg-error/20 text-error'
  return 'bg-elevated text-muted'
}

function selectAnswer(idx: number) {
  if (answered.value) return
  selectedIndex.value = idx
  answered.value = true

  const isCorrect = idx === currentQuestion.value.correctIndex

  if (isCorrect) {
    correctCount.value++
    // Конфети на правильный ответ
    confetti({
      particleCount: 80,
      spread: 60,
      origin: { y: 0.7 },
      colors: ['#22c55e', '#16a34a', '#bbf7d0'],
    })
  }
}

function nextQuestion() {
  if (isLastQuestion.value) {
    finishQuiz()
    return
  }
  currentIndex.value++
  selectedIndex.value = null
  answered.value = false
}

async function finishQuiz() {
  isFinished.value = true

  // Конфети на результат если >= 60%
  if (score.value >= 60) {
    confetti({
      particleCount: 200,
      spread: 100,
      origin: { y: 0.5 },
    })
  }

  // Начислить XP если не начислен ранее
  if (!alreadyRewarded.value && score.value >= 60) {
    alreadyRewarded.value = true

    // ========================= MOCK =========================
    const earned = Math.round(quiz.value!.xpReward * (score.value / 100))
    xpEarned.value = earned
    auth.addXp(earned)
    // ========================================================

    // ----- РЕАЛЬНЫЙ XP (раскомментировать когда бэк готов) -----
    // try {
    //   const res = await fetch(`http://localhost:5145/api/Quiz/${quiz.value!.id}/complete`, {
    //     method: 'POST',
    //     headers: { 'Content-Type': 'application/json' },
    //     credentials: 'include',
    //     body: JSON.stringify({ score: score.value }),
    //   })
    //   const data = await res.json()
    //   xpEarned.value = data.xpEarned
    //   if (auth.user) { auth.user.xp = data.xp; auth.user.level = data.level }
    // } catch {
    //   toast.add({ title: 'Не удалось начислить XP', color: 'error', icon: 'i-lucide-circle-x' })
    // }
    // -----------------------------------------------------------

    toast.add({
      title: `Тест пройден! +${xpEarned.value} XP 🎉`,
      color: 'success',
      icon: 'i-lucide-trophy',
    })
  }
}

function restart() {
  currentIndex.value = 0
  selectedIndex.value = null
  answered.value = false
  correctCount.value = 0
  isFinished.value = false
  xpEarned.value = 0
  // alreadyRewarded намеренно НЕ сбрасываем — XP второй раз не дадим
}

// Результат
const resultEmoji = computed(() => {
  if (score.value === 100) return '🏆'
  if (score.value >= 80) return '🎉'
  if (score.value >= 60) return '👍'
  if (score.value >= 40) return '😅'
  return '💪'
})

const resultTitle = computed(() => {
  if (score.value === 100) return 'Идеально!'
  if (score.value >= 80) return 'Отличный результат!'
  if (score.value >= 60) return 'Хорошая работа!'
  if (score.value >= 40) return 'Неплохо, но есть куда расти'
  return 'Попробуй ещё раз!'
})

const resultDescription = computed(() => {
  if (score.value >= 60) return 'Ты справился с тестом и получил XP. Продолжай в том же духе!'
  return 'Не расстраивайся — повтори материал и попробуй снова. У тебя всё получится!'
})

onMounted(() => {
  if (!study.modules.length) study.fetchModules()
})
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
