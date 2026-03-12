<script setup lang="ts">
import { ref } from 'vue'
import { deleteEmployeeById, updateEmployee } from '../services/employeeService'
import type { EmployeeDTO } from '../types/employeeDTO'
import EmployeeForm from '../components/EmployeeForm.vue'
import EmployeeList from '../components/EmployeeList.vue'
import { useToast } from '../composables/useToast'

const { show: showToast } = useToast()
const formMode = ref(false);
const editMode = ref(false);

const getInitialForm = (): EmployeeDTO => ({
  id: undefined,
  firstName: '',
  lastName: '',
  middleName: '',
  email: '',
  assignedProjectIds: [],
  managedProjectIds: []
})

const form = ref<EmployeeDTO>(getInitialForm())

async function onSubmitButtonClicked() {
    try {
    const result = await updateEmployee(form.value)
    console.log(result)
      editMode.value = false;
    showToast('Employee updated', 'success')
  } catch (err: any) {
    showToast('Error updating employee: ' + err.message, 'error')
  }

}
function onBackButtonClicked() {
  if (editMode.value) {
    editMode.value = false;
    return;
  }
  if (!editMode.value) {
    formMode.value = false;
  }
  
  Object.assign(form.value, getInitialForm())
}
async function OnDeleteButtonClicked() {
  if (!form.value.id) {
    console.error("No id provided")
  }
  try {
    const result = await deleteEmployeeById(form.value.id as number); 
    editMode.value = false;
    formMode.value = false;
    Object.assign(form.value, getInitialForm())
    showToast('Employee deleted', 'success')
    }
    catch (err: any) {
      showToast('Error deleting employee: ' + err.message, 'error')
    }
  }


const onEmployeeClicked = (data: EmployeeDTO) => {
  formMode.value = true;
  Object.assign(form.value, data) 
}


</script>

<template>
  
<div class="entries-container">
  <form>
    <h1> Manage employees</h1>
  <div v-if="formMode">
  <EmployeeForm v-model:form="form" :edit-mode="editMode"/>
  </div>
  <EmployeeList  
      :onEmployeeClicked="onEmployeeClicked"
      v-if="!formMode"
    />
  <div class="button-container">
      <button type="button" class="submit-btn" v-if="formMode" @click="onBackButtonClicked">Back</button>
      <button type="button" class="submit-btn" v-if="formMode && !editMode" @click="editMode = !editMode">Edit</button>
      <button type="button" class="submit-btn" v-if="formMode && editMode" @click="onSubmitButtonClicked">Submit and exit edit mode</button>
      <button type="button" class="delete-btn" v-if="formMode" @click="OnDeleteButtonClicked">Delete</button>
 </div>
  </form>
</div>

</template>

<style scoped>

@import '../assets/main.css';

</style>