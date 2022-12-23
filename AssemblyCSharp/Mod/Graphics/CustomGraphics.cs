using System;
using System.Reflection;
using UnityEngine;

namespace Mod.Graphics
{
    public static class CustomGraphics
    {
        private static Texture2D aaLineTex = null;
        private static Texture2D lineTex = null;
        private static Rect lineRect = new Rect(0, 0, 1, 1);
        static int[] array = new int[] { 0, 0, 1, 1, 2, 3, 4 };
        static int[] array2 = new int[] { 0, 0, 0, 0, 600841, 3346944, 3932211, 6684682 };
        static int[] array3 = new int[4]
        {
            0,
            68,
            68,
            0
        };
        static int[] array4 = new int[] { 0, 48, 0, 48 };
        static int[] size = new int[6] { 2, 1, 1, 1, 1, 1 };
        static int[][] colorBorder = new int[5][]
        {
            new int[6] { 18687, 16869, 15052, 13235, 11161, 9344 },
            new int[6] { 45824, 39168, 32768, 26112, 19712, 13056 },
            new int[6] { 16744192, 15037184, 13395456, 11753728, 10046464, 8404992 },
            new int[6] { 13500671, 12058853, 10682572, 9371827, 7995545, 6684800 },
            new int[6] { 16711705, 15007767, 13369364, 11730962, 10027023, 8388621 }
        };
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

        static int unknown_method_0(int int_0)
        {
            int num = 32;
            int num2 = int_0 % 128;
            if (0 <= num2 && num2 < num)
            {
                return num2 % num;
            }
            if (num <= num2 && num2 < num * 2)
            {
                return num;
            }
            if (num * 2 <= num2 && num2 < num * 3)
            {
                return num - num2 % num;
            }
            return 0;
        }

        static int unknown_method_1(int int_1)
        {
            int num = 22;
            int num2 = int_1 % 88;
            if (0 <= num2 && num2 < num)
            {
                return 0;
            }
            if (num <= num2 && num2 < num * 2)
            {
                return num2 % num;
            }
            if (num * 2 <= num2 && num2 < num * 3)
            {
                return num;
            }
            return num - num2 % num;
        }

        public static void PaintItemEffectInPanel(mGraphics g, int x, int y, int param)
        {
            if (param > 7)
                param = 7;
            if (param < 0)
                param = 0;
            if (param >= 4)
            {
                g.setColor(array2[param]);
                g.fillRect(x - 18, y - 12, 34, 23);
            }
            if (param < 4)
            {
                if (param == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < size.Length; j++)
                        {
                            int num = x - 17 + unknown_method_0(GameCanvas.gameTick + array3[i] - j * 4);
                            int num2 = y - 12 + unknown_method_1(GameCanvas.gameTick + array4[i] - j * 4);
                            g.setColor(colorBorder[array[param - 1]][j]);
                            g.fillRect(num - size[j] / 2, num2 - size[j] / 2, size[j], size[j]);
                        }
                    }
                    return;
                }
                if (param != 2)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        for (int l = 0; l < size.Length; l++)
                        {
                            int num3 = x - 17 + unknown_method_0(GameCanvas.gameTick + array3[k] - l * 4);
                            int num4 = y - 12 + unknown_method_1(GameCanvas.gameTick + array4[k] - l * 4);
                            g.setColor(colorBorder[0][l]);
                            g.fillRect(num3 - size[l] / 2, num4 - size[l] / 2, size[l], size[l]);
                        }
                    }
                    for (int m = 2; m < 4; m++)
                    {
                        for (int n = 0; n < size.Length; n++)
                        {
                            int num5 = x - 17 + unknown_method_0(GameCanvas.gameTick + array3[m] - n * 4);
                            int num6 = y - 12 + unknown_method_1(GameCanvas.gameTick + array4[m] - n * 4);
                            g.setColor(colorBorder[1][n]);
                            g.fillRect(num5 - size[n] / 2, num6 - size[n] / 2, size[n], size[n]);
                        }
                    }
                    return;
                }
            }
            for (int num7 = 0; num7 < 4; num7++)
            {
                for (int num8 = 0; num8 < size.Length; num8++)
                {
                    int num9 = x - 17 + unknown_method_0(GameCanvas.gameTick + array3[num7] - num8 * 4);
                    int num10 = y - 12 + unknown_method_1(GameCanvas.gameTick + array4[num7] - num8 * 4);
                    g.setColor(colorBorder[array[param - 1]][num8]);
                    g.fillRect(num9 - size[num8] / 2, num10 - size[num8] / 2, size[num8], size[num8]);
                }
            }
        }
    }
}