import axios from 'axios'

import type {ListDTO} from '../types/listDTO'
import type { EmployeeDTO } from '../types/employeeDTO';
import type { EmployeeFullNameDTO } from '../types/employeeFullName';
import type { QueryDTO } from '../types/queryDTO';


export async function createEmployee(dto: EmployeeDTO) {

  var result = await axios.post('/api/employee', dto)
  return result;
}
export async function updateEmployee(dto: EmployeeDTO) {
  var result = await axios.put(`/api/employee${dto.id}`, dto)
  return result;
}
export async function getEmployee(id: number) {

  var result = await axios.get(`/api/employee/${id}`)
  return result.data;
}
export async function getEmployeesList(dto: QueryDTO): Promise<ListDTO<EmployeeDTO>> {
  const result = await axios.get('/api/employee/list', {
    params: dto,
  });
  return result.data;
}
export async function deleteEmployeeById(id: number) {

  var result = await axios.delete(`/api/employee/${id}`)
  return result.data;
}
export async function getEmployeeFullName(id: number) : Promise<EmployeeFullNameDTO> {

  var result = await axios.get(`/api/employee/fullname`, {
    params: {id}
  })
  return result.data;
}

export async function searchEmployees(
  name: string,
  itemLimit: number = 10
): Promise<EmployeeFullNameDTO[]> {

  const response = await axios.get<EmployeeFullNameDTO[]>(
    '/api/employee/search',
    {
      params: {
        name,       
        itemLimit   
      }
    }
  );

  return response.data;
}
