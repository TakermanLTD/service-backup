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
                    <strong>Total Size: </strong>: {{ dashboard.totalSize.toFixed(2) }} MB<br />
                    <strong>Scheduled for: </strong> {{ moment(dashboard.scheduledFor).format("DD MMM YYYY - hh:mm") }} <br />
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