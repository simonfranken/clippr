import { clipboardService } from '@/services';
import type { Clip } from '@/services/dtos';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useClipboardStore = defineStore('clipboard', () => {
  const clips = ref<Clip[]>();

  const fetchClips = async () => {
    try {
      clipsAreLoading.value = true;
      clips.value = await clipboardService.get();
      clipLoadingHasFailed.value = false;
    } catch {
      clipLoadingHasFailed.value = true;
    } finally {
      clipsAreLoading.value = false;
    }
  };
  const clipsAreLoading = ref(false);
  const clipLoadingHasFailed = ref(false);

  return { clips, clipsAreLoading, clipLoadingHasFailed, fetchClips };
});
