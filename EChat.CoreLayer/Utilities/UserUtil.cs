using System;
using System.Security.Claims;

namespace EChat.CoreLayer.Utilities
{
    public static class UserUtil
    {
        public static long GetUserId(this ClaimsPrincipal? claims)
        {
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt64(userId);
        }

        public static string GetUserName(this ClaimsPrincipal? claims)
        {
            
            return claims.FindFirst(ClaimTypes.Name).Value;
            
        }
    }
}