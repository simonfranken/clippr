import { defineStore } from 'pinia';
import { type ExternalProvider, type IdentityServiceConfig } from '@/services/dtos';
import { computed, ref } from 'vue';
import { configurationService } from '@/services';
import { IdentityService } from '@/services/identityService';
import axios from 'axios';
import { UserManager } from 'oidc-client-ts';
import { Routes } from '@/router/routes';
import router from '@/router';
import { type RouteLocationRaw } from 'vue-router';

interface JwtUser {
  sub: string;
  email: string;
  given_name: string;
  family_name: string;
  exp: number;
}

export const useAuthStore = defineStore('auth', () => {
  const externalProviders = ref<ExternalProvider[]>();
  const identityService = ref<IdentityService>(
    new IdentityService(
      axios.create({
        headers: {
          'Content-Type': 'application/json',
        },
      }),
    ),
  );

  const authCompleted = computed(() => user.value !== undefined);
  const user = computed<JwtUser | undefined>(() => {
    if (token.value !== undefined) {
      return decodeJWT(token.value);
    }
    return undefined;
  });

  const token = ref<string>();
  const storeToken = () => {
    if (token.value !== undefined) {
      window.localStorage.setItem('jwt', token.value);
    } else {
      window.localStorage.removeItem('jwt');
    }
  };
  const restoreToken = () => {
    setToken(window.localStorage.getItem('jwt') ?? undefined);
  };
  const setToken = (newToken: string | undefined = undefined) => {
    if (newToken === undefined) {
      token.value = undefined;
    } else {
      const user = decodeJWT(newToken);
      if (user.exp * 1000 - Date.now() < 0) {
        return;
      }
      token.value = newToken;
      const msUntilExpiration = user.exp * 1000 - Date.now();
      setTimeout(setToken, msUntilExpiration);
    }
    storeToken();
  };

  const initCompleted = ref(false);
  const initIsLoading = ref(false);
  const initFailed = ref(false);
  const init = async () => {
    try {
      initIsLoading.value = true;
      initIdentityService(await configurationService.getIdpConfiguration());
      externalProviders.value = await identityService.value.getProviders();
      restoreToken();
      initFailed.value = false;
      initCompleted.value = true;
    } catch {
      initFailed.value = true;
    } finally {
      initIsLoading.value = false;
    }
  };

  const initIdentityService = (config: IdentityServiceConfig) => {
    identityService.value.axios.defaults.baseURL = config.issuer;
  };

  const externalAuthenticationIsLoading = ref<string>();
  const externalAuthenticationFailed = ref<string>();
  const triggerExternalAuthentication = async (
    providerKey: string,
    requestedRoute: RouteLocationRaw = { name: Routes.Home },
  ) => {
    const provider = externalProviders.value?.find((x) => x.providerKey === providerKey);
    if (provider === undefined) {
      throw new Error('External provider not found.');
    }
    const userManager = getOidcUserManager(provider, requestedRoute);
    try {
      externalAuthenticationIsLoading.value = providerKey;
      await userManager.signinRedirect();
      externalAuthenticationFailed.value = undefined;
    } catch {
      externalAuthenticationFailed.value = providerKey;
    } finally {
      externalAuthenticationIsLoading.value = undefined;
    }
  };

  const handleExternalAuthenticationCallback = async (providerKey: string, url: string) => {
    const provider = externalProviders.value?.find((x) => x.providerKey === providerKey);
    if (provider === undefined) {
      throw new Error('External provider not found.');
    }
    const userManager = getOidcUserManager(provider, url);

    try {
      externalAuthenticationIsLoading.value = provider.providerKey;
      const user = await userManager.signinCallback(url);
      if (user === undefined) {
        throw Error();
      }
      const token = await identityService.value.externalLogin({
        providerKey: providerKey,
        token: user.id_token!,
      });
      setToken(token);
      externalAuthenticationFailed.value = undefined;
    } catch {
      externalAuthenticationFailed.value = provider.providerKey;
    } finally {
      externalAuthenticationIsLoading.value = undefined;
    }
  };

  const logout = () => {
    setToken();
  };

  return {
    initIsLoading,
    initFailed,
    init,
    externalProviders,
    initCompleted,
    triggerExternalAuthentication,
    authCompleted,
    externalAuthenticationFailed,
    externalAuthenticationIsLoading,
    handleExternalAuthenticationCallback,
    user,
    token,
    logout,
  };
});

const getOidcUserManager = (provider: ExternalProvider, requestedRoute: RouteLocationRaw) => {
  const redirect_uri =
    window.location.origin +
    router.resolve({
      name: Routes.SignInCallback,
      query: {
        providerKey: provider.providerKey,
        requestedUrl: router.resolve(requestedRoute).fullPath,
      },
    }).fullPath;
  return new UserManager({
    redirect_uri: redirect_uri,
    userStore: undefined,
    automaticSilentRenew: false,
    loadUserInfo: false,
    scope: 'profile email openid',
    authority: provider.issuer,
    client_id: provider.audience,
  });
};

const decodeJWT = (token: string): JwtUser => {
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const jsonPayload = decodeURIComponent(
    window
      .atob(base64)
      .split('')
      .map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      })
      .join(''),
  );

  return JSON.parse(jsonPayload);
};
