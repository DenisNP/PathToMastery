<template>
    <div class="relative" >
        <div class="db-color" :style="`background: ${gradient};`"/>
        <div class="db-icon-frame"/>
        <div class="db-icon">{{icon || ''}}</div>
    </div>
</template>

<script>
export default {
    name: 'DoButtonLite',
    computed: {
        gradient() {
            const idx = Math.max(0, this.color - 1);
            const colors = this.$store.state.colors[idx];
            return `linear-gradient(10deg, ${colors[1]} 0%, ${colors[2]} 100%)`;
        },
    },
    props: {
        icon: {
            type: String,
            required: true,
        },
        color: {
            type: Number,
            required: true,
        },
    },
};
</script>
