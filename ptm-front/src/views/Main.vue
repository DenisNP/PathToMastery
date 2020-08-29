<template>
    <f7-page>
        <f7-navbar>
            <img src="../assets/logo.svg" class="logo-image ml3" slot="left">
        </f7-navbar>
        <calendar/>
        <div class="w-100 mt4 relative">
            <div class="buttons-block ml-auto mr-auto">
                <do-button
                    class="btn"
                    :data="$store.state.first"
                    :class="{
                        'btn-left': current === 2,
                        'btn-main': current === 1,
                        'btn-right': current === 3,
                    }"
                    @edit="edit(1)"
                    @activate="activate(1)"
                    :active="current === 1"
                />
                <do-button
                    class="btn"
                    :data="$store.state.second"
                    :class="{
                        'btn-left': current === 3,
                        'btn-main': current === 2,
                        'btn-right': current === 1,
                    }"
                    @edit="edit(2)"
                    @activate="activate(2)"
                    :active="current === 2"
                />
                <do-button
                    class="btn"
                    :data="$store.state.third"
                    :class="{
                        'btn-left': current === 1,
                        'btn-main': current === 3,
                        'btn-right': current === 2,
                    }"
                    @edit="edit(3)"
                    @activate="activate(3)"
                    :active="current === 3"
                />
            </div>
        </div>
    </f7-page>
</template>

<script>
import Calendar from '@/components/Calendar.vue';
import DoButton from '@/components/DoButton.vue';

export default {
    name: 'Main',
    computed: {
        current() {
            return this.$store.state.calendarSelected;
        },
    },
    methods: {
        edit(num) {
            this.$f7.views.main.router.navigate(`/create/${num}`);
        },
        activate(num) {
            this.$store.commit('setSelected', num);
        },
    },
    components: { DoButton, Calendar },
};
</script>

<style scoped>
.logo-image {
    height: 26px;
}

.buttons-block {
    overflow: visible;
    position: absolute;
    left: 50vw;
}

.btn {
    position: absolute;
    transition: transform, left, opacity, z-index;
    transition-duration: 0.4s;
}

.btn-left {
    left: -106px;
    transform: scale(0.9);
    opacity: 0.4;
}

.btn-main {
    left: -42px;
    z-index: 1;
}

.btn-right {
    left: 22px;
    transform: scale(0.9);
    opacity: 0.4;
}
</style>
