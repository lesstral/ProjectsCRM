<script setup lang="ts">
import {  ref } from 'vue'
import {  deleteProjectById, updateProject } from '../services/projectService'
import type { ProjectDTO } from '../types/projectDTO'
import ProjectForm from '../components/ProjectForm.vue'
import ProjectList from '../components/ProjectList.vue'
import { stripTime } from '../utils/stripTime'
import { useToast } from '../composables/useToast'
const { show: showToast } = useToast()
const formMode = ref(false);
const editMode = ref(false);

const getInitialForm = (): ProjectDTO => ({
  id: undefined,
  name: '',
  startDate: '',
  endDate: '',
  priority: 1,
  companyContractorName: '',
  companyExecutorName: '',
  projectManagerId: undefined,  
  employeeIds: []            
})

const form = ref<ProjectDTO>(getInitialForm())

async function onSubmitButtonClicked() {
    try {
    const result = await updateProject(form.value)
    console.log(result)
      editMode.value = false;
    showToast('Project updated', 'success')
  } catch (err: any) {
    showToast('Error updating project: ' + err.message, 'error')
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
    const result = await deleteProjectById(form.value.id as number); 
    editMode.value = false;
    formMode.value = false;
    Object.assign(form.value, getInitialForm())
    showToast('Project deleted', 'success')
    }
    catch (err: any) {
      showToast('Error deleting project: ' + err.message, 'error')
    }
  }


const onProjectClicked = (data: ProjectDTO) => {
  formMode.value = true;
  data.startDate = stripTime(data.startDate);
  data.endDate = stripTime(data.endDate);
  Object.assign(form.value, data) 
}


</script>

<template>
  
<div class="entries-container">
  <form>
    <h1> Manage projects</h1>
  <div v-if="formMode">
  <ProjectForm v-model:form="form" :edit-mode="editMode"/>
  </div>
  <ProjectList  
      :onProjectClicked="onProjectClicked"
      v-if="!formMode"
    />
  <div class="button-container">
      <button type="button" button class="submit-btn" v-if="formMode" @click="onBackButtonClicked">Back</button>
      <button type="button" button class="submit-btn" v-if="formMode && !editMode" @click="editMode = !editMode">Edit</button>
      <button type="button" button class="submit-btn" v-if="formMode && editMode" @click="onSubmitButtonClicked">Submit and exit edit mode</button>
      <button type="button" button class="delete-btn" v-if="formMode" @click="OnDeleteButtonClicked">Delete</button>
 </div>
  </form>
</div>

</template>

<style scoped>

@import '../assets/main.css';

</style>