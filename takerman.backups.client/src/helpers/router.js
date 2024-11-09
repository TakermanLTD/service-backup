import { createRouter, createWebHistory } from "vue-router";
import Login from "../views/Login.vue";
import Home from "../views/Home.vue";
import Projects from "../views/Projects.vue";
import Packages from "../views/Packages.vue";

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Login },
        { path: '/home', component: Home, name: "home" },
        { path: '/projects', component: Projects, name: "projects" },
        { path: '/packages/:project', component: Packages, name: "packages" },
        { path: '/:pathMatch(.*)*', redirect: '/' }
    ]
});

export default router;