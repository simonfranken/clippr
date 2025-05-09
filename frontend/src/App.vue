<script setup lang="ts">
import { computed } from 'vue';
import { useUserStore } from './stores/user';
import { RouterLink, RouterView } from 'vue-router';
import { Routes } from './router/routes';

const userStore = useUserStore();
const user = computed(() => userStore.user);
const isAuthenticated = computed(() => userStore.user !== undefined);
</script>

<template>
  <div class="flex flex-col p-3 h-[100vh]">
    <div class="mb-7 flex items-center justify-between">
      <div class="flex">
        <RouterLink :to="{ name: Routes.Home }">
          <h1 class="text-primary text-2xl font-bold">sway</h1>
        </RouterLink>
      </div>
      <div>
        <button v-if="userStore.signOutIsLoading" class="btn btn-secondary" disabled>
          <span class="loading loading-spinner"></span>
          Logging out
        </button>
        <div v-else-if="isAuthenticated" class="dropdown dropdown-end">
          <div tabindex="0" role="button" class="btn btn-secondary mb-2">
            {{ user?.profile.given_name }}
          </div>
          <ul
            tabindex="0"
            class="dropdown-content menu bg-base-100 rounded-box z-1 w-52 p-2 shadow-sm"
          >
            <li @click="userStore.signOutRedirect"><a>Log out</a></li>
            <li><a>Settings</a></li>
          </ul>
        </div>

        <button v-else-if="userStore.signInIsLoading" class="btn btn-secondary" disabled>
          <span class="loading loading-spinner"></span>
          Logging in
        </button>
      </div>
    </div>
    <div class="grow overflow-hidden">
      <RouterView></RouterView>
    </div>
  </div>
</template>
