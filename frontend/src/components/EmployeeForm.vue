<template>
  <div class="employee-form">

    <div class="entry-card">
      <label>First Name</label>
      <input v-if="props.editMode" v-model="form.firstName" type="text" required />
      <span v-else>{{ form.firstName }}</span>
    </div>

    <div class="entry-card">
      <label>Last Name</label>
      <input v-if="props.editMode" v-model="form.lastName" type="text" required />
      <span v-else>{{ form.lastName }}</span>
    </div>

    <div class="entry-card">
      <label>Middle Name</label>
      <input v-if="props.editMode" v-model="form.middleName" type="text" />
      <span v-else>{{ form.middleName }}</span>
    </div>

    <div class="entry-card">
      <label>Email</label>
      <input v-if="props.editMode" v-model="form.email" type="email" required />
      <span v-else>{{ form.email }}</span>
    </div>

    <!-- managed projects-->
    <div class="entry-card">
      <label>Managed Projects</label>

      <div class="assigned-projects">
        <div 
          v-for="name in managedProjectNames" 
          :key="name"
          class="project-tag"

        >
          <span>{{ name }}</span>
        </div>
        <div v-if="!managedProjectNames.length" class="no-projects">
          No projects managed
        </div>
      </div>
    </div>

    <!-- assigned projects -->
    <div class="entry-card">
      <label>Assigned Projects</label>
      <div class="assigned-projects">
        <div 
          v-for="name in assignedProjectNames" 
          :key="name"
          class="project-tag"
        >
          <span>{{ name }}</span>
        </div>
        <div v-if="!assignedProjectNames.length" class="no-projects">
          No projects assigned
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted} from 'vue'
import type { EmployeeDTO } from '../types/employeeDTO'
import type { ProjectDTO } from '../types/projectDTO'
import type { EmployeeFullNameDTO } from '../types/employeeFullName';
import { getProjectById } from '../services/projectService'
import { useToast } from '../composables/useToast'

const { show: showToast } = useToast()
const form = defineModel<EmployeeDTO>('form', { required: true })

const managedProjectNames = ref<string[]>([])
const assignedProjectNames = ref<string[]>([])

const props = defineProps<{
  editMode: boolean,
}>()


if (!form.value.managedProjectIds) {
  form.value.managedProjectIds = []
}
if (!form.value.assignedProjectIds) {
  form.value.assignedProjectIds = []
}

// load project names for display
// should probably implement endpoint for names only
const loadProjectNames = async (ids: number[] | undefined): Promise<string[]> => {
  if (!ids?.length) return []
  
  try {
    const names = await Promise.all(
      ids.map(async (id) => {
        try {
          const project = await getProjectById(id)
          return project?.name || `Project #${id}`
        } catch {
          return `Project #${id}`
        }
      })
    )
    return names
  } catch {
    return []
  }
}

onMounted(async () => {
  const [managed, assigned] = await Promise.all([
    loadProjectNames(form.value.managedProjectIds),
    loadProjectNames(form.value.assignedProjectIds)
  ])
  managedProjectNames.value = managed
  assignedProjectNames.value = assigned
})


</script>

<style scoped>
@import '../assets/main.css';

.employee-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.assigned-projects {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  min-height: 24px;
}

.project-tag {
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

.project-tag.edit-mode {
  background: #e6f3ff;
  border-color: #b8d9ff;
}

.project-tag.edit-mode:hover {
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

.no-projects {
  color: #777;
  font-style: italic;
  font-size: 13px;
  padding: 4px 0;
}

@media (max-width: 768px) {
  .project-tag {
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