<template>
  <div class="employee-list" style="--columns: 1.5fr 1.5fr 1.5fr 2fr 1fr 1fr;">

    <div class="header-row" :style="{ gridTemplateColumns: gridTemplate }">
      <span 
        v-for="column in columns" 
        :key="column.key"
        @click="handleSort(column.key)"
      >
        {{ column.label }}
      </span>
    </div>

    <EmployeeListElement
      v-for="employee in employees"
      :key="employee.id"
      :data="employee"
      :grid-columns="gridTemplate"
      @edit="onEmployeeClicked"
    />
  </div>

  <div class="pagination">
    <button
      v-for="pageNum in totalPages"
      :key="pageNum"
      :class="{ active: pageNum === query.page}"
      @click="goToPage(pageNum)"
      type="button"
    >
      {{ pageNum }}
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import type { EmployeeDTO } from '../types/employeeDTO'
import type { QueryDTO } from '../types/queryDTO'
import { getEmployeesList } from '../services/employeeService'
import EmployeeListElement from './EmployeeListElement.vue'
import { useToast } from '../composables/useToast'

const { show: showToast } = useToast()

const props = defineProps<{
  onEmployeeClicked: (employee: EmployeeDTO) => void
}>()

const ITEMS_PER_PAGE = 20

const columns = [
  { key: 'firstName', label: 'First Name', flex: 1.5 },
  { key: 'lastName', label: 'Last Name', flex: 1.5 },
  { key: 'middleName', label: 'Middle Name', flex: 1.5 },
  { key: 'email', label: 'Email', flex: 2 },
]

const gridTemplate = computed(() => {
  return columns.map(col => `${col.flex}fr`).join(' ')
})

const totalPages = computed(() => {
  return Math.ceil(totalItems.value / ITEMS_PER_PAGE)
})

const employees = ref<EmployeeDTO[]>([])
const totalItems = ref(0)

const query = ref<QueryDTO>({
  page: 1,
  itemsPerPage: ITEMS_PER_PAGE,
  sortBy: 'id',
  descending: false
})

const fetchEmployees = async () => {
  try {
    const result = await getEmployeesList(query.value)
    employees.value = result.items
    totalItems.value = result.totalItemCount
  } catch (err: any) {
    showToast('Error loading employees: ' + err.message, 'error')
  }
}

const handleSort = (key: string) => {
  // if clicking the same column, toggle direction
  if (query.value.sortBy === key) {
    query.value.descending = !query.value.descending
  } else {
    // ascending
    query.value.sortBy = key
    query.value.descending = false
  }

  // reset to page 1 when sorting changes
  query.value.page = 1
  fetchEmployees()
}

const goToPage = (page: number) => {
  query.value.page = page
  fetchEmployees()
}

onMounted(() => {
  fetchEmployees()
})
</script>

<style scoped>
@import '../assets/main.css';
* {
  box-sizing: border-box;
}
span {
  text-align: center;
  display: inline-block;
  font-family: 'Roboto', sans-serif;
  font-weight: bold;
  font-size: 15px;
  cursor: pointer;
}
span:hover {
  background-color: #f0f0f0;
}
.header-row {
  display: grid;
  border: 1px solid #ddd;
  border-radius: 5px;
  background-color: #f5f5f5;
  font-weight: bold;
}

.header-row span {
  padding: 15px;
  border-right: 1px solid #ddd;
  text-align: center;
  font-family: 'Roboto', sans-serif;
  font-weight: bold;
  font-size: Auto;
  display: block; 
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
   white-space: normal;    
  word-wrap: break-word;   
  overflow-wrap: break-word;
  line-height: 1.4;        
  min-width: 0;   
}

.pagination {
  margin-top: 20px;
  justify-content: center;  
  align-items: center; 
  display: flex;
  gap: 5px;
}

button.active {
  font-weight: bold;
  border: 1px solid #333;
}
</style>