import {useEffect, useState} from 'react';
import VideoWrapper from "../../../UI/VideoWrapper/VideoWrapper.tsx";

const RandomVideoModule = ({children}: { children: React.ReactNode }) => {

    const [randomVideo, setRandomVideo] = useState('');

    useEffect(() => {
        // Создайте массив с ссылками на ваши видео
        const videoList = [
            "video/trailer1.mp4",
            "video/trailer2.mp4",
            "video/trailer3.mp4",
            "video/trailer4.mp4",
            "video/trailer5.mp4"
        ];

        // Выберите случайное видео из массива
        const randomVideo = videoList[Math.floor(Math.random() * videoList.length)];

        // Установите выбранное видео в состояние компонента
        setRandomVideo(randomVideo);
    }, []);

    return (
        <div>
            {randomVideo && (
                <VideoWrapper src={randomVideo}>
                    {children}
                </VideoWrapper>
            )}
        </div>
    );
};

export default RandomVideoModule;