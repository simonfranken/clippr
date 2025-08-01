<script lang="ts" setup>
import { onMounted, ref, watch, type PropType } from 'vue';
import { IconTransition, TransitionDirection } from '../generic/IconTransition';

const props = defineProps({
  loading: {
    type: Boolean,
    default: false,
  },
  disabled: {
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
  requiresConfirmation: {
    type: Boolean,
    default: false,
  },
  succeeded: {
    type: Boolean,
  },
  autoSucceed: {
    type: Boolean,
    default: false,
  },
  size: {
    type: String as PropType<'xs' | 'sm' | 'default'>,
    default: () => 'default',
  },
  type: {
    type: String as PropType<'submit' | 'button'>,
    default: () => 'button',
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

const handleClick = (submit: boolean = false) => {
  if (props.type === 'submit' && !submit) {
    return;
  }
  if (showSuccess.value) {
    return;
  }
  if (awaitingConfirmation.value || !props.requiresConfirmation) {
    emits('submit');
    if (props.autoSucceed) {
      setSucceeded();
    }
  } else if (props.requiresConfirmation) {
    awaitingConfirmation.value = true;
    setTimeout(() => (awaitingConfirmation.value = false), 2000);
  }
};

const awaitingConfirmation = ref(false);
const showSuccess = ref(false);

const updateIcons = () => {
  if (props.loading) {
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
  } else if (showSuccess.value) {
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
  showSuccess.value = true;
  setTimeout(() => (showSuccess.value = false), 1000);
};

watch(
  () => props.succeeded,
  (value) => {
    if (value) {
      setSucceeded();
    }
  },
);
watch(
  () => props.loading,
  (value) => {
    if (!value && props.autoSucceed) {
      setSucceeded();
    }
    updateIcons();
  },
);

watch(() => props.failed, updateIcons);
watch(() => props.failed, updateIcons);
watch(showSuccess, updateIcons);
watch(awaitingConfirmation, updateIcons);
onMounted(updateIcons);
</script>
<template>
  <button
    :type="type"
    class="btn"
    :class="{
      'btn-success': showSuccess,
      '!btn-error': failed,
      'btn-outline': outlined && icon == Icons.Default,
      'btn-sm': props.size === 'sm',
      'btn-xs': props.size === 'xs',
    }"
    :disabled="loading || (disabled && !showSuccess)"
    @click="() => handleClick(false)"
    @submit="() => handleClick(true)"
  >
    <slot name="label-left"></slot>
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
    <slot name="label-right"></slot>
  </button>
</template>
