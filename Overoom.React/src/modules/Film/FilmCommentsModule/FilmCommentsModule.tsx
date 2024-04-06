import {useCallback, useEffect, useState} from 'react';
import {useInjection} from "inversify-react";
import {ICommentsService} from "../../../services/CommentsService/ICommentsService.ts";
import CommentList from "../../../components/Comments/CommentList/CommentList.tsx";
import AddCommentForm from "../AddCommentForm/AddCommentForm.tsx";
import {CommentData} from "../../../components/Comments/CommentItem/CommentData.ts";
import ContentBlock from "../../../UI/ContentBlock/ContentBlock.tsx";

const FilmCommentsModule = ({id, className}: { id: string, className?: string }) => {
//todo: check avatars
    const [comments, setComments] = useState<CommentData[]>([]);
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const commentsService = useInjection<ICommentsService>('CommentsService');

    useEffect(() => {
        const processComments = async () => {
            const response = await commentsService.get({filmId: id})
            setPage(2);
            setHasMore(response.countPages > 1)
            setComments(response.comments)
        };

        processComments().then()
    }, [commentsService, id]);

    const next = useCallback(async () => {
        const response = await commentsService.get({filmId: id, page: page})
        setPage(page + 1);
        setHasMore(response.countPages !== page)
        setComments(prev => [...prev, ...response.comments])
    }, [id, page, commentsService])

    const removeComment = useCallback(async (comment: CommentData) => {
        await commentsService.delete(comment.id)
        setComments(comments.filter(c => c.id !== comment.id))
    }, [comments])

    const addComment = useCallback(async (text: string) => {
        const comment = await commentsService.add({text: text, filmId: id})
        setComments(prev => [comment, ...prev])
    }, [commentsService, id])

    return (
        <>
            <ContentBlock className={className}>
                <AddCommentForm callback={addComment}/>
            </ContentBlock>
            <CommentList hasMore={hasMore} next={next} comments={comments} removeComment={removeComment}/>
        </>
    );
};

export default FilmCommentsModule;