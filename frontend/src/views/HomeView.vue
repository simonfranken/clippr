<script setup lang="ts">
import Clipboard from '@/components/Clipboard.vue';
import { useAuthStore } from '@/stores/auth';
import { storeToRefs } from 'pinia';

const { authCompleted } = storeToRefs(useAuthStore());
</script>

<template>
  <Clipboard v-if="authCompleted"></Clipboard>
</template>
