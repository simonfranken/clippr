import type { AxiosInstance } from 'axios';
import type { ExternalProvider } from './dtos';
import type { ExternalLogin } from './dtos/auth/externalLogin';

export class IdentityService {
  axios: AxiosInstance;
  url = 'auth';

  constructor(axios: AxiosInstance) {
    this.axios = axios;
  }

  async getProviders(): Promise<{
    externalProviders: ExternalProvider[];
    enableInternalAuth: boolean;
  }> {
    return (
      await this.axios.get<{ externalProviders: ExternalProvider[]; enableInternalAuth: boolean }>(
        `${this.url}/providers`,
      )
    ).data;
  }

  async externalLogin(externalLogin: ExternalLogin): Promise<string> {
    return (await this.axios.post<string>(`${this.url}/external-login`, externalLogin)).data;
  }

  async login(email: string, password: string): Promise<string> {
    return (await this.axios.post<string>(`${this.url}/login`, { email, password })).data;
  }
}
