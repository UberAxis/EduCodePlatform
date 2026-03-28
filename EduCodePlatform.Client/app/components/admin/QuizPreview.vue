<template>
  <div v-if="task && task.type === 'Quiz'" class="bg-accented rounded-lg p-6 border border-default">
    <h3 class="text-lg font-bold text-highlighted mb-4">Предпросмотр теста</h3>
    
    <div class="bg-elevated rounded-lg p-4 mb-6">
      <div v-html="renderedMarkdown" class="prose prose-invert max-w-none mb-6"></div>
    </div>

    <div class="space-y-3">
      <div
        v-for="(option, index) in task.quizOptions"
        :key="index"
        :class="[
          'p-4 rounded-lg border-2 transition cursor-pointer',
          index === task.correctAnswerIndex
            ? 'border-green-500 bg-green-900 bg-opacity-30'
            : 'border-default bg-accented hover:border-slate-500'
        ]"
      >
        <div class="flex items-center space-x-3">
          <div
            :class="[
              'w-5 h-5 rounded-full border-2 flex items-center justify-center',
              index === task.correctAnswerIndex
                ? 'border-green-500 bg-green-500'
                : 'border-slate-500'
            ]"
          >
            <span v-if="index === task.correctAnswerIndex" class="text-highlighted text-xs font-bold">✓</span>
          </div>
          <span class="text-highlighted">{{ option.text }}</span>
          <span
            v-if="index === task.correctAnswerIndex"
            class="ml-auto text-green-400 text-xs font-semibold bg-green-900 px-2 py-1 rounded"
          >
            Правильный ответ
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { marked } from 'marked'

interface QuizOption {
  text: string
}

const props = defineProps<{
  task?: {
    type: string
    markdownContent: string
    quizOptions?: QuizOption[]
    correctAnswerIndex?: number
  } | null
}>()

const renderedMarkdown = computed(() => {
  if (!props.task?.markdownContent) return ''
  return marked(props.task.markdownContent, {
    breaks: true,
    gfm: true
  })
})
</script>

<style scoped>
:deep(.prose) {
  color: inherit;
}

:deep(.prose p) {
  margin: 0.5rem 0;
}

:deep(.prose h1) {
  font-size: 1.875rem;
  font-weight: bold;
  margin: 1rem 0 0.5rem 0;
  color: #f1f5f9;
}

:deep(.prose h2) {
  font-size: 1.5rem;
  font-weight: bold;
  margin: 0.875rem 0 0.5rem 0;
  color: #e2e8f0;
}

:deep(.prose h3) {
  font-size: 1.25rem;
  font-weight: bold;
  margin: 0.75rem 0 0.5rem 0;
  color: #cbd5e1;
}

:deep(.prose code) {
  background-color: #334155;
  padding: 0.125rem 0.25rem;
  border-radius: 0.25rem;
  color: #fca5a5;
  font-size: 0.875em;
}

:deep(.prose pre) {
  background-color: #1e293b;
  padding: 1rem;
  border-radius: 0.5rem;
  overflow-x: auto;
  margin: 1rem 0;
}

:deep(.prose pre code) {
  background-color: transparent;
  padding: 0;
  color: #e2e8f0;
}

:deep(.prose a) {
  color: #60a5fa;
  text-decoration: underline;
}

:deep(.prose a:hover) {
  color: #93c5fd;
}
</style>
