using System;
using System.Collections.Generic;
using System.Linq;
using Roblox.Moderation;
using Roblox.Platform.Membership;
using Roblox.Website.Models.Moderation;

namespace Roblox.Website.ModelFactories.Moderation
{
    public class PunishmentModelFactory
    {
        private static IUserFactory _userFactory;


        static PunishmentModelFactory()
        {
            _userFactory = Global.MembershipDomainFactories.UserFactory;
        }

        public static PunishmentModel PopulatePunishmentModel(Punishment punishment, IUser moderator)
        {
            return new PunishmentModel
            {
                ID = punishment.ID,
                Action = punishment.PunishmentType.Value,
                ModeratorID = punishment.ModeratorID,
                ModeratorName = moderator.Name,
                InternalNote = punishment.Comment,
                ModeratorMessage = punishment.ModeratorMessage,
                Created = punishment.Created,
                Expiration = punishment.Expiration
            };
        }

        public static IList<PunishmentModel> PopulatePunishmentModels(long userId)
        {
            var punishmentModels = new List<PunishmentModel>();
            var punishments = Punishment.GetPunishmentsByUserIDPaged(0, 256, (int)userId).Reverse();

            foreach (var punishment in punishments)
            {
                var moderator = _userFactory.MustGetUser(punishment.ModeratorID);
                var punishmentModel = PopulatePunishmentModel(punishment, moderator);

                punishmentModels.Add(punishmentModel);
            }

            return punishmentModels;
        }
    }
}