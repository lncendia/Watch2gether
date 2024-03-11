import {Film} from "../../../services/FilmsService/Models/Film.ts";
import FilmPlayer from "../../../components/Film/FilmPlayer/FilmPlayer.tsx";

const FilmPlayerModule = ({film, className}: { film: Film, className?: string }) => {
    return (
        <FilmPlayer className={className} cdns={film.cdnList}/>
    );
};

export default FilmPlayerModule;