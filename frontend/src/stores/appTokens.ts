import { defineStore } from 'pinia';
import type { AppToken } from '@/services/dtos';
import { ref } from 'vue';
import { appTokenService } from '@/services';

export const useAppTokenStore = defineStore('appToken', () => {
  const appTokens = ref<AppToken[]>();
  const appTokensAreLoading = ref(false);
  const appTokenLoadingHasFailed = ref(false);

  const fetchAppTokens = async () => {
    try {
      appTokensAreLoading.value = true;
      appTokens.value = await appTokenService.get();
      appTokenLoadingHasFailed.value = false;
    } catch {
      appTokenLoadingHasFailed.value = true;
    } finally {
      appTokensAreLoading.value = false;
    }
  };

  return {
    appTokens,
    appTokensAreLoading,
    loadingAppTokensFailed: appTokenLoadingHasFailed,
    fetchAppTokens,
  };
});
