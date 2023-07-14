using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod.Auto
{
	public class AutoItem : IActionListener, IChatable
	{
		public static AutoItem gI()
		{
			if (AutoItem.Instance == null)
			{
				AutoItem.Instance = new AutoItem();
			}
			return AutoItem.Instance;
		}

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
					ShowMenu();

				}
				catch
				{
					GameScr.info1.addInfo("Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
					ShowMenu();
				}
				ChatTextField.gI().strChat = "Chat";
				ChatTextField.gI().tfChat.name = "chat";
				ChatTextField.gI().isShow = false;
				return;
			}
		}

		public void onCancelChat()
		{
		}

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
					ShowMenu();
					return;
				case 3:
					return;
				default:
					return;
			}
		}
		[ChatCommand("item"), HotkeyCommand('i')]
		public static void ShowMenu()
		{
			MyVector myVector = new MyVector();
			List<int> list = new List<int>();
			list.Clear();
			for (int i = global::Char.myCharz().arrItemBag.Length - 1; i >= 0; i--)
			{
				Item item = global::Char.myCharz().arrItemBag[i];
				if (item != null && !list.Contains((int)item.template.id) && AutoItem.ItemCanUse(item))
				{
					list.Add((int)item.template.id);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!AutoItem.checkList(list[j]))
				{
					myVector.addElement(new Command(string.Concat(new string[]
					{
					ItemTemplates.get((short)list[j]).name,
					"\n[TẮT]"
					}), AutoItem.gI(), 1, list[j]));
				}
				else
				{
					for (int k = 0; k < AutoItem.listItemUse.Count; k++)
					{
						if (AutoItem.listItemUse[k].idItems == list[j])
						{
							myVector.addElement(new Command(string.Concat(new string[]
							{
							ItemTemplates.get((short)list[j]).name,
							"\n[",
							AutoItem.listItemUse[k].timeDelay.ToString(),
							" giây]"
							}), AutoItem.gI(), 2, list[j]));
						}
					}
				}
			}
			GameCanvas.menu.startAt(myVector, 3);
		}
		public static bool ItemCanUse(Item item)
		{
			return item.template.type == 29 || item.template.type == 27 || item.template.type == 6 || item.template.type == 31;
		}
		public AutoItem(int ID, int time)
		{
			this.idItems = ID;
			this.timeDelay = time;
		}
		public AutoItem()
		{
		}

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

		private static int idItemObj;
	}
}
