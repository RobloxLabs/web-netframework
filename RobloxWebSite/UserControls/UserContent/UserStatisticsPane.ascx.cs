﻿using System;
using System.Resources;
using Roblox.Website.ViewModels.Users;

namespace Roblox.Website.UserControls.UserContent
{
    public partial class UserStatisticsPane : System.Web.UI.UserControl
    {
        public ProfileViewModel UserProfile;
        public RobloxResourceSet Resources;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserProfile == null)
                return;
            Resources = UserProfile.ProfileLangResources;

            PopulateStatistics();
        }

        private void PopulateStatistics()
        {
            lFriendsStatistics.Text = UserProfile.FriendsCount.ToString("N0");
            pInvitations.Visible = UserProfile.MayEdit;
            // lFriendsInvitedStatistics.Text
            // lForumPostsStatistics.Text
            // lProfileViewsStatistics.Text
            // lPlaceVisitsStatistics.Text
            // lKillsStatistics.Text
            pDeaths.Visible = UserProfile.MayEdit;
            // lDeathsStatistics.Text
            // lHighestEverVotingAccuracyStatistics.Text
            pConsecutiveDaysInBC.Visible = false;
        }
    }
}