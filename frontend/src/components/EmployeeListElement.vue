<template>
    <div 
    class="employee-element" 
    :style="{ gridTemplateColumns: gridColumns }"
    @click="handleClick"
  >
    <span>{{ data.firstName }}</span>
    <span>{{ data.lastName }}</span>
    <span>{{ data.middleName }}</span>
    <span>{{ data.email }}</span>
  </div>
</template>

<script setup lang="ts">
import type { EmployeeDTO } from '../types/employeeDTO';

const props = defineProps<{ 
  data: EmployeeDTO
  gridColumns: string
}>()
const data = props.data


const emit = defineEmits<{
  (e: 'edit', employee: EmployeeDTO): void
}>()


const handleClick = () => {
  emit('edit', data)
}
</script>

<style scoped>
@import '../assets/main.css';
.employee-element {
  display: grid;
  grid-template-columns: var(--columns);
  border: 1px solid #ddd;
  border-radius: 5px;
  background-color: white;
  cursor: pointer;
  margin-top: 5px;
  transition: background-color 0.2s;
}

.employee-element:hover {
  background-color: #f0f0f0;
}

.employee-element span {
  padding: 15px;
  border-right: 1px solid #eee;
  overflow: hidden;
  text-overflow: ellipsis;
  text-align: center;
  font-family: 'Roboto', sans-serif;
   white-space: normal;     
  word-wrap: break-word;   
  overflow-wrap: break-word;
  line-height: 1.4;  
}
.employee-element span:last-child {
  border-right: none;
}
</style>