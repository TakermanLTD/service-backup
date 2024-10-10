<template>
    <div>{{ content ? content : $t('common.loading') }}</div>
</template>
<script lang="js">
export default {
    props: {
        content: String
    }
}
</script>
<style scoped></style>