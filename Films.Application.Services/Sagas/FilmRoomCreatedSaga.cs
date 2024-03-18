// using MassTransit;
// using Overoom.IntegrationEvents.Rooms.FilmRooms;
//
// namespace Films.Application.Services.Sagas;
//
// public class FilmRoomCreatedSaga : MassTransitStateMachine<FilmRoomCreatedState>
// {
//     public FilmRoomCreatedSaga()
//     {
//         InstanceState(x => x.CurrentState);
//         Event(() => FilmRoomCreated, x => x.CorrelateById(context => context.Message.Id));
//         Initially(When(FilmRoomCreated).TransitionTo(FilmRoomProcessing));
//         During(FilmRoomProcessing, When(FilmRoomProcessed).Finalize());
//     }
//
//     public Event<FilmRoomCreatedIntegrationEvent>? FilmRoomCreated { get; private set; }
//     public State? FilmRoomProcessing { get; private set; }
//     public Event<FilmRoomProcessedIntegrationEvent>? FilmRoomProcessed { get; private set; }
// }
//
// public class FilmRoomCreatedState : SagaStateMachineInstance
// {
//     public Guid CorrelationId { get; set; }
//     public int CurrentState { get; set; }
// }