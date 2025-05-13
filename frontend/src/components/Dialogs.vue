<!-- eslint-disable vue/multi-word-component-names -->
<script setup lang="ts">
import { computed, onMounted, reactive, useTemplateRef, watch, type ShallowRef } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import AppTokenDialog from './appToken/AppTokenDialog.vue';

interface Dialog {
  name: string;
  queryParam: string;
  component: Readonly<ShallowRef<HTMLDialogElement | null>> | HTMLDialogElement | null;
  opened: boolean;
}

const dialogs = reactive<Dialog[]>([
  {
    name: 'appToken',
    queryParam: 'appTokenDialog',
    component: useTemplateRef('appTokenDialog'),
    opened: false,
  },
]);

const route = useRoute();
const router = useRouter();
watch(
  () => route.query,
  () => {
    dialogs
      .filter((d) => route.query[d.queryParam] !== undefined)
      .forEach((d) => {
        if (route.query[d.queryParam] === String(true)) {
          (d.component as HTMLDialogElement).showModal();
          d.opened = true;
        } else {
          (d.component as HTMLDialogElement).close();
          d.opened = false;
        }
      });
  },
);

const onCloseDialog = (d: Dialog) => {
  router.push({
    query: {
      ...route.query,
      [d.queryParam]: undefined,
    },
  });
  d.opened = false;
};

const initCloseEventListener = () => {
  dialogs.forEach((d) => {
    d.component!.addEventListener('close', () => onCloseDialog(d));
  });
};

const appTokenDialogOpened = computed(
  () => dialogs.find((x) => x.name === 'appToken')?.opened ?? false,
);

onMounted(initCloseEventListener);
</script>
<template>
  <div>
    <dialog ref="appTokenDialog" class="modal">
      <AppTokenDialog :opened="appTokenDialogOpened"></AppTokenDialog>
    </dialog>
  </div>
</template>
