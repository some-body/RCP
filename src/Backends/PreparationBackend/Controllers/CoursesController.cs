﻿using Distributed;
using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace PreparationBackend.Controllers
{
    public class CoursesController : ApiController
    {
        private IRepository<Course> _courseRepository;

        public CoursesController()
        {
            _courseRepository = new CoursesRepository();
        }

        // GET: api/Courses
        public IEnumerable<CourseDto> Get()
        {
            return _courseRepository
                .GetAll()
                .Select(e => new CourseDto
                {
                    Id = e.Id ?? 0,
                    Name = e.Name,
                    Description = e.Description
                })
                .AsEnumerable();
        }

        // GET: api/Courses/5
        public PreparationCourseDto Get(int id)
        {
            try
            {
                var course = _courseRepository.GetById(id);
                return new PreparationCourseDto
                {
                    Id = course.Id ?? 0,
                    Name = course.Name,
                    Description = course.Description,
                    MaterialText = course.MaterialText,
                    Questions = course.Questions
                };
            }
            catch
            {
                //ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
        }

        // POST: api/Courses
        public QueryResult Post([FromBody]Course entity)
        {
            var result = new QueryResult();
            try
            {
                _courseRepository.Save(entity);
                //ActionContext.Response.StatusCode = HttpStatusCode.Created;
                result.Success = true;
            }
            catch(Exception ex)
            {
                //ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }

        // DELETE: api/Courses/5
        public QueryResult Delete(int id)
        {
            var result = new QueryResult();
            try
            {
                _courseRepository.RemoveById(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }
    }
}
