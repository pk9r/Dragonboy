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
			Texture2D coolDownOverlay = new Texture2D(20 * mGraphics.zoomLevel, 20 * mGraphics.zoomLevel, TextureFormat.RGBA32, false);
            for (int i = 0; i < 20 * mGraphics.zoomLevel; i++)
                for (int j = 0; j < 20 * mGraphics.zoomLevel; j++)
                    coolDownOverlay.SetPixel(i, j, new Color(0, 0, 0, 0f));
			#region Code đang chạy ok cấm sửa
			for (int i = 0; i < 10 * mGraphics.zoomLevel; i++)
				coolDownOverlay.SetPixel(10 * mGraphics.zoomLevel, i + 10 * mGraphics.zoomLevel, new Color(0, 0, 0, 0.4f));
            if (alpha < 270)
			{
                //góc phần tư thứ 1
                for (int i = 0; i <= 10 * mGraphics.zoomLevel; i++)
					for (int j = 0; j <= 10 * mGraphics.zoomLevel; j++)
						coolDownOverlay.SetPixel(i, j + 10 * mGraphics.zoomLevel, new Color(0, 0, 0, 0.4f));
			}
			if (alpha < 180)
			{
                //góc phần tư thứ 3
                for (int i = 0; i <= 10 * mGraphics.zoomLevel; i++)
					for (int j = 0; j <= 10 * mGraphics.zoomLevel; j++)
						coolDownOverlay.SetPixel(i, j, new Color(0, 0, 0, 0.4f));
			}
			if (alpha < 90)
			{
				//góc phần tư thứ 4
                for (int i = 0; i <= 10 * mGraphics.zoomLevel; i++)
                    for (int j = 0; j <= 10 * mGraphics.zoomLevel; j++)
                        coolDownOverlay.SetPixel(i + 10 * mGraphics.zoomLevel, j, new Color(0, 0, 0, 0.4f));
            }
			for (int i = 0; i < 10 * mGraphics.zoomLevel; i++)	//height
			{
				if (alpha <= 90)    //>= 75%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * alpha) * i);
					if (distance >= 10 * mGraphics.zoomLevel || distance <= 0)
						continue;
                    coolDownOverlay.SetPixel(distance + 10 * mGraphics.zoomLevel, i - 10 * mGraphics.zoomLevel, new Color(1f, 0, 0, 1f));
				}
				else if (alpha <= 180)  //>= 50%, < 75%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * (180 - alpha)) * (10 * mGraphics.zoomLevel - i));
					if (distance >= 10 * mGraphics.zoomLevel || distance <= 0)
						continue;
					coolDownOverlay.SetPixel(distance + 10 * mGraphics.zoomLevel, i, new Color(1f, 0, 0, 1f));
				}
				else if (alpha <= 270)  //>= 25%, < 50%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * (alpha - 180)) * (10 * mGraphics.zoomLevel - i));
					if (distance >= 10 * mGraphics.zoomLevel || distance <= 0)
						continue;
					coolDownOverlay.SetPixel(10 * mGraphics.zoomLevel - distance, i, new Color(1f, 0, 0, 1f));
				}
				else //	< 25%
				{
					int distance = (int)(Mathf.Tan(Mathf.Deg2Rad * (360 - alpha)) * i);
					if (distance >= 10 * mGraphics.zoomLevel || distance <= 0)
						continue;
					coolDownOverlay.SetPixel(10 * mGraphics.zoomLevel - distance, i - 10 * mGraphics.zoomLevel, new Color(1f, 0, 0, 1f));
				}
            }
			for (int i = 0; i < 10 * mGraphics.zoomLevel; i++)  //y
			{
				int j = 0;
                for (; j < 10 * mGraphics.zoomLevel; j++)  //x
				{
					if (alpha < 90)
					{
                        if (coolDownOverlay.GetPixel(j + 10 * mGraphics.zoomLevel, i + 10 * mGraphics.zoomLevel).r == 1f)  //red
                        {
                            for (int k = j; k <= 10 * mGraphics.zoomLevel; k++)
                                coolDownOverlay.SetPixel(k + 10 * mGraphics.zoomLevel, i + 10 * mGraphics.zoomLevel, new Color(0, 0, 0, 0.4f));
                            break;
                        }
                    }
					else if (alpha < 180)
					{
						if (coolDownOverlay.GetPixel(j + 10 * mGraphics.zoomLevel, i).r == 1f)  //red
						{
							for (int k = 0; k <= j; k++)
								coolDownOverlay.SetPixel(k + 10 * mGraphics.zoomLevel, i, new Color(0, 0, 0, 0.4f));
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
                        if (coolDownOverlay.GetPixel(j, i + 10 * mGraphics.zoomLevel).r == 1f)  //red
                        {
                            for (int k = j; k <= 10 * mGraphics.zoomLevel; k++)
                                coolDownOverlay.SetPixel(k, i + 10 * mGraphics.zoomLevel, new Color(0, 0, 0, 0.4f));
                            break;
                        }
                    }
				}
				if (j >= 10 * mGraphics.zoomLevel)
					for (j = 0; j <= 10 * mGraphics.zoomLevel; j++)  //x
					{
						if (alpha < 45)
						{
                            coolDownOverlay.SetPixel(j + 10 * mGraphics.zoomLevel, i + 10 * mGraphics.zoomLevel, new Color(0, 0, 0, 0.4f));
                        }
						else if (alpha < 135)
						{
							coolDownOverlay.SetPixel(j + 10 * mGraphics.zoomLevel, i, new Color(0, 0, 0, 0.4f));
						}
						else if (alpha < 225)
						{
                            coolDownOverlay.SetPixel(j, i, new Color(0, 0, 0, 0.4f));
                        }
						else if (alpha < 315)
						{
                            if (i != 0) coolDownOverlay.SetPixel(j, i + 10 * mGraphics.zoomLevel, new Color(0, 0, 0, 0.4f));
                        }
					}
            }
            #endregion
            coolDownOverlay.Apply();
			Graphics.DrawTexture(new Rect((x - 10) * mGraphics.zoomLevel, (y - 10) * mGraphics.zoomLevel, 20 * mGraphics.zoomLevel, 20 * mGraphics.zoomLevel), coolDownOverlay);
			string cooldownpaint = $"{(coolDown - coolingDown) / 1000f:#.0}";
			if (cooldownpaint.Length > 4)
				cooldownpaint = cooldownpaint.Substring(0, cooldownpaint.IndexOf('.'));
            mFont.tahoma_7.drawString(g, cooldownpaint, x + 1, y - 11 + mFont.tahoma_7.getHeight() / 2, mFont.CENTER);
		}
		else
		{
			paintCanNotUseSkill = false;
		}
	}
}
