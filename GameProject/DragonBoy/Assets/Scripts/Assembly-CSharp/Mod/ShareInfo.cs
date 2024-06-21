using Mod.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod
{
    internal class ShareInfo : ThreadActionUpdate<ShareInfo>
    {
        internal override int Interval => 1000;

        protected override void update()
        {
            var myChar = Char.myCharz();
            var myPet = Char.myPetz();

            if (myChar.cName == "")
                return;

            SocketClient.gI.sendMessage(new
            {
                action = "updateInfo",
                Utils.status,
                myChar.cName,
                myChar.cgender,
                TileMap.mapName,
                TileMap.mapID,
                TileMap.zoneID,
                myChar.cx,
                myChar.cy,
                myChar.cHP,
                myChar.cHPFull,
                myChar.cMP,
                myChar.cMPFull,
                myChar.cStamina,
                myChar.cPower,
                myChar.cTiemNang,
                myChar.cHPGoc,
                myChar.cMPGoc,
                myChar.cDefGoc,
                myChar.cDamGoc,
                myChar.cCriticalGoc,
                myChar.cDamFull,
                myChar.cDefull,
                myChar.cCriticalFull,
                cPetHP = myPet.cHP,
                cPetHPFull = myPet.cHPFull,
                cPetMP = myPet.cMP,
                cPetMPFull = myPet.cMPFull,
                cPetStamina = myPet.cStamina,
                cPetPower = myPet.cPower,
                cPetTiemNang = myPet.cTiemNang,
                cPetDamFull = myPet.cDamFull,
                cPetDefull = myPet.cDefull,
                cPetCriticalFull = myPet.cCriticalFull,
                myChar.xu,
                myChar.luong,
                myChar.luongKhoa
            });
        }
    }
}
