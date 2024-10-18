import { createRouter, createWebHistory } from "vue-router";
import Databases from "../views/Databases.vue";
import Backups from "../views/Backups.vue";

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Databases },
        { path: '/databases', component: Databases, name: "databases" },
        { path: '/backups/:database', component: Backups, name: "backups" },
        { path: '/:pathMatch(.*)*', redirect: '/' }
    ]
});

export default router;