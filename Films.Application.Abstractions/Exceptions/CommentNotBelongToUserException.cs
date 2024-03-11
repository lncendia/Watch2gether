namespace Films.Application.Abstractions.Exceptions;

public class CommentNotBelongToUserException() : Exception("Comment does not belong to user");