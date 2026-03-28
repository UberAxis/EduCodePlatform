<template>
  <div class="flex flex-col">

    <!-- HERO -->
    <section class="min-h-[90vh] flex items-center justify-center bg-muted px-4">
      <div class="max-w-3xl w-full flex flex-col items-center text-center gap-8">
        <div class="flex items-center gap-4">
          <img src="/logo.png" alt="logo" class="size-24" />
          <span class="text-6xl font-extrabold text-primary leading-tight">
            Кодо<span class="text-secondary">Сфера</span>
          </span>
        </div>

        <h1 class="text-3xl md:text-4xl font-bold text-highlighted leading-snug">
          Научись программировать —<br />
          <span class="text-primary">весело, понятно и бесплатно</span>
        </h1>

        <p class="text-xl text-muted max-w-xl">
          Интерактивные уроки, задания с проверкой и система опыта, которая
          превращает обучение в настоящее приключение 🚀
        </p>

        <div class="flex flex-wrap gap-4 justify-center">
          <UButton
            label="Начать обучение"
            icon="i-lucide-rocket"
            size="xl"
            to="/lessons"
          />
          <UButton
            label="Войти в аккаунт"
            icon="i-lucide-log-in"
            size="xl"
            variant="ghost"
            color="neutral"
            to="/login"
          />
        </div>

        <!-- Статы -->
        <div class="grid grid-cols-3 gap-8 mt-4 w-full max-w-md">
          <div class="flex flex-col items-center gap-1">
            <span class="text-3xl font-extrabold text-primary">10+</span>
            <span class="text-sm text-muted text-center">Модулей</span>
          </div>
          <div class="flex flex-col items-center gap-1">
            <span class="text-3xl font-extrabold text-primary">50+</span>
            <span class="text-sm text-muted text-center">Уроков</span>
          </div>
          <div class="flex flex-col items-center gap-1">
            <span class="text-3xl font-extrabold text-primary">∞</span>
            <span class="text-sm text-muted text-center">Интереса</span>
          </div>
        </div>
      </div>
    </section>

    <!-- ВОЗМОЖНОСТИ -->
    <section class="py-24 px-4">
      <div class="max-w-5xl mx-auto flex flex-col gap-12">
        <div class="text-center flex flex-col gap-3">
          <h2 class="text-3xl font-extrabold text-highlighted">Почему КодоСфера?</h2>
          <p class="text-lg text-muted">Всё что нужно для старта — в одном месте</p>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <UCard
            v-for="feature in features"
            :key="feature.title"
            class="text-center"
          >
            <div class="flex flex-col items-center gap-4 py-4">
              <div class="size-16 rounded-full bg-primary/10 flex items-center justify-center">
                <UIcon :name="feature.icon" class="size-8 text-primary" />
              </div>
              <h3 class="text-lg font-bold text-highlighted">{{ feature.title }}</h3>
              <p class="text-muted">{{ feature.description }}</p>
            </div>
          </UCard>
        </div>
      </div>
    </section>

    <!-- КАК ЭТО РАБОТАЕТ -->
    <section class="py-24 px-4 bg-muted">
      <div class="max-w-4xl mx-auto flex flex-col gap-12">
        <div class="text-center flex flex-col gap-3">
          <h2 class="text-3xl font-extrabold text-highlighted">Как это работает?</h2>
          <p class="text-lg text-muted">Три простых шага до первой программы</p>
        </div>

        <div class="flex flex-col gap-6">
          <div
            v-for="(step, i) in steps"
            :key="step.title"
            class="flex items-start gap-6"
          >
            <div class="size-12 shrink-0 rounded-full bg-primary flex items-center justify-center text-white font-extrabold text-xl">
              {{ i + 1 }}
            </div>
            <UCard class="flex-1">
              <div class="flex items-start gap-4">
                <UIcon :name="step.icon" class="size-6 text-primary mt-0.5 shrink-0" />
                <div class="flex flex-col gap-1">
                  <h3 class="text-lg font-bold text-highlighted">{{ step.title }}</h3>
                  <p class="text-muted">{{ step.description }}</p>
                </div>
              </div>
            </UCard>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="py-24 px-4">
      <div class="max-w-2xl mx-auto">
        <UCard class="text-center">
          <div class="flex flex-col items-center gap-6 py-8">
            <span class="text-5xl">🎯</span>
            <h2 class="text-3xl font-extrabold text-highlighted">
              Готов начать?
            </h2>
            <p class="text-lg text-muted max-w-md">
              Зарегистрируйся бесплатно и начни своё путешествие в мир программирования прямо сейчас
            </p>
            <div class="flex flex-wrap gap-4 justify-center">
              <UButton
                label="Создать аккаунт"
                icon="i-lucide-user-plus"
                size="xl"
                to="/register"
              />
              <UButton
                label="Смотреть уроки"
                icon="i-lucide-book-open"
                size="xl"
                variant="outline"
                to="/lessons"
              />
            </div>
          </div>
        </UCard>
      </div>
    </section>

  </div>
</template>

<script setup lang="ts">
const features = [
  {
    icon: 'i-lucide-book-open-check',
    title: 'Интерактивные уроки',
    description: 'Теория + практика в каждом уроке. Пишешь код прямо в браузере и сразу видишь результат.',
  },
  {
    icon: 'i-lucide-trophy',
    title: 'Система опыта',
    description: 'Зарабатывай XP за выполнение заданий, повышай уровень и следи за своим прогрессом.',
  },
  {
    icon: 'i-lucide-layers',
    title: 'Структурированная программа',
    description: 'Модули идут от простого к сложному. Никакой каши — только чёткий путь вперёд.',
  },
]

const steps = [
  {
    icon: 'i-lucide-user-plus',
    title: 'Зарегистрируйся',
    description: 'Создай аккаунт за 30 секунд — нужно только имя и пароль. Никакой почты.',
  },
  {
    icon: 'i-lucide-book-open',
    title: 'Выбери урок',
    description: 'Открой каталог модулей, выбери тему и начни с первого урока. Всё бесплатно.',
  },
  {
    icon: 'i-lucide-code-2',
    title: 'Выполняй задания',
    description: 'Решай практические задачи, получай XP и поднимайся в уровнях. Учёба как игра.',
  },
]
</script>
