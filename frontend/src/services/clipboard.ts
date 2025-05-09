import type { AxiosInstance } from 'axios';
import type { Clip } from './dtos';

export class ClipboardService {
  axios: AxiosInstance;
  url = 'clipboard';

  constructor(axios: AxiosInstance) {
    this.axios = axios;
  }

  async get(): Promise<Clip[]> {
    return (await this.axios.get<Clip[]>(this.url)).data;
  }
}
