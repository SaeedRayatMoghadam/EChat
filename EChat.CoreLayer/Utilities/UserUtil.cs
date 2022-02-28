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
    }
}