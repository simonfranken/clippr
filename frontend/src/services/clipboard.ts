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

  async createText(content: string): Promise<void> {
    return (await this.axios.post<void>(this.url, content)).data;
  }

  async createFile(content: File): Promise<void> {
    return (
      await this.axios.postForm<void>(`${this.url}/file`, {
        file: content,
      })
    ).data;
  }
}
