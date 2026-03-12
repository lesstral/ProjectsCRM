<script setup lang="ts">
defineProps<{
  message: string
  type: 'success' | 'error' | 'info'
  isVisible: boolean
}>()
</script>

<template>
  <transition name="toast">
    <div v-if="isVisible" class="toast-container">
      <div :class="['toast-content', type]">
        <span class="toast-icon">
          {{ type === 'success' ? '✔' : type === 'error' ? '✖' : 'ℹ' }}
        </span>
        <span class="toast-message">{{ message }}</span>
      </div>
    </div>
  </transition>
</template>

<style scoped>
.toast-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;
}

.toast-content {
  display: flex;
  align-items: center;
  padding: 12px 20px;
  border-radius: 8px;
  color: white;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  min-width: 250px;
  font-weight: 500;
}

.toast-content.success { background-color: #22c55e; }
.toast-content.error { background-color: #ef4444; }
.toast-content.info { background-color: #3b82f6; }

.toast-icon {
  margin-right: 10px;
  font-size: 1.2rem;
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.4s ease;
}

.toast-enter-from,
.toast-leave-to {
  opacity: 0;
  transform: translateX(50px);
}
</style>