import axios from 'axios'
import type { ProjectDTO } from '../types/projectDTO'
import type {ListDTO} from '../types/listDTO'
import type { QueryDTO } from '../types/queryDTO';



export async function createProject(dto: ProjectDTO) {

  var result = await axios.post('/api/project', dto)
  return result;
}
export async function updateProject(dto: ProjectDTO) {
  var result = await axios.put(`/api/project/${dto.id}`, dto)
  return result;
}
export async function getProjectById(id: number) : Promise<ProjectDTO> {

  var result = await axios.get(`/api/project/${id}`)
  return result.data;
}
export async function deleteProjectById(id: number) {

  var result = await axios.delete(`/api/project/${id}`)
  return result.data;
}
export async function getProjectsList(dto: QueryDTO): Promise<ListDTO<ProjectDTO>> {
  const result = await axios.get('/api/project/list', {
    params: dto,
  });
  return result.data;
}

 
