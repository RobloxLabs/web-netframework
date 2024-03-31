using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Roblox.Moderation;
using Roblox.Platform.Membership;
using Roblox.Website.ModelFactories.Moderation;

using UserEntity = Roblox.Users.Entities.User;

namespace Roblox.Website.Admi
{
    public partial class User : System.Web.UI.Page
    {
        public long UserID { get; set; }

        private IUser _IUser;
        private IUserFactory _userFactory;

        public ICollection<PunishmentType> GetPunishmentTypes() => PunishmentType.AllPunishmentTypes;

        protected void Page_Init(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(Request["ID"], out id))
                id = Roblox.User.RobloxAccountID;

            _userFactory = Global.MembershipDomainFactories.UserFactory;
            _IUser = _userFactory.GetUser(id);
            UserID = _IUser.Id;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var punishments = Punishment.GetActivePunishmentsByUserIDPaged(0, 1, (int)UserID);
            PunishmentType accountState = PunishmentType.None;

            if (punishments.Count > 0)
            {
                var punishment = punishments.First();
                accountState = punishment.PunishmentType;
                AccountStateModerationNoteTextBox.Text = punishment.Comment;
                AccountStateMessageToUserTextBox.Text = punishment.ModeratorMessage;
            }

            PunishmentOptionsRadioButtonList.SelectedValue = accountState.ID.ToString();

            PopulatePunishments();

            DataBind();
        }

        protected void PopulatePunishments()
        {
            UserPunishmentsGridView.DataSource = PunishmentModelFactory.PopulatePunishmentModels(_IUser.Id);
        }

        protected void OverrideAccountStateButton_Click(object sender, EventArgs e)
        {
            var comment = AccountStateModerationNoteTextBox.Text.Trim();
            var message = AccountStateMessageToUserTextBox.Text.Trim();
            var punishmentType = PunishmentType.Get(byte.Parse(PunishmentOptionsRadioButtonList.SelectedValue));

            // Don't allow blank moderator messages for punished users
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (punishmentType == PunishmentType.None)
                {
                    // Overrides the account state to treat user as though they aren't punished
                    var user = UserEntity.MustGet(UserID);
                    var account = user.Account;
                    account.AccountStatusID = AccountStatus.OkId;
                    account.Save();
                }
                else
                {
                    // Punish the user
                    var moderator = _userFactory.GetCurrentUser();
                    var punishment = Punishment.CreateNew(
                        (int)UserID,
                        punishmentType.ID,
                        null,
                        (int)moderator.Id,
                        comment,
                        message
                    );
                }
            }
        }
    }
}