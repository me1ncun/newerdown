using NewerDown.Domain.Result;

namespace NewerDown.Application.Errors;

public class UserErrors
{
    public static readonly Error UserNotFound = new Error(
        "Users.UserNotFound", "User not found");
    
    public static readonly Error InvalidCredentials = new Error(
        "Followers.InvalidCredentials", "Invalid credentials");
    
    public static readonly Error PasswordChangeError = new Error(
        "Followers.PasswordChangeError", "Erorr while changing password");
    
    public static readonly Error RegistrationFailed = new Error(
        "Followers.RegistrationFailed", "User registration failed");
    
    public static readonly Error AlreadyExists = new Error(
        "Followers.AlreadyExists", "User with given email or username already exists");

    public static readonly Error NonPublicProfile = new Error(
        "Followers.NonPublicProfile", "Can't follow non-public profiles");

    public static readonly Error AlreadyFollowing = new Error(
        "Followers.AlreadyFollowing", "Already following");
}