<template>
    <div class="container">
        <div class="row">
            <h2 class="text-center">Projects</h2>
            <p>
                <strong class="text-center">{{ state }}</strong>
            </p>
        </div>
        <div class="row">
            <button @click="backupAll()" class="col btn btn-info">backup all</button>
            <button @click="sync()" class="col btn btn-info">sync</button>
        </div>
        <div class="row">
            <table class="table table-borderless">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Packages Count</th>
                        <th>Total Size</th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(project, key) in projects" :key="key">
                        <td>{{ project.name }}</td>
                        <td>{{ project.packagesCount }}</td>
                        <td>{{ project.totalSizeMB.toFixed(2) }} MB</td>
                        <td>
                            <button class="btn btn-info" @click="backup(project.name)">backup</button>
                            <button class="btn btn-info" @click="viewPackages(project.name)">packages</button>
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
            projects: [],
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
        viewPackages(project) {
            this.$router.push({ name: 'packages', params: { project } });
        },
        async getAll() {
            this.state = 'loading';
            this.projects = await (await fetch('/Projects/GetAll')).json();
            this.state = '';
        },
        async backupAll() {
            this.state = 'loading';
            await fetch('Projects/BackupAll');
            this.state = 'backup all finished';
        },
        async backup(name) {
            this.state = 'loading';
            await fetch('Projects/Backup?name=' + name);
            this.state = 'backup all finished';
        },
        sync() {
        }
    }
}
</script>

<style scoped></style>