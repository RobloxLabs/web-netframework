﻿using System;
using System.Collections.Generic;
using System.Linq;
using Roblox.Website.ViewModels.Admi.UserAdmin;
using Roblox.Moderation;
using Roblox.Platform.Membership;

namespace Roblox.Website.UserControls.Admi.UserAdmin
{
    public partial class UserNotesPane : System.Web.UI.UserControl
    {
        private static readonly IUserFactory _userFactory = Global.MembershipDomainFactories.UserFactory;

        public UserNotesViewModel Model { get; set; }
        public int UserID { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PopulateUserNotes();

            UserNotesRepeater.DataSource = Model.UserNotes;
            UserNotesRepeater.DataBind();
        }

        protected void btnAddUserNote_Click(object sender, EventArgs e)
        {
            var text = txtUserNotes.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtUserNotes.Text))
            {
                var authenticatedUser = _userFactory.GetCurrentUser();

                var userNote = new UserModerationNote()
                {
                    UserID = UserID,
                    ModeratorID = (int)authenticatedUser.Id,
                    Text = text
                };

                userNote.Save();
                txtUserNotes.Text = string.Empty;
            }
        }

        protected void PopulateUserNotes()
        {
            var userNoteModels = new List<UserNoteModel>();
            var userModerationNotes = UserModerationNote.GetUserModerationNotesByUserIDPaged(0, 256, UserID);

            foreach (var note in userModerationNotes.Reverse())
            {
                var moderator = _userFactory.MustGetUser(note.ModeratorID);
                userNoteModels.Add(new UserNoteModel
                {
                    ID = note.ID,
                    Text = note.Text,
                    ModeratorID = moderator.Id,
                    ModeratorName = moderator.Name,
                    Created = note.Created
                });
            }

            Model = new UserNotesViewModel()
            {
                UserID = UserID,
                UserNotes = userNoteModels
            };
        }
    }
}