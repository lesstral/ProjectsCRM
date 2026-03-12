<template>
  <div class="autocomplete">
    <input
      v-model="search"
      @input="onInput"
      @focus="showDropdown = true"
      @blur="handleBlur"
      type="text"
      placeholder="Search employee..."
      class="dropdown-input"
    />

    <ul v-if="showDropdown && availableResults.length" class="dropdown-list">
      <li
        v-for="employee in availableResults"
        :key="employee.id"
        @mousedown.prevent="selectEmployee(employee)"
        class="dropdown-item"
      >
        {{ employee.fullName }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { computed } from 'vue'
import { searchEmployees } from '../services/employeeService'
import type { EmployeeFullNameDTO } from '../types/employeeFullName';

const emit = defineEmits<{
  (e: 'select', id: number): void
}>()
const props = defineProps<{
    onEmployeeSelect?: (employee: EmployeeFullNameDTO) => void,
  checkIfAssigned: (employee: EmployeeFullNameDTO) => boolean
}>()
const search = ref('')
const results = ref<EmployeeFullNameDTO[]>([])
const showDropdown = ref(false)
const availableResults = computed(() => 
  results.value.filter(emp => !props.checkIfAssigned(emp))
)
let debounceTimeout: number | null = null

const onInput = () => {
  if (debounceTimeout) clearTimeout(debounceTimeout)

  debounceTimeout = window.setTimeout(async () => {
    if (search.value.length < 2) {
      results.value = []
      return
    }

      results.value = await searchEmployees(search.value, 10)
    showDropdown.value = results.value.length > 0
  }, 300)
}

const selectEmployee = (employee: EmployeeFullNameDTO) => {

  search.value = ''
  results.value = []
  showDropdown.value = false
  

  if (debounceTimeout) {
    clearTimeout(debounceTimeout)
    debounceTimeout = null
  }
  

    if (props.onEmployeeSelect) {
        props.onEmployeeSelect(employee)
    }

}


const handleBlur = () => {

  setTimeout(() => {
    showDropdown.value = false
  }, 150)
}
</script>

<style scoped>
@import '../assets/main.css';
.autocomplete {
  position: relative;
  width: 100%;

}



.dropdown-list {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  margin: 0;
  padding: 4px 0;
  list-style: none;
  background-color: white;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
  max-height: 200px;
  overflow-y: auto;
  z-index: 1000;
}

.dropdown-item {
  padding: 8px 12px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.dropdown-item:hover {
  background-color: #f0f0f0;
}


</style>