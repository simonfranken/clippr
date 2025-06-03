import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { User, UserManager, WebStorageStateStore, type UserManagerSettings } from 'oidc-client-ts';
import { configurationService } from '@/services';

const settings: Omit<UserManagerSettings, 'authority' | 'client_id'> = {
  redirect_uri: window.location.origin + '/signin-callback',
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  automaticSilentRenew: true,
  loadUserInfo: true,
  scope: 'profile email openid',
};

export const useUserStore = defineStore('user', () => {
  const userManager = ref<UserManager>();
  const user = ref<User>();
  const signInIsLoading = ref(false);
  const signOutIsLoading = ref(false);

  const userIsAuthenticated = computed(() => user.value !== undefined);

  const initUserManager = async () => {
    const idpConfig = await configurationService.getIdpConfiguration();
    userManager.value = new UserManager({
      ...settings,
      client_id: idpConfig.clientId,
      authority: idpConfig.authority,
    });
    userManager.value.events.addAccessTokenExpired(() => {
      user.value = undefined;
    });
  };

  const signInRedirect = async (url_state: string) => {
    signInIsLoading.value = true;
    await userManager.value!.signinRedirect({ url_state });
  };

  const signOutRedirect = async () => {
    signOutIsLoading.value = true;
    await userManager.value!.signoutRedirect();
  };

  const signInCallback = async (url: string) => {
    signInIsLoading.value = true;
    user.value = await userManager.value!.signinCallback(url);
    signInIsLoading.value = false;
  };

  const loadUser = async () => {
    const loadedUser = await userManager.value!.getUser();
    if (loadedUser !== null && loadedUser.expired === false) {
      user.value = loadedUser;
    }
  };

  return {
    user,
    signInRedirect,
    signInCallback,
    loadUser,
    signInIsLoading,
    signOutRedirect,
    signOutIsLoading,
    userIsAuthenticated,
    initUserManager,
  };
});
