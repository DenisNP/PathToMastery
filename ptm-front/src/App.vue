<template>
    <f7-app :params="f7params">
        <transition name="popup">
            <div class="onboarding" v-if="$store.state.showOnboarding">
                <f7-swiper pagination>
                    <f7-swiper-slide>
                        <div class="onboarding-slide">
                            <div class="ob-pic">
                                <img src="./assets/onboarding_0.jpg">
                            </div>
                            <div class="ob-text">
                                «Путь К Мастерству» — трекер привычек.
                                Создайте себе задание и установите дни его выполнения.
                                <span class="ob-small">
                                    <i>Путь в тысячу ли начинается с одного шага (Лао-Цзы)</i>
                                </span>
                            </div>
                            <f7-button fill @click="slideNext">Далее</f7-button>
                        </div>
                    </f7-swiper-slide>
                    <f7-swiper-slide>
                        <div class="onboarding-slide">
                            <div class="ob-pic">
                                <img src="./assets/onboarding_1.jpg">
                            </div>
                            <div class="ob-text">
                                Заходите в приложение и отмечайте свой прогресс.
                                Приложение будет напоминать вам об этом.
                                <span class="ob-small">
                                    <i>
                                        Я не боюсь того, кто изучает 10000 различных ударов;
                                        я боюсь того, кто изучает один удар 10000 раз
                                        (Брюс Ли)
                                    </i>
                                </span>
                            </div>
                            <f7-button fill @click="slideNext">Далее</f7-button>
                        </div>
                    </f7-swiper-slide>
                    <f7-swiper-slide>
                        <div class="onboarding-slide">
                            <div class="ob-pic">
                                <img src="./assets/onboarding_2.jpg">
                            </div>
                            <div class="ob-text">
                                В ключевые моменты делитесь успехами с друзьями с помощью Историй
                                со специальным стикером.
                                <span class="ob-small">
                                    <i>
                                        Дорога возникает под шагами идущего (Буддистская пословица)
                                    </i>
                                </span>
                            </div>
                            <f7-button fill @click="start">Начать</f7-button>
                        </div>
                    </f7-swiper-slide>
                </f7-swiper>
            </div>
        </transition>
        <f7-view :push-state="true" main url="/"/>
    </f7-app>
</template>

<script>
import Main from '@/views/Main.vue';
import Create from '@/views/Create.vue';

export default {
    name: 'App',
    data() {
        return {
            f7params: {
                name: 'Путь к мастерству',
                routes: [
                    {
                        path: '/',
                        component: Main,
                    },
                    {
                        path: '/create/:pathId',
                        component: Create,
                    },
                ],
                dialog: {
                    buttonOk: 'Да',
                    buttonCancel: 'Отмена',
                },
            },
        };
    },
    methods: {
        slideNext() {
            const sw = this.$f7.swiper.get();
            if (sw) sw.slideNext();
        },
        start() {
            this.$store.dispatch('saveOnboarding');
        },
    },
    computed: {
        isLoading() {
            return this.$store.state.isLoading;
        },
    },
    watch: {
        isLoading(l) {
            if (l) this.$f7.preloader.show();
            else this.$f7.preloader.hide();
        },
    },
    mounted() {
        window.addEventListener('contextmenu', (e) => { e.preventDefault(); });
        this.$store.dispatch('init');
    },
};
</script>

<style>
body {
    -webkit-user-select: none;
    user-select: none;
    overscroll-behavior-y: none;
}

html,
body {
    position: fixed;
    overflow: hidden;
    background-color: #E5E5E5;
}

:root, :root.theme-dark, :root .theme-dark {
    --f7-theme-color: #222121;
    --f7-theme-color-rgb: 34, 33, 33;
    --f7-theme-color-shade: #0d0d0d;
    --f7-theme-color-tint: #373535;
    --f7-navbar-height: 52px;
    --f7-bars-bg-color: #FBFBFB;
    --f7-bars-bg-color-rgb: rgb(251, 251, 251);
    --f7-navbar-bg-color: #FBFBFB;
    --f7-navbar-bg-color-rgb: rgb(251, 251, 251);
    --f7-bars-translucent-opacity: 1.0;
    --f7-bars-border-color: transparent;
    --f7-page-bg-color: #FBFBFB;
    --f7-navbar-shadow-image: none;
}

.emoji-picker {
    --ep-color-bg: #FBFBFB!important;
    --ep-color-active: var(--f7-theme-color)!important;
}

.my-text-center {
    text-align: center;
}

.sheet-modal {
    border-radius: 20px 20px 0 0;
}

.ios-translucent-bars .navbar-bg {
    background-color: var(--f7-navbar-bg-color)!important;
}

@font-face {
    font-family: 'Maler';
    src: url('./assets/Maler.ttf') format('truetype');
    font-weight: normal;
    font-style: normal;
}

.c-font {
  font-family: 'Maler', sans-serif;
}

.onboarding {
    position: absolute;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: rgba(0, 0, 0, 0.2);
    z-index: 5001;
}

.onboarding-slide {
    width: calc(100vw - 40px);
    /*noinspection CssInvalidFunction*/
    height: calc(100vh - 155px - env(safe-area-inset-top) - env(safe-area-inset-bottom));
    max-height: 140vw;
    /*noinspection CssInvalidFunction*/
    margin-top: calc(55px + env(safe-area-inset-top));
    margin-left: 20px;
    background: white;
    border-radius: 15px;
    padding: 40px 20px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}

.popup-enter-active, .popup-leave-active {
    transition: all .3s;
}

.popup-enter, .popup-leave-to {
    transform: scale(0.1);
    opacity: 0;
}

.ob-text {
    text-align: center;
    font-size: 14px;
}

.ob-small {
    font-size: 13px;
    margin-top: 15px;
    margin-bottom: 5px;
    display: block;
}

.ob-pic {
    width: calc(100vw - 140px);
    margin-left: auto;
    margin-right: auto;
    border-radius: 10px;
    overflow: hidden;
    line-height: 0;
}

.ob-pic > img {
    width: 100%;
}
</style>
