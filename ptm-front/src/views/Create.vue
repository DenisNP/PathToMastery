<template>
    <f7-page>
        <f7-navbar :title="data.name ? 'Редактировать путь' : 'Новый путь'" back-link/>
        <div class="pl3 mt2 mb2 fw5">
            Название
        </div>
        <f7-list no-hairlines-md style="margin: 0;">
            <f7-list-input
                :value="name"
                @input="name = $event.target.value"
                type="text"
                placeholder="Делать зарядку каждый день"
                maxlength="30"
                minlength="1"
                clear-button
            />
        </f7-list>
        <div
            v-show="showIcons"
            class="fixed absolute--fill w-100 h-100 z-5 flex items-center justify-content-center"
        >
            <div class="absolute--fill w-100 h-100 bg-black o-40" @click="showIcons = false"/>
            <VEmojiPicker
                class="absolute ml-auto mr-auto"
                :show-search="false"
                :emojiWithBorder="false"
                @select="changeIcon"
            />
        </div>
        <div class="pl3 fw5 mt4 mb2">
            Иконка и цвет
        </div>
        <div class="w-100 flex items-center content-center">
            <div class="relative colors-block">
                <div
                    v-for="(c, i) in colors"
                    class="c-circle"
                    :key="c[0]"
                    :style="`background-color: ${c[0]}; left: ${c[1]}px; top: ${c[2]}px;`"
                    @click="setColor(i + 1)"
                >
                    <div class="color-selected" v-show="i === color - 1"/>
                </div>
                <do-button-lite
                    :color="color"
                    :icon="icon"
                    class="big-button"
                    @click.native="showIcons = true"
                />
            </div>
        </div>
        <div class="pl3 fw5 mt4 mb3">
            Дни выполнения
        </div>
        <div class="flex justify-around ph2">
            <f7-button
                v-for="(d, i) in allDays"
                fill
                :key="d"
                class="day-btn flex content-center justify-center"
                :class="{'day-btn-inactive': !days.includes(i + 1)}"
                @click="toggleDay(i + 1)"
            >
                {{d}}
            </f7-button>
        </div>
        <div class="pl3 fw5 mt4 mb3">
            Уведомления
        </div>
        <f7-list no-hairlines-md style="margin: 0;">
            <f7-list-item>
                <span>Включить уведомления</span>
                <f7-toggle
                    :checked="notifyEnable"
                    ref="notifyToggle"
                    @toggle:change="enableNotify"
                />
            </f7-list-item>
            <f7-list-input
                label="Когда уведомлять"
                type="time"
                :disabled="!notifyEnable"
                @input="setTime($event.target.value)"
                :value="notifyTime"
            />
        </f7-list>
        <f7-button
            :disabled="this.days.length === 0 || !this.name"
            class="big-btn"
            large
            fill
            @click="savePath"
        >
            Сохранить
        </f7-button>
        <f7-button v-show="data.name" class="big-btn color-red" large outline @click="deletePath">
            Удалить путь
        </f7-button>
    </f7-page>
</template>

<script>
import VEmojiPicker from 'v-emoji-picker';
import DoButtonLite from '@/components/DoButtonLite.vue';
import VKC from '@denisnp/vkui-connect-helper';

export default {
    name: 'Create',
    data() {
        return {
            name: '',
            color: 1,
            icon: '🚴🏻‍♂️',
            showIcons: false,
            days: [],
            notifyEnable: false,
            notifyHour: 10,
            notifyMinute: 0,
            allDays: ['пн', 'вт', 'ср', 'чт', 'пт', 'сб', 'вс'],
        };
    },
    computed: {
        data() {
            const n = Number.parseInt(this.pathId, 10);
            switch (n) {
            case 1:
                return this.$store.state.first.data;
            case 3:
                return this.$store.state.third.data;
            default:
                return this.$store.state.second.data;
            }
        },
        colors() {
            const r = 80;
            return this.$store.state.colors.map((c, i) => {
                const angle = (i * 2 * Math.PI) / this.$store.state.colors.length - Math.PI / 2;
                return [c[0], r * Math.cos(angle), r * Math.sin(angle)];
            });
        },
        notifyTime() {
            return `${this.notifyHour.toString().padStart(2, '0')}:${this.notifyMinute.toString().padStart(2, '0')}`;
        },
    },
    methods: {
        changeIcon(emoji) {
            this.showIcons = false;
            this.icon = emoji.data;
        },
        setColor(num) {
            this.color = num;
        },
        toggleDay(d) {
            const idx = this.days.indexOf(d);
            if (idx >= 0) this.days.splice(idx, 1);
            else this.days.push(d);
        },
        async enableNotify(e) {
            const enable = e.target.checked;
            if (enable && !this.$store.state.notifications) {
                const [enabled] = await VKC.send('VKWebAppAllowNotifications');
                if (!enabled) {
                    this.notifyEnable = false;
                    this.$refs.notifyToggle.toggle();
                    return;
                }
                this.$store.commit('setNotifications', true);
            }
            this.notifyEnable = enable;
        },
        setTime(val) {
            const data = val.split(':');
            const h = Number.parseInt(data[0], 10);
            const m = Number.parseInt(data[1] || '0', 10);
            this.notifyHour = Number.isNaN(h) ? 10 : h;
            this.notifyMinute = Number.isNaN(m) ? 0 : m;
        },
        savePath() {
            this.days.sort();
            if (this.data.days.length > 0 && this.data.days.join('|') !== this.days.join('|')) {
                this.$f7.dialog.confirm(
                    'Редактирование дней выполнения Пути может сказаться на непрерывности вашего прогресса. Продолжить?',
                    'Изменить путь',
                    () => this.confirmEdit(),
                );
            } else {
                this.confirmEdit();
            }
        },
        async deletePath() {
            this.$f7.dialog.confirm(
                'Удаление Пути необратимо и сотрёт весь ваш прогресс по нему. Продолжить?',
                'Удалить путь',
                () => this.confirmDelete(),
            );
        },
        async confirmEdit() {
            const result = await this.$store.dispatch('api', {
                method: 'create',
                data: {
                    id: this.pathId,
                    color: this.color,
                    name: this.name,
                    icon: this.icon,
                    notify: this.notifyEnable ? (this.notifyHour * 100 + this.notifyMinute) : -1,
                    days: this.days,
                },
            });
            if (result && result.state) {
                this.$f7.views.main.router.back();
            }
        },
        async confirmDelete() {
            const result = await this.$store.dispatch('api', {
                method: 'delete',
                data: {
                    id: this.pathId,
                },
            });
            if (result && result.state) {
                this.$f7.views.main.router.back();
            }
        },
    },
    mounted() {
        this.name = this.data.name;
        this.color = this.data.color || 1;
        this.icon = this.data.icon || '🚴🏻‍♂️';
        this.days = [...this.data.days] || [];
        if (this.data.notify < 0) {
            this.notifyHour = 10;
            this.notifyMinute = 0;
            this.notifyEnable = false;
        } else {
            this.notifyHour = Math.floor(this.data.notify / 100);
            this.notifyMinute = this.data.notify - this.notifyHour * 100;
            this.notifyEnable = this.$store.state.notifications;
        }
    },
    props: {
        pathId: {
            type: String,
            required: true,
        },
    },
    components: {
        DoButtonLite,
        VEmojiPicker,
    },
};
</script>

<style>
    .ios .day-btn {
        padding-top: 5px;
    }

    .md .day-btn {
        padding-top: 2px;
    }
</style>

<style scoped>
    .c-circle {
        width: 40px;
        height: 40px;
        position: absolute;
        border-radius: 50%;
        transform: translate(-50%, -50%);
    }

    .colors-block {
        height: 200px;
        left: 50vw;
        top: 100px;
    }

    .big-button {
        transform: translate(-50%, -50%);
    }

    .color-selected {
        width: 50px;
        height: 50px;
        position: absolute;
        border-radius: 50%;
        top: -5px;
        left: -5px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05), inset 0px 4px 4px rgba(0, 0, 0, 0.2);
    }

    .day-btn {
        width: 11vw;
        height: 11vw;
        text-transform: none;
        text-overflow: inherit;
    }

    .day-btn-inactive {
        background-color: #E0E0E0;
        color: rgba(0, 0, 0, 0.2);
    }

    .big-btn {
        margin: 12px;
    }

    .splitter {
        background: rgba(0, 0, 0, 0.2);
        height: 1px;
        margin-top: 24px;
        margin-bottom: 14px;
    }
</style>
