<template>
    <f7-page>
        <f7-navbar>
            <img src="../assets/logo.svg" class="logo-image ml3">
        </f7-navbar>
        <calendar/>
    </f7-page>
</template>

<script>
import Calendar from '@/components/Calendar.vue';

export default {
    name: 'Main',
    components: { Calendar },
};
</script>

<style scoped>
.logo-image {
    height: 26px;
}
</style>
