using Mod.Xmap;
using System.Collections.Generic;
using UnityEngine;

namespace Mod.Auto
{
    public class AutoPet
    {
        public static List<int> mobTemplateIdList = new List<int>();

        public static List<int> mobIdList = new List<int>();
        private static long lastTimePick;
        private static int lastX;
        private static bool isAssignedLastX;
        private static int lastXPet;
        private static bool isAssignedLastXPet;
        private static bool isMyPetDied;

        public static bool isPicking;
        private static long lastTimePetFollow = mSystem.currentTimeMillis();
        private static bool isGoHomeGetMorePean;
        private static bool isMagicTreeUpgrading;
        private static bool isMagicTreeOutOfPean;
        private static long lastTimeCheckMagicTree;
        private static KeyValuePair<int, int> mapZoneGoBack = new KeyValuePair<int, int>();
        private static long delayCheckPet;

        public static bool isFirstTimeCkeckPet = true;
        private static bool isTTNL;

        public static bool isSaoMayLuoiThe;
        public static AutoPetMode mode { get; private set; }
        public static AttackMode modeAttackWhenNeeded { get; private set; }

        public static void update()
        {
            if (mode == AutoPetMode.Disabled) return;
            if (isFirstTimeCkeckPet)
            {
                delayCheckPet = mSystem.currentTimeMillis();
                isFirstTimeCkeckPet = false;
            }
            if (mSystem.currentTimeMillis() - delayCheckPet < 3000) return;
            if (GameCanvas.gameTick % (30 * Time.timeScale) != 0 || XmapController.gI.IsActing) return;
            AutoPick();
            if (Char.myPetz().cStamina < 5 && GameScr.hpPotion > 0 && !(Char.myPetz().cHP <= 0 || Char.myPetz().isDie)) GameScr.gI().doUseHP();
            if (isMagicTreeOutOfPean && mSystem.currentTimeMillis() - lastTimeCheckMagicTree >= 600000)
            {
                isMagicTreeOutOfPean = false;
                lastTimeCheckMagicTree = mSystem.currentTimeMillis();
            }
            if (!isMagicTreeUpgrading)
            {
                if (GameScr.hpPotion == 0 && !isGoHomeGetMorePean && !isMagicTreeOutOfPean)
                {
                    isGoHomeGetMorePean = true;
                    mapZoneGoBack = new KeyValuePair<int, int>(TileMap.mapID, TileMap.zoneID);
                }
            }
            if (isGoHomeGetMorePean && !isMagicTreeOutOfPean && GameCanvas.gameTick % (60 * Time.timeScale) == 0)
            {
                if (TileMap.mapID != Char.myCharz().cgender + 21)
                {
                    if (!XmapController.gI.IsActing && GameScr.hpPotion == 0) XmapController.start(Char.myCharz().cgender + 21);
                }
                else
                {
                    if (GameCanvas.menu.showMenu) GameCanvas.menu.doCloseMenu();
                    if (GameScr.gI().magicTree.currPeas > 0)
                    {
                        Service.gI().openMenu(4);
                        Service.gI().confirmMenu(4, 0);
                        return;
                    }
                    else isMagicTreeOutOfPean = true;
                    if (GameScr.gI().magicTree.isUpdateTree)
                    {
                        lastTimeCheckMagicTree = mSystem.currentTimeMillis();
                        isMagicTreeUpgrading = true;
                    }
                    if ((GameScr.gI().magicTree.isPeasEffect || GameScr.gI().magicTree.isUpdateTree || GameScr.gI().magicTree.currPeas == 0) && !XmapController.gI.IsActing) XmapController.start(mapZoneGoBack.Key);
                }
                if (TileMap.mapID == mapZoneGoBack.Key && !XmapController.gI.IsActing)
                {
                    if (TileMap.zoneID == mapZoneGoBack.Value)
                    {
                        if (Char.myCharz().cx == lastX) isGoHomeGetMorePean = false;
                    }
                    else if (GameCanvas.gameTick % (120 * Time.timeScale) == 0) Service.gI().requestChangeZone(mapZoneGoBack.Value, 0);
                }
            }
            TeleToSafePos();
            AutoSkill();
            if (isMyPetDied) return;
            else if (isAssignedLastXPet)
            {
                isAssignedLastXPet = false;
                Utilities.teleportMyChar(lastXPet);
            }
            if (mSystem.currentTimeMillis() - lastTimePetFollow > 600000 || GameScr.findCharInMap(-Char.myCharz().charID) != null && Utilities.getDistance(Char.myCharz(), GameScr.findCharInMap(-Char.myCharz().charID)) > 400)
            {
                lastTimePetFollow = mSystem.currentTimeMillis();
                Service.gI().petStatus(0);
                Utilities.teleportMyChar(Char.myCharz().cx);
                return;
            }
            if (mode == AutoPetMode.Normal)
            {
                //up đệ thường
                if (Char.myPetz().petStatus != 2) Service.gI().petStatus(2);    //tấn công
            }
            else
            {
                if (isTTNL) return;
                if (mode == AutoPetMode.AvoidSuperMob)
                {
                    //up đệ né siêu quái
                    if (Char.myPetz().petStatus != 1) Service.gI().petStatus(1);    //bảo vệ
                    Mob closestMob = ClosestMob();
                    if (closestMob != null && closestMob.x > 50 && closestMob.y > 50)
                    {
                        Char.myCharz().currentMovePoint = new MovePoint(closestMob.x + Res.random(-5, 5), closestMob.y);
                    }
                }
                else if (mode == AutoPetMode.Kaioken)
                {
                    //up đệ kaioken
                    if (Char.myPetz().petStatus != 2) Service.gI().petStatus(2);    //tấn công
                    if (GameCanvas.gameTick % (60 * Time.timeScale) == 0)
                    {
                        Char.myCharz().cy--;
                        Service.gI().charMove();
                    }
                    if (GameCanvas.gameTick % (60 * Time.timeScale) == 30 * Time.timeScale)
                    {
                        Char.myCharz().cy++;
                        Service.gI().charMove();
                    }
                }
            }
        }

        private static void TeleToSafePos()
        {
            if (!isPicking && (Char.myCharz().arrItemBody[5] == null || Char.myCharz().arrItemBody[5].template.id != 449))
            {
                Char myPetInMap = GameScr.findCharInMap(-Char.myCharz().charID);
                Char myPet = Char.myPetz();
                if (myPet != null)
                {
                    if (myPet.cHP <= 0 || myPet.isDie)
                    {
                        if (!isMyPetDied)
                        {
                            isMyPetDied = true;
                            if (!isAssignedLastXPet)
                            {
                                isAssignedLastXPet = true;
                                lastXPet = Char.myCharz().cx;
                            }
                            Utilities.teleportMyChar(50);
                        }
                    }
                    else if (isMyPetDied) isMyPetDied = false;
                }
                else if (myPetInMap != null)
                {
                    if (myPetInMap.cHP <= 0 || myPetInMap.isDie)
                    {
                        if (!isMyPetDied)
                        {
                            isMyPetDied = true;
                            if (!isAssignedLastXPet)
                            {
                                isAssignedLastXPet = true;
                                lastXPet = Char.myCharz().cx;
                            }
                            Utilities.teleportMyChar(50);
                        }
                    }
                    else if (isMyPetDied) isMyPetDied = false;
                }
                else if (isMyPetDied) isMyPetDied = false;
            }
        }

        private static void AutoSkill()
        {
            //auto skill 3
            Skill skill3 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[2]);
            Skill skill1 = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[0]);
            if (skill3.point > 0 && mSystem.currentTimeMillis() - skill3.lastTimeUseThisSkill > skill3.coolDown)
            {
                if ((Char.myPetz().cHP * 100 / Char.myPetz().cHPFull < 10 || Char.myPetz().cMP * 100 / Char.myPetz().cMPFull < 10 || Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 10 || Char.myCharz().cMP * 100 / Char.myCharz().cMPFull < 10) && Char.myCharz().cgender == 1 && skill3.manaUse < Char.myCharz().cMP)
                {
                    if (skill3.point > 1) Utilities.buffMe();
                    else
                    {
                        MyVector vecPet = new MyVector();
                        vecPet.addElement(GameScr.findCharInMap(-Char.myCharz().charID));
                        Service.gI().selectSkill(skill3.template.id);
                        Service.gI().sendPlayerAttack(new MyVector(), vecPet, 2);
                        Service.gI().selectSkill(skill1.template.id);
                    }
                    skill3.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    return;
                }
                if ((Char.myCharz().cHP * 100 / Char.myCharz().cHPFull < 10 || Char.myCharz().cMP * 100 / Char.myCharz().cMPFull < 10) && Char.myCharz().cgender == 2)
                {
                    GameScr.gI().doUseSkillNotFocus(skill3);
                    isTTNL = true;
                    skill3.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                    return;
                }
            }
            if (Char.myCharz().cgender == 2 && isTTNL && mSystem.currentTimeMillis() - skill3.lastTimeUseThisSkill > 10000)
            {
                Char.myCharz().myskill = skill1;
                isTTNL = false;
            }
            //auto khống chế siêu quái
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob superMob = (Mob)GameScr.vMob.elementAt(i);
                if (superMob.levelBoss != 0 && !superMob.isMobMe && superMob.hp > 0)
                {
                    Char.myCharz().mobFocus = superMob;
                    if (Char.myCharz().cgender == 0)
                    {
                        Skill skillTDHS = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[2]);
                        if (skillTDHS.point > 0 && skillTDHS.manaUse < Char.myCharz().cMP && mSystem.currentTimeMillis() - skillTDHS.lastTimeUseThisSkill > skillTDHS.coolDown)
                        {
                            MyVector myVector = new MyVector();
                            myVector.addElement(superMob);
                            Service.gI().selectSkill(skillTDHS.template.id);
                            Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
                            Service.gI().selectSkill(skill1.template.id);
                            skillTDHS.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                        }
                    }
                    else if (Char.myCharz().cgender == 1)
                    {
                        Skill skillChocolate = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]);
                        if (skillChocolate.point > 0 && skillChocolate.manaUse < Char.myCharz().cMP && mSystem.currentTimeMillis() - skillChocolate.lastTimeUseThisSkill > skillChocolate.coolDown)
                        {
                            MyVector myVector = new MyVector();
                            myVector.addElement(superMob);
                            Service.gI().selectSkill(skillChocolate.template.id);
                            Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
                            Service.gI().selectSkill(skill1.template.id);
                            skillChocolate.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                        }
                    }
                    else if (Char.myCharz().cgender == 2)
                    {
                        Skill skillTie = Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[6]);
                        if (skillTie.point > 0 && skillTie.manaUse < Char.myCharz().cMP && mSystem.currentTimeMillis() - skillTie.lastTimeUseThisSkill > skillTie.coolDown)
                        {
                            MyVector myVector = new MyVector();
                            myVector.addElement(superMob);
                            Service.gI().selectSkill(skillTie.template.id);
                            Service.gI().sendPlayerAttack(myVector, new MyVector(), 1);
                            Service.gI().selectSkill(skill1.template.id);
                            skillTie.lastTimeUseThisSkill = mSystem.currentTimeMillis();
                        }
                    }
                    return;
                }
            }
            //đánh khi đệ cần
            if (isSaoMayLuoiThe)
            {
                isSaoMayLuoiThe = false;
                Service.gI().selectSkill(skill1.template.id);
                switch (modeAttackWhenNeeded)
                {
                    case AttackMode.AttackClosestMob:
                        MyVector vecMob = new MyVector();
                        vecMob.addElement(ClosestMob());
                        Service.gI().sendPlayerAttack(vecMob, new MyVector(), 1);
                        break;
                    case AttackMode.AttackMyPet:
                        if (Char.myCharz().cFlag != 8) Service.gI().getFlag(1, 8);
                        MyVector vecPet = new MyVector();
                        vecPet.addElement(GameScr.findCharInMap(-Char.myCharz().charID));
                        Service.gI().sendPlayerAttack(new MyVector(), vecPet, 2);
                        break;
                    case AttackMode.AttackMyself:
                        if (Char.myCharz().cFlag != 8) Service.gI().getFlag(1, 8);
                        Service.gI().sendPlayerAttack(new MyVector(), Utilities.getMyVectorMe(), 2);
                        break;
                }
            }
        }

        private static Mob ClosestMob()
        {
            Mob result = null;
            int minDistance = 9999;
            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob mob = (Mob)GameScr.vMob.elementAt(i);
                if (mob.status != 0 && mob.status != 1 && mob.hp > 0 && !mob.isMobMe && mob.levelBoss == 0 && mob.getTemplate().type != 4 && (mobTemplateIdList.Count > 0 && mobTemplateIdList.Contains(mob.templateId) || mobIdList.Count > 0 && mobIdList.Contains(mob.mobId) || mobTemplateIdList.Count == 0 || mobIdList.Count == 0))
                {
                    int distance = Res.distance(mob.x, mob.y, Char.myCharz().cx, Char.myCharz().cy);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        result = mob;
                    }
                }
            }
            return result;
        }

        private static void AutoPick()
        {
            if (mSystem.currentTimeMillis() - lastTimePick > 550)
            {
                bool hasPickableItem = false;
                if (GameScr.vItemMap.size() == 0) isPicking = false;
                for (int i = GameScr.vItemMap.size() - 1; i >= 0; i--)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
                    if (itemMap.template.id >= 828 && itemMap.template.id <= 842 || itemMap.template.id == 859 || itemMap.template.id == 362 || itemMap.template.id >= 353 && itemMap.template.id <= 360)
                    {
                        GameScr.vItemMap.removeElementAt(i);
                        continue;
                    }
                    if (itemMap.template.id == 74)
                    {
                        Service.gI().pickItem(itemMap.itemMapID);
                        lastTimePick = mSystem.currentTimeMillis();
                        return;
                    }
                    int distance = Utilities.getDistance(itemMap, Char.myCharz());
                    if (itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1)
                    {
                        hasPickableItem = true;
                        if (!isAssignedLastX)
                        {
                            isAssignedLastX = true;
                            lastX = Char.myCharz().cx;
                        }
                        if (distance > 60)
                        {
                            Utilities.teleportMyChar(itemMap.x);
                        }
                        else
                        {
                            Char.myCharz().currentMovePoint = new MovePoint(itemMap.x, itemMap.y);
                        }
                        Service.gI().pickItem(itemMap.itemMapID);
                        isPicking = true;
                        lastTimePick = mSystem.currentTimeMillis();
                        break;
                    }
                }
                if (isAssignedLastX && !hasPickableItem)
                {
                    if (lastX <= 50 && lastXPet > 50) lastX = lastXPet;
                    isPicking = false;
                    if (Res.distance(Char.myCharz().cx, Char.myCharz().cy, lastX, Utilities.getYGround(lastX)) > 60)
                    {
                        Utilities.teleportMyChar(lastX);
                    }
                    else
                    {
                        Char.myCharz().currentMovePoint = new MovePoint(lastX, Utilities.getYGround(lastX));
                    }
                    isAssignedLastX = false;
                }
            }

        }

        public static void setState(int value) => mode = (AutoPetMode)value;

        public static void setAttackState(int value) => modeAttackWhenNeeded = (AttackMode)value;

        public enum AutoPetMode
        {
            Disabled,
            Normal,
            AvoidSuperMob,
            Kaioken
        }

        public enum AttackMode
        {
            AttackClosestMob,
            AttackMyPet,
            AttackMyself
        }
    }
}
