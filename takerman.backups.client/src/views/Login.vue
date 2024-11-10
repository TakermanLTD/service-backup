<template>
    <div class="container">
        <form @submit.prevent="login">
            <div class="row">
                <label for="username">Username:</label>
                <input class="form-control" type="text" id="username" v-model="username" />
            </div>
            <div class="row">
                <label for="password">Password:</label>
                <input class="form-control" type="password" id="password" v-model="password" />
            </div>
            <div class="row">
                <button class="btn btn-success" type="submit">Login</button>
            </div>
        </form>
    </div>
</template>

<script>
export default {
    data() {
        return {
            username: '',
            password: '',
            isAuthenticated: false
        };
    },
    mounted() {
        this.isAuthenticated = localStorage.getItem('authenticated') === 'true';
        if (this.isAuthenticated) {
            this.$router.push('/dashboard');
        }
    },
    methods: {
        login() {
            const hardcodedUsername = 'takerman';
            const hardcodedPassword = 'Hakerman91!';

            if (this.username === hardcodedUsername && this.password === hardcodedPassword) {
                localStorage.setItem('authenticated', 'true');
                this.$router.push('/dashboard');
            } else {
                alert('Invalid credentials');
            }
        },
    },
};
</script>