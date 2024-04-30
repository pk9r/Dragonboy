using UnityEngine;

namespace Mod.Auto
{
    internal class AutoPean
    {
        internal static bool _isAutoRequest;
        internal static bool isAutoRequest 
        {
            get => _isAutoRequest;
            set
            {
                _isAutoRequest = value;
                if (value)
                    HandleSenzuBeans();
            }
        }

        internal static bool _isAutoDonate;
        internal static bool isAutoDonate 
        {
            get => _isAutoDonate;
            set
            {
                _isAutoDonate = value;
                if (value)
                    HandleSenzuBeans();
            }
        }

        internal static bool _isAutoHarvest;
        internal static bool isAutoHarvest
        {
            get => _isAutoHarvest;
            set
            {
                _isAutoHarvest = value;
                if (value)
                    HandleSenzuBeans();
            }
        }

        internal static void Update()
        {
            if (GameCanvas.gameTick % (60 * Time.timeScale) != 0)
                return;
            HandleSenzuBeans();
        }

        static void HandleSenzuBeans()
        {
            if (isAutoRequest && ClanUtils.CanAskForPeans())
                ClanUtils.RequestPeans();
            if (isAutoDonate && ClanUtils.CanDonatePeans())
                ClanUtils.DonatePeans();
            if (isAutoHarvest)
                HarvestMagicTree();
        }

        internal static void HarvestMagicTree()
        {
            var magicTree = GameScr.gI().magicTree;
            if (!Utils.IsMyCharHome() || magicTree.isUpdate || magicTree.isPeasEffect || magicTree.currPeas == 0)
                return;
            Service.gI().openMenu(4);
            Service.gI().confirmMenu(4, 0);
        }
    }
}
