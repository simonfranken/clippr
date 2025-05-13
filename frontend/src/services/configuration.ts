import type { AxiosInstance } from 'axios';
import type { IdpConfiguration } from './dtos';

export class ConfigurationService {
  axios: AxiosInstance;
  url = 'configuration';

  constructor(axios: AxiosInstance) {
    this.axios = axios;
  }

  async getIdpConfiguration(): Promise<IdpConfiguration> {
    return (await this.axios.get<IdpConfiguration>(`${this.url}/idp`)).data;
  }
}
