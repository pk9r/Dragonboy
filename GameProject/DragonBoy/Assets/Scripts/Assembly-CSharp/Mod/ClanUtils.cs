using System;
using System.Collections.Generic;

namespace Mod
{
    internal class ClanUtils
    {
        static DateTime lastRequestedPean = DateTime.MinValue;
        const sbyte PeanRequestInterval = 5;

        internal static bool CanAskForPeans() => (DateTime.Now - lastRequestedPean).TotalMinutes >= PeanRequestInterval;

        internal static bool CanDonatePeans()
        {
            for (int i = 0; i < ClanMessage.vMessage.size(); i++)
            {
                ClanMessage msg = (ClanMessage)ClanMessage.vMessage.elementAt(i);
                if (msg.type == 1 && msg.recieve < msg.maxCap && msg.playerId != Char.myCharz().charID)
                    return true;
            }
            return false;
        }

        internal static void RequestPeans()
        {
            Service.gI().clanMessage(1, null, -1);
            lastRequestedPean = DateTime.Now;
        }

        internal static void DonatePeans()
        {
            foreach (var msg in GetDonationMsgs())
                Service.gI().clanDonate(msg.id);
        }

        static List<ClanMessage> GetDonationMsgs()
        {
            List<ClanMessage> clanMessages = new List<ClanMessage>();
            for (int i = 0; i < ClanMessage.vMessage.size(); i++)
            {
                ClanMessage msg = (ClanMessage)ClanMessage.vMessage.elementAt(i);
                if (msg.type == 1 && msg.recieve < msg.maxCap && msg.playerId != Char.myCharz().charID)
                    clanMessages.Add(msg);
            }
            return clanMessages;
        }
    }
}
