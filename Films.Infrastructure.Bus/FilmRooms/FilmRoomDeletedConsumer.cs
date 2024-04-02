// using Films.Application.Abstractions.Commands.FilmRooms;
// using MassTransit;
// using MediatR;
// using Overoom.IntegrationEvents.Rooms.FilmRooms;
//
// namespace Films.Infrastructure.Bus.FilmRooms;
//
// /// <summary>
// /// Обработчик интеграционного события FilmRoomDeletedIntegrationEvent
// /// </summary>
// /// <param name="mediator">Медиатор</param>
// public class FilmRoomDeletedConsumer(ISender mediator) : IConsumer<FilmRoomDeletedIntegrationEvent>
// {
//     /// <summary>
//     /// Метод обработчик 
//     /// </summary>
//     /// <param name="context">Контекст сообщения</param>
//     public Task Consume(ConsumeContext<FilmRoomDeletedIntegrationEvent> context)
//     {
//         // Получаем данные события
//         var integrationEvent = context.Message;
//
//         // Отправляем команду на обработку события
//         return mediator.Send(new Delete
//         {
//             UserId = integrationEvent.ViewerId,
//             RoomId = integrationEvent.RoomId
//         }, context.CancellationToken);
//     }
// }