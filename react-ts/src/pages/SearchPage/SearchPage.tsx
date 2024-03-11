import FilmsModule from "../../modules/Films/FilmsModule/FilmsModule.tsx";
import {useLocation} from "react-router-dom";
import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";

const getTitle = (state: any) => {
    if (state.genre) return `Жанр: ${state.genre}`
    if (state.person) return `Принял участие: ${state.person}`
    if (state.country) return `Страна: ${state.country}`
    if (state.serial !== undefined) return state.serial ? "Сериалы" : "Фильмы"
    if (state.year) return `Всё за ${state.year} год`
    return ''
}

const SearchPage = () => {

    const {state} = useLocation();
    return (
        <>
            <BlockTitle title={getTitle(state)} className="mt-5"/>
            <FilmsModule year={state?.year} genre={state?.genre} person={state?.person} country={state?.country}
                         serial={state?.serial}/>
        </>
    );
};

export default SearchPage;