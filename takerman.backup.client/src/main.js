import { createApp } from 'vue';
import { createI18n } from 'vue-i18n';
import { createPinia } from 'pinia';
import App from './App.vue';
import mitt from 'mitt';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import "bootstrap";
import "./assets/css/style.css";
import moment from 'moment';
import en from './assets/languages/en.json';
import bg from './assets/languages/bg.json';
import cookies from './helpers/cookies';
import router from './helpers/router.js';

Date.prototype.toJSON = function () { return moment(this).format(); }
const emitter = mitt();
let pinia = createPinia();
const i18n = createI18n({
	locale: cookies.get('language') || 'en',
	legacy: false,
	locale: cookies.get('language') || 'en',
	fallbackLocale: 'en',
	formatFallbackMessages: true,
	messages: {
		en: en,
		bg: bg
	}
});

const app = createApp(App);
app.config.globalProperties.emitter = emitter;
app.config.productionTip = false;
app.use(pinia)
	.use(i18n)
	.use(router)
	.mount('#app');
