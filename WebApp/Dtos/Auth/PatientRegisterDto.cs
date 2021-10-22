﻿using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace WebApp.Dtos.Auth
{
    public class PatientRegisterDto : RegisterDto
    {

        public string PatientNumber { get; set; }
        [Required]
        [Display(Name = "Student Or Employee Number")]
        [StringLength(7)]
        public string IdNumber { get; set; }
        public string PictureUrl { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public IFormFile Picture { get; set; }
    }
}