<template>
    <div class="container">
        <div v-if="this.dashboard" class="row">
            <div class="col">
                <hgroup class="text-center">
                    <h2>
                        Stats
                    </h2>
                </hgroup>
                <br />
                <p class="text-center">
                    <strong>Total Size: </strong>: {{ this.dashboard?.totalSize?.toFixed(2) }} MB
                </p>
                <p class="text-center">
                    <strong>Scheduled for: </strong> {{ moment(this.dashboard?.scheduledFor).format("DD MMM YYYY - hh:mm") }} <br />
                </p>
            </div>
        </div>
    </div>
</template>
<script lang="js">
import moment from 'moment';

export default {
    data() {
        return {
            dashboard: null,
            moment: moment
        }
    },
    async created() {
        this.dashboard = await (await fetch('Dashboard/Get')).json();
    }
}
</script>
<style scoped></style>