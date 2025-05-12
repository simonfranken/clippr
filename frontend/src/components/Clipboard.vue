<!-- eslint-disable vue/multi-word-component-names -->
<script setup lang="ts">
import ClipContainer from '@/components/ClipContainer.vue';
import { useClipboardStore } from '@/stores/clipboard';
import { computed, onMounted, ref } from 'vue';
import CreateClipCard from './CreateClipCard.vue';

const clipboardStore = useClipboardStore();
const columnCount = ref(1);
const clipContainerColumnCount = computed(() => Math.max(columnCount.value - 1, 1));

const container = ref();
const containerObserver = new ResizeObserver((entries) => {
  const width = entries.find((entry) => entry.target === container.value)!.borderBoxSize[0]
    .inlineSize;
  if (width >= 800) {
    columnCount.value = 4;
  } else if (width >= 700) {
    columnCount.value = 3;
  } else if (width >= 600) {
    columnCount.value = 2;
  } else {
    columnCount.value = 1;
  }
});
onMounted(() => {
  clipboardStore.fetchClips();
  containerObserver.observe(container.value);
});
</script>
<template>
  <div class="overflow-auto max-h-full gap-3" :class="{ flex: columnCount > 1 }" ref="container">
    <div class="basis-0 grow mb-3" :class="{ 'max-w-[24rem]': columnCount > 1 }">
      <CreateClipCard @create-success="clipboardStore.fetchClips"></CreateClipCard>
    </div>
    <ClipContainer
      class="basis-0 grow"
      :clips="clipboardStore.clips"
      :style="{
        flexGrow: clipContainerColumnCount,
      }"
      :skeleton="clipboardStore.clipsAreLoading"
      :show-failed-loading="clipboardStore.clipLoadingHasFailed"
      :column-count="clipContainerColumnCount"
    ></ClipContainer>
  </div>
</template>
