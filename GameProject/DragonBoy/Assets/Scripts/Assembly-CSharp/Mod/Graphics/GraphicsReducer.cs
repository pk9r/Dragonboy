using System;
using Mod.Constants;
using UnityEngine;

namespace Mod.Graphics
{
    internal class GraphicsReducer
    {
        static bool lastIsFill;
        static Image mapTile = new Image();
        static Color colorMap = new Color(0.93f, 0.27f, 0f);

        static ReduceGraphicsLevel _level;
        internal static ReduceGraphicsLevel Level
        {
            get => _level;
            set => _level = value;
        }

        #region Events
        internal static bool OnServerEffectPaint()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnNpcPaint(Npc _this, mGraphics g)
        {
            if (_level < ReduceGraphicsLevel.Level2)
                return false;
            if (Char.isLoadingMap || _this.isHide || !_this.isPaint() || _this.statusMe == 15)
                return true;
            if (_this.cTypePk != 0)
                return false;
            if (_this.template == null)
                return true;
            if (_this.template.npcTemplateId != 4 && _this.template.npcTemplateId != 51 && _this.template.npcTemplateId != 50)
                g.drawImage(TileMap.bong, _this.cx, _this.cy, 3);
            if (_level == ReduceGraphicsLevel.Level2)
            {
                g.setColor(Color.green);
                g.drawRect(_this.cx - 12, _this.cy - _this.ch, 24, _this.ch);
                if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus == _this && ChatPopup.currChatPopup == null) 
                    g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, _this.cx, _this.cy - _this.ch - 7, mGraphics.BOTTOM | mGraphics.HCENTER);
            }
            else if (_level > ReduceGraphicsLevel.Level2)
                if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus == _this && ChatPopup.currChatPopup == null)
                    g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, _this.cx, _this.cy - _this.ch - 7, mGraphics.BOTTOM | mGraphics.HCENTER);
            if (_this.indexEffTask < 0 || _this.effTask == null || _this.cTypePk != 0)
                return true;
            SmallImage.drawSmallImage(g, _this.effTask.arrEfInfo[_this.indexEffTask].idImg, _this.cx + _this.effTask.arrEfInfo[_this.indexEffTask].dx, _this.cy + _this.effTask.arrEfInfo[_this.indexEffTask].dy - _this.dyEff, 0, mGraphics.VCENTER | mGraphics.HCENTER);
            if (GameCanvas.gameTick % 2 == 0)
            {
                _this.indexEffTask++;
                if (_this.indexEffTask >= _this.effTask.arrEfInfo.Length)
                    _this.indexEffTask = 0;
            }
            return true;
        }

        internal static bool OnTileMapPaintOutTile()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnTileMapPaintTile(mGraphics g)
        {
            if (_level > ReduceGraphicsLevel.Level2)
                return true;
            if (_level >= ReduceGraphicsLevel.Level1)
            {
                PaintTileMap(g);
                return true;
            }
            return false;
        }

        internal static bool OnMobPaint(Mob _this, mGraphics g)
        {
            if (_level <= ReduceGraphicsLevel.Level1)
                return false;
            if (_this.isHide)
                return true;
            if (_this.isMafuba)
                return true;
            if (_this.isShadown && _this.status != 0)
                _this.paintShadow(g);
            if (!_this.isPaint() || (_this.status == 1 && _this.p3 > 0 && GameCanvas.gameTick % 3 == 0))
                return true;
            if (_level >= ReduceGraphicsLevel.Level3)
                return true;
            g.translate(0, GameCanvas.transY);
            g.setColor(Color.yellow);
            if (_this.levelBoss != 0)
                g.setColor(Color.red);
            g.drawRect(Mathf.RoundToInt(_this.x - _this.w / 2), _this.y - _this.h - 15, _this.w, _this.h);
            g.translate(0, -GameCanvas.transY);
            if (Char.myCharz().mobFocus == null || Char.myCharz().mobFocus != _this || _this.status == 1 || _this.hp <= 0 || _this.imgHPtem == null)
                return true;
            int imageWidth = mGraphics.getImageWidth(_this.imgHPtem);
            int imageHeight = mGraphics.getImageHeight(_this.imgHPtem);
            int num = imageWidth * _this.per / 100;
            int num2 = num;
            if (_this.per_tem >= _this.per)
            {
                num2 = imageWidth * (_this.per_tem -= ((GameCanvas.gameTick % 6 <= 3) ? _this.offset : _this.offset++)) / 100;
                if (_this.per_tem <= 0)
                    _this.per_tem = 0;
                if (_this.per_tem < _this.per)
                    _this.per_tem = _this.per;
                if (_this.offset >= 3)
                    _this.offset = 3;
            }
            g.drawImage(GameScr.imgHP_tm_xam, _this.x - (imageWidth >> 1), _this.y - _this.h - 5, mGraphics.TOP | mGraphics.LEFT);
            g.setColor(0xFFFFFF);
            g.fillRect(_this.x - (imageWidth >> 1), _this.y - _this.h - 5, num2, 2);
            g.drawRegion(_this.imgHPtem, 0, 0, num, imageHeight, 0, _this.x - (imageWidth >> 1), _this.y - _this.h - 5, mGraphics.TOP | mGraphics.LEFT);
            return true;
        }

        internal static bool OnMagicTreePaint(MagicTree _this, mGraphics g)
        {
            if (_level < ReduceGraphicsLevel.Level2)
                return false;
            if (_this.id == 0)
                return true;
            if (_level == ReduceGraphicsLevel.Level2)
            {
                g.setColor(Color.green);
                g.drawRect(_this.cx - 12, _this.cy - SmallImage.smallImg[_this.id][4], 24, SmallImage.smallImg[_this.id][4]);
            }
            if (Char.myCharz().npcFocus != null && Char.myCharz().npcFocus == _this)
            {
                g.drawRegion(Mob.imgHP, 0, 0, 9, 6, 0, _this.cx, _this.cy - SmallImage.smallImg[_this.id][4] - 1, mGraphics.BOTTOM | mGraphics.HCENTER);
                if (_this.name != null)
                    mFont.tahoma_7b_white.drawString(g, _this.name, _this.cx, _this.cy - SmallImage.smallImg[_this.id][4] - 20, mFont.CENTER, mFont.tahoma_7_grey);
            }
            else if (_this.name != null)
                mFont.tahoma_7b_white.drawString(g, _this.name, _this.cx, _this.cy - SmallImage.smallImg[_this.id][4] - 17, mFont.CENTER, mFont.tahoma_7_grey);
            try
            {
                for (int i = 0; i < _this.currPeas; i++)
                {
                    g.setColor(Color.cyan);
                    g.drawRect(_this.cx + _this.peaPostionX[i] - SmallImage.smallImg[_this.id][3] / 2, _this.cy + _this.peaPostionY[i] - SmallImage.smallImg[_this.id][4], Image.getImageWidth(MagicTree.pea), Image.getImageHeight(MagicTree.pea));
                }
            }
            catch { }
            if (_this.indexEffTask < 0 || _this.effTask == null || _this.cTypePk != 0)
                return true;
            SmallImage.drawSmallImage(g, _this.effTask.arrEfInfo[_this.indexEffTask].idImg, _this.cx + _this.effTask.arrEfInfo[_this.indexEffTask].dx, _this.cy - 15 + _this.effTask.arrEfInfo[_this.indexEffTask].dy, 0, mGraphics.VCENTER | mGraphics.HCENTER);
            if (GameCanvas.gameTick % 2 == 0)
            {
                _this.indexEffTask++;
                if (_this.indexEffTask >= _this.effTask.arrEfInfo.Length)
                    _this.indexEffTask = 0;
            }
            return true;
        }

        internal static bool OnItemMapPaint(ItemMap _this, mGraphics g)
        {
            if (_this.template.type != ItemTemplateType.Satellite) //!isAuraItem()
                return false;
            if (_level > ReduceGraphicsLevel.Level2)
                return true;
            if (_level > ReduceGraphicsLevel.Level1)
            {
                g.drawImage(TileMap.bong, _this.x + 3, _this.y, mGraphics.VCENTER | mGraphics.HCENTER);
                g.setColor(Color.gray);
                g.drawRect(_this.x - 12, _this.y - Image.getImageHeight(ItemMap.imageAuraItem1), 24, Image.getImageHeight(ItemMap.imageAuraItem1));
                return true;
            }
            return false;
        }

        internal static bool OnInfoMePaint(InfoMe _this, mGraphics g)
        {
            if (_level <= ReduceGraphicsLevel.Level1)
                return false;
            if (_this.info.info == null || _this.info.info.charInfo != null || _this.charId == null)
                return false;
            if ((_this == GameScr.info2 && GameScr.gI().isVS()) || (_this == GameScr.info2 && GameScr.gI().popUpYesNo != null) || !GameScr.isPaint || (GameCanvas.currentScreen != GameScr.gI() && GameCanvas.currentScreen != CrackBallScr.gI()) || ChatPopup.serverChatPopUp != null || !_this.isUpdate || Char.ischangingMap || (GameCanvas.panel.isShow && _this == GameScr.info2))
                return true;
            g.translate(-g.getTranslateX(), -g.getTranslateY());
            g.setClip(0, 0, GameCanvas.w, GameCanvas.h);
            _this.info?.paint(g, _this.cmx, _this.cmy, _this.dir);
            g.setColor(0xDB70AA);
            if (Char.myCharz().cgender == 0)
                g.setColor(new Color(0.2f, 0.66f, 0.92f));
            if (Char.myCharz().cgender == 1)
                g.setColor(0x459625);
            g.drawRect(_this.cmtoX - 10, _this.cmtoY - 4 + ((GameCanvas.gameTick % 10 <= 5) ? 0 : 1), 15, 15);
            g.translate(-g.getTranslateX(), -g.getTranslateY());
            return true;
        }

        internal static bool OnmGraphicsDrawImage(Image image)
        {
            if (_level > ReduceGraphicsLevel.Level1 && image == TileMap.imgLight)
                return true;
            return false;
        }

        internal static bool OnGameScrPaintBgItem()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnGameScrPaintEffect()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnEffectPaint()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnCharPaintCharBody(Char _this, mGraphics g, int cx, int cy, int cdir, bool isPaintBag)
        {
            if (_level > ReduceGraphicsLevel.Level2)
                return true;
            if (_level <= ReduceGraphicsLevel.Level1)
                return false;
            if (_this.bag >= 0 && _this.statusMe != 14 && _this.isMonkey == 0)
            {
                if (!ClanImage.idImages.containsKey(_this.bag.ToString() + string.Empty))
                {
                    ClanImage.idImages.put(_this.bag.ToString() + string.Empty, new ClanImage());
                    Service.gI().requestBagImage((sbyte)_this.bag);
                }
                else
                {
                    ClanImage clanImage = (ClanImage)ClanImage.idImages.get(_this.bag.ToString() + string.Empty);
                    if (clanImage.idImage != null && isPaintBag)
                        _this.paintBag(g, clanImage.idImage, cx, cy, cdir, true);
                }
            }
            g.setColor(Color.white);
            if (_this.me)
                g.setColor(Color.blue);
            if (_this.IsPet())
                g.setColor(Color.cyan);
            else if (_this.cTypePk == 5)
            {
                g.setColor(Color.red);
                if ((_this.isCharge || _this.isFlyAndCharge || _this.isStandAndCharge) && GameCanvas.gameTick % 8 >= 4)
                    g.setColor(Color.white);
            }
            int height = 35;
            int width = 12;
            if (_this.IsPet())
                height = 30;
            if (_this.cTypePk == 5)
            {
                width = 15;
                height = 40;
            }
            g.drawRect(cx - width, cy - height, width * 2, height);
            if (_this.statusMe == 14)
            {
                if (GameCanvas.gameTick % 4 > 0)
                    g.drawImage(ItemMap.imageFlare, cx, cy - _this.ch - 11, mGraphics.HCENTER | mGraphics.VCENTER);
                SmallImage.drawSmallImage(g, 79, cx, cy - _this.ch - 8, 0, mGraphics.HCENTER | mGraphics.BOTTOM);
            }
            if (_this.protectEff)
            {
                g.setColor(Color.green);
                g.drawRect(cx - 35, cy - 55, 70, 70);
            }
            if (_this.cFlag != 0 && _this.cFlag != -1)
                SmallImage.drawSmallImage(g, _this.flagImage, cx - (cdir == 1 ? 10 : 0), cy - _this.ch - 30 + ((GameCanvas.gameTick % 20 > 10) ? (GameCanvas.gameTick % 4 / 2) : 0), 0, 0);
            return true;
        }

        internal static bool OnCharPaintMapLine()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintEffect()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintEff_Pet()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintEff_LvUp_Front()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintEff_LvUp_Behind()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintEffFront()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintEffBehind()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintAuraFront()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintAuraBehind()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintSuperEffFront()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintSuperEffBehind()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintMount2()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnCharPaintMount1(Char _this, mGraphics g)
        {
            if (_level <= ReduceGraphicsLevel.Level1)
                return false;
            if (_this.xMount <= GameScr.cmx || _this.xMount >= GameScr.cmx + GameCanvas.w)
                return true;
            g.setColor(0x00FF8D);
            g.drawRect(_this.xMount - 20, _this.yMount, 40, 15);
            return true;
        }

        internal static bool OnCharPaint()
        {
            if (_level > ReduceGraphicsLevel.Level2)
                return true;
            return false;
        }

        internal static bool OnCharUpdateSuperEff()
        {
            if (_level > ReduceGraphicsLevel.Level1)
                return true;
            return false;
        }

        internal static bool OnBgItemPaint()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnBackgroundEffectAddEffect()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnBackgroundEffectPaintFog()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnBackgroundEffectPaintCloud2()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnBackgroundEffectUpdateFog()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnBackgroundEffectUpdateCloud2()
        {
            if (_level > ReduceGraphicsLevel.Off)
                return true;
            return false;
        }

        internal static bool OnBackgroundEffectInitCloud()
        {
            if (_level > ReduceGraphicsLevel.Off)
            {
                BackgroudEffect.imgCloud1 = null;
                BackgroudEffect.imgFog = null;
                return true;
            }
            return false;
        }
        #endregion

        internal static void InitializeTileMap(bool isFill)
        {
            if (isFill == lastIsFill && mapTile != null)
                return;
            lastIsFill = isFill;
            mapTile.w = mapTile.h = 25 * mGraphics.zoomLevel;
            mapTile.texture = new Texture2D(25 * mGraphics.zoomLevel, 25 * mGraphics.zoomLevel);
            for (int i = 0; i < mapTile.texture.width; i++)
                for (int j = 0; j < mapTile.texture.height; j++)
                    mapTile.texture.SetPixel(i, j, isFill ? colorMap : Color.clear);
            if (!isFill)
            {
                for (int i = 0; i < mapTile.texture.width; i++)
                {
                    for (int j = 0; j < mGraphics.zoomLevel; j++)
                    {
                        mapTile.texture.SetPixel(i, j, colorMap);
                        mapTile.texture.SetPixel(j, i, colorMap);
                        mapTile.texture.SetPixel(mapTile.texture.width - j, i, colorMap);
                        mapTile.texture.SetPixel(i, mapTile.texture.height - j, colorMap);
                    }
                }
            }
            mapTile.texture.Apply();
        }

        internal static void PaintTileMap(mGraphics g)
        {
            InitializeTileMap(_level == ReduceGraphicsLevel.Level1);
            //vertical
            for (int x = 2; x < TileMap.tmw - 2; x++)
            {
                for (int y = 0; y < TileMap.tmh - 1; y++)
                {
                    if (TileMap.maps[y * TileMap.tmw + x] == 0)
                        continue;
                    if (!TileMap.tileTypeAt(x * TileMap.size, y * TileMap.size, 2) && (_level >= ReduceGraphicsLevel.Level2 || !IsTileMapICantEnter(x * TileMap.size, y * TileMap.size)))
                        continue;
                    int maxy2 = 0;
                    for (int y2 = y + 1; y2 < TileMap.tmh - 1; y2++)
                    {
                        if (TileMap.maps[y2 * TileMap.tmw + x] == 0)
                            break;
                        if (x + 1 < TileMap.tmw && (TileMap.tileTypeAt((x + 1) * TileMap.size, y2 * TileMap.size, 2) || (_level < ReduceGraphicsLevel.Level2 && IsTileMapICantEnter((x + 1) * TileMap.size, y2 * TileMap.size))))
                            maxy2 = Math.Max(maxy2, y2);
                        if (x > 0 && (TileMap.tileTypeAt((x - 1) * TileMap.size, y2 * TileMap.size, 2) || (_level < ReduceGraphicsLevel.Level2 && IsTileMapICantEnter((x - 1) * TileMap.size, y2 * TileMap.size))))
                            maxy2 = Math.Max(maxy2, y2);
                    }
                    for (int i = y; i < maxy2 + 1; i++)
                    {
                        if (x >= GameScr.gssx && x <= GameScr.gssxe && i >= GameScr.gssy && i <= GameScr.gssye)
                            g.drawImage(mapTile, x * TileMap.size, i * TileMap.size + 8);
                    }
                }
            }
            //horizontal
            for (int x = GameScr.gssx + 1; x < GameScr.gssxe; x++)
            {
                for (int y = GameScr.gssy; y < GameScr.gssye; y++)
                {
                    if (TileMap.maps[y * TileMap.tmw + x] == 0)
                        continue;
                    if (TileMap.tileTypeAt(x * TileMap.size, y * TileMap.size, 2) || (_level < ReduceGraphicsLevel.Level2 && IsTileMapICantEnter(x * TileMap.size, y * TileMap.size)))
                        g.drawImage(mapTile, x * TileMap.size, y * TileMap.size + 8);
                }
            }
        }

        static bool IsTileMapICantEnter(int px, int py) => TileMap.tileTypeAt(px, py, 4) || TileMap.tileTypeAt(px, py, 8) || TileMap.tileTypeAt(px, py, 8192);
    }

    internal enum ReduceGraphicsLevel
    {
        Off,
        Level1,
        Level2,
        Level3
    }
}