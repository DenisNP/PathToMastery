<template>
    <f7-page>
        <f7-navbar>
            <img src="../assets/logo.svg" class="logo-image ml3">
        </f7-navbar>
        <calendar/>
        <div class="w-100 mt4 relative">
            <div class="buttons-block ml-auto mr-auto">
                <do-button
                    class="btn"
                    :data="$store.state.first.data"
                    :class="{
                        'btn-left': current === 2,
                        'btn-main': current === 1,
                        'btn-right': current === 3,
                    }"
                />
                <do-button
                    class="btn"
                    :data="$store.state.second.data"
                    :class="{
                        'btn-left': current === 3,
                        'btn-main': current === 2,
                        'btn-right': current === 1,
                    }"
                />
                <do-button
                    class="btn"
                    :data="$store.state.third.data"
                    :class="{
                        'btn-left': current === 1,
                        'btn-main': current === 3,
                        'btn-right': current === 2,
                    }"
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
