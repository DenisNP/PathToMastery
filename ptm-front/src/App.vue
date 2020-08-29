<template>
    <f7-app :params="f7params">
        <f7-view :push-state="true" main url="/"/>
    </f7-app>
</template>

<script>
import Main from '@/views/Main.vue';

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
                ],
                dialog: {
                    buttonOk: 'Да',
                    buttonCancel: 'Отмена',
                },
            },
        };
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
}

:root {
    --f7-theme-color: #222121;
    --f7-theme-color-rgb: 34, 33, 33;
    --f7-theme-color-shade: #0d0d0d;
    --f7-theme-color-tint: #373535;
    --f7-navbar-height: 52px;
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
</style>
