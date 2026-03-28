# Используем официальный образ Node.js
FROM node:20-slim AS base
WORKDIR /app

# Устанавливаем зависимости
FROM base AS build
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Финальный образ для запуска
FROM base AS final
WORKDIR /app
COPY --from=build /app/.output /app/.output

# Nuxt 3 слушает на 3000 порту по умолчанию
ENV HOST=0.0.0.0
ENV PORT=3000
EXPOSE 3000

# Запускаем сервер Nuxt
CMD ["node", ".output/server/index.mjs"]
