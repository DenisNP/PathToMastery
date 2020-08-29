import { getSearch, getUserId, isDev } from './utils';

export default async (method, data) => {
    const apiAddress = isDev() ? 'http://localhost:5000' : '';
    const search = getSearch();
    const params = {};
    search.forEach((value, key) => {
        if (key.startsWith('vk_')) params[key] = value;
    });

    const request = {
        userId: getUserId(),
        sign: search.get('sign') || '',
        params,
    };

    try {
        const response = await fetch(`${apiAddress}/${method}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                ...request,
                ...data,
            }),
        });

        const content = await response.json();
        return content;
    } catch (e) {
        console.error(e);
        return null;
    }
};
