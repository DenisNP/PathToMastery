<template>
    <div>
        <div class="c-font f4 tc pv3">{{monthName}}</div>
        <div class="cal-sheet overflow-scroll">
            <week v-for="w in weeks" :key="`${w[0].d}_${w[0].m}_${w[0].y}`" :days="w"/>
        </div>
    </div>
</template>

<script>
import { chunk } from '@/common/utils';
import Week from '@/components/Week.vue';

export default {
    name: 'Calendar',
    components: { Week },
    computed: {
        monthName() {
            return this.$store.getters.monthName.toUpperCase();
        },
        weeks() {
            const { days } = this.$store.getters.calendar;
            return chunk(days, 7);
        },
    },
};
</script>

<style scoped>
    .cal-sheet {
        width: 100%;
        height: calc(100vh - 300px);
    }
</style>