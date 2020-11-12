﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebService.DataService.DTO;
using WebService.DataService.Repositories;

namespace WebService.DataService.BusinessLogic
{
    public class TitleDataService : ITitlesDataService
    {
        private readonly TitleRepository _titles;
        private readonly CastInfoRepository _castInfo;
        private readonly UserRatingRepository _userRating;
        private readonly TitleFormatRepository _titleFormat;
        private readonly IGenericRepository<TitleGenres> _titleGenre;
        private readonly TitleAliasRepository _titleAlias;
        private readonly TitleInfoRepository _titleInfo;
        private readonly IGenericRepository<Episodes> _episodes;
        private readonly CommentsRepository _comments;
        private readonly CastsRepository _casts;
        private readonly BookmarkRepository _bookmarks;
        private readonly FlaggedCommentsRepository _flaggedComments;

        public TitleDataService()
        {
            var context = new ImdbContext();
            _castInfo = new CastInfoRepository(context);
            _titles = new TitleRepository(context);
            _casts = new CastsRepository(context);
            _userRating = new UserRatingRepository(context);
            _titleFormat = new TitleFormatRepository(context);
            _titleGenre = new GenericRepository<TitleGenres>(context);
            _titleAlias = new TitleAliasRepository(context);
            _titleInfo = new TitleInfoRepository(context);
            _episodes = new GenericRepository<Episodes>(context);
            _comments = new CommentsRepository(context);
            _bookmarks = new BookmarkRepository(context);
            _flaggedComments = new FlaggedCommentsRepository(context);
        }

        public async Task<Titles> GetTitleById(object id)
        {
            return await _titles.ReadById(id);
        }

        public async Task<List<Titles>> GetPopularTitles(int num, string type)
        {
            return await _titles.GetPopularTitles(num, type);
        }

        public async void RateTitle(UserRating rating)
        {
            await _userRating.Create(rating);
            return;
        }

        public async Task<List<Comments>> GetCommentsByTitleId(string id)
        {
            return await _comments.WhereByTitleId(id);
        }

        public async Task<List<Casts>> GetCastsByTitleId(string id)
        {
            return await _casts.WhereByTitleId(id);
        }

        public async Task<List<CastInfo>> SearchByName(string name)
        {
            return await _castInfo.SearchByName(name);
        }

        public async Task<List<Casts>> GetAllCasts()
        {
            return await _casts.ReadAll();
        }

        public async Task<List<TitleInfo>> GetTitleInfoByTitleId(string id)
        {
            return await _titleInfo.WhereByTitleId(id);
        }

        public async Task<List<TitleAlias>> GetTitleAliasByTitleId(string id)
        {
            return await _titleAlias.WhereByTitleId(id);
        }

        public async Task<List<TitleFormats>> GetTitleFormatByTitleId(string id)
        {
            return await _titleFormat.WhereByTitleId(id);
        }

        public async Task<List<Titles>> SearchForTitle(int? num, string searchString)
        {
            return await _titles.SearchForTitle(num, searchString);
        }

        public async Task<List<Titles>> GetAllTitles()
        {
            return await _titles.ReadAll();
        }

        public async Task<List<UserRating>> GetAllUserRatings()
        {
            return await _userRating.ReadAll();
        }

        public async Task<UserRating> UpdateUserRating(UserRating entity)
        {
            return await _userRating.Update(entity);
        }

        public async Task<UserRating> DeleteUserRating(UserRating entity)
        {
            return await _userRating.Delete(entity);
        }

        public async Task<UserRating> CreateUserRating(UserRating entity)
        {
            return await _userRating.Create(entity);
        }

        public async Task<Comments> CreateComment(Comments entity)
        {
            entity.CommentTime = DateTime.Now;
            return await _comments.Create(entity);
        }

        public async Task<Comments> UpdateComment(int id, Comments comment)
        {
            var entity = _comments.ReadById(id).Result;
            entity.Comment = comment.Comment;
            entity.CommentTime = comment.CommentTime;
            entity.IsEdited = true;
            return await _comments.Update(entity);
        }

        public async Task<FlaggedComment> FlagComment(int id, FlaggedComment comment)
        {

            return await _flaggedComments.FlagComment(comment);
        }

        public async Task<List<FlaggedComment>> GetFlaggedComment(int userId, int commentId)
        {
            return await _flaggedComments.WhereByUserIdAndCommentId(userId, commentId);
        }

        public async Task<FlaggedComment> DeleteFlaggedComment(int userId, int commentId)
        {
            var flaggedComment = await GetFlaggedComment(userId, commentId);
            return await _flaggedComments.Delete(flaggedComment.First());
        }

        public async Task<List<UserRating>> GetUserRatingByUserIdAndTitleId(int userId, string titleId)
        {
            return await _userRating.WhereByUserIdAndTitleId(userId, titleId);
        }

        public async Task<Bookmarks> CreateBookmark(Bookmarks entity)
        {
            return await _bookmarks.Create(entity);
        }

        public async Task<List<Bookmarks>> GetBookmark(string tid, int uid)
        {
            return await _bookmarks.WhereByTitleAndUserId(uid, tid);
        }
    }
}
