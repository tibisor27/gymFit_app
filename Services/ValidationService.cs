using System.Text.RegularExpressions;
using GymFit.BE.DTOs;

namespace GymFit.BE.Services
{
    public interface IValidationService
    {
        (bool isValid, string[] errors) ValidatePassword(string password);
        (bool isValid, string[] errors) ValidateEmail(string email);
        (bool isValid, string[] errors) ValidatePhoneNumber(string phoneNumber);
    }

    public class ValidationService : IValidationService
    {
        public (bool isValid, string[] errors) ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is required");
                return (false, errors.ToArray());
            }

            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                errors.Add("Password must contain at least one uppercase letter");

            if (!Regex.IsMatch(password, @"[a-z]"))
                errors.Add("Password must contain at least one lowercase letter");

            if (!Regex.IsMatch(password, @"\d"))
                errors.Add("Password must contain at least one number");

            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
                errors.Add("Password must contain at least one special character");

            if (Regex.IsMatch(password, @"(.)\1{2,}"))
                errors.Add("Password cannot contain three or more consecutive identical characters");

            return (!errors.Any(), errors.ToArray());
        }

        public (bool isValid, string[] errors) ValidateEmail(string email)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email is required");
                return (false, errors.ToArray());
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                errors.Add("Invalid email format");

            return (!errors.Any(), errors.ToArray());
        }

        public (bool isValid, string[] errors) ValidatePhoneNumber(string phoneNumber)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                errors.Add("Phone number is required");
                return (false, errors.ToArray());
            }

            if (!Regex.IsMatch(phoneNumber, @"^(\+4|0)[0-9]{9}$"))
                errors.Add("Phone number must be in Romanian format");

            return (!errors.Any(), errors.ToArray());
        }
    }
} 