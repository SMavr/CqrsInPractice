﻿using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Logic.AppServices;
using Logic.Dtos;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Messages messages;

        public StudentController(Messages messages)
        {
            this.messages = messages;
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            List<StudentDto> list = messages
                .Dispatch(new GetListQuery(enrolled, number));
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Register([FromBody] NewStudentDto dto)
        {
            var command = new RegisterCommand(
                dto.Name, dto.Email,
                dto.Course1, dto.Course1Grade,
                dto.Course2, dto.Course2Grade);

            Result result = messages.Dispatch(command);
            return FromResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Unregister(long id)
        {
            Result result = messages.Dispatch(new UnregisterCommand(id));
            return FromResult(result);
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentEnrollmentDto dto)
        {
            Result result = messages.Dispatch(new EnrollCommand(id, dto.Course, dto.Grade));
            return FromResult(result);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            Result result = messages.Dispatch(new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade));
            return FromResult(result);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentsDto dto)
        {
            Result result = messages.Dispatch(new DisenrollCommand(id, enrollmentNumber, dto.Comment));
            return FromResult(result);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Email, dto.Name);
            Result result = messages.Dispatch(command);
            return FromResult(result);
        }
    }
}
