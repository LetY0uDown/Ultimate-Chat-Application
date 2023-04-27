namespace Host.Services.Interfaces;

public interface IPasswordEncoder
{
    string Encode(string password);
}