using System.Collections;

namespace Mod
{
    /// <summary>
    /// Class để các extension override, chứa các method được gọi khi một sự kiện nào đó trong game diễn ra.
    /// </summary>
    public abstract class Extension
    {
        public abstract bool onSendChat(string text);

        public abstract void onGameStarted();

        public abstract bool onGameClosing();

        public abstract void onSaveRMSString(string filename, string data);

        public abstract void onKeyMapLoaded(Hashtable h);

        public abstract bool onSetResolution();

        public abstract void onGameScrPressHotkeysUnassigned();

        public abstract void onPaintChatTextField(mGraphics g);

        public abstract bool onStartChatTextField(ChatTextField sender);

        public abstract bool onLoadRMSInt(string file, int result);

        public abstract bool onGetRMSPath(string result);

        public abstract bool onTeleportUpdate(Teleport teleport);

        public abstract void onUpdateChatTextField(ChatTextField sender);

        public abstract bool onClearAllRMS();

        public abstract void onUpdateGameScr();

        //public abstract void onLogin(string username, string pass, sbyte type);

        public abstract void onServerListScreenLoaded();

        public abstract void onSessionConnecting(string host, int port);

        public abstract void onSceenDownloadDataShow();

        public abstract bool onCheckZoomLevel();

        public abstract bool onKeyPressedz(int keyCode, bool isFromSync);

        public abstract bool onKeyReleasedz(int keyCode, bool isFromAsync);

        public abstract bool onChatPopupMultiLine(string chat);

        public abstract bool onAddBigMessage(string chat, Npc npc);

        public abstract void onInfoMapLoaded();

        public abstract void onPaintGameScr(mGraphics g);

        public abstract bool onUseSkill(Skill skill);

        public abstract void onFixedUpdateMain();

        public abstract void onAddInfoMe(string str);

        public abstract void onUpdateKeyTouchControl();

        public abstract void onSetPointItemMap(int xEnd, int yEnd);

        public abstract bool onMenuStartAt(MyVector menuItems);

        public abstract void onAddInfoChar(string info, Char c);

        public abstract void onLoadImageGameCanvas();

        public abstract bool onPaintBgGameScr(mGraphics g);

        public abstract void onMobStartDie(Mob instance);

        public abstract void onUpdateMob(Mob instance);

        public abstract Image onCreateImage(string filename);
    }
}
