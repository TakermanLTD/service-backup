<template>
    <div class="container">
        <div class="row">
            <h2 class="text-center">Databases</h2>
            <p>
                <strong class="text-center">{{ state }}</strong>
            </p>
        </div>
        <div class="row">
            <button @click="optimizeAll()" class="btn btn-info">Optimize all</button>
            <button @click="backupAll()" class="btn btn-info">Backup all</button>
        </div>
        <div class="row">
            <table class="table table-borderless">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>State</th>
                        <th>Recovery Model</th>
                        <th>Size</th>
                        <th>Log Size</th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(database, key) in databases" :key="key">
                        <td>{{ database.name }}</td>
                        <td>{{ database.state }}</td>
                        <td>{{ database.recoveryModel }}</td>
                        <td>{{ database.dataSizeMB.toFixed(2) }} MB</td>
                        <td>{{ database.logSizeMB.toFixed(2) }} MB</td>
                        <td>
                            <button class="btn btn-info" @click="viewBackups(database.name)">backups</button>
                            <button class="btn btn-info" @click="remove(database.name)">remove</button>
                            <button class="btn btn-info" @click="optimize(database.name)">optimize</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="js">

export default {
    data() {
        return {
            databases: [],
            state: '',
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
        await this.getAll();
    },
    methods: {
        viewBackups(database) {
            this.$router.push({ name: 'backups', params: { database } });
        },
        async getAll() {
            this.state = 'loading';
            this.databases = await (await fetch('/Databases/GetAll')).json();
            this.state = '';
        },
        async optimize(database) {
            await fetch('/Databases/Optimize?database=' + database);
            this.state = 'optimization finished';
            this.getForDatabase();
        },
        async optimizeAll() {
            await fetch('/Databases/OptimizeAll');
            this.state = 'optimize all finished';
        },
        async backupAll() {
            await fetch('/Backups/BackupAll?');
            this.state = 'backup all finished';
        },
        async remove(database) {
            this.state = 'loading';
            let result = await fetch('/Databases/Delete?database=' + database);
            if (result)
                this.state = 'removed';
            else
                this.state = 'cannot remove';

            this.getAll();
        }
    }
}
</script>

<style scoped></style>