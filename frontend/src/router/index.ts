import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import { Routes } from './routes';
import { authMiddleware } from './middleware/auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: Routes.Home,
      component: HomeView,
    },
    {
      path: '/signin-callback',
      name: Routes.SignInCallback,
      component: HomeView,
    },
  ],
});

router.beforeEach(authMiddleware);

declare module 'vue-router' {
  interface RouteMeta {
    fullSize?: boolean;
  }
}

export default router;
