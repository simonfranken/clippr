<script setup lang="ts">
import { ClipType, type Clip } from '@/services/dtos';
import {
  CheckBadgeIcon,
  ClipboardDocumentCheckIcon,
  ClipboardDocumentIcon,
} from '@heroicons/vue/24/outline';
import { formatDistance } from 'date-fns';
import { computed, ref, type PropType } from 'vue';

const props = defineProps({
  clip: {
    type: Object as PropType<Clip>,
    required: true,
  },
});

const getText = (): string => {
  return atob(props.clip.base64Data);
};

const relativeTime = computed((): string => {
  const datetime = new Date(props.clip.createdAt);
  return formatDistance(datetime, new Date(), { addSuffix: true });
});

const copySuccessful = ref(false);
const onCopy = () => {
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
      <p v-html="getText()"></p>
      <div class="justify-between items-end card-actions relative">
        <small class="text-accent">{{ relativeTime }}</small>
        <button
          class="btn btn-sm"
          :class="{
            'btn-success': copySuccessful,
          }"
          @click.stop="onCopy"
        >
          <ClipboardDocumentCheckIcon
            class="size-4 transition-opacity duration-500 ease-in-out"
            :class="{
              'opacity-0': !copySuccessful,
            }"
          />
          <ClipboardDocumentIcon
            class="size-4 absolute transition-opacity duration-500 ease-in-out"
            :class="{
              'opacity-0': copySuccessful,
            }"
          />
        </button>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
