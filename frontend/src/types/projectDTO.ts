export interface ProjectDTO {
    id?: number
  name: string
  startDate: string
  endDate: string
  priority: number
  companyContractorName: string
  companyExecutorName: string
  projectManagerId?: number 
  employeeIds?: number[]
}