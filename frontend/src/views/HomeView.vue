<script setup lang="ts">
import Clipboard from '@/components/Clipboard.vue';
import { useUserStore } from '@/stores/user';

const userStore = useUserStore();
</script>

<template>
  <Clipboard v-if="userStore.userIsAuthenticated"></Clipboard>
</template>
