<script lang="ts" setup>
import { onMounted, ref, watch, type PropType } from 'vue';
import { IconTransition, TransitionDirection } from '../generic/IconTransition';

const props = defineProps({
  submitIsLoading: {
    type: Boolean,
    default: false,
  },
  outlined: {
    type: Boolean,
    default: false,
  },
  failed: {
    type: Boolean,
    default: false,
  },
  confirmationRequired: {
    type: Boolean,
    default: false,
  },
  size: {
    type: String as PropType<'xs' | 'sm' | 'default'>,
    default: () => 'default',
  },
});

const emits = defineEmits(['submit']);

enum Icons {
  Default = 'default',
  Confirmation = 'confirmation',
  Success = 'success',
  Loading = 'loading',
  Failed = 'failed',
}

const onClick = () => {
  if (awaitingConfirmation.value || !props.confirmationRequired) {
    emits('submit');
    setSucceeded();
  } else if (props.confirmationRequired) {
    awaitingConfirmation.value = true;
    setTimeout(() => (awaitingConfirmation.value = false), 2000);
  }
};

const awaitingConfirmation = ref(false);
const succeeded = ref(false);

const updateIcons = () => {
  if (props.submitIsLoading) {
    transitionDirection.value = TransitionDirection.Left;
    icon.value = Icons.Loading;
  } else if (awaitingConfirmation.value) {
    transitionDirection.value = TransitionDirection.Left;
    icon.value = Icons.Confirmation;
  } else if (props.failed) {
    if (icon.value === Icons.Confirmation) {
      transitionDirection.value = TransitionDirection.Right;
    } else {
      transitionDirection.value = TransitionDirection.Left;
    }
    icon.value = Icons.Failed;
  } else if (succeeded.value) {
    transitionDirection.value = TransitionDirection.Left;
    icon.value = Icons.Success;
  } else {
    transitionDirection.value = TransitionDirection.Right;
    icon.value = Icons.Default;
  }
};
const icon = ref(Icons.Default);
const transitionDirection = ref(TransitionDirection.Left);

const setSucceeded = () => {
  succeeded.value = true;
  setTimeout(() => (succeeded.value = false), 1000);
};

watch(
  () => props.submitIsLoading,
  (value) => {
    if (!value && !props.failed) {
      setSucceeded();
    }
    updateIcons();
  },
);

watch(() => props.failed, updateIcons);
watch(() => props.failed, updateIcons);
watch(succeeded, updateIcons);
watch(awaitingConfirmation, updateIcons);
onMounted(updateIcons);
</script>
<template>
  <button
    class="btn"
    :class="{
      'btn-success': succeeded,
      'btn-outline': outlined && icon == Icons.Default,
      'btn-sm': props.size === 'sm',
      'btn-xs': props.size === 'xs',
    }"
    :disabled="submitIsLoading"
    @click="onClick"
  >
    <slot name="label"></slot>
    <IconTransition :icon-key="icon" :transition-direction="transitionDirection">
      <template #default>
        <slot :name="Icons.Default"></slot>
      </template>
      <template #confirmation>
        <slot :name="Icons.Confirmation"></slot>
      </template>
      <template #success>
        <slot :name="Icons.Success"></slot>
      </template>
      <template #failed>
        <slot :name="Icons.Failed"></slot>
      </template>
      <template #loading>
        <span
          class="loading loading-dots"
          :class="{
            'loading-xs': props.size === 'xs',
            'loading-sm': props.size === 'sm',
          }"
        ></span>
      </template>
    </IconTransition>
  </button>
</template>
