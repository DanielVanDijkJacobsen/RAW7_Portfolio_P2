﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.DataService.BusinessLogic;
using WebService.DataService.DTO;
using WebService.DTOs;
using WebService.Filters;
using WebService.Services;
using WebService.Utils;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingsController : ControllerBase
    {
        private readonly ICastsDataService _castDataService;
        private readonly ITitlesDataService _titleDataService;
        private readonly IUsersDataService _userDataService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;


        public RatingsController(IUsersDataService userDataService, ICastsDataService castDataService, ITitlesDataService titleDataService, IMapper mapper, IUriService uriService)
        {
            _mapper = mapper;
            _userDataService = userDataService;
            _castDataService = castDataService;
            _titleDataService = titleDataService;
            _uriService = uriService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateRating(RatingForCreateDto ratingForCreateDto)
        {
            var newRating = _mapper.Map<UserRating>(ratingForCreateDto);
            var rating = _titleDataService.GetUserRatingByUserIdAndTitleId(newRating.UserId, newRating.TitleId).Result;
            if (rating.Count > 0)
                return NotFound();
            var response = _titleDataService.RateTitle(_mapper.Map<UserRating>(newRating)).Result;
            _castDataService.UpdateNameRating(newRating.TitleId);
            return Created("", response.ToString());
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateRating(RatingForCreateDto ratingForCreateDto)
        {
            var newRating = _mapper.Map<UserRating>(ratingForCreateDto);
            var rating = _titleDataService.GetUserRatingByUserIdAndTitleId(newRating.UserId, newRating.TitleId).Result;
            if (rating.Count < 1)
                return NotFound();
            if (_userDataService.UpdateUserRating(newRating) == null)
                return NotFound();
            _castDataService.UpdateNameRating(newRating.TitleId);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public IActionResult DeleteRating(int userId, string titleId)
        {
           if (_userDataService.DeleteUserRating(userId, titleId) == null)
                return NotFound();
            _castDataService.UpdateNameRating(titleId);
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetRatings(int? userId, string titleId, [FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            List<UserRating> userRatings = new List<UserRating>();
            var totalRecords = 0;

            if (userId != null && titleId != null)
            {
                userRatings = _titleDataService.GetUserRatingByUserIdAndTitleId((int)userId, titleId, validFilter).Result;
                totalRecords = _titleDataService.GetUserRatingByUserIdAndTitleId((int)userId, titleId).Result.Count;
            }
            if (userId != null)
            {
                userRatings = _userDataService.GetUserRatingsByUserId((int)userId, validFilter).Result;
                totalRecords = _userDataService.GetUserRatingsByUserId((int)userId).Result.Count;
            }
            else if (titleId != null)
            {
                userRatings = _titleDataService.GetUserRatingByTitleId(titleId, validFilter).Result;
                totalRecords = _titleDataService.GetUserRatingByTitleId(titleId, validFilter).Result.Count;
            }

            if (userRatings.Count < 1)
                return NotFound();

            var response = PaginationHelper.CreatePagedReponse<UserRatingDto>(_mapper.Map<IEnumerable<UserRatingDto>>(userRatings), validFilter, totalRecords, _uriService, route);

            return Ok(response);
        }
        //[HttpGet]
        //public IActionResult GetRatings(RatingForCreateDto ratingForCreateDto)
        //{
        //    List<UserRating> userRatings = new List<UserRating>();
        //    if (ratingForCreateDto.TitleId != null && ratingForCreateDto.UserId > 0)
        //        userRatings = _titleDataService
        //            .GetUserRatingByUserIdAndTitleId(ratingForCreateDto.UserId, ratingForCreateDto.TitleId).Result;
        //    if (ratingForCreateDto.UserId > 0 && ratingForCreateDto.TitleId == null)
        //         userRatings = _userDataService.GetUserRatingsByUserId(ratingForCreateDto.UserId).Result;
        //    if (ratingForCreateDto.UserId < 1 && ratingForCreateDto.TitleId != null)
        //        userRatings = _titleDataService.GetUserRatingByTitleId(ratingForCreateDto.TitleId).Result;
        //    if (userRatings == null)
        //        return NotFound();
        //    return Ok(_mapper.Map<IEnumerable<UserRatingDto>>(userRatings));
        //}
    }
}
