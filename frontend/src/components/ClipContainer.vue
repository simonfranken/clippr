<script setup lang="ts">
import { computed, nextTick, onMounted, ref, useTemplateRef, watch, type PropType } from 'vue';
import ClipCard from './ClipCard.vue';
import type { Clip } from '@/services/dtos';
import ClipCreateForm from './ClipCreateForm.vue';

const props = defineProps({
  clips: {
    type: Array as PropType<Clip[]>,
    default: () => [],
  },
  createForm: {
    type: Boolean,
    default: true,
  },
});

defineEmits<{
  (e: 'createClip'): void;
}>();

const sortedClips = computed(() =>
  [...props.clips].sort((a, b) => Date.parse(b.createdAt) - Date.parse(a.createdAt)),
);

const container = ref();
const containerObserver = new ResizeObserver((entries) => {
  const width = entries.find((entry) => entry.target === container.value)!.borderBoxSize[0]
    .inlineSize;
  if (width >= 800) {
    updateColumnCount(3);
  } else if (width >= 600) {
    updateColumnCount(2);
  } else {
    updateColumnCount(1);
  }
});

const updateColumnCount = (newCount: number) => {
  if (newCount !== columnCount.value) {
    columnCount.value = newCount;
  }
};

const columnCount = ref(1);
const columns = ref<{ columnIndex: number; clips: Clip[] }[]>([]);
const initializeColumns = () => {
  columns.value = [...Array(columnCount.value).keys()].map((x) => ({
    columnIndex: x,
    clips: [],
  }));
  fillItems();
};
watch(sortedClips, initializeColumns);
watch(columnCount, initializeColumns);
onMounted(() => {
  initializeColumns();
  containerObserver.observe(container.value);
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
        <div
          ref="columnElements"
          v-for="(x, i) in columns"
          :key="x.columnIndex"
          class="grow basis-0 flex flex-col gap-3 h-min p-1"
        >
          <ClipCreateForm
            v-if="createForm && i == 0"
            @submit="$emit('createClip')"
          ></ClipCreateForm>
          <ClipCard v-for="c in x.clips" :key="`item-${c.id}`" :clip="c"></ClipCard>
        </div>
      </div>
    </div>
  </div>
</template>
