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