using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roblox.Platform.Membership;
using Roblox.Platform.Email;
using Roblox.Platform.Roles;
using Roblox.Website.Models.Moderation;

namespace Roblox.Website.ModelFactories.Moderation
{
    public class PunishableUserModelFactory
    {
        private static IUserFactory _userFactory;
        private static IUserEmailFactory _userEmailFactory;
        private static IRoleSetReader _roleSetReader;

        static PunishableUserModelFactory()
        {
            _userFactory = Global.MembershipDomainFactories.UserFactory;
            _userEmailFactory = Global.EmailDomainFactories.UserEmailFactory;
            _roleSetReader = Global.RolesDomainFactories.RoleSetReader;
        }

        public static PunishableUserModel PopulatePunishableUserModel(IUser user)
        {

            var roleSet = _roleSetReader.GetHighestRoleSetForAccountId(user.AccountId);
            var userEmail = _userEmailFactory.GetCurrentVerified(user);
            var emailAddress = userEmail != null ? userEmail.EmailAddress.Address : null;

            var model = new PunishableUserModel
            {
                ID = user.Id,
                Name = user.Name,
                IsOnline = false,
                EmailAddress = emailAddress,
                RoleSet = roleSet.Name,
                Created = user.Created,
                LastActivityDate = null,
                IsApproved = user.AccountStatus == Platform.Membership.AccountStatus.Ok,
                LastLocation = null,
                MembershipType = "None",
                ModerationStatus = user.AccountStatus.ToString()
            };

            return model;
        }

        public static IList<PunishableUserModel> PopulatePunishableUserModels(int? userId, string userName, string emailAddress, int? ipAddress = null)
        {
            var users = Roblox.User.FindUsers(userId, userName, emailAddress, ipAddress);
            var results = new List<PunishableUserModel>();

            foreach (var user in users)
            {
                var user2 = _userFactory.GetUser(user.ID);
                var model = PopulatePunishableUserModel(user2);

                results.Add(model);
            }

            return results;
        }
    }
}