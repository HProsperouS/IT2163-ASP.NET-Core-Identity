using FreshFarmMarket_211283E.Models;
using Microsoft.AspNetCore.Identity;

namespace FreshFarmMarket_211283E.Services
{
    public class PasswordResetServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public PasswordResetServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // password is the new password
        public Result<bool> VerifyPasswordProcess(ApplicationUser user, string password)
        {
            var lastPasswordChangeTime = user.lastPasswordChange;
            if(lastPasswordChangeTime != null) {
                // 1. Check for minimum password change time - 30 mis
                var now = DateTime.UtcNow;
                var timeSinceLastPasswordChange = now - lastPasswordChangeTime.Value;
                // cannot change password within 30 mins from the last change of password 
                var passwordChangeInterval = TimeSpan.FromMinutes(1);
                if (timeSinceLastPasswordChange < passwordChangeInterval)
                {
                    return Result<bool>.Failure("You cannot change password within 30 minutes from the last change.",false);
                }

                // 2. Check for Password History
                else if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success ||
                (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PreviousPasswordHash1 ?? "", password) == PasswordVerificationResult.Success ||
                _userManager.PasswordHasher.VerifyHashedPassword(user, user.PreviousPasswordHash2 ?? "", password) == PasswordVerificationResult.Success))
                {
                    return Result<bool>.Failure("Use a new password! You cannot reuse your previous or current password.",false);
                }

                return Result<bool>.Success("Success", true);

            }
            else
            {
                if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success ||
                (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PreviousPasswordHash1 ?? "", password) == PasswordVerificationResult.Success ||
                _userManager.PasswordHasher.VerifyHashedPassword(user, user.PreviousPasswordHash2 ?? "", password) == PasswordVerificationResult.Success))
                {
                    return Result<bool>.Failure("Use a new password! You cannot reuse your previous or current password.", false);
                }

                return Result<bool>.Success("Success", true);
            }
        }
    }
   

    public class Result<T>
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Result(ResultStatus status, string message, T data)
        {
            this.Status = status;
            this.Message = message;
            this.Data = data;
        }

        public static Result<T> Failure(string errorMessage, T data)
        {
            return new Result<T>(ResultStatus.Failure, errorMessage, data);
        }

        public static Result<T> Success(string message, T data)
        {
            return new Result<T>(ResultStatus.Success, message, data);
        }
    }


    public enum ResultStatus
    {
        Success,
        Failure
    }
}
