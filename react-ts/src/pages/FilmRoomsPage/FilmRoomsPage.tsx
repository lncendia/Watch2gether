import BlockTitle from "../../UI/BlockTitle/BlockTitle.tsx";
import PopularFilmsModule from "../../modules/Films/PopularFilmsModule/PopularFilmsModule.tsx";
import AuthorizeModule from "../../modules/Authorization/AuthorizeModule.tsx";

const FilmRoomsPage = () => {
    return (
        <>
            <AuthorizeModule showError={false}>
                <BlockTitle title="Ваши комнаты" className="mt-3 mb-3"/>
                <PopularFilmsModule/>
            </AuthorizeModule>
        </>
    );
};

export default FilmRoomsPage;