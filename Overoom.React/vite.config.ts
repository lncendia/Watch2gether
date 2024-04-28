import * as fs from 'fs'
import * as path from 'path'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import sass from 'sass'

// Шаблон имен css модулей
const generateScopedName = "[name]__[local]__[hash:base64:5]";

// Папка с сертификатами
const baseFolder = 
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;

// Имя сертификата
const certName = process.env.npm_package_name;

// Конфигурация сборщика Vite
export default defineConfig({
  
  // базовый url
  base: '/',
  
  // Массив плагинов
  plugins: [react()],

  // Конфигурация production версии приложения
  preview: {
    
    // Рабочий порт production приложения
    port: 5173,
    
    // Если порт занят, приложение напишет об этом и не запустится
    strictPort: true,
    
    // Конфигурация сертификатов https соединения
    https: {
      key: fs.readFileSync(path.join(baseFolder, `${certName}.key`)),
      cert: fs.readFileSync(path.join(baseFolder, `${certName}.pem`))
    }
  },

  // Конфигурация develop версии приложения
  server: {
    
    // Запуск develop сервера для хостинга приложения
    host: true,

    // Если порт занят, приложение напишет об этом и не запустится
    strictPort: true,

    // Рабочий порт develop приложения
    port: 5173,

    // Конфигурация сертификатов https соединения
    https: {
      key: fs.readFileSync(path.join(baseFolder, `${certName}.key`)),
      cert: fs.readFileSync(path.join(baseFolder, `${certName}.pem`))
    },
    
    // Порт для HMR
    hmr: {
      port: 5173
    }
  },

  // Настройка css 
  css: {
    
    // Опции препроцессора
    preprocessorOptions: {
      
      // Указание пакетов для парсинга scss в css
      scss: {
        implementation: sass
      }
    },
    
    // Настройка модульных стилей
    modules: {
      
      // Генерация имен классов
      generateScopedName: generateScopedName
    },
  }
})
