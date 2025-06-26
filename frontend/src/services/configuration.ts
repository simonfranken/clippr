import type { AxiosInstance } from 'axios';
import type { IdentityServiceConfig } from './dtos';

export class ConfigurationService {
  axios: AxiosInstance;
  url = 'configuration';

  constructor(axios: AxiosInstance) {
    this.axios = axios;
  }

  async getIdpConfiguration(): Promise<IdentityServiceConfig> {
    return (await this.axios.get<IdentityServiceConfig>(`${this.url}/idp`)).data;
  }
}
