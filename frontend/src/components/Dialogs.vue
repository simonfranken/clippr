<!-- eslint-disable vue/multi-word-component-names -->
<script setup lang="ts">
import {
  onMounted,
  ref,
  watch,
  type Component,
  type ComputedOptions,
  type MethodOptions,
} from 'vue';
import AppTokenDialog from './appToken/AppTokenDialog.vue';
import { useRoute, useRouter, type LocationQuery } from 'vue-router';
import SignInDialog from './signIn/SignInDialog.vue';

interface Dialog {
  key: string;
  queryParam: string;
  component: Component<
    { opened: boolean },
    never,
    never,
    ComputedOptions,
    MethodOptions,
    { 'update:opened': (value: boolean) => never }
  >;
  opened: boolean;
  dialog?: HTMLDialogElement;
}

const dialogs = ref<Dialog[]>([
  {
    key: 'appToken',
    component: AppTokenDialog,
    opened: false,
    queryParam: 'showAppTokenDialog',
  },
  {
    key: 'signIn',
    component: SignInDialog,
    opened: false,
    queryParam: 'showSignInDialog',
  },
]);

const initEventListeners = () => {
  dialogs.value.forEach((d) => d.dialog?.addEventListener('close', () => updateOpened(d, false)));
};

const router = useRouter();
const route = useRoute();
const updateOpened = (d: Dialog, opened: boolean) => {
  if (opened) {
    d.dialog?.show();
  } else {
    d.dialog?.close();
  }
  d.opened = opened;
  router.replace({
    query: {
      ...route.query,
      [d.queryParam]: String(opened),
    },
  });
};

const updateFromUrl = (query: LocationQuery) => {
  dialogs.value
    .filter((d) => (route.query[d.queryParam] === 'true') !== d.opened)
    .forEach((d) => updateOpened(d, query[d.queryParam] === 'true'));
};
watch(() => route.query, updateFromUrl);
onMounted(() => {
  initEventListeners();
  updateFromUrl(route.query);
});
</script>
<template>
  <div>
    <div v-for="d in dialogs" :key="d.key">
      <dialog class="modal" :ref="(el) => (d.dialog = el as HTMLDialogElement)">
        <component
          :is="d.component"
          :opened="d.opened"
          @update:opened="(opened: boolean) => updateOpened(d, opened)"
        ></component>
      </dialog>
    </div>
  </div>
</template>
