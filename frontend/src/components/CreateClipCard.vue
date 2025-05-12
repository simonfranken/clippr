<script setup lang="ts">
import { clipboardService } from '@/services';
import {
  ClipboardDocumentListIcon,
  PaperAirplaneIcon,
  CheckBadgeIcon,
  LightBulbIcon,
} from '@heroicons/vue/24/outline';
import { ref, watch } from 'vue';
import { TransitionDirection, IconTransition } from './generic/IconTransition';

const inputTextArea = ref();

const emits = defineEmits<{
  (e: 'createSuccess'): void;
}>();

const textFieldIsEmpty = ref(true);
const onTextFieldInput = (e: Event) => {
  textFieldIsEmpty.value = (e.target as HTMLTextAreaElement).value === '';
};

const createIsLoading = ref(false);
const createHasFailed = ref(false);
const getTextContent = async (): Promise<string> => {
  if (!textFieldIsEmpty.value) {
    return (inputTextArea.value as HTMLTextAreaElement).value;
  }
  return await navigator.clipboard.readText();
};
const create = async () => {
  try {
    createIsLoading.value = true;
    await clipboardService.createText(await getTextContent());
    (inputTextArea.value as HTMLTextAreaElement).value = '';
    textFieldIsEmpty.value = true;
    createHasFailed.value = false;
    createSuccessful.value = true;
    emits('createSuccess');
    setTimeout(() => (createSuccessful.value = false), 1000);
  } catch {
    createHasFailed.value = true;
  } finally {
    createIsLoading.value = false;
  }
};

const createSuccessful = ref(false);

const transitionDirection = ref<TransitionDirection>(TransitionDirection.Left);
const icon = ref<'check' | 'copy' | 'confirm' | 'loading'>('copy');
const updateIcon = async () => {
  if (createSuccessful.value) {
    transitionDirection.value = TransitionDirection.Left;
    icon.value = 'check';
  } else if (createIsLoading.value) {
    transitionDirection.value = TransitionDirection.Left;
    icon.value = 'loading';
  } else if (textFieldIsEmpty.value) {
    transitionDirection.value = TransitionDirection.Right;
    icon.value = 'copy';
  } else {
    if (icon.value === 'check') {
      transitionDirection.value = TransitionDirection.Right;
    } else {
      transitionDirection.value = TransitionDirection.Left;
    }
    icon.value = 'confirm';
  }
};

const onKeyDownTextArea = (e: KeyboardEvent) => {
  if (e.key === 'Enter' && !e.shiftKey) {
    create();
  }
};

watch(createSuccessful, updateIcon);
watch(textFieldIsEmpty, updateIcon);
watch(createIsLoading, updateIcon);
</script>
<template>
  <div class="card card-sm bg-base-100 shadow-sm">
    <div class="card-body">
      <textarea
        ref="inputTextArea"
        class="textarea w-full resize-none h-fit"
        placeholder="Paste here"
        @input="onTextFieldInput"
        @keydown="onKeyDownTextArea"
      />
      <small class="mb-3 text-accent flex items-baseline">
        <LightBulbIcon class="size-2 me-1"></LightBulbIcon>
        Shift + Enter for line breaks
      </small>
      <div class="justify-end items-end card-actions">
        <button
          class="btn btn-sm"
          :class="{
            'btn-success': createSuccessful,
            'btn-primary': !createSuccessful,
          }"
          :disabled="createIsLoading"
          @click="create"
        >
          <IconTransition :icon-key="icon" :transition-direction="transitionDirection">
            <template #check>
              <CheckBadgeIcon class="size-4"> </CheckBadgeIcon>
            </template>
            <template #loading>
              <span class="loading loading-dots size-4"></span>
            </template>
            <template #copy>
              <ClipboardDocumentListIcon class="size-4"></ClipboardDocumentListIcon>
            </template>
            <template #confirm>
              <PaperAirplaneIcon class="size-4"></PaperAirplaneIcon>
            </template>
          </IconTransition>
        </button>
      </div>
    </div>
  </div>
</template>
