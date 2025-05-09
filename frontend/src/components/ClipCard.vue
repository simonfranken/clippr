<script setup lang="ts">
import type { Clip } from '@/services/dtos';
import { formatDistance } from 'date-fns';
import { computed, type PropType } from 'vue';

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
</script>
<template>
  <div class="card card-sm bg-base-100 shadow-sm">
    <div class="card-body">
      <p>
        {{ getText() }}
      </p>
      <div class="justify-between items-end card-actions">
        <small class="text-accent">{{ relativeTime }}</small>
        <button class="btn btn-sm">Copy</button>
      </div>
    </div>
  </div>
</template>
