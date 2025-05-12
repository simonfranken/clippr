<script setup lang="ts">
import { nextTick, ref, watch, type PropType } from 'vue';
import { TransitionDirection } from './transitionDirection';

const props = defineProps({
  iconKey: {
    type: String,
    required: true,
  },
  transitionDirection: {
    type: String as PropType<TransitionDirection>,
  },
});

watch(
  () => props.iconKey,
  async (value) => {
    if (activeIcon.value === 'a') {
      if (props.transitionDirection === TransitionDirection.Right) {
        setTransitionDirectionRight();
      } else {
        setTransitionDirectionLeft();
      }
      await nextTick();
      iconB.value = value;
      activeIcon.value = 'b';
    } else {
      if (props.transitionDirection === TransitionDirection.Left) {
        setTransitionDirectionLeft();
      } else {
        setTransitionDirectionRight();
      }
      await nextTick();
      iconA.value = value;
      activeIcon.value = 'a';
    }
  },
);

const iconA = ref(props.iconKey);
const iconB = ref('');

const activeIcon = ref<'a' | 'b'>('a');

const enterFromTransform = ref('');
const enterToTransform = ref('');
const setTransitionDirectionLeft = () => {
  enterFromTransform.value = 'translateX(100%)';
  enterToTransform.value = 'translateX(-100%)';
};
const setTransitionDirectionRight = () => {
  enterFromTransform.value = 'translateX(-100%)';
  enterToTransform.value = 'translateX(100%)';
};
</script>
<template>
  <div class="relative">
    <Transition>
      <slot v-if="activeIcon == 'a'" :name="iconA"></slot>
    </Transition>
    <Transition>
      <slot v-if="activeIcon == 'b'" :name="iconB"></slot>
    </Transition>
  </div>
</template>
<style scoped>
.v-enter-active,
.v-leave-active {
  transition-duration: 250ms;
  transition-timing-function: ease-in-out;
  transition-property: opacity, transform;
}
.v-leave-active {
  top: 0;
  position: absolute;
}
.v-enter-from {
  transform: v-bind(enterFromTransform);
  opacity: 0;
}
.v-leave-to {
  transform: v-bind(enterToTransform);
  opacity: 0;
}
</style>
