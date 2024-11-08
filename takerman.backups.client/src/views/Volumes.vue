<template>
    <div class="container">
        <div class="row">
            <button @click="backup()" class="col btn btn-info">backup</button>
            <button @click="maintain()" class="col btn btn-info">maintain</button>
        </div>
        <div class="row">
            <div>
                <div class="col">Name</div>
                <div class="col">Size</div>
                <div class="col">Created</div>
                <div></div>
            </div>
        </div>
        <div class="row">
            <div v-for="(volume, key) in volumes" :key="key">
                <div class="col">{{ volume.name }}</div>
                <div class="col">{{ volume.size }}</div>
                <div class="col">{{ volume.created }}</div>
                <div>
                    <button @click="remove(volume)" class="btn btn-info">remove</button>
                    <button @click="download(volume)" class="btn btn-info">download</button>
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="js">
export default {
    data() {
        return {
            volumes: []
        }
    },
    async created() {
        this.volumes = await (await fetch('/Volumes/GetAll')).json();
    },
    methods: {
        async remove(volume) {
            await fetch('/Volumes/Remove?volume=' + volume.name);

        },
        async download(volume) {

        },
        async backup() {
            await fetch('/Volumes/Backup');
        },
        async maintain() {
            await fetch('/Volumes/Maintain');
        }
    }
}
</script>
<style scoped></style>