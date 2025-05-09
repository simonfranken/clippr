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
      children: [
        {
          path: 'signin-callback',
          component: HomeView,
          name: Routes.SignInCallback,
        },
      ],
    },
  ],
});

router.beforeEach(authMiddleware);

export default router;
