<!-- eslint-disable vue/multi-word-component-names -->
<script setup lang="ts">
import ClipContainer from '@/components/ClipContainer.vue';
import { useClipboardStore } from '@/stores/clipboard';
import { onMounted } from 'vue';

const clipboardStore = useClipboardStore();
onMounted(clipboardStore.fetchClips);
</script>
<template>
  <ClipContainer
    :clips="clipboardStore.clips"
    @create-clip="clipboardStore.fetchClips"
  ></ClipContainer>
</template>
