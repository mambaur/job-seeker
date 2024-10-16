using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobSeeker.Models;
using JobSeeker.Repositories;

namespace JobSeeker.Helpers
{
    public class AuthHelper
    {
        public static User? GetLoggedInUser(HttpContext context)
        {
            AuthRepository authRepository = new AuthRepository();
            if (context.Session.TryGetValue("Username", out _))
            {
                var username = context.Session.GetString("Username") ?? "";
                var existingUser = authRepository.GetUserByUsername(username);
                return existingUser;
            }
            return null; // Jika tidak ada user yang ditemukan
        }
    }
}