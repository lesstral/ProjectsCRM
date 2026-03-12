<template>
  <div class="project-wizard">

    <div class="wizard-header">
      <div 
        v-for="(step, index) in steps" 
        :key="index"
        class="wizard-step"
        :class="{
          'active': currentStep === index,
          'completed': currentStep > index
        }"
        @click="goToStep(index)"
      >
        <div class="step-number">{{ index + 1 }}</div>
        <span class="step-label">{{ step }}</span>
      </div>
    </div>

    <div class="wizard-content">
      <!-- step 1 -->
      <div v-if="currentStep === 0" class="wizard-step-content">
        <div class="entry-card">
          <label>Name *</label>
          <input 
            v-model="form.name" 
            type="text" 
            required 
            placeholder="Enter project name"
          />

        </div>

        <div class="entry-card">
          <label>Start Date *</label>
          <input 
            v-model="form.startDate" 
            type="date" 
          />

        </div>

        <div class="entry-card">
          <label>End Date *</label>
          <input 
            v-model="form.endDate" 
            type="date" 
          />

        </div>

        <div class="entry-card">
          <label>Priority (1-10)</label>
          <input 
            v-model.number="form.priority" 
            type="number" 
            min="1" 
            max="10"
            placeholder="5"
          />

        </div>
      </div>

      <!-- step 2-->
      <div v-if="currentStep === 1" class="wizard-step-content">
        <div class="entry-card">
          <label>Company Contractor Name</label>
          <input 
            v-model="form.companyContractorName" 
            type="text" 
            placeholder="Contractor company"
          />
        </div>

        <div class="entry-card">
          <label>Company Executor Name</label>
          <input 
            v-model="form.companyExecutorName" 
            type="text" 
            placeholder="Executor company"
          />
        </div>
      </div>

      <!-- step3 -->
      <div v-if="currentStep === 2" class="wizard-step-content">
        <div class="entry-card">
          <label>Project Manager</label>
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
        </div>
      </div>

      <!-- step 4 -->
      <div v-if="currentStep === 3" class="wizard-step-content">
        <div class="entry-card">
          <label>Employees on the project</label>
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
            >
              <span>{{ employee.fullName }}</span>
              <button 
                @click="removeEmployee(employee.id)"
                class="remove-btn"
                title="Remove employee"
              >
                ×
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- step5 -->
      <div v-if="currentStep === 4" class="wizard-step-content">
        <div class="entry-card">
          <label>Project Documents</label>
          <div 
            class="file-drop-zone"
            :class="{ 'drag-over': isDragging }"
            @dragenter.prevent="isDragging = true"
            @dragover.prevent="isDragging = true"
            @dragleave.prevent="isDragging = false"
            @drop.prevent="handleFileDrop"
          >
            <input 
              type="file" 
              multiple 
              class="file-input"
              @change="handleFileSelect"
            />
            <div class="drop-content">
              <span class="drop-icon">📁</span>
                <strong>Drag & drop files here</strong><br/>
                or click to browse<br/>
                <span class="hint">HTML5 mock uploader — no actual upload</span>
            </div>
          </div>
          
          <!-- mock file list -->
          <div v-if="mockFiles.length" class="mock-file-list">
            <div v-for="(file, idx) in mockFiles" :key="idx" class="mock-file-item">
              <span class="file-icon">📄</span>
              <span class="file-name">{{ file.name }}</span>
              <span class="file-size">{{ formatFileSize(file.size) }}</span>
              <button 
                @click="removeMockFile(idx)"
                class="remove-btn"
                title="Remove file"
              >×</button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="wizard-footer">
      <button 
        @click="prevStep" 
        :disabled="currentStep === 0"
        class="submit-btn"
      >
        ← Previous
      </button>
      
      <div class="step-info">
        Step {{ currentStep + 1 }} of {{ steps.length }}
      </div>
      
      <button 
        @click="nextStep" 
        :disabled="!canProceed"
        class="submit-btn"
      >
        {{ currentStep === steps.length - 1 ? 'Finish' : 'Next' }} →
      </button>
    </div>


</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed, reactive } from 'vue'
import type { ProjectDTO } from '../types/projectDTO'
import { formatDate } from '../utils/formatDate'
import EmployeeSearch from '../components/EmployeeSearch.vue'
import type { EmployeeFullNameDTO } from '../types/employeeFullName'
import { getEmployeeFullName } from '../services/employeeService'
import { useToast } from '../composables/useToast'
import { createProject } from '../services/projectService'

const { show: showToast } = useToast()
const selectedEmployees = ref<EmployeeFullNameDTO[]>([])
const projectManagerName = ref<string>('')
const currentStep = ref(0)
const isDragging = ref(false)
const mockFiles = ref<File[]>([])

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
const steps = [
  'Project Basics',
  'Contractor Info', 
  'Project Manager',
  'Team Members',
  'Documents'
]

// Validation: can user proceed to next step?
const canProceed = computed(() => {

  switch (currentStep.value) {
    case 0: // Project Basics
      return !!form.name && !!form.startDate && !!form.endDate
    case 1: // Contractor Info - optional
      return true
    case 2: // PM - optional
      return true
    case 3: // Employees - optional
      return true
    case 4: // Files - mock, always OK
      return true
    default:
      return true
  }
})



// Load project manager name
const loadProjectManager = async () => {
  if (!form.projectManagerId) {
    projectManagerName.value = ''
    return
  }
  
  try {
    const manager = await getEmployeeFullName(form.projectManagerId)
    projectManagerName.value = manager?.fullName || 'Unknown'
  } catch (err: any) {
    console.error('Failed to load project manager:', err)
    showToast('Error Loading Project Manager Name: ' + err.message, 'error')
    projectManagerName.value = 'Error loading'
  }
}


const loadEmployees = async () => {
  if (!form.employeeIds?.length) {
    selectedEmployees.value = []
    return
  }

  try {
    const employees = await Promise.all(
      form.employeeIds.map(id => getEmployeeFullName(id))
    )
    selectedEmployees.value = employees.filter((e): e is EmployeeFullNameDTO => e != null)
  } catch (error) {
    selectedEmployees.value = []
  }
}

// Initial load
onMounted(async () => {
  await Promise.all([loadProjectManager(), loadEmployees()])
})

// Watch for changes
watch(() => form.projectManagerId, loadProjectManager)
watch(() => form.employeeIds, loadEmployees, { deep: true })

// Wizard Navigation
const goToStep = (step: number) => {
  if ( step >= 0 && step < steps.length) {
    currentStep.value = step
  }
}

const nextStep = () => {
  if (currentStep.value < steps.length - 1 && canProceed.value) {
    currentStep.value++
  } else if (currentStep.value === steps.length - 1) {
    submit()

  }
}

const prevStep = () => {
  if (currentStep.value > 0) {
    currentStep.value--
  }
}

// Manager handlers
const handleManagerSelect = (employee: EmployeeFullNameDTO) => {
  if (checkIfManagerAssigned(employee)) return
  form.projectManagerId = employee.id
  projectManagerName.value = employee.fullName
}

const checkIfManagerAssigned = (employee: EmployeeFullNameDTO): boolean => {
  return form.projectManagerId === employee.id
}

const clearManager = () => {
  form.projectManagerId = undefined
  projectManagerName.value = ''
}

// Employee handlers
const handleEmployeeSelect = (employee: EmployeeFullNameDTO) => {
  if (checkIfAssigned(employee)) return
  assignEmployee(employee)
}

const assignEmployee = (employee: EmployeeFullNameDTO) => {
  if (checkIfAssigned(employee)) return
  
  if (!form.employeeIds) {
    form.employeeIds = []
  }
  form.employeeIds.push(employee.id)
}

const checkIfAssigned = (employee: EmployeeFullNameDTO): boolean => {
  return form.employeeIds?.includes(employee.id) ?? false
}

const removeEmployee = (employeeId: number) => {
  if (form.employeeIds) {
    form.employeeIds = form.employeeIds.filter(id => id !== employeeId)
  }
}

// File Upload Mock Handlers
const handleFileDrop = (event: DragEvent) => {
  isDragging.value = false
  
  const files = Array.from(event.dataTransfer?.files || [])
  addMockFiles(files)
}

const handleFileSelect = (event: Event) => {
  
  const files = Array.from((event.target as HTMLInputElement).files || [])
  addMockFiles(files)
  // Reset input so same file can be selected again
  ;(event.target as HTMLInputElement).value = ''
}

const addMockFiles = (files: File[]) => {
  files.forEach(file => {
    mockFiles.value.push({
      name: file.name,
      size: file.size,
      type: file.type,
      lastModified: file.lastModified
    } as File)
  })
  showToast(`Added ${files.length} file(s) (mock)`, 'info')
}

const removeMockFile = (index: number) => {
  mockFiles.value.splice(index, 1)
}

const formatFileSize = (bytes: number): string => {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]
}
</script>

<style scoped>
@import '../assets/main.css';

.project-wizard {
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 800px;
  margin: 0 auto;
}


.wizard-header {
  display: flex;
  justify-content: space-between;
  padding: 16px 8px;
  background: #f8fafc;
  border-radius: 12px;
  border: 1px solid #e2e8f0;
  position: relative;
}

.wizard-header::before {
  content: '';
  position: absolute;
  top: 28px;
  left: 10%;
  right: 10%;
  height: 3px;
  background: #cbd5e1;
  z-index: 0;
}

.wizard-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 6px;
  position: relative;
  z-index: 1;
  cursor: pointer;
  opacity: 1;
  transition: opacity 0.2s;
}

.wizard-step:hover {
  opacity: 0.9;
}

.step-number {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: #fff;
  border: 2px solid #cbd5e1;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: 14px;
  transition: all 0.2s;
}

.step-label {
  font-size: 11px;
  color: #64748b;
  text-align: center;
  max-width: 90px;
  line-height: 1.3;
}

.wizard-step.active .step-number {
  background: #3b82f6;
  border-color: #3b82f6;
  color: white;
}

.wizard-step.active .step-label {
  color: #3b82f6;
  font-weight: 500;
}

.wizard-step.completed .step-number {
  background: #10b981;
  border-color: #10b981;
  color: white;
}

.wizard-step.completed::after {
  content: '✓';
  position: absolute;
  top: -2px;
  right: -2px;
  font-size: 12px;
  color: white;
  background: #10b981;
  width: 16px;
  height: 16px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}


.wizard-content {
  min-height: 300px;
}

.wizard-step-content {
  display: flex;
  flex-direction: column;
  gap: 16px;
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}

.file-drop-zone {
  border: 2px dashed #cbd5e1;
  border-radius: 12px;
  padding: 32px 24px;
  text-align: center;
  background: #f8fafc;
  transition: all 0.2s;
  position: relative;
  cursor: pointer;
}

.file-drop-zone.drag-over {
  border-color: #3b82f6;
  background: #eff6ff;
  transform: scale(1.01);
}

.file-drop-zone:hover {
  border-color: #94a3b8;
}

.file-input {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  opacity: 0;
  cursor: pointer;
}

.drop-content {
  pointer-events: none;
}

.drop-icon {
  font-size: 32px;
  display: block;
  margin-bottom: 12px;
}

.hint {
  font-size: 12px;
  color: #94a3b8;
  font-style: italic;
}

.mock-file-list {
  margin-top: 16px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.mock-file-item {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 10px 14px;
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
}

.file-icon {
  font-size: 16px;
}

.file-name {
  flex: 1;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.file-size {
  color: #64748b;
  font-size: 12px;
  margin: 0 8px;
}

.wizard-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 16px;
  border-top: 1px solid #e2e8f0;
}

.step-info {
  color: #64748b;
  font-size: 14px;
  font-weight: 500;
}

.btn-primary,
.btn-secondary {
  padding: 10px 24px;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
  border: none;
  font-size: 14px;
}

.btn-primary {
  background: #3b82f6;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #2563eb;
  transform: translateY(-1px);
}

.btn-primary:disabled {
  background: #cbd5e1;
  cursor: not-allowed;
  transform: none;
}

.btn-secondary {
  background: #f1f5f9;
  color: #334155;
}

.btn-secondary:hover:not(:disabled) {
  background: #e2e8f0;
}

.btn-secondary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.entry-card {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.entry-card label {
  font-weight: 500;
  font-size: 14px;
  color: #334155;
}

.entry-card input {
  padding: 10px 12px;
  border: 1px solid #cbd5e1;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.2s;
}

.entry-card input:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
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
  .wizard-header {
    flex-wrap: wrap;
    gap: 12px;
    padding: 12px;
  }
  
  .wizard-header::before {
    display: none;
  }
  
  .wizard-step {
    flex: 0 0 calc(33.333% - 8px);
  }
  
  .step-label {
    font-size: 10px;
    max-width: 70px;
  }
  
  .wizard-footer {
    flex-direction: column;
    gap: 12px;
  }
  
  .wizard-footer button {
    width: 100%;
  }
  
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