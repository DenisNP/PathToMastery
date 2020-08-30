<template>
    <div class="relative" @touchstart="mousedown" @touchend="mouseup" @touchcancel="mouseup">
        <div class="db-color" :style="`background: ${gradient};`"/>
        <div class="db-icon-frame"/>
        <div class="db-icon" :class="{'db-fill-icon': !data.data.icon}">
            {{data.data.icon || '➕'}}
        </div>
        <div class="fill-disk" :style="`transform: scale(${progress});`"/>
    </div>
</template>

<script>
export default {
    name: 'DoButton',
    data() {
        return {
            timer: 0,
            awaitClick: false,
            touchStartTime: 0,
            progress: 0,
            interval: 0,
            waitTime: 2000,
            disableMouseUp: false,
        };
    },
    computed: {
        color() {
            return this.$store.state.colors[Math.max(0, this.data.data.color - 1)];
        },
        gradient() {
            return `linear-gradient(10deg, ${this.color[1]} 0%, ${this.color[2]} 100%)`;
        },
    },
    methods: {
        mousedown() {
            this.touchStartTime = 0;
            clearInterval(this.interval);
            this.progress = 0;
            if (this.active && this.data.canBeDone) {
                this.touchStartTime = (new Date()).getTime();
                this.interval = setInterval(this.calculateProgress, 10);
            }
        },
        mouseup() {
            if (this.disableMouseUp) return;
            if (this.active) {
                const diff = this.touchStartTime === 0
                    ? 0
                    : (new Date()).getTime() - this.touchStartTime;

                this.touchStartTime = 0;
                clearInterval(this.interval);
                if (diff < this.waitTime) {
                    this.touchStartTime = 0;
                    this.progress = 0;
                    if (diff < this.waitTime / 2 || !this.data.canBeDone) this.clicked();
                } else if (this.data.canBeDone) {
                    // this.setDone();
                }
            } else {
                this.$emit('activate');
            }
        },
        clicked() {
            if (this.awaitClick) {
                this.awaitClick = false;
                clearTimeout(this.timer);
                this.timer = 0;
                this.touchStartTime = 0;
                this.clickedDouble();
            } else {
                this.awaitClick = true;
                this.timer = setTimeout(() => this.clickedOnce(), 250);
            }
        },
        clickedOnce() {
            this.awaitClick = false;
            this.touchStartTime = 0;
            this.timer = 0;
            if (this.data.data.name) {
                this.$emit('hint', this.data.canBeDone);
            } else {
                this.$emit('edit');
            }
        },
        clickedDouble() {
            this.$emit('edit');
        },
        calculateProgress() {
            this.progress = ((new Date()).getTime() - this.touchStartTime) / this.waitTime;
            if (this.progress >= 1) {
                this.setDone();
            }
        },
        setDone() {
            this.disableMouseUp = true;
            this.touchStartTime = 0;
            clearInterval(this.interval);
            this.progress = 0;
            this.$emit('done');
            setTimeout(() => {
                this.disableMouseUp = false;
            }, 2000);
        },
    },
    props: {
        data: {
            type: Object,
            required: true,
        },
        active: {
            type: Boolean,
            required: true,
        },
    },
};
</script>

<style>
    .db-color {
        width: 84px;
        height: 84px;
        border-radius: 50%;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.25);
    }

    .db-icon-frame {
        position: absolute;
        width: 44px;
        height: 44px;
        background: rgba(0, 0, 0, 0.2);
        top: 20px;
        left: 20px;
        border-radius: 4px;
        transform: rotate(45deg);
        border: 1px solid rgba(255, 255, 255, 0.3);
    }

    .db-icon {
        position: absolute;
        width: 44px;
        height: 44px;
        top: 20px;
        left: 20px;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 22px;
        /*opacity: 0.9;*/
    }

    .db-fill-icon {
        color: transparent;
        text-shadow: 0 0 0 rgba(255, 255, 255, 0.4);
        font-size: 18px;
    }

    .fill-disk {
        position: absolute;
        width: 84px;
        height: 84px;
        border-radius: 50%;
        background: white;
        top: 0;
        left: 0;
        opacity: 0.4;
    }
</style>
