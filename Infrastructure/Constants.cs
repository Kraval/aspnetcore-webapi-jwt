using AutoMapper;
using AspNetCoreWebApiJwt.ViewModels;
using AspNetCoreWebApiJwt.Models;

namespace AspNetCoreWebApiJwt.Infrastructure
{
    public static class Constants
    {
        public const string JWT_Claim_Id = "id";
        public const string JWT_Claim_ApipAccess="api_access";
    }
}