<script setup lang="ts">
import { clipboardService } from '@/services';
import {
  PaperAirplaneIcon,
  CheckBadgeIcon,
  LightBulbIcon,
  ArrowUpTrayIcon,
  ExclamationTriangleIcon,
} from '@heroicons/vue/24/outline';
import { ref } from 'vue';
import { SubmitButton } from './SubmitButton';

const emits = defineEmits<{
  (e: 'createSuccess'): void;
}>();

const createTextIsLoading = ref(false);
const createTextHasFailed = ref(false);
const textAreaValue = ref('');
const createText = async () => {
  if (!textAreaValue.value.length) {
    return;
  }
  try {
    createTextIsLoading.value = true;
    await clipboardService.createText(textAreaValue.value);
    textAreaValue.value = '';
    createTextHasFailed.value = false;
    emits('createSuccess');
  } catch {
    createTextHasFailed.value = true;
  } finally {
    createTextIsLoading.value = false;
  }
};

const onKeyDownTextArea = async (e: KeyboardEvent) => {
  if (e.key === 'Enter' && !e.shiftKey) {
    await createText();
    e.preventDefault();
  }
};

const fileInput = ref();
const createFileIsLoading = ref(false);
const createFileHasFailed = ref(false);
const createFileHasSucceeded = ref(false);

const validateFile = (e: HTMLInputElement): boolean => {
  if (e.files?.length !== 1) {
    return false;
  }
  return true;
};
const clearFiles = (e: HTMLInputElement) => {
  e.files = null;
};
const selectFile = () => {
  (fileInput.value as HTMLInputElement).click();
};
const createFile = async () => {
  if ((fileInput.value as HTMLInputElement).files?.length !== 1) {
    return;
  }
  try {
    createFileHasSucceeded.value = false;
    createFileIsLoading.value = true;
    await clipboardService.createFile((fileInput.value as HTMLInputElement).files![0]);
    emits('createSuccess');
    createFileHasSucceeded.value = true;
    createFileHasFailed.value = false;
  } catch {
    createFileHasFailed.value = true;
  } finally {
    createFileIsLoading.value = false;
  }
};
</script>
<template>
  <div class="card card-sm bg-base-100 shadow-sm">
    <div class="card-body">
      <textarea
        ref="inputTextArea"
        class="textarea w-full resize-none h-fit"
        placeholder="Paste here"
        @keydown="onKeyDownTextArea"
        v-model="textAreaValue"
      />
      <small class="mb-3 text-accent flex items-baseline">
        <LightBulbIcon class="size-2 me-1"></LightBulbIcon>
        Shift + Enter for line breaks
      </small>
      <div class="justify-end items-end card-actions">
        <SubmitButton
          size="sm"
          class="btn-secondary"
          outlined
          @submit="selectFile"
          :loading="createFileIsLoading"
          :failed="createFileHasFailed"
          :disabled="createTextIsLoading"
          :succeeded="createFileHasSucceeded"
        >
          <template #default>
            <ArrowUpTrayIcon class="size-4"></ArrowUpTrayIcon>
          </template>
          <template #success>
            <CheckBadgeIcon class="size-4"></CheckBadgeIcon>
          </template>
          <template #failed>
            <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
          </template>
        </SubmitButton>
        <input
          ref="fileInput"
          type="file"
          class="hidden"
          @cancel="createFile"
          @change="createFile"
        />
        <SubmitButton
          size="sm"
          class="btn-primary"
          @submit="createText"
          auto-succeed
          :loading="createTextIsLoading"
          :failed="createTextHasFailed"
          :disabled="createFileIsLoading || textAreaValue.length === 0"
        >
          <template #default>
            <PaperAirplaneIcon class="size-4"></PaperAirplaneIcon>
          </template>
          <template #success>
            <CheckBadgeIcon class="size-4"> </CheckBadgeIcon>
          </template>
          <template #failed>
            <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
          </template>
        </SubmitButton>
      </div>
    </div>
  </div>
</template>
