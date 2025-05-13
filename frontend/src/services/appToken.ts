import type { AxiosInstance } from 'axios';
import type { AppToken } from './dtos';

export class AppTokenService {
  axios: AxiosInstance;
  url = 'apptoken';

  constructor(axios: AxiosInstance) {
    this.axios = axios;
  }

  async get(): Promise<AppToken[]> {
    return (await this.axios.get<AppToken[]>(this.url)).data;
  }

  async createAppToken(): Promise<string> {
    return (await this.axios.post<string>(this.url)).data;
  }

  async deleteAppToken(id: string): Promise<void> {
    await this.axios.delete(`${this.url}/${id}`);
  }
}
