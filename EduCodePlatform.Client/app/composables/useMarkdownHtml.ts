import { marked } from 'marked'

let markedConfigured = false

function ensureMarkedOptions() {
  if (markedConfigured) return
  marked.use({
    gfm: true,
    breaks: true,
  })
  markedConfigured = true
}

/** Рендер Markdown в HTML (GFM), без «ломаных» inline-кодов из-за экранирования. */
export async function markdownToHtml(source: string): Promise<string> {
  const md = source.trim()
  if (!md) return ''
  ensureMarkedOptions()
  const out = await marked.parse(md)
  return typeof out === 'string' ? out : String(out)
}
