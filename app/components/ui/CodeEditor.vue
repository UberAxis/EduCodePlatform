<template>
  <div
    class="rounded-lg overflow-hidden border border-slate-200 shadow-sm"
    :style="{ height }"
  >
    <ClientOnly>
      <vue-monaco-editor
        v-model:value="localValue"
        :language="language"
        :theme="theme"
        :options="editorOptions"
        style="width: 100%; height: 100%;"
        @mount="handleMount"
      />
      <template #fallback>
        <div
          class="flex items-center justify-center bg-slate-900 text-white font-mono text-sm w-full h-full"
        >
          <UIcon name="i-lucide-loader" class="animate-spin mr-2" />
          Запуск редактора...
        </div>
      </template>
    </ClientOnly>
  </div>
</template>

<script setup lang="ts">
const props = defineProps({
  modelValue: String,
  language: { type: String, default: 'javascript' },
  theme: { type: String, default: 'vs-dark' },
  height: { type: String, default: '400px' }
})

const emit = defineEmits(['update:modelValue', 'mount'])

const localValue = computed({
  get: () => props.modelValue,
  set: (val) => emit('update:modelValue', val)
})

const editorOptions = {
  minimap: { enabled: false },
  fontSize: 16,
  lineNumbers: 'on',
  automaticLayout: true,
  scrollBeyondLastLine: false,
  fixedOverflowWidgets: true,
  roundedSelection: true,
  padding: { top: 16, bottom: 16 }
}

const handleMount = (editorInstance: any) => {
  nextTick(() => {
    editorInstance.layout()
  })
  emit('mount', editorInstance)
}
</script>
