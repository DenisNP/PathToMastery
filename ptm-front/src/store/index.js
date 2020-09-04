import Vue from 'vue';
import Vuex from 'vuex';
import VKC from '@denisnp/vkui-connect-helper';
import {
    getAppId, getPlatform, getSearch, isDev, months,
} from '@/common/utils';
import api from '@/common/api';

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        isLoading: false,
        noInternet: false,
        showOnboarding: false,
        calendarSelected: 2,
        notifications: true,
        currentMonth: 8,
        colors: [
            ['#222121', '#29323C', '#485563'],
            ['#FF1F47', '#FF0844', '#FFB199'],
            ['#85878B', '#78787A', '#BDBBBE'],
            ['#3D4EB8', '#4250A7', '#6B7CE8'],
            ['#48B6AD', '#48BDB7', '#85D8D3'],
            ['#0C74D5', '#3780D7', '#45A6FF'],
            ['#FFA800', '#FDA085', '#F6D365'],
        ],
        user: {
            id: '463377',
        },
        first: {
            canBeDone: false,
            data: {
                name: '',
                icon: '',
                color: 1,
                days: [],
                notify: 0,
                done: [],
            },
            days: [],
        },
        second: {
            canBeDone: false,
            data: {
                name: '',
                icon: '',
                color: 1,
                days: [],
                notify: 0,
                done: [],
            },
            days: [],
        },
        third: {
            canBeDone: false,
            data: {
                name: '',
                icon: '',
                color: 1,
                days: [],
                notify: 0,
                done: [],
            },
            days: [],
        },
        village: {
            pagodas: [],
            paths: [],
        },
    },
    getters: {
        calendar(state) {
            switch (state.calendarSelected) {
            case 1:
                return state.first;
            case 3:
                return state.third;
            default:
                return state.second;
            }
        },
        monthName(state) {
            return months[state.currentMonth - 1];
        },
    },
    mutations: {
        setSelected(state, sel) {
            state.calendarSelected = sel;
        },
        setNotifications(state, n) {
            state.notifications = n;
        },
        setCurrentMonth(state, m) {
            state.currentMonth = m;
        },
        setNoInternet(state, ni) {
            state.noInternet = ni;
        },
        setLoading(state, l) {
            state.isLoading = l;
        },
        setShowOnboarding(state, onb) {
            state.showOnboarding = onb;
        },
        setState(state, s) {
            state.user = s.user;
            state.first = s.first;
            state.second = s.second;
            state.third = s.third;
            state.village = s.village;
        },
    },
    actions: {
        async api({ commit }, { method, data, disableLoading }) {
            if (!disableLoading) commit('setLoading', true);
            const result = await api(method, data);
            commit('setNoInternet', !result);
            if (!disableLoading) commit('setLoading', false);
            if (result && result.state) commit('setState', result.state);
            return result;
        },

        async init({ commit, dispatch }) {
            VKC.init({
                appId: getAppId(),
                accessToken: getPlatform() === 'local' ? process.env.VUE_APP_VK_DEV_TOKEN : '',
                asyncStyle: true,
                uploadProxy: isDev() ? 'http://localhost:5000/proxy' : '/proxy',
                apiVersion: '5.120',
            });

            // set bar color
            VKC.bridge().send(
                'VKWebAppSetViewSettings',
                { status_bar_style: 'dark', action_bar_color: '#FBFBFB' },
            );

            // notifications
            const notifications = Number.parseInt(getSearch().get('vk_are_notifications_enabled'), 10) !== 0;
            commit('setNotifications', notifications);

            // onboarded
            const [onb] = await VKC.send('VKWebAppStorageGet', { keys: ['onboarded'] });
            if (onb && onb.keys) {
                if (!onb.keys.some((k) => k.key === 'onboarded' && k.value)) {
                    commit('setShowOnboarding', true);
                }
            }

            // init
            await dispatch('api', { method: 'init', data: {} });
        },

        saveOnboarding({ commit }) {
            commit('setShowOnboarding', false);
            VKC.send('VKWebAppStorageSet', { key: 'onboarded', value: '1' });
        },
    },
});
