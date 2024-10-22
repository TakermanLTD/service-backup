<template>
    <div class="container">
        <div class="row">
            <button class="btn btn-info" @click="backup()">Backup</button>
            <button class="btn btn-info" @click="removeAll()">Delete All</button>
        </div>
        <div class="row">
            <h2 class="text-center">Backups</h2>
            <p>
                <strong class="text-center">{{ this.state }}</strong>
            </p>
            <table class="table table-borderless">
                <tr v-for="(backup, key) in this.backups" :key="key">
                    <td>{{ backup.id }}</td>
                    <td>{{ backup.name }}</td>
                    <td>{{ backup.location }}</td>
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
import { useRoute } from 'vue-router';


export default {
    data() {
        return {
            backups: [],
            state: '',
            database: ''
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
            let result = await (await fetch('/Backups/Backup?database=' + this.database + "&incremental=" + false)).json();
            if (result)
                this.state = 'backup finished';
            this.getForDatabase();
        },
        async restore(backup) {
            let result = await (await fetch('/Backups/Restore?backup=' + backup + "&database=" + this.database)).json();
            if (result)
                this.state = 'restore finished';
            this.getForDatabase();
        },
        async remove(backup) {
            let result = await (await fetch('/Backups/Delete?backup=' + backup)).json();
            if (result)
                this.state = 'removed';
            this.getForDatabase();
        },
        async removeAll() {
            let result = await (await fetch('/Backups/DeleteAll?database=' + this.database)).json();
            if (result)
                this.state = 'removed all';
            this.getForDatabase();
        }
    }
}
</script>

<style scoped></style>