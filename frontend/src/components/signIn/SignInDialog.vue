<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { storeToRefs } from 'pinia';
import { SubmitButton } from '../SubmitButton';
import {
  ArrowRightEndOnRectangleIcon,
  CheckBadgeIcon,
  ExclamationTriangleIcon,
} from '@heroicons/vue/24/outline';
import { reactive, watch } from 'vue';
import { KeyIcon, UserIcon } from '@heroicons/vue/24/solid';

const authStore = useAuthStore();

const opened = defineModel('opened', {
  type: Boolean,
  required: true,
});

const {
  externalProviders,
  externalAuthenticationIsLoading,
  externalAuthenticationFailed,
  authCompleted,
  loginIsLoading,
  loginHasFailed,
  enableInteralAuth,
} = storeToRefs(authStore);

const triggerExternalAuthentication = (providerKey: string) => {
  authStore.triggerExternalAuthentication(providerKey);
};

const loginForm = reactive({
  email: '',
  password: '',
});
const login = () => {
  authStore.login(loginForm.email, loginForm.password);
};

watch([authCompleted, opened], () => {
  if (authCompleted.value) {
    opened.value = false;
  }
});
</script>
<template>
  <div class="modal-box">
    <h3 class="text-lg font-bold">Sign In</h3>
    <div class="flex flex-col items-center">
      <div class="w-full">
        <small class="text-accent">Use social authentication</small>
        <div class="flex justify-evenly pt-4">
          <SubmitButton
            v-for="provider in externalProviders"
            class="btn-primary"
            :key="provider.providerKey"
            :loading="externalAuthenticationIsLoading === provider.providerKey"
            :failed="externalAuthenticationFailed === provider.providerKey"
            @submit="() => triggerExternalAuthentication(provider.providerKey)"
            auto-succeed
          >
            <template #default>
              <ArrowRightEndOnRectangleIcon class="size-4"></ArrowRightEndOnRectangleIcon>
            </template>
            <template #success>
              <CheckBadgeIcon class="size-4"></CheckBadgeIcon>
            </template>
            <template #failed>
              <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
            </template>
            <template #label-right>{{ provider.providerKey }}</template>
          </SubmitButton>
        </div>
      </div>
      <div v-if="externalProviders?.length && enableInteralAuth" class="divider">or</div>
      <form v-if="enableInteralAuth" class="flex flex-col gap-2 max-w-72" @submit.prevent="login">
        <label class="input w-full">
          <UserIcon class="size-3"></UserIcon>
          <input
            required
            v-model="loginForm.email"
            name="email"
            type="email"
            class="grow"
            placeholder="Email"
          />
        </label>
        <label class="input w-full">
          <KeyIcon class="size-3"></KeyIcon>
          <input
            v-model="loginForm.password"
            required
            name="password"
            type="password"
            class="grow"
            placeholder="Password"
          />
        </label>
        <SubmitButton
          type="submit"
          class="btn-primary"
          :failed="loginHasFailed"
          :loading="loginIsLoading"
          auto-succeed
        >
          <template #default>
            <ArrowRightEndOnRectangleIcon class="size-4"></ArrowRightEndOnRectangleIcon>
          </template>
          <template #success>
            <CheckBadgeIcon class="size-4"></CheckBadgeIcon>
          </template>
          <template #failed>
            <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
          </template>
          <template #label-right>Sign In</template>
        </SubmitButton>
      </form>
    </div>
  </div>
</template>
