import { type NavigationGuard } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import { Routes } from '../routes';

export const authMiddleware: NavigationGuard = async (to) => {
  const authStore = useAuthStore();
  if (!authStore.initCompleted) {
    await authStore.init();
  }

  if (to.name === Routes.SignInCallback) {
    authStore.handleExternalAuthenticationCallback(to.query['providerKey'] as string, to.fullPath);

    return {
      query: {
        showSignInDialog: 'true',
        ...to.query,
      },
    };
  }

  if (!authStore.authCompleted && to.query.showSignInDialog !== 'true') {
    return {
      query: {
        showSignInDialog: 'true',
      },
    };
  }
};
