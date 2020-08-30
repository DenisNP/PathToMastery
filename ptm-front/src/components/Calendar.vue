<template>
    <div>
        <div class="c-font f4 tc pv3">{{monthName}}</div>
        <div
            class="cal-sheet overflow-y-scroll overflow-x-hidden"
            @scroll="calendarScroll"
            ref="calendarSheet"
        >
            <week
                v-for="w in weeks"
                :key="`${w[0].d}_${w[0].m}_${w[0].y}`"
                :days="w"
                :data="data"
            />
        </div>
    </div>
</template>

<script>
import { chunk } from '@/common/utils';
import Week from '@/components/Week.vue';

const weeksToCheck = 6;
const weekHeight = 54;

export default {
    name: 'Calendar',
    components: { Week },
    methods: {
        calendarScroll(e) {
            let skipWeeks = Math.round(e.target.scrollTop / weekHeight);
            if (this.weeks.length < skipWeeks + weeksToCheck) {
                skipWeeks = this.weeks.length - weeksToCheck;
            }

            const monthsCount = {};
            for (let i = skipWeeks; i < skipWeeks + weeksToCheck; i++) {
                const week = this.weeks[i];
                week.forEach((d) => {
                    const key = d.m.toString();
                    if (!monthsCount[key]) monthsCount[key] = 1;
                    else monthsCount[key] -= -1;
                });
            }

            const countsArray = Object.entries(monthsCount);

            countsArray.sort((a, b) => b[1] - a[1]);
            if (countsArray.length === 0) return;

            const month = Number.parseInt(countsArray[0][0], 10);
            if (month !== this.$store.state.currentMonth) {
                this.$store.commit('setCurrentMonth', month);
            }
        },
    },
    computed: {
        monthName() {
            return this.$store.getters.monthName.toUpperCase();
        },
        weeks() {
            const { days } = this.$store.getters.calendar;
            return chunk(days, 7);
        },
        data() {
            return this.$store.getters.calendar.data;
        },
    },
    mounted() {
        const now = new Date();
        const currentWeekIdx = this.weeks
            .findIndex(
                (w) => w.some(
                    (d) => d.d === now.getDate()
                        && d.m === now.getMonth() + 1
                        && d.y === now.getFullYear(),
                ),
            );

        const weekToScroll = Math.max(0, currentWeekIdx - 2);
        this.$nextTick(() => {
            this.$refs.calendarSheet.scrollTop = weekHeight * weekToScroll;
        });
    },
};
</script>

<style scoped>
    .cal-sheet {
        width: 100%;
        height: calc(100vh - 300px);
    }
</style>
