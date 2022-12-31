using System;
using System.Collections;

namespace Mod
{
    /// <summary>
    /// Class để các extension override, chứa các method được gọi khi một sự kiện nào đó trong game diễn ra.
    /// </summary>
    public class Extension
    {
        public virtual bool onSendChat(string text)
		{
			throw new NotImplementedException();
		}

        public virtual void onGameStarted()
		{
            throw new NotImplementedException();
        }

        public virtual bool onGameClosing()
		{
            throw new NotImplementedException();
        }

        public virtual void onSaveRMSString(string filename, string data)
		{
            throw new NotImplementedException();
        }

        public virtual void onKeyMapLoaded(Hashtable h)
		{
            throw new NotImplementedException();
        }

        public virtual bool onSetResolution()
		{
            throw new NotImplementedException();
        }

        public virtual void onGameScrPressHotkeysUnassigned()
		{
            throw new NotImplementedException();
        }

        public virtual void onPaintChatTextField(mGraphics g)
		{
            throw new NotImplementedException();
        }

        public virtual bool onStartChatTextField(ChatTextField sender)
		{
            throw new NotImplementedException();
        }

        public virtual bool onLoadRMSInt(string file, int result)
		{
            throw new NotImplementedException();
        }

        public virtual bool onGetRMSPath(string result)
		{
            throw new NotImplementedException();
        }

        public virtual bool onTeleportUpdate(Teleport teleport)
		{
            throw new NotImplementedException();
        }

        public virtual void onUpdateChatTextField(ChatTextField sender)
		{
            throw new NotImplementedException();
        }

        public virtual bool onClearAllRMS()
		{
            throw new NotImplementedException();
        }

        public virtual void onUpdateGameScr()
		{
            throw new NotImplementedException();
        }

        //public abstract void onLogin(string username, string pass, sbyte type);

        public virtual void onServerListScreenLoaded()
		{
            throw new NotImplementedException();
        }

        public virtual void onSessionConnecting(string host, int port)
		{
            throw new NotImplementedException();
        }

        public virtual void onSceenDownloadDataShow()
		{
            throw new NotImplementedException();
        }

        public virtual bool onCheckZoomLevel()
		{
            throw new NotImplementedException();
        }

        public virtual bool onKeyPressedz(int keyCode, bool isFromSync)
		{
            throw new NotImplementedException();
        }

        public virtual bool onKeyReleasedz(int keyCode, bool isFromAsync)
		{
            throw new NotImplementedException();
        }

        public virtual bool onChatPopupMultiLine(string chat)
		{
            throw new NotImplementedException();
        }

        public virtual bool onAddBigMessage(string chat, Npc npc)
		{
            throw new NotImplementedException();
        }

        public virtual void onInfoMapLoaded()
		{
            throw new NotImplementedException();
        }

        public virtual void onPaintGameScr(mGraphics g)
		{
            throw new NotImplementedException();
        }

        public virtual bool onUseSkill(Skill skill)
		{
            throw new NotImplementedException();
        }

        public virtual void onFixedUpdateMain()
		{
            throw new NotImplementedException();
        }

        public virtual void onAddInfoMe(string str)
		{
            throw new NotImplementedException();
        }

        public virtual void onUpdateKeyTouchControl()
		{
            throw new NotImplementedException();
        }

        public virtual void onSetPointItemMap(int xEnd, int yEnd)
		{
            throw new NotImplementedException();
        }

        public virtual bool onMenuStartAt(MyVector menuItems)
		{
            throw new NotImplementedException();
        }

        public virtual void onAddInfoChar(string info, Char c)
		{
            throw new NotImplementedException();
        }

        public virtual void onLoadImageGameCanvas()
		{
            throw new NotImplementedException();
        }

        public virtual bool onPaintBgGameScr(mGraphics g)
		{
            throw new NotImplementedException();
        }

        public virtual void onMobStartDie(Mob instance)
		{
            throw new NotImplementedException();
        }

        public virtual void onUpdateMob(Mob instance)
		{
            throw new NotImplementedException();
        }

        public virtual Image onCreateImage(string filename)
		{
            throw new NotImplementedException();
        }
    }
}
