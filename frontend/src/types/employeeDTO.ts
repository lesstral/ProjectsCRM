export interface EmployeeDTO{
    id?: number
  firstName: string
  lastName: string
  middleName: string
  email: string
  assignedProjectIds?: number[]
  managedProjectIds?: number[]
}