<template>
    <div class="flex justify-around ph2">
        <div
            v-for="d in days"
            :key="d.dow"
            class="tc width-100 pv3 f5 relative"
            :class="{'o-40': d.m !== $store.state.currentMonth}"
        >
            {{d.d}}
        </div>
    </div>
</template>

<script>
export default {
    name: 'Week',
    props: {
        days: {
            type: Array,
            required: true,
        },
    },
};
</script>

<style scoped>

</style>
