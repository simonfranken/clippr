<script setup lang="ts">
import { computed, nextTick, onMounted, ref, useTemplateRef, watch, type PropType } from 'vue';
import ClipCard from './ClipCard.vue';
import type { Clip } from '@/services/dtos';
import { ExclamationTriangleIcon } from '@heroicons/vue/24/outline';

const props = defineProps({
  clips: {
    type: Array as PropType<Clip[]>,
    default: () => [],
  },
  skeleton: {
    type: Boolean,
    default: false,
  },
  showFailedLoading: {
    type: Boolean,
    default: false,
  },
  columnCount: {
    type: Number,
    default: 1,
  },
});

const sortedClips = computed(() =>
  [...props.clips].sort((a, b) => Date.parse(b.createdAt) - Date.parse(a.createdAt)),
);

const columns = ref<{ columnIndex: number; clips: Clip[] }[]>([]);
const initializeColumns = () => {
  columns.value = [...Array(props.columnCount).keys()].map((x) => ({
    columnIndex: x,
    clips: [],
  }));
  fillItems();
};
watch(sortedClips, initializeColumns);
watch(() => props.columnCount, initializeColumns);
onMounted(() => {
  initializeColumns();
});

const columnUIElements = useTemplateRef('columnElements');

const getSmallestColumn = () =>
  columnUIElements.value?.reduce((prev, curr, i) => {
    if (columnUIElements.value![prev].clientHeight > curr.clientHeight) {
      return i;
    }
    return prev;
  }, 0) ?? 0;

const fillItems = async () => {
  for (const clip of sortedClips.value) {
    await nextTick();
    columns.value[getSmallestColumn()].clips.push(clip);
  }
};
</script>
<template>
  <div class="overflow-hidden flex flex-col max-h-full max-w-full relative">
    <div class="grow overflow-auto" ref="container">
      <div class="flex gap-3">
        <template v-if="showFailedLoading">
          <div role="alert" class="alert alert-error grow">
            <ExclamationTriangleIcon class="size-4"></ExclamationTriangleIcon>
            <span>Error! Failed loading clips.</span>
          </div>
        </template>
        <template v-else>
          <div
            ref="columnElements"
            v-for="x in columns"
            :key="x.columnIndex"
            class="grow basis-0 flex flex-col gap-3 h-min p-1"
          >
            <template v-if="skeleton">
              <ClipCard v-for="x in Array(2).keys()" :key="`item-${x}`" skeleton></ClipCard>
            </template>
            <template v-else>
              <ClipCard v-for="c in x.clips" :key="`item-${c.id}`" :clip="c"></ClipCard>
            </template>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>
