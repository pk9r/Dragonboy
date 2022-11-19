using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
	public const sbyte ATT_STAND = 0;

	public const sbyte ATT_FLY = 1;

	public const sbyte SKILL_AUTO_USE = 0;

	public const sbyte SKILL_CLICK_USE_ATTACK = 1;

	public const sbyte SKILL_CLICK_USE_BUFF = 2;

	public const sbyte SKILL_CLICK_NPC = 3;

	public const sbyte SKILL_CLICK_LIVE = 4;

	public SkillTemplate template;

	public short skillId;

	public int point;

	public long powRequire;

	public int coolDown;

	public long lastTimeUseThisSkill;

	public int dx;

	public int dy;

	public int maxFight;

	public int manaUse;

	public SkillOption[] options;

	public bool paintCanNotUseSkill;

	public short damage;

	public string moreInfo;

	public short price;

	public string strTimeReplay()
	{
		if (coolDown % 1000 == 0)
		{
			return coolDown / 1000 + string.Empty;
		}
		int num = coolDown % 1000;
		return coolDown / 1000 + "." + ((num % 100 != 0) ? (num / 10) : (num / 100));
	}

	public void paint(int x, int y, mGraphics g)
	{
		SmallImage.drawSmallImage(g, template.iconId, x, y, 0, StaticObj.VCENTER_HCENTER);
		long coolingDown = mSystem.currentTimeMillis() - lastTimeUseThisSkill;
		if (coolingDown < coolDown)
		{
			//g.setColor(2721889, 0.7f);
			//if (paintCanNotUseSkill && GameCanvas.gameTick % 6 > 2)
			//	g.setColor(876862);
            //int num3 = (int)(coolingDown * 20 / coolDown);
            //g.fillRect(x - 10, y - 10 + num3, 20, 20 - num3);
            float alpha = coolingDown * 360 / coolDown;
			Texture2D coolDownOverlay = new Texture2D(22 * mGraphics.zoomLevel, 22 * mGraphics.zoomLevel, TextureFormat.RGBA32, false);
			//fill with transparent
            for (int i = 0; i < coolDownOverlay.width; i++)
                for (int j = 0; j < coolDownOverlay.height; j++)
                    coolDownOverlay.SetPixel(i, j, new Color(0, 0, 0, 0f));
			#region Code đang chạy ok cấm sửa
			//paint first line
			for (int i = 0; i < coolDownOverlay.width / 2; i++)
				coolDownOverlay.SetPixel(coolDownOverlay.width / 2, i + coolDownOverlay.height / 2, new Color(0, 0, 0, 0.4f));
			if (alpha < 270)
			{
                //góc phần tư thứ 1
                for (int i = 0; i < coolDownOverlay.width / 2; i++)
					for (int j = 0; j < coolDownOverlay.height / 2; j++)
						coolDownOverlay.SetPixel(i, j + coolDownOverlay.height / 2, new Color(0, 0, 0, 0.4f));
			}
			if (alpha < 180)
			{
                //góc phần tư thứ 3
                for (int i = 0; i < coolDownOverlay.width / 2; i++)
					for (int j = 0; j < coolDownOverlay.height / 2; j++)
						coolDownOverlay.SetPixel(i, j, new Color(0, 0, 0, 0.4f));
			}
			if (alpha < 90)
			{
				//góc phần tư thứ 4
                for (int i = 0; i < coolDownOverlay.width / 2; i++)
                    for (int j = 0; j <= coolDownOverlay.height / 2; j++)
                        coolDownOverlay.SetPixel(i + coolDownOverlay.width / 2, j, new Color(0, 0, 0, 0.4f));
            }
			//find main line
			for (int i = 0; i < coolDownOverlay.width / 2; i++)	//height
			{
				if (alpha <= 90)    //>= 75%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * alpha) * i);
					if (distance >= coolDownOverlay.width / 2 || distance <= 0)
						continue;
                    coolDownOverlay.SetPixel(distance + coolDownOverlay.width / 2, i - coolDownOverlay.width / 2, new Color(1f, 0, 0, 1f));
				}
				else if (alpha <= 180)  //>= 50%, < 75%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * (180 - alpha)) * (coolDownOverlay.width / 2 - i));
					if (distance >= coolDownOverlay.width / 2 || distance <= 0)
						continue;
					coolDownOverlay.SetPixel(distance + coolDownOverlay.width / 2, i, new Color(1f, 0, 0, 1f));
				}
				else if (alpha <= 270)  //>= 25%, < 50%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * (alpha - 180)) * (coolDownOverlay.width / 2 - i));
					if (distance >= coolDownOverlay.width / 2 || distance <= 0)
						continue;
					coolDownOverlay.SetPixel(coolDownOverlay.width / 2 - distance, i, new Color(1f, 0, 0, 1f));
				}
				else //	< 25%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * (360 - alpha)) * i);
					if (distance >= coolDownOverlay.width / 2 || distance <= 0)
						continue;
					coolDownOverlay.SetPixel(coolDownOverlay.width / 2 - distance, i - coolDownOverlay.width / 2, new Color(1f, 0, 0, 1f));
				}
            }
			//fill remain part
			for (int i = 0; i < coolDownOverlay.width / 2; i++)  //y
			{
				int j = 0;
                for (; j < coolDownOverlay.width / 2; j++)  //x
				{
					if (alpha < 90)
					{
                        if (coolDownOverlay.GetPixel(j + coolDownOverlay.width / 2, i + coolDownOverlay.width / 2).r == 1f)  //red
                        {
                            for (int k = j; k <= coolDownOverlay.width / 2; k++)
                                coolDownOverlay.SetPixel(k + coolDownOverlay.width / 2, i + coolDownOverlay.width / 2, new Color(0, 0, 0, 0.4f));
                            break;
                        }
                    }
					else if (alpha < 180)
					{
						if (coolDownOverlay.GetPixel(j + coolDownOverlay.width / 2, i).r == 1f)  //red
						{
							for (int k = 0; k <= j; k++)
								coolDownOverlay.SetPixel(k + coolDownOverlay.width / 2, i, new Color(0, 0, 0, 0.4f));
							break;
						}
					}
					else if (alpha < 270)
					{
                        if (coolDownOverlay.GetPixel(j, i).r == 1f)  //red
                        {
                            for (int k = 0; k <= j ; k++)
                                coolDownOverlay.SetPixel(k, i, new Color(0, 0, 0, 0.4f));
                            break;
                        }
                    }
					else if (alpha < 360)
					{
                        if (coolDownOverlay.GetPixel(j, i + coolDownOverlay.width / 2).r == 1f)  //red
                        {
                            for (int k = j; k <= coolDownOverlay.width / 2; k++)
                                coolDownOverlay.SetPixel(k, i + coolDownOverlay.width / 2, new Color(0, 0, 0, 0.4f));
                            break;
                        }
                    }
				}
				if (j >= coolDownOverlay.width / 2)
					for (j = 0; j < coolDownOverlay.width / 2; j++)  //x
					{
						if (alpha < 45)
						{
                            coolDownOverlay.SetPixel(j + coolDownOverlay.width / 2, i + coolDownOverlay.width / 2, new Color(0, 0, 0, 0.4f));
                        }
						else if (alpha <= 135)
						{
							coolDownOverlay.SetPixel(j + coolDownOverlay.width / 2, i, new Color(0, 0, 0, 0.4f));
						}
						else if (alpha < 225)
						{
                            coolDownOverlay.SetPixel(j, i, new Color(0, 0, 0, 0.4f));
                        }
						else if (alpha < 315)
						{
                            if (i != 0) coolDownOverlay.SetPixel(j, i + coolDownOverlay.width / 2, new Color(0, 0, 0, 0.4f));
                        }
					}
            }
			//paint edges
			//red -> yellow -> green
			Color color = new Color(Mathf.Clamp(2f - (float)coolingDown * 2f / (float)coolDown, 0, 1f), Mathf.Clamp((float)coolingDown * 2f / (float)coolDown, 0, 1f), 0);
			for (int i = 0; i < mGraphics.zoomLevel; i++)
			{
                //top
                for (int j = 0; j < coolDownOverlay.width; j++)
                    if (coolDownOverlay.GetPixel(j, coolDownOverlay.height - 1 - i).a == 0.4f)
                        coolDownOverlay.SetPixel(j, coolDownOverlay.height - 1 - i, color);
                //bottom
                for (int j = 0; j < coolDownOverlay.width; j++)
                    if (coolDownOverlay.GetPixel(j, 0 + i).a == 0.4f)
                        coolDownOverlay.SetPixel(j, 0 + i, color);
                //left
                for (int j = 0; j < coolDownOverlay.height; j++)
                    if (coolDownOverlay.GetPixel(0 + i, j).a == 0.4f)
                        coolDownOverlay.SetPixel(0 + i, j, color);
                //right
                for (int j = 0; j < coolDownOverlay.height; j++)
                    if (coolDownOverlay.GetPixel(coolDownOverlay.width - 1 - i, j).a == 0.4f)
                        coolDownOverlay.SetPixel(coolDownOverlay.width - 1 - i, j, color);
            }
            #endregion
            coolDownOverlay.Apply();
			Graphics.DrawTexture(new Rect(x * mGraphics.zoomLevel - coolDownOverlay.width / 2, y * mGraphics.zoomLevel - coolDownOverlay.height / 2, coolDownOverlay.width, coolDownOverlay.height), coolDownOverlay);

			////Code dp 
			//g.setColor(new Color(1, 0, 0));
   //         int totalLength = (int)((coolDown - coolingDown) * 84 / coolDown);
   //         if (totalLength > 74)
   //         {
   //             g.fillRect(x + 84 - totalLength, y - 11, totalLength - 74, 1);
   //             g.fillRect(x + 10, y - 11, 1, 22);
   //             g.fillRect(x - 11, y + 10, 21, 1);
   //             g.fillRect(x - 11, y - 11, 1, 22);
   //             g.fillRect(x - 11, y - 11, 11, 1);
   //         }
   //         else if (totalLength > 53)
   //         {
   //             g.fillRect(x + 10, y + 63 - totalLength, 1, totalLength - 53);
   //             g.fillRect(x - 11, y + 10, 22, 1);
   //             g.fillRect(x - 11, y - 11, 1, 22);
   //             g.fillRect(x - 11, y - 11, 11, 1);
   //         }
   //         else if (totalLength > 32)
   //         {
   //             g.fillRect(x - 11, y + 10, totalLength - 32, 1);
   //             g.fillRect(x - 11, y - 11, 1, 22);
   //             g.fillRect(x - 11, y - 11, 11, 1);
   //         }
   //         else if (totalLength > 11)
   //         {
   //             g.fillRect(x - 11, y - 11, 1, totalLength - 11);
   //             g.fillRect(x - 11, y - 11, 11, 1);
   //         }
   //         else
   //             g.fillRect(x - totalLength, y - 11, totalLength, 1);

            string cooldownpaint = $"{(coolDown - coolingDown) / 1000f:#.0}";
			if (cooldownpaint.Length > 4)
				cooldownpaint = cooldownpaint.Substring(0, cooldownpaint.IndexOf('.'));
			GUIStyle style = new GUIStyle(GUI.skin.label);
            style.font = (Font)Resources.Load("FontSys/x" + mGraphics.zoomLevel + "/chelthm");
			//gUIStyle.normal.textColor = color;
			style.normal.textColor = Color.black;
            style.alignment = TextAnchor.UpperCenter;
			g.drawString(cooldownpaint, x - 1 - GameCanvas.w / 2, y - 12 + mFont.tahoma_7.getHeight() / 2, style);
		}
		else
		{
			paintCanNotUseSkill = false;
		}
	}
}
