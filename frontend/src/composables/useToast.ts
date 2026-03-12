import { reactive, readonly } from 'vue'

const state = reactive({
  message: '',
  type: 'success' as 'success' | 'error' | 'info',
  isVisible: false
})

let timeoutId: ReturnType<typeof setTimeout> | null = null

export function useToast() {
  const show = (message: string, type: 'success' | 'error' | 'info' = 'success', duration = 3000) => {
    // сlear existing timer if user triggers a new toast quickly
    if (timeoutId) clearTimeout(timeoutId)

    state.message = message
    state.type = type
    state.isVisible = true

    // auto hide
    timeoutId = setTimeout(() => {
      state.isVisible = false
    }, duration)
  }

  const hide = () => {
    state.isVisible = false
  }

  return {
    toastState: readonly(state), 
    show,
    hide
  }
}