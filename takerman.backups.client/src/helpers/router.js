import { createRouter, createWebHistory } from "vue-router";
import Login from "../views/Login.vue";
import Dashboard from "../views/Dashboard.vue";
import Projects from "../views/Projects.vue";
import Packages from "../views/Packages.vue";

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Login },
        { path: '/dashboard', component: Dashboard, name: "dashboard" },
        { path: '/projects', component: Projects, name: "projects" },
        { path: '/packages/:project', component: Packages, name: "packages" },
        { path: '/:pathMatch(.*)*', redirect: '/' }
    ]
});

export default router;