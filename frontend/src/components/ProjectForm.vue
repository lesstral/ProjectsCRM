<template>
  <div class="project-form">

    <div class="entry-card">
      <label>Name</label>
      <input v-if="props.editMode" v-model="form.name" type="text" required />
      <span v-else>{{ form.name }}</span>
    </div>

    <div class="entry-card">
      <label>Start Date</label>
      <input v-if="props.editMode" v-model="form.startDate" type="date" />
      <span v-else>{{ formatDate(form.startDate) }}</span>
    </div>

    <div class="entry-card">
      <label>End Date</label>
      <input v-if="props.editMode" v-model="form.endDate" type="date" />
      <span v-else>{{ formatDate(form.endDate) }}</span>
    </div>

    <div class="entry-card">
      <label>Priority</label>
      <input v-if="props.editMode" v-model.number="form.priority" type="number" min="1" max="10" />
      <span v-else>{{ form.priority }}</span>
    </div>

    <div class="entry-card">
      <label>Company Contractor Name</label>
      <input v-if="props.editMode" v-model="form.companyContractorName" type="text" />
      <span v-else>{{ form.companyContractorName }}</span>
    </div>

    <div class="entry-card">
      <label>Company Executor Name</label>
      <input v-if="props.editMode" v-model="form.companyExecutorName" type="text" />
      <span v-else>{{ form.companyExecutorName }}</span>
    </div>

    <!-- project manager -->
    <div class="entry-card">
      <label>Project Manager</label>
      <template v-if="props.editMode">
        <EmployeeSearch
          :on-employee-select="handleManagerSelect"
          :check-if-assigned="checkIfManagerAssigned"
          placeholder="Search manager by name..."
          class="manager-search"
        />
        <div v-if="form.projectManagerId" class="selected-manager">
          <span class="manager-name">{{ projectManagerName || 'Loading' }}</span>
          <button @click="clearManager" class="remove-btn" title="Clear manager">×</button>
        </div>
      </template>
      <span v-else>{{ projectManagerName || 'Not assigned' }}</span>
    </div>


    <div class="entry-card">
      <label>Employees on the project</label>
      

      <div v-if="props.editMode" class="employee-search-container">
        <EmployeeSearch 
          :on-employee-select="handleEmployeeSelect"
          :check-if-assigned="checkIfAssigned"
          placeholder="Search employees by name..."
        />
      </div>
      

      <div class="assigned-employees">
        <div 
          v-for="employee in selectedEmployees" 
          :key="employee.id"
          class="employee-tag"
          :class="{ 'edit-mode': props.editMode }"
        >
          <span>{{ employee.fullName }}</span>

          <button 
            v-if="props.editMode"
            @click="removeEmployee(employee.id)"
            class="remove-btn"
            title="Remove employee"
          >
            ×
          </button>
        </div>
        <div v-if="!selectedEmployees.length" class="no-employees">
          No employees assigned
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import type { ProjectDTO } from '../types/projectDTO'
import { formatDate } from '../utils/formatDate'
import EmployeeSearch from './EmployeeSearch.vue'
import type { EmployeeFullNameDTO } from '../types/employeeFullName'
import { getEmployeeFullName } from '../services/employeeService'
import { useToast } from '../composables/useToast'

const { show: showToast } = useToast()
const form = defineModel<ProjectDTO>('form', { required: true })
const selectedEmployees = ref<EmployeeFullNameDTO[]>([])
const projectManagerName = ref<string>('')

const props = defineProps<{
  editMode: boolean,
}>()


if (!form.value.employeeIds) {
  form.value.employeeIds = []
}


const loadProjectManager = async () => {
  if (!form.value.projectManagerId) {
    projectManagerName.value = ''
    return
  }
  
  try {
    const manager = await getEmployeeFullName(form.value.projectManagerId)
    projectManagerName.value = manager?.fullName || 'Unknown'
  } catch (err: any) {
    console.error('Failed to load project manager:', err)
    showToast('Error Loading Project Manager Name: ' + err.message, 'error')
    projectManagerName.value = 'Error loading'
  }
}

// load employee details by id
const loadEmployees = async () => {
  if (!form.value.employeeIds?.length) {
    selectedEmployees.value = []
    return
  }

  try {
    const employees = await Promise.all(
      form.value.employeeIds.map(id => getEmployeeFullName(id))
    )
    selectedEmployees.value = employees.filter((e): e is EmployeeFullNameDTO => e != null)

  } catch (error) {
    selectedEmployees.value = []
  }
}

// initial load
onMounted(async () => {
  await Promise.all([
    loadProjectManager(),
    loadEmployees()
  ])
})


watch(() => form.value.projectManagerId, loadProjectManager)
watch(() => form.value.employeeIds, loadEmployees, { deep: true })

// handlres
const handleManagerSelect = (employee: EmployeeFullNameDTO) => {
  console.log('Manager selected:', employee)
  if (checkIfManagerAssigned(employee)) return
  form.value.projectManagerId = employee.id
  projectManagerName.value = employee.fullName
  console.log('Manager form:', form.value)
}

const checkIfManagerAssigned = (employee: EmployeeFullNameDTO): boolean => {
  return form.value.projectManagerId === employee.id
}

const clearManager = () => {
  form.value.projectManagerId = undefined
  projectManagerName.value = ''
}


const handleEmployeeSelect = (employee: EmployeeFullNameDTO) => {
  if (checkIfAssigned(employee)) return
  assignEmployee(employee)
}

const assignEmployee = (employee: EmployeeFullNameDTO) => {
  if (checkIfAssigned(employee)) return
  
  if (!form.value.employeeIds) {
    form.value.employeeIds = []
  }
  
  form.value.employeeIds.push(employee.id)

}

const checkIfAssigned = (employee: EmployeeFullNameDTO): boolean => {
  return form.value.employeeIds?.includes(employee.id) ?? false
}

const removeEmployee = (employeeId: number) => {
  if (form.value.employeeIds) {
    form.value.employeeIds = form.value.employeeIds.filter(id => id !== employeeId)
  }
}
</script>

<style scoped>
@import '../assets/main.css';

.project-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.manager-search {
  margin-bottom: 8px;
}

.selected-manager {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 6px 10px;
  background: #e8f4fd;
  border: 1px solid #b3d9f5;
  border-radius: 6px;
  font-size: 14px;
}

.employee-search-container {
  margin-bottom: 12px;
}

.assigned-employees {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  min-height: 24px;
}

.employee-tag {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 6px 12px;
  background: #f0f7ff;
  border: 1px solid #c2e0ff;
  border-radius: 20px;
  font-size: 13px;
  color: #333;
  transition: background 0.2s;
}

.employee-tag.edit-mode {
  background: #e6f3ff;
  border-color: #b8d9ff;
}

.employee-tag.edit-mode:hover {
  background: #d9edff;
}

.remove-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  padding: 0;
  border: none;
  background: transparent;
  color: #666;
  cursor: pointer;
  font-size: 18px;
  line-height: 1;
  border-radius: 50%;
  transition: all 0.2s;
}

.remove-btn:hover {
  color: #fff;
  background: #ff4444;
}

.no-employees {
  color: #777;
  font-style: italic;
  font-size: 13px;
  padding: 4px 0;
}


@media (max-width: 768px) {
  .employee-tag {
    font-size: 12px;
    padding: 4px 10px;
  }
  
  .remove-btn {
    width: 16px;
    height: 16px;
    font-size: 16px;
  }
}
</style>