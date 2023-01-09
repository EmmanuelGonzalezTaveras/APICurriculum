﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Validaciones
{
    public class PesoArchivoValidacion: ValidationAttribute
    {
        private readonly int pesoMaximoEnMagabytes;

        public PesoArchivoValidacion(int PesoMaximoEnMagabytes)
        {
            pesoMaximoEnMagabytes = PesoMaximoEnMagabytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > pesoMaximoEnMagabytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayhor a {pesoMaximoEnMagabytes}mb.");
            }

            return ValidationResult.Success;
        }
    }
}
