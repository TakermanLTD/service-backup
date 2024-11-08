<template>
    <header>
        <nav class="navbar navbar-expand-md navbar-light">
            <div class="container-fluid">
                <router-link class="navbar-brand" to="/">Backups</router-link>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navigation"
                        aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navigation">
                    <div class="navbar-nav">
                        <router-link class="nav-link active" aria-current="page" to="/home">Home</router-link>
                        <router-link class="nav-link active" aria-current="page" to="/databases">Databases</router-link>
                        <router-link class="nav-link active" aria-current="page" to="/volumes">Volumes</router-link>
                    </div>
                </div>
            </div>
        </nav>
    </header>
</template>

<script lang="js">
export default {
}
</script>

<style scoped></style>