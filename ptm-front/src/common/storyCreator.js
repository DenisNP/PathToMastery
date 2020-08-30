import {
    chunk, drawImage, getAppId, getNumericPhrase,
} from '@/common/utils';
import sticker from '../assets/sticker.png';

// eslint-disable-next-line import/prefer-default-export
export const generateSticker = async (d, name) => {
    const canvas = document.getElementById('canvas');
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    await drawImage(sticker, 0, 0);
    ctx.font = 'bold 30px sans-serif';
    ctx.textAlign = 'center';
    ctx.fillStyle = '#222121';

    ctx.fillText(
        `${d.msD} ${getNumericPhrase(d.msD, 'день', 'дня', 'дней')}`,
        155,
        80,
    );

    ctx.font = '16px sans-serif';
    ctx.textAlign = 'center';
    ctx.fillStyle = '#222121';

    if (name.length > 15) {
        const words = name.split(' ');
        const lines = chunk(words, words.length / 2);

        const firstLine = lines[0].join(' ');
        if (lines.length === 1) {
            ctx.fillText(firstLine, 155, 32);
        } else if (lines.length >= 2) {
            ctx.fillText(firstLine, 155, 25);
            let secondLine = lines[1].join(' ');
            if (lines.length > 2) secondLine += ` ${lines[2].join(' ')}`;
            ctx.fillText(secondLine, 155, 45);
        }
    } else {
        ctx.fillText(name, 155, 32);
    }

    return canvas.toDataURL('image/png');
};

export const createStory = async (d, name) => ({
    background_type: 'none',
    stickers: [
        {
            sticker_type: 'renderable',
            sticker: {
                content_type: 'image',
                blob: await generateSticker(d, name),
                transform: {
                    rotation: -10,
                    translation_x: 0,
                    translation_y: 0.3,
                    gravity: 'center',
                },
                clickable_zones: [
                    {
                        action_type: 'link',
                        action: {
                            link: `https://vk.com/app${getAppId()}`,
                        },
                        // eslint-disable-next-line max-len
                        clickable_area: [{ x: 0, y: 0 }, { x: 9999, y: 0 }, { x: 9999, y: 9999 }, { x: 0, y: 9999 }],
                    },
                ],
                can_delete: true,
            },
        },
    ],
});
