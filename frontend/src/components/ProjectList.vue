<template>
  <div class="project-list" style="--columns: 2fr 1fr 1.5fr 1.5fr 1.5fr 1.5fr;">

    <div class="header-row" :style="{ gridTemplateColumns: gridTemplate }">
      <span 
        v-for="column in columns" 
        :key="column.key"
        @click="handleSort(column.key)"
      >
        {{ column.label }}
      </span>
    </div>


    <ProjectListElement
      v-for="project in projects"
      :key="project.id"
      :data="project"
      :grid-columns="gridTemplate"
      @edit="onProjectClicked"
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
import type { ProjectDTO } from '../types/projectDTO'
import type { QueryDTO } from '../types/queryDTO'
import { getProjectsList } from '../services/projectService'
import ProjectListElement from './ProjectListElement.vue'
import { useToast } from '../composables/useToast'

const { show: showToast } = useToast()

const props = defineProps<{
  onProjectClicked: (project: ProjectDTO) => void
}>()


const ITEMS_PER_PAGE = 20

const columns = [
  { key: 'name', label: 'Project Name', flex: 2 },
  { key: 'priority', label: 'Priority', flex: 1 },
  { key: 'startDate', label: 'Start Date', flex: 1.5 },
  { key: 'endDate', label: 'End Date', flex: 1.5 },
  { key: 'companyContractorName', label: 'Contractor', flex: 1.5 },
  { key: 'companyExecutorName', label: 'Executor', flex: 1.5 }
]


const gridTemplate = computed(() => {
  return columns.map(col => `${col.flex}fr`).join(' ')
})

const totalPages = computed(() => {
  return Math.ceil(totalItems.value / ITEMS_PER_PAGE)
})


const projects = ref<ProjectDTO[]>([])
const totalItems = ref(0)


const query = ref<QueryDTO>({
  page: 1,
  itemsPerPage: ITEMS_PER_PAGE,
  sortBy: 'id',
  descending: false
})



const fetchProjects = async () => {
  try {
 
    const result = await getProjectsList(
      query.value
    )
    
    projects.value = result.items
    totalItems.value = result.totalItemCount
  } catch (err: any) {
    showToast('Error loading projects: ' + err.message, 'error')
  }
}

const handleSort = (key: string) => {
  // if clicking the same column, toggle direction
  if (query.value.sortBy === key) {
    query.value.descending = !query.value.descending
  } else {
    //ascending
    query.value.sortBy = key
    query.value.descending = false
  }

  // reset to page 1 when sorting changes
  query.value.page = 1
  console.log("sort by " + query.value.sortBy)
  fetchProjects()
}

const goToPage = (page: number) => {
  query.value.page  = page
  fetchProjects()
}


onMounted(() => {
  fetchProjects()
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