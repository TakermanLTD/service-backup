<template>
    <hgroup>
        Backups
    </hgroup>
    <div class="container">
        <div class="row">
            <button @click="backupAll()">Backup All</button>
        </div>
        <div class="row">
            <h2>Databases</h2>
            <table class="table table-borderless">
                <tr v-for="(database, key) in databases" :key="key">
                    <td>{{ database.name }}</td>
                    <td>
                        <button @click="backup(database.name)">backup</button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</template>

<script lang="js">

export default {
    data() {
        return {
            databases: []
        }
    },
    async mounted() {
        this.databases = await fetch('/Databases/GetAll');
    },
    methods: {
        async backup(name) {
            await fetch('/Databases/Backup?name' + name);
        },
        async backupAll() {
            for (const database in databases) {
                await backup(database.name);
            }
        }
    }
}
</script>

<style scoped></style>