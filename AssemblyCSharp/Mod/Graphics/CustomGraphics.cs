using Mod.ModMenu;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Mod.Graphics
{
    public static class CustomGraphics
    {
        private static Texture2D aaLineTex = null;
        private static Texture2D lineTex = null;
        private static Rect lineRect = new Rect(0, 0, 1, 1);
        static int[] array2 = new int[] { 0, 0, 0, 0, 600841, 3346944, 3932211, 6684682 };
        static int[] size = new int[6] { 2, 1, 1, 1, 1, 1 };
        static int[,] colorBorder = new int[5,6]
        {
            { 18687, 16869, 15052, 13235, 11161, 9344 },
            { 45824, 39168, 32768, 26112, 19712, 13056 },
            { 16744192, 15037184, 13395456, 11753728, 10046464, 8404992 },
            { 13500671, 12058853, 10682572, 9371827, 7995545, 6684800 },
            { 16711705, 15007767, 13369364, 11730962, 10027023, 8388621 }
        };
        static Texture2D mapTexture = new Texture2D(GameCanvas.w * mGraphics.zoomLevel, GameCanvas.h * mGraphics.zoomLevel);

        static Color colorMap = new Color(0.93f, 0.27f, 0f);

        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
        {
            float dx = pointB.x - pointA.x;
            float dy = pointB.y - pointA.y;
            float len = Mathf.Sqrt(dx * dx + dy * dy);
            if (len < 0.001f) return;
            Texture2D tex;
            if (antiAlias)
            {
                width *= 3.0f;
                tex = aaLineTex;
            }
            else tex = lineTex;
            float wdx = width * dy / len;
            float wdy = width * dx / len;
            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.m00 = dx;
            matrix.m01 = -wdx;
            matrix.m03 = pointA.x + 0.5f * wdx;
            matrix.m10 = dy;
            matrix.m11 = wdy;
            matrix.m13 = pointA.y - 0.5f * wdy;
            GL.PushMatrix();
            GL.MultMatrix(matrix);
            GUI.color = color;
            GUI.DrawTexture(lineRect, tex);
            GL.PopMatrix();
        }

        public static void DrawCircle(Vector2 center, int radius, Color color, float thichness, bool antiAlias, int segmentsPerQuarter)
        {
            float rh = (float)radius * 0.551915024494f;

            Vector2 p1 = new Vector2(center.x, center.y - radius);
            Vector2 p1_tan_a = new Vector2(center.x - rh, center.y - radius);
            Vector2 p1_tan_b = new Vector2(center.x + rh, center.y - radius);

            Vector2 p2 = new Vector2(center.x + radius, center.y);
            Vector2 p2_tan_a = new Vector2(center.x + radius, center.y - rh);
            Vector2 p2_tan_b = new Vector2(center.x + radius, center.y + rh);

            Vector2 p3 = new Vector2(center.x, center.y + radius);
            Vector2 p3_tan_a = new Vector2(center.x - rh, center.y + radius);
            Vector2 p3_tan_b = new Vector2(center.x + rh, center.y + radius);

            Vector2 p4 = new Vector2(center.x - radius, center.y);
            Vector2 p4_tan_a = new Vector2(center.x - radius, center.y - rh);
            Vector2 p4_tan_b = new Vector2(center.x - radius, center.y + rh);

            DrawBezierLine(p1, p1_tan_b, p2, p2_tan_a, color, thichness, antiAlias, segmentsPerQuarter);
            DrawBezierLine(p2, p2_tan_b, p3, p3_tan_b, color, thichness, antiAlias, segmentsPerQuarter);
            DrawBezierLine(p3, p3_tan_a, p4, p4_tan_b, color, thichness, antiAlias, segmentsPerQuarter);
            DrawBezierLine(p4, p4_tan_a, p1, p1_tan_a, color, thichness, antiAlias, segmentsPerQuarter);
        }

        public static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
        {
            Vector2 lastV = CubeBezier(start, startTangent, end, endTangent, 0);
            for (int i = 1; i < segments + 1; ++i)
            {
                Vector2 v = CubeBezier(start, startTangent, end, endTangent, i / (float)segments);
                DrawLine(lastV, v, color, width, antiAlias);
                lastV = v;
            }
        }


        private static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
        {
            float rt = 1 - t;
            return rt * rt * rt * s + 3 * rt * rt * t * st + 3 * rt * t * t * et + t * t * t * e;
        }

        static CustomGraphics()
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (lineTex == null)
            {
                lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                lineTex.SetPixel(0, 1, Color.white);
                lineTex.Apply();
            }
            if (aaLineTex == null)
            {
                aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, false);
                aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
                aaLineTex.SetPixel(0, 1, Color.white);
                aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
                aaLineTex.Apply();
            }
        }

        public static void DrawCircle(mGraphics mgraphic, int x, int y, int radius, int thickness)
        {
            #region Reflection
            Type mgraphics = typeof(mGraphics);
            BindingFlags nonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
            bool isTranslate = (bool)mgraphics.GetField("isTranslate", nonPublicInstance).GetValue(mgraphic);
            bool isClip = (bool)mgraphics.GetField("isClip", nonPublicInstance).GetValue(mgraphic);
            int translateX = (int)mgraphics.GetField("translateX", nonPublicInstance).GetValue(mgraphic);
            int translateY = (int)mgraphics.GetField("translateY", nonPublicInstance).GetValue(mgraphic);
            int clipTX = (int)mgraphics.GetField("clipTX", nonPublicInstance).GetValue(mgraphic);
            int clipTY = (int)mgraphics.GetField("clipTY", nonPublicInstance).GetValue(mgraphic);
            float r = (float)mgraphics.GetField("r", nonPublicInstance).GetValue(mgraphic);
            float g = (float)mgraphics.GetField("g", nonPublicInstance).GetValue(mgraphic);
            float b = (float)mgraphics.GetField("b", nonPublicInstance).GetValue(mgraphic);
            float a = (float)mgraphics.GetField("a", nonPublicInstance).GetValue(mgraphic);
            #endregion
            Color color = new Color(r, g, b, a);
            x -= GameScr.cmx;
            y -= GameScr.cmy;
            x *= mGraphics.zoomLevel;
            y *= mGraphics.zoomLevel;
            radius *= mGraphics.zoomLevel;
            thickness *= mGraphics.zoomLevel;
            if (isTranslate)
            {
                x += translateX;
                y += translateY;
            }
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            if (isClip)
            {
                num3 = mgraphic.clipX;
                num4 = mgraphic.clipY;
                num5 = mgraphic.clipW;
                num6 = mgraphic.clipH;
                if (isTranslate)
                {
                    num3 += clipTX;
                    num4 += clipTY;
                }
            }
            if (isClip)
            {
                GUI.BeginGroup(new Rect(num3, num4, num5, num6));
            }
            Color oldColor = GUI.color;
            DrawCircle(new Vector2(x - num3, y - num4), radius, color, thickness, true, 10);
            GUI.color = oldColor;
            if (isClip)
            {
                GUI.EndGroup();
            }
        }

        public static void DrawCircle(mGraphics g, IMapObject mapObject, int radius, int thickness) => DrawCircle(g, mapObject.getX(), mapObject.getY(), radius, thickness);

        public static void drawRect(mGraphics g, int x, int y, int w, int h, int thickness)
        {
            g.fillRect(x, y, w, thickness);
            g.fillRect(x, y, thickness, h);
            g.fillRect(x + w, y, thickness, h + thickness);
            g.fillRect(x, y + h, w + thickness, thickness);
        }

        public static void drawLine(mGraphics gr, int x1, int y1, int x2, int y2, int thickness)
        {
            #region Reflection
            Type mgraphics = typeof(mGraphics);
            BindingFlags nonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
            bool isTranslate = (bool)mgraphics.GetField("isTranslate", nonPublicInstance).GetValue(gr);
            bool isClip = (bool)mgraphics.GetField("isClip", nonPublicInstance).GetValue(gr);
            int translateX = (int)mgraphics.GetField("translateX", nonPublicInstance).GetValue(gr);
            int translateY = (int)mgraphics.GetField("translateY", nonPublicInstance).GetValue(gr);
            int clipTX = (int)mgraphics.GetField("clipTX", nonPublicInstance).GetValue(gr);
            int clipTY = (int)mgraphics.GetField("clipTY", nonPublicInstance).GetValue(gr);
            float r = (float)mgraphics.GetField("r", nonPublicInstance).GetValue(gr);
            float g = (float)mgraphics.GetField("g", nonPublicInstance).GetValue(gr);
            float b = (float)mgraphics.GetField("b", nonPublicInstance).GetValue(gr);
            float a = (float)mgraphics.GetField("a", nonPublicInstance).GetValue(gr);
            #endregion
            x1 *= mGraphics.zoomLevel;
            y1 *= mGraphics.zoomLevel;
            x2 *= mGraphics.zoomLevel;
            y2 *= mGraphics.zoomLevel;
            if (x1 == x2 && y1 == y2) return;
            if (isTranslate)
            {
                x1 += translateX;
                y1 += translateY;
                x2 += translateX;
                y2 += translateY;
            }
            string key = "dl" + r + g + b;
            Texture2D texture2D = (Texture2D)mGraphics.cachedTextures[key];
            if (texture2D == null)
            {
                texture2D = new Texture2D(thickness, thickness);
                Color color = new Color(r, g, b, a);
                for (int i = 0; i < thickness; i++)
                {
                    for (int j = 0; j < thickness; j++)
                    {
                        texture2D.SetPixel(i, j, color);
                    }
                }
                texture2D.Apply();
                typeof(mGraphics).GetMethod("cache", nonPublicInstance).Invoke(gr, new object[] { key, texture2D });
            }
            Vector2 vector = new Vector2(x1, y1);
            Vector2 vector2 = new Vector2(x2, y2);
            Vector2 vector3 = vector2 - vector;
            float num3 = 57.29578f * Mathf.Atan(vector3.y / vector3.x);
            if (vector3.x < 0f)
            {
                num3 += 180f;
            }
            int num4 = (int)Mathf.Ceil(0f);
            GUIUtility.RotateAroundPivot(num3, vector);
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            if (isClip)
            {
                num5 = gr.clipX;
                num6 = gr.clipY;
                num7 = gr.clipW;
                num8 = gr.clipH;
                if (isTranslate)
                {
                    num5 += clipTX;
                    num6 += clipTY;
                }
            }
            if (isClip)
            {
                GUI.BeginGroup(new Rect(num5, num6, num7, num8));
            }
            UnityEngine.Graphics.DrawTexture(new Rect(vector.x - num5, vector.y - num4 - num6, vector3.magnitude, thickness), texture2D);
            if (isClip)
            {
                GUI.EndGroup();
            }
            GUIUtility.RotateAroundPivot(0f - num3, vector);
        }

        public static void PaintStar(mGraphics g, Panel instance, Item item, int num)
        {
            if (Utilities.getStar(item, out uint star, out uint starE))
            {
                if (instance == GameCanvas.panel)
                {
                    if (star > 0)
                    {
                        mFont.tahoma_7b_red.drawString(g, star.ToString(), Panel.WIDTH_PANEL - instance.xScroll - Image.getImageWidth(Panel.imgStar) - mFont.tahoma_7b_red.getWidth(star.ToString()) - 1, num, mFont.LEFT);
                        g.drawImage(Panel.imgStar, Panel.WIDTH_PANEL - instance.xScroll - Image.getImageWidth(Panel.imgStar) - 1, num + 1);
                    }
                    if (starE > 0)
                    {
                        if (star == 0)
                        {
                            mFont.tahoma_7b_red.drawString(g, starE.ToString(), Panel.WIDTH_PANEL - instance.xScroll - Image.getImageWidth(Panel.imgMaxStar) - mFont.tahoma_7b_red.getWidth(starE.ToString()) - 1, num, mFont.LEFT);
                            g.drawImage(Panel.imgMaxStar, Panel.WIDTH_PANEL - instance.xScroll - Image.getImageWidth(Panel.imgMaxStar) - 1, num + 1);
                        }
                        else
                        {
                            mFont.tahoma_7b_red.drawString(g, starE.ToString(), Panel.WIDTH_PANEL - instance.xScroll - mFont.tahoma_7b_red.getWidth(starE.ToString() + star.ToString()) - Image.getImageWidth(Panel.imgMaxStar) * 2 - 2, num, mFont.LEFT);
                            g.drawImage(Panel.imgMaxStar, Panel.WIDTH_PANEL - instance.xScroll - mFont.tahoma_7b_red.getWidth(starE.ToString()) - Image.getImageWidth(Panel.imgMaxStar) * 2 - 3, num + 1);
                        }
                    }
                }
                else if (instance == GameCanvas.panel2)
                {
                    if (star > 0)
                    {
                        mFont.tahoma_7b_red.drawString(g, star.ToString(), GameCanvas.w - Image.getImageWidth(Panel.imgStar) - mFont.tahoma_7b_red.getWidth(star.ToString()) - 4, num, mFont.LEFT);
                        g.drawImage(Panel.imgStar, GameCanvas.w - Image.getImageWidth(Panel.imgStar) - 4, num + 1);
                    }
                    if (starE > 0)
                    {
                        if (star == 0)
                        {
                            mFont.tahoma_7b_red.drawString(g, starE.ToString(), GameCanvas.w - Image.getImageWidth(Panel.imgMaxStar) - mFont.tahoma_7b_red.getWidth(starE.ToString()) - 4, num, mFont.LEFT);
                            g.drawImage(Panel.imgMaxStar, GameCanvas.w - Image.getImageWidth(Panel.imgMaxStar) - 4, num + 1);
                        }
                        else
                        {
                            mFont.tahoma_7b_red.drawString(g, starE.ToString(), GameCanvas.w - mFont.tahoma_7b_red.getWidth(starE.ToString() + star.ToString()) - Image.getImageWidth(Panel.imgMaxStar) * 2 - 5, num, mFont.LEFT);
                            g.drawImage(Panel.imgMaxStar, GameCanvas.w - mFont.tahoma_7b_red.getWidth(starE.ToString()) - Image.getImageWidth(Panel.imgMaxStar) * 2 - 6, num + 1);
                        }
                    }
                }
            }
        }

        static int upgradeEffectX(int tick, int w)
        {
            int n = tick % (4 * w);
            if (0 <= n && n < w)
                return n % w;
            else if (w <= n && n < 2 * w)
                return w;
            else if (2 * w <= n && n < 3 * w)
                return w - n % w;
            else 
                return 0;
        }

        static int upgradeEffectY(int tick, int h)
        {
            int n = tick % (4 * h);
            if (0 <= n && n < h)
                return 0;
            else if (h <= n && n < 2 * h)
                return n % h;
            else if (2 * h <= n && n < 3 * h)
                return h;
            else 
                return h - n % h;
        }

        public static void PaintItemEffectInPanel(mGraphics g, int x, int y, int w, int h, Item item)
        {
            if (item.itemOption == null)
                return;
            ItemOption itemOption = null;
            for (int i = 0; i < item.itemOption.Length; i++)
            {
                ItemOption iOption = item.itemOption[i];
                if (iOption.optionTemplate == null)
                    continue;
                if (iOption.optionTemplate.id >= 127 && iOption.optionTemplate.id <= 135)   //Set kích hoạt
                {
                    itemOption = iOption;
                    break;
                }
                else if (iOption.optionTemplate.id <= 36 && iOption.optionTemplate.id >= 34)    //tinh ấn/nguyệt ấn/nhật ấn
                    itemOption = iOption;
                else if (itemOption == null || itemOption.optionTemplate == null || itemOption.optionTemplate.id > 36 || itemOption.optionTemplate.id < 34)
                {
                    if (iOption.optionTemplate.id == 72)   //cấp #
                        itemOption = iOption;
                    else if ((itemOption == null || itemOption.optionTemplate == null || (itemOption != null && itemOption.optionTemplate != null && itemOption.optionTemplate.id == 72 && itemOption.param < (int)System.Math.Ceiling((double)iOption.param / 2))) && iOption.optionTemplate.id == 107) //đồ sao
                        itemOption = iOption;
                }
            }
            if (itemOption == null)
                return;
            int id = itemOption.optionTemplate.id;
            if ((id > 36 || id < 34) && id != 72 && (id < 127 || id > 135) && id != 107)
                return;
            int param = itemOption.param;
            if (param > 7)
                param = 7;
            if (id >= 127 && id <= 135)
                param = 7;
            if (id == 107) 
            {
                if (param > 1)
                    param = (int)System.Math.Ceiling((double)param / 2);
                else if (param == 1)
                    return;
            }
            if (param <= 0)
                return;
            if (param >= 4 && param <= 7 && (id > 36 || id < 34))
            {
                g.setColor(array2[param]);
                g.fillRect(x - w / 2, y - h / 2, w, h);
            }
            for (int j = 0; j < size.Length; j++)
            {
                int fx = x - w / 2 + 1 + upgradeEffectX(GameCanvas.gameTick - j * 4, w - 2);
                int fy = y - h / 2 + 1 + upgradeEffectY(GameCanvas.gameTick - j * 4, h - 2);
                g.setColor(colorBorder[0, j]);
                g.fillRect(fx - size[j] / 2, fy - size[j] / 2, size[j], size[j]);
                if (param <= 1)
                    continue;
                if (param > 2)
                    g.setColor(colorBorder[1, j]);
                fx = x - w / 2 + 1 + upgradeEffectX(GameCanvas.gameTick + 68 - j * 4, w - 2);
                fy = y - h / 2 + 1 + upgradeEffectY(GameCanvas.gameTick + 48 - j * 4, h - 2);
                g.fillRect(fx - size[j] / 2, fy - size[j] / 2, size[j], size[j]);
                if (param <= 3)
                    continue;
                if (param > 4)
                    g.setColor(colorBorder[2, j]);
                fx = x - w / 2 + 1 + upgradeEffectX(GameCanvas.gameTick + 68 - j * 4, w - 2);
                fy = y - h / 2 + 1 + upgradeEffectY(GameCanvas.gameTick - j * 4, h - 2);
                g.fillRect(fx - size[j] / 2, fy - size[j] / 2, size[j], size[j]);
                if (param <= 5)
                    continue;
                if (param > 6)
                    g.setColor(colorBorder[3, j]);
                fx = x - w / 2 + 1 + upgradeEffectX(GameCanvas.gameTick - j * 4, w - 2);
                fy = y - h / 2 + 1 + upgradeEffectY(GameCanvas.gameTick + 48 - j * 4, h - 2);
                g.fillRect(fx - size[j] / 2, fy - size[j] / 2, size[j], size[j]);
            }
        }

        public static void PaintTileMap(mGraphics g)
        {
            //if (mapTexture.width != GameCanvas.w * mGraphics.zoomLevel || mapTexture.height != GameCanvas.h * mGraphics.zoomLevel)
            //    mapTexture = new Texture2D(GameCanvas.w * mGraphics.zoomLevel, GameCanvas.h * mGraphics.zoomLevel);
            //for (int x = 0; x < mapTexture.width; x++)
            //    for (int y = 0; y < mapTexture.height; y++)
            //        mapTexture.SetPixel(x, y, colorMap);
            ////g.setColor(15615232);
            //for (int i = GameScr.gssx; i < GameScr.gssxe; i++)
            //{
            //    for (int j = GameScr.gssy; j < GameScr.gssye; j++)
            //    {
            //        if (TileMap.maps[j * TileMap.tmw + i] != 0)
            //        {
            //            if ((!TileMap.tileTypeAt(i * 24, (j + 1) * 24, 2) && !TileMap.tileTypeAt(i * 24, (j + 2) * 24, 2) && !TileMap.tileTypeAt(i * 24, j * 24, 2)) || TileMap.tileTypeAt(i * 24, j * 24, 2))
            //            {
            //                for (int x = 0; x < 24 * mGraphics.zoomLevel; x++)
            //                {
            //                    for (int y = 0; y < 24 * mGraphics.zoomLevel; y++)
            //                    {
            //                        if (ModMenuMain.getStatusInt("levelreducegraphics") == 2 && i > 0)
            //                        {
            //                            if ((x + 1) / mGraphics.zoomLevel == 0 || (x + 1) / mGraphics.zoomLevel == 24 || (y + 1) / mGraphics.zoomLevel == 0 || (y + 1) / mGraphics.zoomLevel == 24)
            //                                mapTexture.SetPixel(x, y, colorMap);
            //                        }
            //                        else
            //                            mapTexture.SetPixel(x, y, colorMap);

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //mapTexture.Apply();
            //UnityEngine.Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mapTexture);
        }
    }
}