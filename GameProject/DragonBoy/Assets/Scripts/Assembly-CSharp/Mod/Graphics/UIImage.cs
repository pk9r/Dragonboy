using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mod.Graphics
{
    internal class UIImage : UnityEngine.UI.Image
    {
        internal static Mesh mesh = new Mesh();
        static Texture2D texture;
        internal static UIImage image;

        protected override void OnPopulateMesh(Mesh m)
        {
            base.OnPopulateMesh(m);
            mesh = m;
        }

        internal static void PopulateMesh() => image.OnPopulateMesh(mesh);

        internal static void OnStart()
        {
            GameObject mainCamera = GameObject.Find("Main Camera");
            Canvas canvas = mainCamera.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            GameObject uiImageCooldown = new GameObject("Cooldown Effect");
            image = uiImageCooldown.AddComponent<UIImage>();
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.black);
            texture.Apply();
            image.sprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            image.type = Type.Filled;
            image.fillMethod = FillMethod.Radial360;
            image.fillClockwise = true;
            uiImageCooldown.transform.SetParent(mainCamera.transform);
            uiImageCooldown.transform.position = new Vector3(-500, -500);
        }
    }
}