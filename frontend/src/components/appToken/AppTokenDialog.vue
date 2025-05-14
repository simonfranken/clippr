<script setup lang="ts">
import { useAppTokenStore } from '@/stores/appTokens';
import { ref, watch } from 'vue';
import AppTokenListItem from './AppTokenListItem.vue';
import { SubmitButton } from '../SubmitButton';
import {
  CheckBadgeIcon,
  ClipboardDocumentCheckIcon,
  ClipboardDocumentIcon,
  ExclamationTriangleIcon,
} from '@heroicons/vue/24/outline';
import { appTokenService } from '@/services';
import { SparklesIcon } from '@heroicons/vue/24/solid';

const props = defineProps({
  opened: {
    type: Boolean,
    required: true,
  },
});

const appTokenStore = useAppTokenStore();

const createIsLoading = ref(false);
const createHasFailed = ref(false);
const createAppToken = async () => {
  try {
    createIsLoading.value = true;
    createdToken.value = await appTokenService.createAppToken();
    createHasFailed.value = false;
    appTokenStore.fetchAppTokens();
  } catch {
    createHasFailed.value = true;
  } finally {
    createIsLoading.value = false;
  }
};

const copyToken = () => {
  navigator.clipboard.writeText(createdToken.value);
};

const createdToken = ref('');

watch(
  () => props.opened,
  async (value) => {
    if (value) {
      appTokenStore.fetchAppTokens();
    }
  },
);
</script>
<template>
  <div class="modal-box">
    <h3 class="text-lg font-bold">App tokens</h3>
    <div class="mb-7">
      <small class="text-accent"
        >App tokens can be used to access clippr from other applications.</small
      >
    </div>
    <div class="w-full">
      <div class="flex justify-between mb-5 gap-3">
        <input
          class="input input-sm w-full"
          :class="{ invisible: createdToken.length === 0 }"
          type="text"
          disabled
          v-model="createdToken"
        />
        <SubmitButton
          class="btn-secondary btn-square"
          :class="{ invisible: createdToken.length === 0 }"
          size="sm"
          @submit="copyToken"
          outlined
        >
          <template #default>
            <ClipboardDocumentIcon class="size-4"></ClipboardDocumentIcon>
          </template>
          <template #success>
            <ClipboardDocumentCheckIcon class="size-4"></ClipboardDocumentCheckIcon>
          </template>
        </SubmitButton>
        <SubmitButton
          class="btn-primary"
          size="sm"
          @submit="createAppToken"
          :submit-is-loading="createIsLoading"
          :failed="createHasFailed"
        >
          <template #label>New</template>
          <template #default>
            <SparklesIcon class="size-4"></SparklesIcon>
          </template>
          <template #failed>
            <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
          </template>
          <template #success>
            <CheckBadgeIcon class="size-4"></CheckBadgeIcon>
          </template>
        </SubmitButton>
      </div>
      <ul v-if="appTokenStore.appTokens?.length" class="list -m-4">
        <AppTokenListItem
          v-for="token in appTokenStore.appTokens"
          :key="token.id"
          :app-token="token"
          @delete="appTokenStore.fetchAppTokens"
        ></AppTokenListItem>
      </ul>
      <ul v-else-if="appTokenStore.appTokensAreLoading" class="list -m-4">
        <AppTokenListItem v-for="x in Array(3).keys()" :key="x" skeleton></AppTokenListItem>
      </ul>
      <div
        v-else-if="appTokenStore.loadingAppTokensFailed"
        role="alert"
        class="alert alert-error grow"
      >
        <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
        <span>Error! Failed loading app tokens.</span>
      </div>
      <small v-else-if="appTokenStore.appTokens?.length === 0" class="text-center text-accent"
        >No app tokens have been generated yet</small
      >
    </div>
    <div class="modal-action">
      <form method="dialog">
        <!-- if there is a button in form, it will close the modal -->
        <button class="btn">Close</button>
      </form>
    </div>
  </div>
</template>
