export const isDev = () => process.env.NODE_ENV === 'development';

export const months = [
    'январь',
    'февраль',
    'март',
    'апрель',
    'май',
    'июнь',
    'июль',
    'август',
    'сентябрь',
    'октябрь',
    'ноябрь',
    'декабрь',
];

export const getSearch = () => {
    const { search } = window.location;
    return new URLSearchParams(search ? search.slice(1) : '');
};

export const getUserId = () => {
    const uidGot = getSearch().get('vk_user_id') || '';
    return (!uidGot && isDev() ? '463377' : uidGot);
};

export const getAppId = () => {
    const aidGot = getSearch().get('vk_app_id') || '';
    return (!aidGot && isDev() ? '7397553' : aidGot);
};

export const getPlatform = () => {
    const p = getSearch().get('vk_platform');
    return (!p && isDev() ? 'local' : p);
};

export const getHash = () => {
    const { hash } = window.location;
    return hash ? hash.slice(1) : '';
};

export const drawImage = (imageSrc, x, y, w, h) => new Promise((resolve) => {
    const img = new Image();
    img.crossOrigin = 'anonymous';
    img.onload = () => {
        const canvas = document.getElementById('canvas');
        const ctx = canvas.getContext('2d');
        if (w && h) ctx.drawImage(img, x, y, w, h);
        else ctx.drawImage(img, x, y);
        resolve();
    };
    img.src = imageSrc;
});

export const chunk = (arr, len) => {
    const chunks = [];
    let i = 0;
    const n = arr.length;

    while (i < n) {
        chunks.push(arr.slice(i, i += len));
    }

    return chunks;
};

export const isCurrentDay = (d) => {
    const now = new Date();
    return now.getDate() === d.d && now.getMonth() + 1 === d.m && now.getFullYear() === d.y;
};

export const getNumericPhrase = (num, one, few, many) => {
    // eslint-disable-next-line no-param-reassign
    num = Number(num) < 0 ? 0 : Number(num);

    let postfix = '';
    if (num < 10) {
        if (num === 1) postfix = one;
        else if (num > 1 && num < 5) postfix = few;
        else postfix = many;
    } else if (num <= 20) {
        postfix = many;
    } else if (num <= 99) {
        const lastOne = num - (Math.floor(num / 10)) * 10;
        postfix = getNumericPhrase(lastOne, one, few, many);
    } else {
        const lastTwo = num - (Math.floor(num / 100)) * 100;
        postfix = getNumericPhrase(lastTwo, one, few, many);
    }
    return postfix;
};

export const range = (num) => [...Array(num).keys()];
