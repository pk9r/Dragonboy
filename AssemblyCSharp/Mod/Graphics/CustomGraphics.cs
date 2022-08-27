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
    }
}