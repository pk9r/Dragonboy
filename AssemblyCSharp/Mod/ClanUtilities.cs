using System;
using System.Collections.Generic;

namespace Mod
{
    internal class ClanUtilities
    {
        private static DateTime lastRequestedPean = DateTime.MinValue;
        private const sbyte PeanRequestInterval = 5;

        public static bool isFromMyClan(Char @char)
        {
            if (@char.charID == Char.myCharz().charID)
                return true;
            else if (@char.isPet())
            {
                @char = GameScr.findCharInMap(-@char.charID);
                if (@char == null)
                    return false;
            }
            return @char.clanID == Char.myCharz().clan.ID;
        }

        public static bool CanAskForPeans()
        {
            TimeSpan timeSinceLastRequest = DateTime.Now - lastRequestedPean;

            return timeSinceLastRequest.TotalMinutes >= PeanRequestInterval;
        }

        public static bool CanDonatePeans()
        {
            for (int i = 0; i < ClanMessage.vMessage.size(); i++)
            {
                ClanMessage msg = (ClanMessage)ClanMessage.vMessage.elementAt(i);

                if (msg.type == 1 && msg.recieve < msg.maxCap && msg.playerId != Char.myCharz().charID) return true;
            }

            return false;
        }

        public static void RequestPeans()
        {
            Service.gI().clanMessage(1, null, -1);
            lastRequestedPean = DateTime.Now;
        }

        public static void DonatePeans()
        {
            var msgs = GetDonationMsgs();

            foreach (var msg in msgs)
            {
                Service.gI().clanDonate(msg.id);
            }
        }

        private static List<ClanMessage> GetDonationMsgs()
        {
            List<ClanMessage> clanMessages = [];

            for (int i = 0; i < ClanMessage.vMessage.size(); i++)
            {
                ClanMessage msg = (ClanMessage)ClanMessage.vMessage.elementAt(i);

                if (msg.type == 1 && msg.recieve < msg.maxCap && msg.playerId != Char.myCharz().charID)
                {
                    clanMessages.Add(msg);
                }
            }

            return clanMessages;
        }
    }
}
