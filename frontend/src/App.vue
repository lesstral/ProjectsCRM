<script setup lang="ts">
import { useToast } from './composables/useToast'
import ToastNotification from './components/ToastNotificiation.vue'

const { toastState } = useToast()
</script>

<template>
  <div class="app">
    <nav class="nav">
      <router-link to="/projects/wizard">Create Project (wizard)</router-link>
      <router-link to="/projects/create">Create Project (form)</router-link>
      <router-link to="/projects/manage">Manage Projects</router-link>
      <router-link to="/employees/create">Create Employee</router-link>
      <router-link to="/employees/manage">Manage Employees</router-link>
    </nav>

    <ToastNotification 
      :message="toastState.message" 
      :type="toastState.type" 
      :is-visible="toastState.isVisible" 
    />

    <router-view />
  </div>
</template>

<style>
.app {
  margin: 0 auto;
  padding: 20px;
  font-family: Arial, sans-serif;
}

h1 {
  color: #42b883;
  text-align: center;
}

nav {
  display: flex;
  flex-direction: row;
  gap: 10px;
  padding: 10px;
  background-color: #f5f5f5;
}
</style>