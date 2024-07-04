using System;
using System.Linq;

namespace Mod.Graphics
{
    internal static class HideGameUI
    {
        internal static bool isEnabled;

        internal static void SetState(bool newState) => isEnabled = newState;

        internal static bool ShouldDrawImage(Image image)
        {
            if (!isEnabled)
                return true;
            if (image == GameScr.imgChat || image == GameScr.imgChat2 || image == GameScr.imgChatPC || image == GameScr.imgChatsPC2)
                return false;
            if (image == GameScr.imgFire0 || image == GameScr.imgFire1)
                return false;
            if (image == GameScr.imgHP1 || image == GameScr.imgHP2 || image == GameScr.imgHP3 || image == GameScr.imgHP4)
                return false;
            if (image == GameScr.imgNR1 || image == GameScr.imgNR2 || image == GameScr.imgNR3 || image == GameScr.imgNR4)
                return false;
            if (image == GameScr.imgLbtn || image == GameScr.imgLbtn2)
                return false;
            if (image == GameScr.imgLbtnFocus || image == GameScr.imgLbtnFocus2)
                return false;
            if (image == GameScr.imgSkill || image == GameScr.imgSkill2)
                return false;
            if (image == GameScr.imgFocus || image == GameScr.imgFocus2)
                return false;
            if (image == GameScr.imgAnalog1 || image == GameScr.imgAnalog2)
                return false;
            if (image == GameScr.imgNut || image == GameScr.imgNutF)
                return false;
            if (image == GameScr.imgPanel || image == GameScr.imgPanel2)
                return false;
            if (image == GameScr.imgHP || image == GameScr.imgHPLost || image == GameScr.imgMP || image == GameScr.imgMPLost)
                return false;
            if (image == GameScr.imgArrow || image == GameScr.imgArrow2 || image == GameScr.arrow)
                return false;
            if (image == GameScr.imgMenu || image == GameScr.imgKhung || image == GameScr.imgSP)
                return false;
            if (image == Menu.imgMenu1 || image == Menu.imgMenu2)
                return false;
            if (image == Command.btn0left || image == Command.btn0mid || image == Command.btn0right)
                return false;
            if (image == Command.btn1left || image == Command.btn1mid || image == Command.btn1right)
                return false;
            return true;
        }
    }
}