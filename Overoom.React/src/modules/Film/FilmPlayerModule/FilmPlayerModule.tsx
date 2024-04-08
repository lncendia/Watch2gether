import {Cdn} from "../../../services/FilmsService/ViewModels/FilmViewModels.ts";
import FilmPlayer from "../../../components/Film/FilmPlayer/FilmPlayer.tsx";

const FilmPlayerModule = ({cdnList, className}: { cdnList: Cdn[], className?: string }) => {
    return (
        <FilmPlayer className={className} cdns={cdnList}/>
    );
};

export default FilmPlayerModule;