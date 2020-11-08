﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IMDBDataService;
using IMDBDataService.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebService.DTOs;

namespace WebService.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public CommentsController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateComment(CommentForCreateOrUpdateDto commentForCreateOrUpdateDto)
        {
            var comment = _mapper.Map<Comments>(commentForCreateOrUpdateDto);
            _dataService.CreateComment(comment);
            return Created("", comment);
        }

        [Authorize]
        [HttpPost("flag")]
        public IActionResult FlagComment(FlaggedCommentForCreateDto flaggedCommentForCreateDto)
        {
            var comment = _mapper.Map<FlaggedComments>(flaggedCommentForCreateDto);
            _dataService.FlagComment(comment);
            return Created("", comment);
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateComment(int id, CommentForCreateOrUpdateDto commentForCreateOrUpdateDto)
        {
            var comment = _mapper.Map<Comments>(commentForCreateOrUpdateDto);
            if (_dataService.UpdateComment(id, comment).Result == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            if (_dataService.DeleteComment(id).Result == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
