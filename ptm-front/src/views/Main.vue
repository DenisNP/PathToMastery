<template>
    <f7-page>
        <f7-navbar>
            <img
                src="../assets/logo.svg"
                class="logo-image ml3"
                slot="left"
                @click="showOnboarding"
            >
        </f7-navbar>
        <calendar @milestone="milestone"/>
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
                    @hint="hint"
                    @done="done(1)"
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
                    @hint="hint"
                    @done="done(2)"
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
                    @hint="hint"
                    @done="done(3)"
                    :active="current === 3"
                />
            </div>
        </div>
        <f7-fab position="right-bottom" class="village-btn" @click="openVillage" v-if="showVillage">
            <img src="../assets/village_button.svg"/>
        </f7-fab>
        <f7-sheet
            style="--f7-sheet-bg-color: #fff;"
            swipe-to-close
            backdrop
            :opened="sheetOpened"
            @sheet:close="sheetOpened = false"
        >
            <div class="flex flex-column justify-center items-center">
                <div class="tc f3 mv2">Веха на Пути</div>
                <img class="sticker-img mt1" :src="stickerImage"/>
                <div class="mt3 mb2 f6 text-color-gray tc mh5">
                    {{milestoneStory
                        ? 'Запишите Историю и расскажите друзьям о ваших успехах!'
                        : 'Важная веха на вашем Пути. Продолжайте непрерывно выполнять задание, ' +
                            'чтобы дойти до неё.'}}
                </div>
                <f7-button @click="createStory" v-show="milestoneStory" fill>
                    Создать историю
                </f7-button>
            </div>
        </f7-sheet>
    </f7-page>
</template>

<script>
import VKC from '@denisnp/vkui-connect-helper';
import Calendar from '@/components/Calendar.vue';
import DoButton from '@/components/DoButton.vue';
import { createStory, generateSticker } from '@/common/storyCreator';
import { isCurrentDay } from '@/common/utils';

export default {
    name: 'Main',
    data() {
        return {
            sheetOpened: false,
            stickerImage: '',
            milestoneStory: false,
            milestoneDay: null,
        };
    },
    computed: {
        current() {
            return this.$store.state.calendarSelected;
        },
        showVillage() {
            return this.$store.state.village
                && this.$store.state.village.pagodas
                && this.$store.state.village.pagodas.length;
        },
    },
    methods: {
        edit(num) {
            this.$f7.views.main.router.navigate(`/create/${num}`);
        },
        activate(num) {
            this.$store.commit('setSelected', num);
        },
        hint(canBeDone) {
            const text = canBeDone
                ? 'Долгое нажатие — отметиться.<br/><br/>Двойное нажатие — редактировать.'
                : 'Сегодня нельзя отметиться на этом Пути.<br/><br/>Двойное нажатие — редактировать.';

            const toast = this.$f7.toast.create({
                text,
                position: 'center',
                cssClass: 'my-text-center',
                closeTimeout: 2500,
            });
            toast.open();
            this.$store.commit('setToast', toast);

            VKC.bridge().send('VKWebAppTapticNotificationOccurred', { type: 'warning' });
        },
        async done(id) {
            await this.$store.dispatch('api', {
                method: 'done',
                data: { id },
            });

            VKC.bridge().send('VKWebAppTapticNotificationOccurred', { type: 'success' });

            const today = this.$store.getters.calendar.days.find((d) => isCurrentDay(d));
            if (today
                && today.msD > 0
                && (
                    today.type === 'Done'
                    || today.type === 'DoneBreak'
                    || today.type === 'DoneLink'
                )) {
                this.milestone(today);
            } else {
                const toast = this.$f7.toast.create({
                    text: 'Готово, продолжайте в том же духе!',
                    position: 'center',
                    cssClass: 'my-text-center',
                    closeTimeout: 2500,
                });
                toast.open();
                this.$store.commit('setToast', toast);
            }
        },
        async milestone(d) {
            this.sheetOpened = true;
            this.milestoneDay = d;
            this.stickerImage = await generateSticker(d, this.$store.getters.calendar.data.name);
            this.milestoneStory = d.type === 'Done'
                || d.type === 'DoneBreak'
                || d.type === 'DoneLink';
        },
        async createStory() {
            const data = await createStory(
                this.milestoneDay,
                this.$store.getters.calendar.data.name,
            );
            VKC.send('VKWebAppShowStoryBox', data);
        },
        openVillage() {
            this.$f7.views.main.router.navigate('/village');
        },
        showOnboarding() {
            this.$store.commit('setShowOnboarding', true);
        },
    },
    components: { DoButton, Calendar },
};
</script>

<style>
    .village-btn > a {
        background-color: #FF1F47;
    }
</style>

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

    .sticker-img {
        width: 225px;
        height: 90px;
        border-radius: 10px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
    }

    .village-btn img {
        min-width: 58px;
    }
</style>
