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
                @milestone="(d) => $emit('milestone', d)"
            />
        </div>
        <div class="empty-placeholder flex-column items-center content-center tc" v-if="!data.name">
            <div class="f4 mt4">Новый Путь</div>
            <div class="f6 mv4 mh4">Создайте новое занятие, которое будете отслеживать.</div>
            <f7-button @click="createPath">Создать</f7-button>
        </div>
    </div>
</template>

<script>
import { chunk, isCurrentDay } from '@/common/utils';
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
        setScroll() {
            const currentWeekIdx = this.weeks.findIndex(
                (w) => w.some((d) => isCurrentDay(d)),
            );

            const weekToScroll = Math.max(0, currentWeekIdx - 2);
            this.$nextTick(() => {
                this.$refs.calendarSheet.scrollTop = weekHeight * weekToScroll;
            });
        },
        createPath() {
            this.$f7.views.main.router.navigate(`create/${this.calendarSelected}`);
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
        calendarSelected() {
            return this.$store.state.calendarSelected;
        },
    },
    watch: {
        calendarSelected() {
            this.setScroll();
        },
    },
    mounted() {
        this.setScroll();
    },
};
</script>

<style scoped>
    .cal-sheet {
        width: 100%;
        height: calc(100vh - 300px);
    }

    .empty-placeholder {
        width: 250px;
        height: 250px;
        position: absolute;
        border: 1px solid #ddd;
        background-color: white;
        border-radius: 20px;
        left: 50%;
        top: calc(50% - 50px);
        transform: translate(-50%, -50%);
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.25);
    }
</style>
