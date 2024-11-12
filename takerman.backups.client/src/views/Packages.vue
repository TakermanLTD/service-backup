<template>
    <div class="container">
        <div class="row">
            <h2 class="text-center">Packages</h2>
            <p>
                <strong class="text-center">{{ this.state }}</strong>
            </p>
        </div>
        <div class="row">
            <button class="col btn btn-info" @click="backup()">backup</button>
        </div>
        <div class="row">
            <table class="table table-borderless">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Created</th>
                        <th>Size</th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(backup, key) in this.packages" :key="key">
                        <td>{{ backup.name }}</td>
                        <td>{{ moment(backup.created).format("YYYY MMM DD hh:mm") }}</td>
                        <td>{{ backup.size.toFixed(2) }} MB</td>
                        <td>
                            <button class="btn btn-info" @click="remove(backup.name)">remove</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="js">
import moment from 'moment';
import { useRoute } from 'vue-router';


export default {
    data() {
        return {
            packages: [],
            state: '',
            project: '',
            moment: moment,
            isAuthenticated: false
        }
    },
    created() {
        this.isAuthenticated = localStorage.getItem('authenticated') === 'true';
        if (!this.isAuthenticated) {
            this.$router.push('/');
        }
    },
    async mounted() {
        const { params } = useRoute();
        this.project = params.project;
        this.getPackages();
    },
    methods: {
        async getPackages() {
            this.state = 'loading';
            this.packages = await (await fetch('/Projects/GetPackages?project=' + this.project)).json();

            if (!this.packages || this.packages.length == 0)
                this.state = 'no packages';
            else
                this.state = '';
        },
        async backup() {
            await fetch('Projects/Backup?project=' + this.project);
            this.state = 'backup finished';
            this.getPackages();
        },
        async remove(backup) {
            if (confirm('Are you sure?')) {
                await fetch('/Projects/Delete?project=' + this.project + '&package=' + backup);
                this.state = 'removed';
                this.getPackages();
            }
        }
    }
}
</script>

<style scoped></style>