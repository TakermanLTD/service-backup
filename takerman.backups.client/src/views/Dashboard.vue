<template>
    <div class="container">
        <div class="row">
            <div class="col">
                <hgroup class="text-center">
                    <h2>
                        Stats
                    </h2>
                </hgroup>
                <br />
                <p class="text-center">
                    <strong>Total Size: </strong>: {{ dashboard.totalSize }} <br />
                    <strong>Scheduled for: </strong> {{ moment(dashboard.scheduledFor).format("YYYY MMM DD hh:mm") }} <br />
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
            dashboard: {},
            moment: moment
        }
    },
    async mounted() {
        this.dashboard = await (await fetch('/Dashboard/Get')).json();
    }
}
</script>
<style scoped></style>