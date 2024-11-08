import { createRouter, createWebHistory } from "vue-router";
import Databases from "../views/Databases.vue";
import Backups from "../views/Backups.vue";
import Login from "../views/Login.vue";
import Volumes from "../views/Volumes.vue";
import Home from "../views/Home.vue";

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Login },
        { path: '/home', component: Home, name: "home" },
        { path: '/databases', component: Databases, name: "databases" },
        { path: '/volumes', component: Volumes, name: "volumes" },
        { path: '/backups/:database', component: Backups, name: "backups" },
        { path: '/:pathMatch(.*)*', redirect: '/' }
    ]
});

export default router;