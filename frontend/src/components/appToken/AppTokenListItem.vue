<script setup lang="ts">
import type { AppToken } from '@/services/dtos';
import { formatDistance } from 'date-fns';
import { computed, ref, type PropType } from 'vue';
import { appTokenService } from '@/services';
import SubmitButton from '../SubmitButton/SubmitButton.vue';
import { ExclamationTriangleIcon, TrashIcon, XMarkIcon } from '@heroicons/vue/24/outline';

const props = defineProps({
  appToken: {
    type: Object as PropType<AppToken>,
  },
  skeleton: {
    type: Boolean,
    default: false,
  },
});

const emits = defineEmits<{
  (e: 'delete'): void;
}>();

const createdAt = computed(() => {
  if (props.appToken === undefined) {
    return '';
  }
  const datetime = new Date(props.appToken.createdAt);
  return formatDistance(datetime, new Date(), { addSuffix: true });
});
const expiresIn = computed(() => {
  if (props.appToken === undefined) {
    return '';
  }
  const datetime = new Date(props.appToken.expirationDate);
  return formatDistance(datetime, new Date(), { addSuffix: true });
});

const deleteLoading = ref(false);
const deleteFailed = ref(false);
const deleteToken = async () => {
  if (props.appToken === undefined) {
    return;
  }
  try {
    deleteLoading.value = true;
    await appTokenService.deleteAppToken(props.appToken.id);
    emits('delete');
    deleteFailed.value = false;
  } catch {
    deleteFailed.value = true;
  } finally {
    deleteLoading.value = false;
  }
};
</script>
<template>
  <li class="list-row">
    <div class="flex flex-col">
      <small> Created {{ createdAt }} </small>
      <small> Expires in {{ expiresIn }} </small>
    </div>
    <div class="w-full flex justify-end items-center">
      <SubmitButton
        :submit-is-loading="deleteLoading"
        :failed="deleteFailed"
        @submit="deleteToken"
        confirmation-required
        size="xs"
        outlined
        class="btn-square btn-error"
      >
        <template #default>
          <XMarkIcon class="size-3"></XMarkIcon>
        </template>
        <template #confirmation>
          <TrashIcon class="size-3"></TrashIcon>
        </template>
        <template #failed>
          <ExclamationTriangleIcon class="size-3"></ExclamationTriangleIcon>
        </template>
      </SubmitButton>
    </div>
  </li>
</template>
