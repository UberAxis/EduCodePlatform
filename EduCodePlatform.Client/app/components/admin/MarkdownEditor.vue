<template>
  <div class="w-full">
    <label class="block text-sm font-semibold text-highlighted mb-2">{{ label }}</label>
    
    <!-- Tab Navigation -->
    <div class="flex space-x-2 mb-4 border-b border-default">
      <button
        @click="activeTab = 'edit'"
        :class="[
          'px-4 py-2 font-semibold border-b-2 transition',
          activeTab === 'edit'
            ? 'text-blue-400 border-blue-400'
            : 'text-muted border-transparent hover:text-highlighted'
        ]"
      >
        <svg class="w-4 h-4 inline mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
        </svg>
        Редактор
      </button>
      <button
        @click="activeTab = 'preview'"
        :class="[
          'px-4 py-2 font-semibold border-b-2 transition',
          activeTab === 'preview'
            ? 'text-blue-400 border-blue-400'
            : 'text-muted border-transparent hover:text-highlighted'
        ]"
      >
        <svg class="w-4 h-4 inline mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
        </svg>
        Превью
      </button>
    </div>

    <!-- Editor Tab -->
    <div v-if="activeTab === 'edit'" class="space-y-4">
      <!-- Toolbar -->
      <div class="bg-accented rounded-lg p-3 flex flex-wrap gap-2">
        <button
          v-for="btn in toolbarButtons"
          :key="btn.id"
          @click="insertMarkdown(btn.before, btn.after)"
          :title="btn.title"
          class="p-2 bg-slate-600 hover:bg-slate-500 text-highlighted rounded transition text-sm font-semibold"
        >
          {{ btn.label }}
        </button>
        <div class="w-px bg-slate-600"></div>
        <button
          @click="insertHeading(1)"
          title="H1"
          class="p-2 bg-slate-600 hover:bg-slate-500 text-highlighted rounded transition text-sm font-bold"
        >
          H1
        </button>
        <button
          @click="insertHeading(2)"
          title="H2"
          class="p-2 bg-slate-600 hover:bg-slate-500 text-highlighted rounded transition text-sm font-bold"
        >
          H2
        </button>
        <button
          @click="insertHeading(3)"
          title="H3"
          class="p-2 bg-slate-600 hover:bg-slate-500 text-highlighted rounded transition text-sm font-bold"
        >
          H3
        </button>
      </div>

      <!-- Editor -->
      <textarea
        ref="editorRef"
        v-model="content"
        :placeholder="placeholder"
        rows="12"
        class="w-full px-4 py-3 bg-accented border border-default text-highlighted rounded-lg focus:outline-none focus:border-blue-500 transition font-mono text-sm resize-none"
      />
      
      <!-- Character count -->
      <div class="text-xs text-muted">
        {{ content.length }} символов
      </div>
    </div>

    <!-- Preview Tab -->
    <div v-if="activeTab === 'preview'" class="bg-accented rounded-lg p-6 min-h-96 max-h-96 overflow-y-auto prose prose-invert max-w-none">
      <div v-html="renderedMarkdown" class="text-slate-200 prose prose-invert prose-sm"></div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { marked } from 'marked'

const props = defineProps<{
  modelValue: string
  label?: string
  placeholder?: string
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const activeTab = ref<'edit' | 'preview'>('edit')
const editorRef = ref<HTMLTextAreaElement | null>(null)

const content = computed({
  get: () => props.modelValue,
  set: (value) => emit('update:modelValue', value)
})

const renderedMarkdown = computed(() => {
  return marked(content.value, {
    breaks: true,
    gfm: true
  })
})

const toolbarButtons = [
  { id: 'bold', label: '<B>Жирный</B>', before: '**', after: '**', title: 'Жирный текст' },
  { id: 'italic', label: '<I>Курсив</I>', before: '*', after: '*', title: 'Курсивный текст' },
  { id: 'code', label: '<Code>Код</Code>', before: '`', after: '`', title: 'Встроенный код' },
  { id: 'link', label: '[Ссылка]', before: '[', after: '](url)', title: 'Вставить ссылку' },
]

const insertMarkdown = (before: string, after: string) => {
  if (!editorRef.value) return

  const textarea = editorRef.value
  const start = textarea.selectionStart
  const end = textarea.selectionEnd
  const selectedText = content.value.substring(start, end) || 'текст'
  const newText = before + selectedText + after

  content.value = 
    content.value.substring(0, start) +
    newText +
    content.value.substring(end)

  // Restore cursor position
  nextTick(() => {
    textarea.focus()
    textarea.selectionStart = start + before.length
    textarea.selectionEnd = start + before.length + selectedText.length
  })
}

const insertHeading = (level: number) => {
  if (!editorRef.value) return

  const textarea = editorRef.value
  const start = textarea.selectionStart
  const end = textarea.selectionEnd
  const selectedText = content.value.substring(start, end) || `Заголовок ${level}`
  const prefix = '#'.repeat(level) + ' '
  const newText = prefix + selectedText

  content.value =
    content.value.substring(0, start) +
    newText +
    content.value.substring(end)

  nextTick(() => {
    textarea.focus()
    textarea.selectionStart = start + prefix.length
    textarea.selectionEnd = start + prefix.length + selectedText.length
  })
}
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

:deep(.prose ul) {
  list-style-type: disc;
  margin-left: 1.5rem;
  margin: 0.5rem 0;
}

:deep(.prose ol) {
  list-style-type: decimal;
  margin-left: 1.5rem;
  margin: 0.5rem 0;
}

:deep(.prose blockquote) {
  border-left: 4px solid #94a3b8;
  padding-left: 1rem;
  margin: 1rem 0;
  color: #cbd5e1;
  font-style: italic;
}

:deep(.prose table) {
  border-collapse: collapse;
  width: 100%;
  margin: 1rem 0;
}

:deep(.prose th),
:deep(.prose td) {
  border: 1px solid #475569;
  padding: 0.5rem;
  text-align: left;
}

:deep(.prose th) {
  background-color: #334155;
  font-weight: bold;
}

:deep(.prose hr) {
  border: none;
  border-top: 1px solid #475569;
  margin: 1rem 0;
}
</style>
