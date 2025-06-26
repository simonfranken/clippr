<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { storeToRefs } from 'pinia';
import { SubmitButton } from '../SubmitButton';
import {
  ArrowRightEndOnRectangleIcon,
  CheckBadgeIcon,
  ExclamationTriangleIcon,
} from '@heroicons/vue/24/outline';
import { watch } from 'vue';
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
} = storeToRefs(authStore);

const triggerExternalAuthentication = (providerKey: string) => {
  authStore.triggerExternalAuthentication(providerKey);
};

watch(authCompleted, () => (opened.value = false));
</script>
<template>
  <div class="modal-box">
    <h3 class="text-lg font-bold">Sign in</h3>
    <div class="mb-7">
      <small class="text-accent">Use social authentication</small>
      <div class="flex flex-col items-center">
        <SubmitButton
          v-for="provider in externalProviders"
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
          <template #label-right> {{ provider.providerKey }} </template>
        </SubmitButton>
      </div>
    </div>
  </div>
</template>
