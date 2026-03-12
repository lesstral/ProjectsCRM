<script setup lang="ts">
import { reactive, ref } from 'vue'
import { createProject } from '../services/projectService'
import type { ProjectDTO } from '../types/projectDTO'
import ProjectForm from '../components/ProjectForm.vue'
import { useToast } from '../composables/useToast'
const { show: showToast } = useToast()

const form = reactive<ProjectDTO>({
  name: '',
  startDate: '',
  endDate: '',
    priority: 1,
  companyContractorName: '',
  companyExecutorName: ''
})



async function submit() {
  try {
    const result = await createProject(form)
    console.log(result)

    showToast('Project created', 'success')
  } catch (err: any) {
    showToast('Error: ' + err.message, 'error')
  }
}
</script>

<template>
  
<div class="entries-container">
  
  <form @submit.prevent="submit">
    <h1> CREATE NEW PROJECT</h1>
<ProjectForm v-model:form="form" :edit-mode="true" />
<button class="submit-btn" type="submit">Create</button>
    
  </form>
</div>
</template>
<style scoped>
  @import '../assets/main.css';
</style>