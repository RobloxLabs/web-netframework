﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RobloxEventManager.ascx.cs" Inherits="Roblox.Website.UserControls.Tracking.RobloxEventManager" %>
    <script type="text/javascript">
        $(function () {
            function trackReturns() {
                function dayDiff(d1, d2) {
                    return Math.floor((d1 - d2) / 86400000);
                }

                var cookieName = 'RBXReturn';
                var cookieOptions = { expires: 9001 };
                var cookie = $.getJSONCookie(cookieName);

                if (typeof cookie.ts === "undefined" || isNaN(new Date(cookie.ts))) {
                    $.setJSONCookie(cookieName, { ts: new Date().toDateString() }, cookieOptions);
                    return;
                }

                var daysSinceFirstVisit = dayDiff(new Date(), new Date(cookie.ts));
                if (daysSinceFirstVisit == 1 && typeof cookie.odr === "undefined") {
                    RobloxEventManager.triggerEvent('rbx_evt_odr', {});
                    cookie.odr = 1;
                }
                if (daysSinceFirstVisit >= 1 && daysSinceFirstVisit <= 7 && typeof cookie.sdr === "undefined") {
                    RobloxEventManager.triggerEvent('rbx_evt_sdr', {});
                    cookie.sdr = 1;
                }

                $.setJSONCookie(cookieName, cookie, cookieOptions);
            }


            RobloxListener.restUrl = window.location.protocol + "//" + "roblox.com/Game/EventTracker.ashx";
            RobloxListener.init();


            GoogleListener.init();




            RobloxEventManager.initialize(true);
            RobloxEventManager.triggerEvent('rbx_evt_pageview');
            trackReturns();



            RobloxEventManager._idleInterval = 450000;
            RobloxEventManager.registerCookieStoreEvent('rbx_evt_initial_install_start');
            RobloxEventManager.registerCookieStoreEvent('rbx_evt_ftp');
            RobloxEventManager.registerCookieStoreEvent('rbx_evt_initial_install_success');
            RobloxEventManager.registerCookieStoreEvent('rbx_evt_fmp');
            RobloxEventManager.startMonitor();


        });

    </script>