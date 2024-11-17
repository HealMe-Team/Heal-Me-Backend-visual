﻿namespace HealMeAppBackend.API.Authentication.Application.OutboundServices
{
    public interface IHashingService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
