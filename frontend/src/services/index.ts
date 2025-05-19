import { useUserStore } from '@/stores/user';
import axios from 'axios';
import { ClipboardService } from './clipboard';
import { AppTokenService } from './appToken';
import { ConfigurationService } from './configuration';

const instance = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_ENDPOINT || `${window.location.origin}/api/`,
  headers: {
    'Content-Type': 'application/json',
  },
});

instance.interceptors.request.use((config) => {
  const token = useUserStore().user?.access_token;
  if (token !== undefined) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const clipboardService = new ClipboardService(instance);
export const appTokenService = new AppTokenService(instance);
export const configurationService = new ConfigurationService(instance);
