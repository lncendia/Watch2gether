import {useInjection} from "inversify-react";
import {IFilmRoomsService} from "../../../services/RoomsService/IFilmRoomsService.ts";
import {useUser} from "../../../contexts/UserContext/UserContext.tsx";
import Offcanvas from "../../../components/Common/Offcanvas/Offcanvas.tsx";
import CreateFilmRoomForm from "../CreateFilmRoomForm/CreateFilmRoomForm.tsx";
import {useNavigate} from "react-router-dom";

interface CreateFilmRoomModuleProps {
    cdnList: [string, string][],
    id: string,
    open: boolean,
    onClose: () => void
}

const CreateFilmRoomModule = (props: CreateFilmRoomModuleProps) => {

    const roomsService = useInjection<IFilmRoomsService>('FilmRoomsService');
    const {authorizedUser} = useUser()

    // Навигационный хук
    const navigate = useNavigate();

    const createRoom = async (cdn: string, open: boolean) => {
        if (authorizedUser === null) return
        const response = await roomsService.create({
            open: open,
            filmId: props.id,
            cdnName: cdn
        })

        navigate("/filmRoom", {state: {id: response.id}})
    }

    return (
        <Offcanvas title="Создание комнаты" show={props.open} onClose={props.onClose}>
            <CreateFilmRoomForm cdnList={props.cdnList} callback={createRoom}/>
        </Offcanvas>
    );
};

export default CreateFilmRoomModule;