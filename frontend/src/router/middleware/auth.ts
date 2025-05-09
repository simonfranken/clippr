import { useRouter, type NavigationGuard } from 'vue-router';
import { Routes } from '../routes';
import { useUserStore } from '@/stores/user';

export const authMiddleware: NavigationGuard = async (to) => {
  const userStore = useUserStore();
  const router = useRouter();

  await userStore.loadUser();

  if (to.name === Routes.SignInCallback) {
    userStore
      .signInCallback(to.fullPath)
      .then(() => {
        if (userStore.user?.url_state !== undefined) {
          router.replace(userStore.user.url_state);
        } else {
          router.replace({ name: Routes.Home });
        }
      })
      .catch(() => {
        console.error('Authentication failed, redirecting ...');
        router.replace({ name: Routes.Home });
      });
  } else if (userStore.user === undefined) {
    userStore.signInRedirect(to.fullPath);
  }
};
