import { createRouter, createWebHistory } from 'vue-router'
import CreateProject from '../pages/CreateProject.vue'
import ManageProjects from '../pages/ManageProjects.vue'
import ManageEmployees from '../pages/ManageEmployees.vue'
import CreateEmployee from '../pages/CreateEmployee.vue'
import WizardProject from '../pages/CreateProjectWizard.vue'

const routes = [
  { path: '/projects/wizard', component: WizardProject },
  { path: '/projects/create', component: CreateProject },
  { path: '/projects/manage', component: ManageProjects },
  { path: '/employees/create', component: CreateEmployee },
  { path: '/employees/manage', component: ManageEmployees }
]

export const router = createRouter({
  history: createWebHistory(),
  routes
})
