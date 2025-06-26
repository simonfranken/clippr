import type { AxiosInstance } from 'axios';
import type { ExternalProvider } from './dtos';
import type { ExternalLogin } from './dtos/auth/externalLogin';

export class IdentityService {
  axios: AxiosInstance;
  url = 'auth';

  constructor(axios: AxiosInstance) {
    this.axios = axios;
  }

  async getProviders(): Promise<ExternalProvider[]> {
    return (await this.axios.get<ExternalProvider[]>(`${this.url}/providers`)).data;
  }

  async externalLogin(externalLogin: ExternalLogin): Promise<string> {
    return (await this.axios.post<string>(`${this.url}/external-login`, externalLogin)).data;
  }
}
