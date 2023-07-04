using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.Auto
{
	public class AutoItem : IActionListener, IChatable
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000021DE File Offset: 0x000003DE
		public static AutoItem gI()
		{
			if (AutoItem.Instance == null)
			{
				AutoItem.Instance = new AutoItem();
			}
			return AutoItem.Instance;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003ED8 File Offset: 0x000020D8
		public static void update()
		{
			if (AutoItem.listItemUse.Count == 0 && GameCanvas.gameTick % 20 == 0)
			{
				return;
			}
			for (int i = 0; i < AutoItem.listItemUse.Count; i++)
			{
				if (mSystem.currentTimeMillis() - AutoItem.listItemUse[i].lastTimeUseItem > (long)(AutoItem.listItemUse[i].timeDelay * 1000))
				{
					AutoItem.listItemUse[i].lastTimeUseItem = mSystem.currentTimeMillis();
					Service.gI().useItem(0, 1, -1, (short)AutoItem.listItemUse[i].idItems);
				}
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003F74 File Offset: 0x00002174
		public void onChatFromMe(string text, string to)
		{
			if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
			{
				ChatTextField.gI().isShow = false;
				return;
			}
			if (ChatTextField.gI().strChat.Equals(AutoItem.inputTimeDelay[0]))
			{
				try
				{
					int time = int.Parse(ChatTextField.gI().tfChat.getText());
					GameScr.info1.addInfo(string.Concat(new string[]
					{
					"Auto: ",
					ItemTemplates.get((short)AutoItem.idItemObj).name,
					" [",
					time.ToString(),
					" giây]"
					}), 0);
					AutoItem.listItemUse.Add(new AutoItem(AutoItem.idItemObj, time));
				}
				catch
				{
					GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
				}
				ChatTextField.gI().strChat = "Chat";
				ChatTextField.gI().tfChat.name = "chat";
				ChatTextField.gI().isShow = false;
				return;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000020C8 File Offset: 0x000002C8
		public void onCancelChat()
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004084 File Offset: 0x00002284
		public void perform(int idAction, object p)
		{
			switch (idAction)
			{
				case 1:
					AutoItem.idItemObj = (int)p;
					ChatTextField.gI().strChat = AutoItem.inputTimeDelay[0];
					ChatTextField.gI().tfChat.name = AutoItem.inputTimeDelay[1];
					GameCanvas.panel.isShow = false;
					ChatTextField.gI().startChat2(AutoItem.gI(), string.Empty);
					return;
				case 2:
					for (int i = 0; i < AutoItem.listItemUse.Count; i++)
					{
						if (AutoItem.listItemUse[i].idItems == (int)p)
						{
							AutoItem.listItemUse.RemoveAt(i);
							GameScr.info1.addInfo("Dừng auto: " + ItemTemplates.get((short)((int)p)).name, 0);
						}
					}
					return;
				case 3:
					return;
				default:
					return;
			}
		}

	
		// Token: 0x06000035 RID: 53 RVA: 0x000021F6 File Offset: 0x000003F6
		public AutoItem(int ID, int time)
		{
			this.idItems = ID;
			this.timeDelay = time;
		}
		public AutoItem()
		{
		}
		// Token: 0x06000036 RID: 54 RVA: 0x00004158 File Offset: 0x00002358
		public static bool checkList(int Id)
		{
			for (int i = 0; i < AutoItem.listItemUse.Count; i++)
			{
				if (AutoItem.listItemUse[i].idItems == Id)
				{
					return true;
				}
			}
			return false;
		}

		private static AutoItem Instance;

		public long lastTimeUseItem;

		public static int TimeUseItem;

		public static List<AutoItem> listItemUse = new List<AutoItem>();

		public int idItems;

		public int timeDelay;
		
		private static string[] inputTimeDelay = new string[]
		{
		"Nhập thời gian sử dụng item <Giây>",
		"By: NVan3177"
		};

		// Token: 0x0400001C RID: 28
		private static int idItemObj;
	}
}
