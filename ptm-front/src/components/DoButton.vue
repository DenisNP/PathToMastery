<template>
    <div class="relative">
        <div class="btn-color" :style="`background: ${gradient};`"/>
        <div class="icon-frame"/>
        <div class="icon" :class="{'fill-icon': !data.icon}">{{data.icon || '➕'}}</div>
    </div>
</template>

<script>
export default {
    name: 'DoButton',
    computed: {
        color() {
            return this.$store.state.colors[Math.max(0, this.data.color - 1)];
        },
        gradient() {
            return `linear-gradient(10deg, ${this.color[1]} 0%, ${this.color[2]} 100%)`;
        },
    },
    props: {
        data: {
            type: Object,
            required: true,
        },
    },
};
</script>

<style scoped>
    .btn-color {
        width: 84px;
        height: 84px;
        border-radius: 50%;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.25);
    }

    .icon-frame {
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

    .icon {
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

    .fill-icon {
        color: transparent;
        text-shadow: 0 0 0 rgba(255, 255, 255, 0.4);
        font-size: 18px;
    }
</style>
