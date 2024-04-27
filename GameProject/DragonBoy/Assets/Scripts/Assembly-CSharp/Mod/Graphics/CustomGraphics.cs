using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod.Graphics
{
    internal static class CustomGraphics
    {
        static Texture2D aaLineTex = null;
        static Texture2D lineTex = null;
        static Rect lineRect = new Rect(0, 0, 1, 1);
        static int[] array2 = new int[] { 0, 0, 0, 0, 600841, 3346944, 3932211, 6684682 };
        static int[] size = new int[6] { 2, 1, 1, 1, 1, 1 };
        static int[,] colorBorder = new int[5, 6]
        {
            { 18687, 16869, 15052, 13235, 11161, 9344 },
            { 45824, 39168, 32768, 26112, 19712, 13056 },
            { 16744192, 15037184, 13395456, 11753728, 10046464, 8404992 },
            { 13500671, 12058853, 10682572, 9371827, 7995545, 6684800 },
            { 16711705, 15007767, 13369364, 11730962, 10027023, 8388621 }
        };

        static float avgR = 0;
        static float avgG = 0;
        static float avgB = 0;
        //static float avgA = 0;
        static float blurPixelCount = 0;
        static Dictionary<string, Texture2D> cachedTextures = new Dictionary<string, Texture2D>();

        static CustomGraphics()
        {
            Initialize();
        }

        internal static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
        {
            float dx = pointB.x - pointA.x;
            float dy = pointB.y - pointA.y;
            float len = Mathf.Sqrt(dx * dx + dy * dy);
            if (len < 0.001f) 
                return;
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

        internal static void DrawCircle(Vector2 center, int radius, Color color, float thichness, bool antiAlias, int segmentsPerQuarter)
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

        internal static void DrawBezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
        {
            Vector2 lastV = CubeBezier(start, startTangent, end, endTangent, 0);
            for (int i = 1; i < segments + 1; ++i)
            {
                Vector2 v = CubeBezier(start, startTangent, end, endTangent, i / (float)segments);
                DrawLine(lastV, v, color, width, antiAlias);
                lastV = v;
            }
        }

        static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
        {
            float rt = 1 - t;
            return rt * rt * rt * s + 3 * rt * rt * t * st + 3 * rt * t * t * et + t * t * t * e;
        }

        static void Initialize()
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

        internal static void DrawCircle(Color color, float x, float y, float radius, float thickness)
        {
            Color oldColor = GUI.color;
            DrawCircle(new Vector2(x, y), Mathf.RoundToInt(radius), color, thickness, true, 10);
            GUI.color = oldColor;
        }

        internal static void DrawCircle(Color color, IMapObject mapObject, float radius, float thickness) => DrawCircle(color, (mapObject.getX() - GameScr.cmx) * mGraphics.zoomLevel, (mapObject.getY() - GameScr.cmy) * mGraphics.zoomLevel, radius * mGraphics.zoomLevel, thickness * mGraphics.zoomLevel);

        internal static void drawRect(mGraphics g, int x, int y, int w, int h, int thickness)
        {
            g.fillRect(x, y, w, thickness);
            g.fillRect(x, y, thickness, h);
            g.fillRect(x + w, y, thickness, h + thickness);
            g.fillRect(x, y + h, w + thickness, thickness);
        }

        internal static void drawLine(Color color, float x1, float y1, float x2, float y2, int thickness)
        {
            if (x1 == x2 && y1 == y2)
                return;
            string key = $"texture_drawline_{color.r}_{color.g}_{color.b}_{color.a}_{thickness}";
            Texture2D texture2D = cachedTextures[key];
            if (texture2D == null)
            {
                texture2D = new Texture2D(thickness, thickness);
                for (int i = 0; i < thickness; i++)
                    for (int j = 0; j < thickness; j++)
                        texture2D.SetPixel(i, j, color);
                texture2D.Apply();
                cachedTextures.Add(key, texture2D);
            }
            Vector2 vector = new Vector2(x1, y1);
            Vector2 vector3 = new Vector2(x2, y2) - vector;
            float angle = Mathf.Rad2Deg * Mathf.Atan(vector3.y / vector3.x);
            if (vector3.x < 0f)
                angle += 180f;
            GUIUtility.RotateAroundPivot(angle, vector);
            UnityEngine.Graphics.DrawTexture(new Rect(vector.x, vector.y, vector3.magnitude, thickness), texture2D);
            GUIUtility.RotateAroundPivot(-angle, vector);
        }

        internal static void PaintItemOptions(mGraphics g, Panel instance, Item item, int y)
        {
            int x = instance.X + Panel.WIDTH_PANEL - 2;
            if (instance == GameCanvas.panel2)
                x -= 2;
            if (Utils.HasActivateOption(item))
            {
                mFont.tahoma_7b_red.drawString(g, "$", x, y, mFont.RIGHT);
                x -= mFont.tahoma_7b_red.getWidth("$") + 2;
            }
            if (Utils.HasStarOption(item, out uint star, out uint starE))
                PaintStarOption(g, x, y, star, starE);
        }

        static void PaintStarOption(mGraphics g, int x, int y, uint star, uint starE)
        {
            if (star > 0)
            {
                mFont.tahoma_7b_red.drawString(g, star.ToString(), x - Image.getImageWidth(Panel.imgStar) - mFont.tahoma_7b_red.getWidth(star.ToString()) - 1, y, mFont.LEFT);
                g.drawImage(Panel.imgStar, x - Image.getImageWidth(Panel.imgStar) - 1, y + 1);
            }
            if (starE > 0)
            {
                if (star == 0)
                {
                    mFont.tahoma_7b_red.drawString(g, starE.ToString(), x - Image.getImageWidth(Panel.imgMaxStar) - mFont.tahoma_7b_red.getWidth(starE.ToString()) - 1, y, mFont.LEFT);
                    g.drawImage(Panel.imgMaxStar, x - Image.getImageWidth(Panel.imgMaxStar) - 1, y + 1);
                }
                else
                {
                    mFont.tahoma_7b_red.drawString(g, starE.ToString(), x - mFont.tahoma_7b_red.getWidth(starE.ToString() + star.ToString()) - Image.getImageWidth(Panel.imgMaxStar) * 2 - 2, y, mFont.LEFT);
                    g.drawImage(Panel.imgMaxStar, x - mFont.tahoma_7b_red.getWidth(starE.ToString()) - Image.getImageWidth(Panel.imgMaxStar) * 2 - 3, y + 1);
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

        internal static void PaintItemEffectInPanel(mGraphics g, int x, int y, int w, int h, Item item)
        {
            if (item.itemOption == null)
                return;
            ItemOption itemOption = item.GetBestItemOption();
            if (itemOption == null)
                return;
            int id = itemOption.optionTemplate.id;
            if ((id > 36 || id < 34) && id != 72 && (id < 127 || id > 135) && id != 107)
                return;
            int param = itemOption.param;
            if (param > 7 || (id >= 127 && id <= 135))
                param = 7;
            if (id == 107)
            {
                if (param > 1)
                    param = (int)Math.Ceiling((double)param / 2);
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

        internal static void DrawAPartOfImage(Image image, float x, float y, float w, float h, int imageX, int imageY, float degAngle, bool scale = true)
        {
            if (scale)
            {
                x *= mGraphics.zoomLevel;
                y *= mGraphics.zoomLevel;
                w *= mGraphics.zoomLevel;
                h *= mGraphics.zoomLevel;
                imageX *= mGraphics.zoomLevel;
                imageY *= mGraphics.zoomLevel;
            }
            int imageW = image.w;
            int imageH = image.h;
            Vector2 pivot = new Vector2(imageX + w / 2 + x, imageY + h / 2 + y);
            GUIUtility.RotateAroundPivot(degAngle, pivot);
            GUI.BeginGroup(new Rect(x - imageX, y - imageY, w, h));
            GUI.DrawTexture(new Rect(0, 0, imageW, imageH), image.texture);
            //GUI.DrawTexture(new Rect(x - imageX, y - imageY, imageW, imageH), image.texture);
            GUI.EndGroup();
            GUIUtility.RotateAroundPivot(-degAngle, pivot);
        }

        internal static void DrawImage(Image image, float x, float y, float degAngle = 0, bool scale = true)
        {
            if (scale)
            {
                x *= mGraphics.zoomLevel;
                y *= mGraphics.zoomLevel;
            }
            int imageW = image.texture.width;
            int imageH = image.texture.height;
            Vector2 pivot = new Vector2(x + imageW / 2, y + imageW / 2);
            GUIUtility.RotateAroundPivot(degAngle, pivot);
            GUI.BeginGroup(new Rect(x, y, imageW, imageH));
            GUI.DrawTexture(new Rect(0, 0, imageW, imageH), image.texture);
            //GUI.DrawTexture(new Rect(x - imageX, y - imageY, imageW, imageH), image.texture);
            GUI.EndGroup();
            GUIUtility.RotateAroundPivot(-degAngle, pivot);
        }

        internal static void fillRect(int x, int y, int w, int h, Color color, bool scale = true)
        {
            if (scale)
            {
                x *= mGraphics.zoomLevel;
                y *= mGraphics.zoomLevel;
            }
            if (w < 0 || h < 0)
                return;
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();
            GUI.DrawTexture(new Rect(x, y, w, h), texture2D);
        }

        //internal static Texture2D CropToSquare(Texture2D textureToCrop, int size)
        //{
        //    int minSide = Math.Min(textureToCrop.width, textureToCrop.height);
        //    Texture2D texture2D = new Texture2D(minSide, minSide);
        //    for (int i = 0; i < minSide * minSide; i++)
        //    {
        //        int offset = Math.Abs(textureToCrop.width - textureToCrop.height) / 2;
        //        int x = i % minSide;
        //        int y = i / minSide;
        //        if (textureToCrop.width > textureToCrop.height)
        //            texture2D.SetPixel(x, y, textureToCrop.GetPixel(x + offset, y));
        //        else if (textureToCrop.width < textureToCrop.height)
        //            texture2D.SetPixel(x, y, textureToCrop.GetPixel(x, y + offset));
        //        else
        //            texture2D.SetPixel(x, y, textureToCrop.GetPixel(x, y));
        //    }
        //    return TextureScaler.ScaleTexture(texture2D, size, size);
        //}

        internal static Texture2D CropToCircle(Texture2D textureToCrop, int borderThickness = 0, bool isApply = true)
        {
            if (textureToCrop.width != textureToCrop.height)
                throw new ArgumentException("textureToCrop isn't a square texture!");
            int centerX = textureToCrop.width / 2;
            int centerY = textureToCrop.height / 2;
            for (int i = 0; i < textureToCrop.width * textureToCrop.height; i++)
            {
                int x = i % textureToCrop.width;
                int y = i / textureToCrop.height;
                double distance = getDistance(centerX, centerY, x, y);
                if (distance >= textureToCrop.width / 2d - 1)
                    textureToCrop.SetPixel(x, y, Color.clear);
                else if (distance >= textureToCrop.width / 2d - 1 - borderThickness)
                    textureToCrop.SetPixel(x, y, new Color(.5f, .5f, .5f));
            }
            if (isApply)
                textureToCrop.Apply();
            return textureToCrop;
        }

        static double getDistance(int x1, int y1, int x2, int y2) => Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

        internal static Texture2D RoundCorner(Texture2D texture, int radius)
        {
            int xLeft = radius;
            int xRight = texture.width - radius;
            int yUpper = radius;
            int yLower = texture.height - radius;
            Color transparent = new Color(0, 0, 0, 0);
            for (int x = 0; x < xLeft; x++)
            {
                for (int y = 0; y < yUpper; y++)
                {
                    double distance = getDistance(xLeft, yUpper, x, y);
                    if (distance > radius)
                        texture.SetPixel(x, y, transparent);
                }
                for (int y = yLower; y < texture.height; y++)
                {
                    double distance = getDistance(xLeft, yLower, x, y);
                    if (distance > radius)
                        texture.SetPixel(x, y, transparent);
                }
            }
            for (int x = xRight; x < texture.width; x++)
            {
                for (int y = 0; y < yUpper; y++)
                {
                    double distance = getDistance(xRight, yUpper, x, y);
                    if (distance > radius)
                        texture.SetPixel(x, y, transparent);
                }
                for (int y = yLower; y < texture.height; y++)
                {
                    double distance = getDistance(xRight, yLower, x, y);
                    if (distance > radius)
                        texture.SetPixel(x, y, transparent);
                }
            }

            return texture;
        }

        internal static void drawCooldownRect(float x, float y, float w, float h, float value, Color color)
        {
            x *= mGraphics.zoomLevel;
            y *= mGraphics.zoomLevel;
            w *= mGraphics.zoomLevel;
            h *= mGraphics.zoomLevel;
            Matrix4x4 matrix = GUI.matrix;
            UIImage.image.rectTransform.sizeDelta = new Vector2(w, h);
            UIImage.image.fillAmount = value;
            UIImage.image.color = color;
            UIImage.image.material.SetPass(0);
            UIImage.PopulateMesh();
            UnityEngine.Graphics.DrawMeshNow(UIImage.mesh, new Vector3(x, y, -1), Quaternion.identity, 0);
            GUI.matrix = matrix;
        }

        internal static Texture2D FlipTextureVertically(Texture2D original)
        {
            var originalPixels = original.GetPixels();
            var newPixels = new Color32[originalPixels.Length];
            var width = original.width;
            var rows = original.height;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
                }
            }
            Texture2D newTexture = new Texture2D(width, rows);
            newTexture.SetPixels32(newPixels);
            newTexture.Apply();
            return newTexture;
        }

        internal static Texture2D FlipTextureHorizontally(Texture2D original)
        {
            var originalPixels = original.GetPixels();
            var newPixels = new Color32[originalPixels.Length];
            var width = original.width;
            var rows = original.height;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < rows; y++)
                {
                    newPixels[x + y * width] = originalPixels[(width - x - 1) + y * width];
                }
            }
            Texture2D newTexture = new Texture2D(width, rows);
            newTexture.SetPixels32(newPixels);
            newTexture.Apply();
            return newTexture;
        }
    }
}