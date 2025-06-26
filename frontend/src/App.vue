<script setup lang="ts">
import { RouterLink, RouterView, useRouter, type RouteLocationRaw } from 'vue-router';
import { Routes } from './router/routes';
import Dialogs from './components/Dialogs.vue';
import { computed } from 'vue';
import { storeToRefs } from 'pinia';
import { useAuthStore } from './stores/auth';
import { ArrowRightStartOnRectangleIcon, KeyIcon } from '@heroicons/vue/24/outline';

const router = useRouter();
const fullSize = computed(() => router.currentRoute.value.meta.fullSize);
const authStore = useAuthStore();
const { user, authCompleted } = storeToRefs(authStore);

const appTokensUrl: RouteLocationRaw = {
  query: {
    showAppTokenDialog: 'true',
  },
};

const logout = () => {
  authStore.logout();
  router.push({ name: Routes.Home });
};
</script>

<template>
  <Dialogs></Dialogs>
  <div class="flex flex-col h-[100vh]" :class="{ 'p-3 background-pattern': !fullSize }">
    <div v-if="!fullSize" class="mb-7 flex items-center justify-between s">
      <div class="flex">
        <RouterLink :to="{ name: Routes.Home }">
          <h1 class="text-primary text-2xl font-bold">clippr</h1>
        </RouterLink>
      </div>
      <div v-if="authCompleted" class="dropdown dropdown-end">
        <div tabindex="0" role="button" class="btn m-1">{{ user?.given_name }}</div>
        <ul
          tabindex="0"
          class="dropdown-content menu bg-base-100 rounded-box z-1 p-2 shadow-sm w-36"
        >
          <li onclick="document.activeElement.blur()">
            <RouterLink :to="appTokensUrl">
              <KeyIcon class="size-4"></KeyIcon>
              App tokens
            </RouterLink>
          </li>
          <li onclick="document.activeElement.blur()">
            <a @click="logout">
              <ArrowRightStartOnRectangleIcon class="size-4"></ArrowRightStartOnRectangleIcon>
              Logout
            </a>
          </li>
        </ul>
      </div>
    </div>
    <div class="grow overflow-hidden">
      <RouterView></RouterView>
    </div>
  </div>
</template>
