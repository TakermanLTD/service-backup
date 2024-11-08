<template>
    <div class="container">
        <div class="row">
            <p>
                Databases: <br />
                size: {{ dashboard.databasesSize }} <br />
                location: {{ dashboard.databasesLocation }}
            </p>
            <p>
                Backups: <br />
                size: {{ dashboard.backupsSize }} <br />
                location: {{ dashboard.backupsLocation }}
            </p>
            <p>
                Volumes: <br />
                size: {{ dashboard.volumesSize }} <br />
                location: {{ dashboard.volumesLocation }}
            </p>
        </div>
    </div>
</template>
<script lang="js">
export default {
    data() {
        return {
            dashboard: {}
        }
    },
    async mounted() {
        this.dashboard = await (await fetch('/Dashboard/Get')).json();
    }
}
</script>
<style scoped></style>