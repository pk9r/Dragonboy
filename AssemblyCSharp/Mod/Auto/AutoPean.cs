using UnityEngine;

namespace Mod.Auto
{
    internal class AutoPean
    {
        public static bool isAutoRequestEnabled { get; private set; } = false;
        public static bool isAutoDonateEnabled { get; private set; } = false;
        public static bool isAutoHarvestEnabled { get; private set; } = false;

        public static void SetAutoRequestState(bool newState)
        {
            isAutoRequestEnabled = newState;
            if (isAutoRequestEnabled) Update();
        }

        public static void SetAutoDonateState(bool newState)
        {
            isAutoDonateEnabled = newState;
            if (isAutoDonateEnabled) Update();
        }

        public static void SetAutoHarvestState(bool newState)
        {
            isAutoHarvestEnabled = newState;
            if (isAutoHarvestEnabled) Update();
        }

        public static void Update()
        {
            if (GameCanvas.gameTick % (60 * Time.timeScale) != 0) return;

            if (isAutoRequestEnabled && ClanUtilities.CanAskForPeans()) ClanUtilities.RequestPeans();
            if (isAutoDonateEnabled && ClanUtilities.CanDonatePeans()) ClanUtilities.DonatePeans();
            if (isAutoHarvestEnabled) HarvestMagicTree();
        }

        public static void HarvestMagicTree()
        {
            var magicTree = GameScr.gI().magicTree;

            if (!CharUtilities.isMyCharHome() || magicTree.isUpdate || magicTree.isPeasEffect || magicTree.currPeas == 0) return;

            Service.gI().openMenu(4);
            Service.gI().confirmMenu(4, 0);
        }
    }
}
