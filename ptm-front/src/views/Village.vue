<template>
    <f7-page class="village-page relative">
        <f7-navbar title="Ваш прогресс" back-link/>
        <transition name="popup">
            <div class="onboarding" v-if="$store.state.showVillageInfo" @click="closeInfo">
                <div class="onboarding-slide slide-small">
                    <div class="ob-pic">
                        <img src="../assets/onboarding_3.jpg">
                    </div>
                    <div class="ob-text">
                        Эта деревня отражает ваши стремления к успеху. Она будет разрастаться
                        по мере непрерывного следования по Путям и, наоборот, уменьшаться при
                        пропусках.
                        <span class="ob-small">
                            <i>Как бы высока ни была гора, когда-нибудь и по ней пройдет дорога
                                (Китайская пословица)</i>
                        </span>
                    </div>
                </div>
            </div>
        </transition>
        <div class="village-bg" @click="showInfo">
            <img src="../assets/village_bg.svg" alt="background"/>
            <div
                class="pagoda"
                v-for="p in village.pagodas"
                :key="p[0]"
                :style="`left: ${points[p.x - 1][p.y - 1][1]}vw;
                    top: ${points[p.x - 1][p.y - 1][0]}vw;`"
            >
                <img class="p-base" src="../assets/pagoda_base.svg" alt="house"/>
                <img
                    class="p-roof"
                    :class="`p-roof-${p.level}`"
                    :src="roofs[p.level - 1][p.color - 1]" alt="roof"/>
            </div>
        </div>
        <div class="village-bottom">
            <div class="village-header">
                <div v-for="p in village.paths" :key="p.name + p.color" class="path-data-block">
                    <f7-badge
                        :style="`--f7-badge-bg-color: ${colors[p.color - 1][0]};
                            --f7-badge-text-color: ${colors[p.color - 1][3]};`">
                        {{p.days}}
                    </f7-badge>
                    <span>{{p.name}}</span>
                </div>
            </div>
<!--            <div class="user-footer flex items-center content-center justify-center">-->
<!--                <div class="flex items-center content-center justify-center">-->
<!--                    <div class="avatar mr2">-->
<!--                        <img :src="user.image" class="w-100 h-100"/>-->
<!--                    </div>-->
<!--                    <div class="fw5">{{user.name}}</div>-->
<!--                </div>-->
<!--            </div>-->
        </div>
    </f7-page>
</template>

<script>
/* eslint-disable global-require */

import { range } from '@/common/utils';

const horStep = 4.4;
const verStep = -7.85;

export default {
    name: 'Village',
    data() {
        return {
            points: [
                range(2).map((i) => [68.5 + i * horStep, 13 + i * verStep]),
                range(4).map((i) => [70 + i * horStep, 30 + i * verStep]),
                range(7).map((i) => [69 + i * horStep, 55 + i * verStep]),
                range(8).map((i) => [74.5 + i * horStep, 69 + i * verStep]),
                range(8).map((i) => [81 + i * horStep, 82 + i * verStep]),
                range(5).map((i) => [94.5 + i * horStep, 82 + i * verStep]),
                range(2).map((i) => [108 + i * horStep, 82 + i * verStep]),
            ],
            roofs: [
                [
                    require('../assets/pagoda_roof_1_1.svg'),
                    require('../assets/pagoda_roof_1_2.svg'),
                    require('../assets/pagoda_roof_1_3.svg'),
                    require('../assets/pagoda_roof_1_4.svg'),
                    require('../assets/pagoda_roof_1_5.svg'),
                    require('../assets/pagoda_roof_1_6.svg'),
                    require('../assets/pagoda_roof_1_7.svg'),
                ],
                [
                    require('../assets/pagoda_roof_2_1.svg'),
                    require('../assets/pagoda_roof_2_2.svg'),
                    require('../assets/pagoda_roof_2_3.svg'),
                    require('../assets/pagoda_roof_2_4.svg'),
                    require('../assets/pagoda_roof_2_5.svg'),
                    require('../assets/pagoda_roof_2_6.svg'),
                    require('../assets/pagoda_roof_2_7.svg'),
                ],
                [
                    require('../assets/pagoda_roof_3_1.svg'),
                    require('../assets/pagoda_roof_3_2.svg'),
                    require('../assets/pagoda_roof_3_3.svg'),
                    require('../assets/pagoda_roof_3_4.svg'),
                    require('../assets/pagoda_roof_3_5.svg'),
                    require('../assets/pagoda_roof_3_6.svg'),
                    require('../assets/pagoda_roof_3_7.svg'),
                ],
            ],
        };
    },
    computed: {
        colors() {
            return this.$store.state.colors;
        },
        village() {
            return this.$store.state.village;
        },
        user() {
            return this.$store.state.usersData[this.$store.state.user.id];
        },
    },
    methods: {
        closeInfo() {
            this.$store.dispatch('saveVillageInfo');
        },
        showInfo() {
            this.$store.commit('setShowVillageInfo', true);
        },
    },
};
</script>

<style scoped>
.village-bg {
    width: 100vw;
    height: 157vw;
    position: relative;
}

.village-bg > img{
    width: 100%;
    height: 100%;
    position: absolute;
}

.village-page {
    background-color: #F0F4EB;
}

.pagoda {
    position: absolute;
}

.p-base {
    position: absolute;
    min-width: 13.3vw;
    min-height: 8.8vw;
    width: 13.3vw;
    height: 8.8vw;
    bottom: 0;
    left: 0;
}

.p-roof {
    position: absolute;
    min-width: 12.3vw;
    width: 12.3vw;
    bottom: 5.8vw;
    left: 0.7vw;
}

.p-roof-1 {
    min-height: 6.4vw;
    height: 6.4vw;
}

.p-roof-2 {
    min-height: 10.1vw;
    height: 10.1vw;
}

.p-roof-3 {
    min-height: 12.5vw;
    height: 12.5vw;
    min-width: 13.3vw;
    width: 13.3vw;
    bottom: 3.3vw;
    left: 0.1vw;
}

.village-bottom {
    position: absolute;
    bottom: 25px;
    background-color: rgba(255, 255, 255, 0.5);
    border-radius: 20px;
    width: calc(100% - 50px);
    /*height: 128px;*/
    height: 77px;
    left: 25px;
}

.village-header {
    position: absolute;
    top: 0;
    left: 0;
    border-radius: 20px;
    background-color: rgba(255, 255, 255, 0.5);
    box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
    width: 100%;
    height: 77px;
    --f7-badge-font-weight: bold;
    display: flex;
    align-items: center;
    justify-content: space-around;
}

.path-data-block {
    display: flex;
    flex-direction: column;
    align-items: center;
    max-width: 30%;
}

.path-data-block > span {
    margin-top: 3px;
    text-overflow: ellipsis;
    max-width: 100%;
    text-align: center;
    max-height: 40px;
    overflow: hidden;
}

.user-footer {
    position: absolute;
    top: 77px;
    left: 0;
    width: 100%;
    height: 51px;
}

.avatar {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    overflow: hidden;
}

.slide-small {
    /*noinspection CssInvalidFunction*/
    height: calc(100vh - 175px - env(safe-area-inset-top) - env(safe-area-inset-bottom));
    margin-top: 75px;
}
</style>
