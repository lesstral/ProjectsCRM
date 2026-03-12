<script setup lang="ts">
import { reactive, ref } from 'vue'
import { createEmployee } from '../services/employeeService'
import type { EmployeeDTO } from '../types/employeeDTO'
import EmployeeForm from '../components/EmployeeForm.vue'
import { useToast } from '../composables/useToast'
const { show: showToast } = useToast()

const form = reactive<EmployeeDTO>({
  firstName: '',
    middleName: '',
    lastName: ' ',
    email: ' '
})



async function submit() {
  try {
    const result = await createEmployee(form)
    console.log(result)
    showToast('Employee created ', 'success')

  } catch (err: any) {
  showToast('Error: ' + err.message, 'error')
  }
}
</script>

<template>
  
<div class="entries-container">
  
  <form @submit.prevent="submit">
    <h1> CREATE NEW EMPLOYEE</h1>
<EmployeeForm v-model:form="form" :edit-mode="true"/>
<button class="submit-btn" type="submit">Create</button>
  </form>
</div>
</template>
<style scoped>
  @import '../assets/main.css';
</style>