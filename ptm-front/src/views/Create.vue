<template>
    <f7-page>
        <f7-navbar :title="data.name ? 'Редактировать путь' : 'Новый путь'"/>
    </f7-page>
</template>

<script>
export default {
    name: 'Create',
    computed: {
        data() {
            const n = Number.parseInt(this.pathId, 10);
            switch (n) {
            case 1:
                return this.$store.state.first.data;
            case 3:
                return this.$store.state.third.data;
            default:
                return this.$store.state.second.data;
            }
        },
    },
    props: {
        pathId: {
            type: String,
            required: true,
        },
    },
};
</script>

<style scoped>

</style>
