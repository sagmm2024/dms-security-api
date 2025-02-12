namespace SecurityApi.Domain.Dtos
{
    public class UserLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UserRegisterDto : UserLoginDto
    {

        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
    }
}
