<script setup lang="ts">
import { ClipType, type Clip } from '@/services/dtos';
import { ClipboardDocumentCheckIcon, ClipboardDocumentIcon } from '@heroicons/vue/24/outline';
import { formatDistance } from 'date-fns';
import { computed, ref, type PropType } from 'vue';
import { IconTransition } from './generic/IconTransition';

const props = defineProps({
  clip: {
    type: Object as PropType<Clip>,
  },
  skeleton: {
    type: Boolean,
    default: false,
  },
});

const getText = (): string => {
  if (props.clip === undefined) {
    return '';
  }
  return atob(props.clip.base64Data);
};

const getHtmlText = () => {
  return getText().replace(/\n/gm, '<br>');
};

const relativeTime = computed((): string => {
  if (props.clip === undefined) {
    return '';
  }
  const datetime = new Date(props.clip.createdAt);
  return formatDistance(datetime, new Date(), { addSuffix: true });
});

const copySuccessful = ref(false);
const onCopy = () => {
  if (props.clip === undefined) {
    return;
  }
  if (props.clip.type === ClipType.Text) {
    navigator.clipboard.writeText(getText());
  }
  copySuccessful.value = true;
  setTimeout(() => (copySuccessful.value = false), 1000);
};
</script>
<template>
  <div
    class="card card-sm bg-base-100 shadow-sm cursor-pointer border border-transparent hover:border-base-300"
    @click="onCopy"
  >
    <div class="card-body">
      <div class="mb-3 max-h-80 overflow-auto">
        <div v-if="skeleton" class="skeleton w-full h-4"></div>
        <p v-else v-html="getHtmlText()"></p>
      </div>
      <div class="justify-between items-end card-actions relative">
        <div v-if="skeleton" class="skeleton w-14 h-3"></div>
        <small v-else class="text-accent">{{ relativeTime }}</small>
        <div v-if="skeleton" class="skeleton h-8 w-11"></div>
        <button
          v-else
          class="btn btn-sm"
          :class="{
            'btn-success': copySuccessful,
          }"
          @click.stop="onCopy"
        >
          <IconTransition :icon-key="copySuccessful ? 'success' : 'copy'">
            <template #copy>
              <ClipboardDocumentIcon class="size-4" />
            </template>
            <template #success>
              <ClipboardDocumentCheckIcon class="size-4" />
            </template>
          </IconTransition>
        </button>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
