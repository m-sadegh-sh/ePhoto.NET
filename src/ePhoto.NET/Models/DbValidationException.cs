using System;

namespace ePhoto.NET.Models {
    public sealed class DbValidationException : Exception {
        public DbValidationException() {}

        public DbValidationException(string message) : base(message) {}

        public DbValidationException(string message, Exception innerException) : base(message, innerException) {}
    }
}