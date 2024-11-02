<template>
    <div class="container">
        <div class="row">
            <button class="btn btn-info" @click="backup()">Backup</button>
        </div>
        <div class="row">
            <h2 class="text-center">Backups</h2>
            <p>
                <strong class="text-center">{{ this.state }}</strong>
            </p>
            <table class="table table-borderless">
                <tr v-for="(backup, key) in this.backups" :key="key">
                    <td>{{ moment(backup.created).format("YYYY MMM DD hh:mm") }}</td>
                    <td>{{ backup.name }}</td>
                    <!-- <td>{{ backup.location }}</td> -->
                    <td>{{ (backup.size / 1024).toFixed(2) }} MB</td>
                    <td>
                        <button class="btn btn-info" @click="restore(backup.name)">restore</button>
                        <button class="btn btn-info" @click="remove(backup.name)">remove</button>
                    </td>
                </tr>
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
            backups: [],
            state: '',
            database: '',
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
        this.database = params.database;
        this.getForDatabase();
    },
    methods: {
        async getForDatabase() {
            this.state = 'loading';
            this.backups = await (await fetch('/Backups/GetForDatabase?database=' + this.database)).json();

            if (!this.backups || this.backups.length == 0)
                this.state = 'no backups';
            else
                this.state = '';
        },
        async backup() {
            await fetch('/Backups/Backup?database=' + this.database);
            this.state = 'backup finished';
            this.getForDatabase();
        },
        async restore(backup) {
            if (confirm('Are you sure?')) {
                await fetch('/Backups/Restore?backup=' + backup + "&database=" + this.database);
                this.state = 'restore finished';
                this.getForDatabase();
            }
        },
        async remove(backup) {
            if (confirm('Are you sure?')) {
                await fetch('/Backups/Delete?database=' + this.database + '&backup=' + backup);
                this.state = 'removed';
                this.getForDatabase();
            }
        }
    }
}
</script>

<style scoped></style>