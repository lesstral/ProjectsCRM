<template>
    <div 
    class="project-element" 
    :style="{ gridTemplateColumns: gridColumns }"
    @click="handleClick"
  >
    <span>{{ data.name }}</span>
    <span>{{ data.priority }}</span>
    <span>{{ formatDate(data.startDate) }}</span>
    <span>{{ formatDate(data.endDate) }}</span>
    <span>{{ data.companyContractorName }}</span>
    <span>{{ data.companyExecutorName }}</span>
  </div>
</template>

<script setup lang="ts">
import type { ProjectDTO } from '../types/projectDTO';
import { formatDate } from '../utils/formatDate';

const props = defineProps<{ 
  data: ProjectDTO
  gridColumns: string
}>()
const data = props.data


const emit = defineEmits<{
  (e: 'edit', project: ProjectDTO): void
}>()


const handleClick = () => {
  emit('edit', data)
}
</script>

<style scoped>
@import '../assets/main.css';
.project-element {
  display: grid;
  grid-template-columns: var(--columns);
  border: 1px solid #ddd;
  border-radius: 5px;
  background-color: white;
  cursor: pointer;
  margin-top: 5px;
  transition: background-color 0.2s;
}

.project-element:hover {
  background-color: #f0f0f0;
}

.project-element span {
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
.project-element span:last-child {
  border-right: none;
}
</style>