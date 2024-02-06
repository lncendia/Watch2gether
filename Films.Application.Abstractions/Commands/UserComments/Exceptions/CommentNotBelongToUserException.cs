namespace Films.Application.Abstractions.Commands.UserComments.Exceptions;

public class CommentNotBelongToUserException() : Exception("Comment does not belong to user");