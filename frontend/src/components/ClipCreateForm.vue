<script setup lang="ts">
import { clipboardService } from '@/services';
import {
  ClipboardDocumentListIcon,
  PaperAirplaneIcon,
  CheckBadgeIcon,
} from '@heroicons/vue/24/outline';
import { ref } from 'vue';

const inputTextArea = ref();

const emits = defineEmits<{
  (e: 'submit'): void;
}>();

const textFieldIsEmpty = ref(true);
const onTextFieldInput = (e: Event) => {
  textFieldIsEmpty.value = (e.target as HTMLTextAreaElement).value === '';
};

const submitIsLoading = ref(false);
const submitIsFailed = ref(false);
const getTextContent = async (): Promise<string> => {
  if (!textFieldIsEmpty.value) {
    return (inputTextArea.value as HTMLTextAreaElement).value;
  }
  return await navigator.clipboard.readText();
};
const onSubmit = async () => {
  try {
    submitIsLoading.value = true;
    await clipboardService.createText(await getTextContent());
    (inputTextArea.value as HTMLTextAreaElement).value = '';
    textFieldIsEmpty.value = true;
    submitIsFailed.value = false;
    submitSuccessful.value = true;
    setTimeout(() => (submitSuccessful.value = false), 1000);
  } catch {
    submitIsFailed.value = true;
  } finally {
    submitIsLoading.value = false;
  }
  emits('submit');
};

const submitSuccessful = ref(false);
</script>
<template>
  <div class="card card-sm bg-base-100 shadow-sm">
    <div class="card-body">
      <textarea
        ref="inputTextArea"
        class="textarea w-full resize-none h-fit mb-3"
        placeholder="Paste here"
        @input="onTextFieldInput"
      />
      <div class="justify-end items-end card-actions">
        <button
          class="btn btn-sm relative btn-primary"
          :class="{
            '!btn-success': submitSuccessful,
          }"
          :disabled="submitIsLoading && submitSuccessful"
          @click="onSubmit"
        >
          <CheckBadgeIcon
            class="absolute size-4 transition-opacity duration-500 ease-in-out"
            :class="{
              'opacity-0': !submitSuccessful,
            }"
          >
          </CheckBadgeIcon>
          <ClipboardDocumentListIcon
            class="size-4 transition-opacity duration-500 ease-in-out"
            :class="{
              'opacity-0': !textFieldIsEmpty || submitSuccessful,
            }"
          ></ClipboardDocumentListIcon>
          <PaperAirplaneIcon
            class="size-4 absolute transition-opacity duration-500 ease-in-out"
            :class="{
              'opacity-0': textFieldIsEmpty || submitSuccessful,
            }"
          ></PaperAirplaneIcon>
        </button>
      </div>
    </div>
  </div>
</template>
