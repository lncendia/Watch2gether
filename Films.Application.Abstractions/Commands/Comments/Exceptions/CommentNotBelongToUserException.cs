namespace Films.Application.Abstractions.Commands.Comments.Exceptions;

public class CommentNotBelongToUserException() : Exception("Comment does not belong to user");