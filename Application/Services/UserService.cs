using Application.Validators;
using Backend.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Security;

namespace Application.Services
{
    public class UserService : BaseDbService, IUserService
    {
        private readonly IMailSettings mailSettings;
        private readonly IAuthorizationManager authorizationManager;
        private readonly UserValidator validator;
        private readonly UserManager<ApplicationUser> userManager;
        public UserService(ApplicationDbContext db,UserValidator validator,
            UserManager<ApplicationUser> userManager, IMailSettings mailSettings,
            IAuthorizationManager manager) : base(db)
        {
            this.validator = validator;
            this.userManager = userManager;
            this.mailSettings = mailSettings;
            this.authorizationManager = manager;
        }

        public async Task<UserModel> GetUser(Guid Id, CancellationToken token)
        {
            var user = await _db.users.FirstOrDefaultAsync(x => x.Id == Id);
            if (user == null)
            {
                throw new AppNotFoundException();
            }

            var model = new UserModel();
            model.Id = user.Id;
            model.Address = user.Address;
            model.PhoneNumber = user.PhoneNumber;
            model.LastName = user.LastName;
            model.Name = user.FirstName;
            model.Email = user.Email;
            model.Gender = user.Gender;
            model.Birthday = user.BirthDay;


            return model;
        }

        public async Task<UserModel> CreateUser(RegisterModel userModel, CancellationToken token)
        {

           var validatorResault = validator.Validate(userModel);

            if (!validatorResault.IsValid)
            {
                throw new AppBadDataException();
            }
            var user = new ApplicationUser
            {

                FirstName = userModel.FirstName,
                UserName = userModel.FirstName,
                LastName = userModel.LastName,
                PhoneNumber = userModel.PhoneNumber,
                Gender = Gender.Male,
                Address = userModel.Address,
                Email = userModel.Email
            };

         var result =  await userManager.CreateAsync(user, userModel.Password);
            if(!result.Succeeded)
            {
                throw new AppBadDataException();
            }
            
            await _db.SaveChangesAsync(token);
            return await GetUser(user.Id, token);
        }
        public async Task ForgetPassword(string email)
        {
     
            string newPassword = GenerateNewPassword();
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to reset password.");
            }
            string emailTo = email;
            string emailBody = $"{mailSettings.EmailBody} Your new password is: {newPassword}";
            await SendEmailAsync(emailTo, mailSettings.EmailSubject, emailBody);



        }
        private string GenerateNewPassword()
        {
            int passwordLength = 12;
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
            Random random = new Random();
            char[] password = new char[passwordLength];

            for (int i = 0; i < passwordLength; i++)
            {
                password[i] = allowedChars[random.Next(allowedChars.Length)];
            }

            return new string(password);
        }
        private async Task SendEmailAsync(string emailTo, string subject, string body)
        {
            using (var client = new SmtpClient(mailSettings.SmtpServer, mailSettings.SmtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(mailSettings.SmtpUserName, mailSettings.SmtpPassword);
                client.EnableSsl = true;

                using (var message = new MailMessage(mailSettings.EmailFrom, emailTo, subject, body))
                {
                    await client.SendMailAsync(message);
                }
            }
        }

        public async Task<UserModel> GetCurrentLoggedInUserData(CancellationToken token)
        {
            var currenUserId = authorizationManager.GetUserId();
            return await GetUser(currenUserId ?? Guid.Empty, token);

        }

        public async Task UpdateUserData(UserModel model,CancellationToken token)
        {
            var currenUserId = authorizationManager.GetUserId();
            var user = await _db.Users.Where(x => x.Id == currenUserId).FirstOrDefaultAsync(token);

            if(user is null)
            {
                throw new AppBadDataException();
            }
            user.FirstName = model.Name;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Gender = model.Gender;
            user.Email = model.Email;
            user.Address = model.Address;
            await _db.SaveChangesAsync();
        }
    }
}
