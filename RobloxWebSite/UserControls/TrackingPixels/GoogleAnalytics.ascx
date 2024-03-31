﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoogleAnalytics.ascx.cs" Inherits="Roblox.Website.UserControls.GoogleAnalytics" %>
    <script type="text/javascript">

        var _gaq = _gaq || [];
        _gaq.push(['_setAccount', 'UA-11419793-2']);
        _gaq.push(['_setCampSourceKey', 'rbx_source']);
        _gaq.push(['_setCampMediumKey', 'rbx_medium']);
        _gaq.push(['_setCampContentKey', 'rbx_campaign']);



        _gaq.push(['b._setAccount', '<%=Roblox.WebsiteSettings.Properties.GoogleAnalytics.Default.GoogleAnalyticsAccountCode%>']);
        _gaq.push(['b._setCampSourceKey', 'rbx_source']);
        _gaq.push(['b._setCampMediumKey', 'rbx_medium']);
        _gaq.push(['b._setCampContentKey', 'rbx_campaign']);


        _gaq.push(['c._setAccount', 'UA-26810151-1']);


        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();

    </script>