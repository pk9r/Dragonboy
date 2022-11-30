using Mod.ModHelper;
using Mod.ModHelper.CommandMod.Chat;

namespace Mod.Auto
{
    public class AutoAttack : ThreadActionUpdate<AutoAttack>
    {
        public override int Interval => 100;

        protected override void update()
        {
            var vMob = new MyVector();
            var vChar = new MyVector();

            var myChar = Char.myCharz();
            if (myChar.mobFocus != null)
                vMob.addElement(myChar.mobFocus);
            else if (myChar.charFocus != null)
                vChar.addElement(myChar.charFocus);

            if (vMob.size() > 0 || vChar.size() > 0)
            {
                var myskill = myChar.myskill;
                long currentTimeMillis = mSystem.currentTimeMillis();

                if (currentTimeMillis - myskill.lastTimeUseThisSkill > myskill.coolDown)
                {
                    Service.gI().sendPlayerAttack(vMob, vChar, -1); // type = -1 -> auto
                    myskill.lastTimeUseThisSkill = currentTimeMillis;
                }
            }
        }

        [ChatCommand("ak")]
        public static void toggleAutoAttack()
        {
            gI.toggle();

            if (gI.IsActing)
                GameScr.info1.addInfo("Đang tự tấn công", 0);
            else
                GameScr.info1.addInfo("Đã tắt tự tấn công", 0);
        }
    }
}
