using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mod.R
{
    internal static class Strings
    {
        internal static readonly string DEFAULT_IP_SERVERS = "Vũ trụ 1:dragon1.teamobi.com:14445:0:0:0,Vũ trụ 2:dragon2.teamobi.com:14445:0:0:0,Vũ trụ 3:dragon3.teamobi.com:14445:0:0:0,Vũ trụ 4:dragon4.teamobi.com:14445:0:0:0,Vũ trụ 5:dragon5.teamobi.com:14445:0:0:0,Vũ trụ 6:dragon6.teamobi.com:14445:0:0:0,Vũ trụ 7:dragon7.teamobi.com:14445:0:0:0,Vũ trụ 8:dragon10.teamobi.com:14446:0:0:0,Vũ trụ 9:dragon10.teamobi.com:14447:0:0:0,Vũ trụ 10:dragon10.teamobi.com:14445:0:0:0,Vũ trụ 11:dragon11.teamobi.com:14445:0:0:0,Vũ trụ 12:dragon12.teamobi.com:14445:0:0:0,Võ đài liên vũ trụ:dragonwar.teamobi.com:20000:0:0:0,Universe 1:dragon.indonaga.com:14445:1:0:0,Naga:dragon.indonaga.com:14446:2:0:0,Super 1:dragon11.teamobi.com:14446:0:1:1,0,0";
        #region Fields
        internal static string communityMod = "";
        internal static string gameVersion = "";
        internal static string registered = "";
        internal static string arbitration = "";
        internal static string autoChatDisabled = "";
        internal static string inputContent = "";
        internal static string delaySeconds = "";
        internal static string viewContent = "";
        internal static string autoChatContent = "";
        internal static string inputDelay = "";
        internal static string timeMilliseconds = "";
        internal static string errorOccurred = "";
        internal static string contentSaved = "";
        internal static string autoAttack = "";
        internal static string gobackTo = "";
        internal static string youAreNotNamekian = "";
        internal static string completed = "";
        internal static string[][] modMenuPanelTabName = new string[0][];
        internal static string someonePet = "";
        internal static string petLostMaster = "";
        internal static string zone = "";
        internal static string vSyncDescription = "";
        internal static string showTargetInfoTitle = "";
        internal static string showTargetInfoDescription = "";
        internal static string autoSendAttackDescription = "";
        internal static string showCharListTitle = "";
        internal static string showCharListDescription = "";
        internal static string showPetInCharListTitle = "";
        internal static string showPetInCharListDescription = "";
        internal static string autoTrainForNewbieTitle = "";
        internal static string autoTrainForNewbieDescription = "";
        internal static string noLongerNewAccount = "";
        internal static string customBackgroundTitle = "";
        internal static string customBackgroundDescription = "";
        internal static string notifyBossTitle = "";
        internal static string notifyBossDescription = "";
        internal static string pickMobTitle = "";
        internal static string pickMobDescription = "";
        internal static string functionShouldBeDisabled = "";
        internal static string functionShouldBeEnabled = "";
        internal static string avoidSuperMobTitle = "";
        internal static string avoidSuperMobDescription = "";
        internal static string vdhTitle = "";
        internal static string vdhDescription = "";
        internal static string autoPickItemTitle = "";
        internal static string autoPickItemDescription = "";
        internal static string pickMyItemOnlyTitle = "";
        internal static string pickMyItemOnlyDescription = "";
        internal static string limitPickTimesTitle = "";
        internal static string limitPickTimesDescription = "";
        internal static string autoAskForPeansTitle = "";
        internal static string autoAskForPeansDescription = "";
        internal static string youAreNotInAClan = "";
        internal static string autoDonatePeansTitle = "";
        internal static string autoDonatePeansDescription = "";
        internal static string autoHarvestPeansTitle = "";
        internal static string autoHarvestPeansDescription = "";
        internal static string setFPSDescription = "";
        internal static string setReduceGraphicsTitle = "";
        internal static string level = "";
        internal static string[] setReduceGraphicsChoices = new string[0];
        internal static string[] setGoBackChoices = new string[0];
        internal static string setAutoTrainPetTitle = "";
        internal static string[] setAutoTrainPetChoices = new string[0];
        internal static string youDontHaveDisciple = "";
        internal static string setAutoAttackWhenDiscipleNeededTitle = "";
        internal static string[] setAutoAttackWhenDiscipleNeededChoices = new string[0];
        internal static string setAutoRescueTitle = "";
        internal static string[] setAutoRescueChoices = new string[0];
        internal static string setTimeChangeCustomBgTitle = "";
        internal static string setTimeChangeCustomBgDescription = "";
        internal static string openXmapMenuTitle = "";
        internal static string openXmapMenuDescription = "";
        internal static string openPickMobMenuTitle = "";
        internal static string openPickMobMenuDescription = "";
        internal static string openTeleportMenuTitle = "";
        internal static string openTeleportMenuDescription = "";
        internal static string openCustomBackgroundMenuTitle = "";
        internal static string openCustomBackgroundMenuDescription = "";
        internal static string openSetsMenuTitle = "";
        internal static string openSetsMenuDescription = "";
        internal static string valueChanged = "";
        internal static string invalidValue = "";
        internal static string inputFPS = "";
        internal static string inputTimeChangeBg = "";
        internal static string inputTimeChangeBgHint = "";
        internal static string customBgChatPopup = "";
        internal static string customBgOpenBgList = "";
        internal static string customBgAddNewBg = "";
        internal static string customBgRemoveAll = "";
        internal static string customBgAllBgRemoved = "";
        internal static string customBgAutoChangeBg = "";
        internal static string customBgSetTimeChange = "";
        internal static string customBgChangeGifSpeed = "";
        internal static string customBgInputGifSpeed = "";
        internal static string speed = "";
        internal static string customBgSwitchToThisBg = "";
        internal static string delete = "";
        internal static string customBgRemovedBg = "";
        internal static string fullPath = "";
        internal static string customBgList = "";
        internal static string inputNumberOutOfRange = "";
        internal static string customBgGifSpeed = "";
        internal static string inputNumberMustBeBiggerThanOrEqual = "";
        internal static string imageVideoFile = "";
        internal static string allFileTypes = "";
        internal static string customBgSelectBgFiles = "";
        internal static string skipSpaceshipTitle = "";
        internal static string skipSpaceshipDescription = "";
        internal static string setAutoRescueSkill3BuffInvalid = "";
        internal static string setAutoRescueSkill3Null = "";
        internal static string pickMobMonsterAdded;
        internal static string pickMobMonsterRemoved;
        internal static string pickMobMonsterTypeAdded;
        internal static string pickMobMonsterTypeRemoved;
        internal static string pickMobAutoPickItemListRemoved;
        internal static string pickMobAutoPickItemListAdded;
        internal static string pickMobAutoPickItemTypesListRemoved;
        internal static string pickMobAutoPickItemTypesListAdded;
        internal static string pickMobPlsFocusOnMonsterOrItem;
        internal static string pickMobMonsterListCleared;
        internal static string pickMobItemListResetToDefault;
        internal static string pickMobConfiguredPickGemsOnly;
        internal static string pickMobSkillListRemoved;
        internal static string pickMobSkillListAdded;
        internal static string pickMobSkillListResetToDefault;
        internal static string pickMobDontPickItemListRemoved;
        internal static string pickMobDontPickItemListAdded;
        internal static string pickMobPlsFocusOnItem;
        internal static string pickMobDontPickItemTypeListAdded;
        internal static string pickMobDontPickItemTypeListRemoved;
        internal static string pickMobFocusedMob;
        internal static string pickMobFocusedItem;
        internal static string pickMobRemoveMobIdFromList;
        internal static string pickMobAddMobIdToList;
        internal static string pickMobRemoveFromList;
        internal static string pickMobAddToList;
        internal static string pickMobAddItemTypeToList;
        internal static string pickMobRemoveItemTypeFromList;
        internal static string pickMobRemoveFromDontPickList;
        internal static string pickMobAddToDontPickList;
        internal static string pickMobRemoveItemTypeFromDontPickList;
        internal static string pickMobAddItemTypeToDontPickList;
        internal static string pickMobClearMonsterList;
        internal static string pickMobAddToSkillList;
        internal static string pickMobRemoveFromSkillList;
        internal static string pickMobResetSkillListToDefault;
        internal static string pickMobResetItemListToDefault;
        internal static string pickMobViewMonsterList;
        internal static string empty;
        internal static string pickMobMonsterIdList;
        internal static string pickMobMonsterTypeList;
        internal static string pickMobViewItemList;
        internal static string pickMobAutoPickItemList;
        internal static object pickMobAutoPickItemTypeList;
        internal static object pickMobDontPickItemList;
        internal static object pickMobDontPickItemTypeList;
        internal static string pickMobViewSkillList;
        internal static string pickMobSkillList;
        #endregion

        internal static void LoadLanguage(sbyte newLanguage)
        {
            switch (newLanguage)
            {
                case 0:
                    LoadLanguageVI();
                    break;
                default:
                    LoadLanguageEN();
                    break;
                //case 2:
                //    LoadLanguageID();
                //    break;
            }
        }

        internal static string OnOffStatus(bool value) => value ? mResources.ON : mResources.OFF;

        static void LoadLanguageVI()
        {
            communityMod = "NRO Mod Cộng đồng";
            gameVersion = "Phiên bản game";
            registered = "Đã đăng ký";
            arbitration = "Trọng tài";
            autoChatDisabled = "Đã tắt Auto Chat";
            inputContent = "Nhập nội dung";
            delaySeconds = "Delay:\n{0} giây";
            viewContent = "Xem nội dung";
            autoChatContent = "Nội dung Auto Chat";
            inputDelay = "Nhập thời gian delay";
            timeMilliseconds = "Thời gian (ms)";
            errorOccurred = "Có lỗi xảy ra";
            contentSaved = "Đã lưu nội dung";
            autoAttack = "Tự đánh";
            gobackTo = "Goback đến map: {0}, khu: {1}, tọa độ: ({2}, {3})";
            youAreNotNamekian = "Bạn không phải là Namek";
            completed = "Đã hoàn thành";
            modMenuPanelTabName = new string[][]
            {
                new string[]{ "Bật/tắt", "" },
                new string[]{ "Điều", "chỉnh" },
                new string[]{ "Chức", "năng" },
            };
            someonePet = $" [<color=cyan>{mResources.pet}</color> của {{0}}]";
            petLostMaster = $" [<color=cyan>{mResources.pet}</color> bị lạc sư phụ]";
            zone = "khu";
            functionShouldBeDisabled = "Bạn cần tắt chức năng \"{0}\"!";
            functionShouldBeEnabled = "Bạn cần bật chức năng \"{0}\"!";
            valueChanged = "Đã thay đổi {0} thành {1}";
            invalidValue = "Giá trị không hợp lệ";
            inputNumberOutOfRange = "Số đã nhập phải trong khoảng {0} và {1}";
            inputNumberMustBeBiggerThanOrEqual = "Số đã nhập phải lớn hơn hoặc bằng {0}";
            empty = "Rỗng";

            inputFPS = "Nhập số khung hình trên giây";
            inputTimeChangeBg = "Nhập thời gian thay đổi hình nền";
            inputTimeChangeBgHint = "Thời gian (giây)";
            customBgChatPopup = "Loại hình nền được hỗ trợ: ảnh (*.jpg, *.png), ảnh động (*.gif), video (*.mp4).\nẢnh động và video tiêu tốn nhiều tài nguyên của hệ thống, nên cân nhắc trước khi sử dụng.";
            customBgOpenBgList = "Mở danh sách hình nền";
            customBgAddNewBg = "Thêm hình nền mới";
            customBgRemoveAll = "Xóa hết hình nền";
            customBgAllBgRemoved = "Đã xóa hết hình nền trong danh sách";
            customBgAutoChangeBg = "Tự động chuyển hình nền";
            customBgSetTimeChange = "Thay đổi thời gian chuyển hình nền";
            imageVideoFile = "Tệp ảnh/video";
            allFileTypes = "Tất cả";
            customBgSelectBgFiles = "Chọn hình nền";
            customBgChangeGifSpeed = "Thay đổi tốc độ ảnh động";
            customBgInputGifSpeed = "Nhập tốc độ ảnh động";
            speed = "Tốc độ";
            customBgSwitchToThisBg = "Chuyển sang hình nền này";
            delete = "Xóa";
            customBgRemovedBg = "Đã xóa hình nền \"{0}\"";
            fullPath = "Đường dẫn đầy đủ";
            customBgList = "Danh sách hình nền tùy chỉnh";
            customBgGifSpeed = "Tốc độ ảnh động";
            pickMobMonsterAdded = "Đã thêm quái id {0} vào danh sách";
            pickMobMonsterRemoved = "Đã xóa quái id {0} khỏi danh sách";
            pickMobMonsterTypeAdded = "Đã thêm \"{0}\" [{1}] vào danh sách";
            pickMobMonsterTypeRemoved = "Đã xóa \"{0}\" [{1}] khỏi danh sách";
            pickMobAutoPickItemListRemoved = "Đã xoá \"{0}\" [{1}] khỏi danh sách tự động nhặt vật phẩm";
            pickMobAutoPickItemListAdded = "Đã thêm \"{0}\" [{1}] vào danh sách tự động nhặt vật phẩm";
            pickMobAutoPickItemTypesListRemoved = "Đã xoá loại vật phẩm {0} khỏi danh sách tự động nhặt";
            pickMobAutoPickItemTypesListAdded = "Đã thêm loại vật phẩm {0} vào danh sách tự động nhặt";
            pickMobPlsFocusOnMonsterOrItem = "Cần trỏ vào quái hay vật phẩm cần thêm vào danh sách tương ứng";
            pickMobMonsterListCleared = "Đã xoá danh sách quái tàn sát";
            pickMobItemListResetToDefault = "Danh sách vật phẩm đã được đặt lại mặc định";
            pickMobConfiguredPickGemsOnly = "Đã cài đặt chỉ nhặt ngọc";
            pickMobSkillListRemoved = "Đã xoá \"{0}\" [{1}] khỏi danh sách kỹ năng dùng để đánh quái";
            pickMobSkillListAdded = "Đã thêm \"{0}\" [{1}] vào danh sách kỹ năng dùng để đánh quái";
            pickMobSkillListResetToDefault = "Đã đặt danh sách kỹ năng dùng để đánh quái về mặc định";
            pickMobDontPickItemListAdded = "Đã thêm \"{0}\" [{1}] vào danh sách vật phẩm không tự động nhặt";
            pickMobDontPickItemListRemoved = "Đã xoá \"{0}\" [{1}] khỏi danh sách vật phẩm không tự động nhặt";
            pickMobDontPickItemTypeListAdded = "Đã thêm loại vật phẩm {0} vào danh sách không tự động nhặt";
            pickMobDontPickItemTypeListRemoved = "Đã xoá loại vật phẩm {0} khỏi danh sách không tự động nhặt";
            pickMobPlsFocusOnItem = "Cần trỏ vào vật phẩm cần thêm vào danh sách";
            pickMobFocusedMob = "Quái đang trỏ vào: {0}, id: {1}, loại: {2}";
            pickMobFocusedItem = "Vật phẩm đang trỏ vào: {0}, id: {1}, loại: {2}";
            pickMobRemoveMobIdFromList = "Xóa quái id ({0}) khỏi danh sách";
            pickMobAddMobIdToList = "Thêm quái id ({0}) vào danh sách";
            pickMobRemoveFromList = "Xóa {0} khỏi danh sách";
            pickMobAddToList = "Thêm {0} vào danh sách";
            pickMobAddItemTypeToList = "Thêm loại vật phẩm ({0}) vào danh sách";
            pickMobRemoveItemTypeFromList = "Xóa loại vật phẩm ({0}) khỏi danh sách";
            pickMobRemoveFromDontPickList = "Xóa {0} khỏi danh sách không nhặt";
            pickMobAddToDontPickList = "Thêm {0} vào danh sách không nhặt";
            pickMobRemoveItemTypeFromDontPickList = "Xóa loại vật phẩm ({0}) khỏi danh sách không nhặt";
            pickMobAddItemTypeToDontPickList = "Thêm loại vật phẩm ({0}) vào danh sách không nhặt";
            pickMobClearMonsterList = "Xóa danh sách tàn sát";
            pickMobAddToSkillList = "Thêm {0} vào danh sách kỹ năng";
            pickMobRemoveFromSkillList = "Xóa {0} khỏi danh sách kỹ năng";
            pickMobResetSkillListToDefault = "Đặt lại danh sách kỹ năng";
            pickMobResetItemListToDefault = "Đặt lại danh sách vật phẩm";
            pickMobViewMonsterList = "Xem danh sách quái";
            pickMobMonsterIdList = "Danh sách id quái";
            pickMobMonsterTypeList = "Danh sách loại quái";
            pickMobViewItemList = "Xem danh sách vật phẩm";
            pickMobAutoPickItemList = "Danh sách vật phẩm tự động nhặt";
            pickMobAutoPickItemTypeList = "Danh sách loại vật phẩm tự động nhặt";
            pickMobDontPickItemList = "Danh sách vật phẩm không tự động nhặt";
            pickMobDontPickItemTypeList = "Danh sách loại vật phẩm không tự động nhặt";
            pickMobViewSkillList = "Xem danh sách kỹ năng";
            pickMobSkillList = "Danh sách kỹ năng dùng để đánh quái";
            //---------------------------------------------------
            vSyncDescription = "Tự động giới hạn FPS theo tốc độ khung hình của màn hình";
            showTargetInfoTitle = "Thông tin đối thủ";
            showTargetInfoDescription = "Hiện gần chính xác thời gian NRD, khiên, khỉ, huýt sáo... của đối thủ";
            autoSendAttackDescription = "Tự động gửi lệnh tấn công mục tiêu mà không di chuyển nhân vật";
            showCharListTitle = "Danh sách nhân vật";
            showCharListDescription = "Hiện danh sách nhân vật trong khu hiện tại";
            showPetInCharListTitle = "Hiện đệ tử trong danh sách nhân vật";
            showPetInCharListDescription = "Hiện đệ tử trong danh sách nhân vật trong khu hiện tại";
            autoTrainForNewbieTitle = "Tự động up SS";
            autoTrainForNewbieDescription = "Tự động up acc mới đến nhiệm vụ vào bang";
            noLongerNewAccount = "Bạn đã qua nhiệm vụ vào bang";
            customBackgroundTitle = "Hình nền tùy chỉnh";
            customBackgroundDescription = "Thay thế hình nền mặc định của game bằng hình nền tùy chỉnh";
            skipSpaceshipTitle = "Bỏ qua tàu vũ trụ";
            skipSpaceshipDescription = "Bỏ qua hoạt ảnh tàu vũ trụ";
            notifyBossTitle = "Thông báo Boss";
            notifyBossDescription = "Hiển thị danh sách thông báo boss";
            pickMobTitle = "Tàn sát";
            pickMobDescription = "Tự động đánh quái";
            avoidSuperMobTitle = "Né siêu quái khi tàn sát";
            avoidSuperMobDescription = "Không đánh siêu quái khi tàn sát";
            vdhTitle = "Vượt địa hình khi tàn sát";
            vdhDescription = "Tự động vượt địa hình khi đang tàn sát";
            autoPickItemTitle = "Tự động nhặt vật phẩm";
            autoPickItemDescription = "Tự động nhặt vật phẩm ở gần";
            pickMyItemOnlyTitle = "Không nhặt vật phẩm của người khác";
            pickMyItemOnlyDescription = "Chỉ nhặt vật phẩm của bản thân, do đệ tử đánh quái rơi ra và vật phẩm không thuộc về bất cứ ai";
            limitPickTimesTitle = "Giới hạn số lần nhặt";
            limitPickTimesDescription = "Giới hạn số lần tự động nhặt một vật phẩm";
            autoAskForPeansTitle = "Auto xin đậu";
            autoAskForPeansDescription = "Tự động gửi tin nhắn xin đậu thần vào kênh chat bang hội";
            youAreNotInAClan = "Bạn không ở trong bang hội nào";
            autoDonatePeansTitle = "Auto cho đậu";
            autoDonatePeansDescription = "Tự động cho đậu thần khi có tin nhắn xin đậu thần của các thành viên trong bang hội";
            autoHarvestPeansTitle = "Auto vặt đậu";
            autoHarvestPeansDescription = "Tự động thu hoạch đậu thần từ cây đậu thần ở nhà";
            setFPSDescription = "Điều chỉnh số lượng khung hình trên giây của game";
            setReduceGraphicsTitle = "Giảm đồ họa";
            level = "Mức";
            setReduceGraphicsChoices = new string[]
            {
                "Đang tắt",
                $"Đang bật ({level.ToLower()} 1)",
                $"Đang bật ({level.ToLower()} 2)",
                $"Đang bật ({level.ToLower()} 3)",
            };
            setGoBackChoices = new string[]
            {
                "Đang tắt",
                "Đang bật (quay lại chỗ cũ khi chết)",
                "Đang bật (đến map, khu và tọa độ cố định khi chết)",
            };
            setAutoTrainPetTitle = "Auto up đệ tử";
            setAutoTrainPetChoices = new string[] 
            {
                "Đang tắt",
                "Đang bật (up đệ thường)", 
                "Đang bật (up đệ né siêu quái)",
                "Đang bật (up đệ Kaioken)" 
            };
            youDontHaveDisciple = "Bạn không có đệ tử";
            setAutoAttackWhenDiscipleNeededTitle = "Đánh khi đệ kêu";
            setAutoAttackWhenDiscipleNeededChoices = new string[] 
            {
                "Đánh quái gần nhất",
                "Đánh đệ tử (tự động bật cờ xám)",
                "Đánh bản thân (tự động bật cờ xám)" 
            };
            setAutoRescueTitle = "Auto trị thương";
            setAutoRescueChoices = new string[]
            { 
                "Đang tắt",
                "Đang bật (trị thương mọi người)",
                "Đang bật (chỉ thành viên trong bang hội)",
                "Đang bật (chỉ đệ tử)",
                "Đang bật (chỉ đệ tử của bản thân)"
            };
            setAutoRescueSkill3Null = "Bạn chưa có kỹ năng Trị thương";
            setAutoRescueSkill3BuffInvalid = "Skill 3 của bạn không phải kỹ năng Trị thương";
            setTimeChangeCustomBgTitle = "Thời gian đổi hình nền";
            setTimeChangeCustomBgDescription = "Điều chỉnh thời gian thay đổi hình nền (giây)";
            openXmapMenuTitle = "Menu Xmap";
            openXmapMenuDescription = "Mở menu Xmap (lệnh \"xmp\" hoặc nút \'x\')";
            openPickMobMenuTitle = "Menu PickMob";
            openPickMobMenuDescription = "Mở menu PickMob (lệnh \"pickmob\")";
            openTeleportMenuTitle = "Menu Teleport";
            openTeleportMenuDescription = "Mở menu dịch chuyển (lệnh \"tele\" hoặc nút \'z\')";
            openCustomBackgroundMenuTitle = "Menu Custom Background";
            openCustomBackgroundMenuDescription = "Mở menu hình nền tùy chỉnh";
            openSetsMenuTitle = "Menu Set đồ";
            openSetsMenuDescription = "Mở menu set đồ (lệnh \"set\" hoặc nút \'`\')";
        }

        static void LoadLanguageEN()
        {
            communityMod = "DBO Community Mod";
            gameVersion = "Game version";
            registered = "Registered";
            arbitration = "Arbitration";
            autoChatDisabled = "Auto Chat disabled";
            inputContent = "Input content";
            delaySeconds = "Delay:\n{0} seconds";
            viewContent = "View content";
            autoChatContent = "Auto Chat contents";
            inputDelay = "Input delay time";
            timeMilliseconds = "Time (ms)";
            errorOccurred = "Error occurred";
            contentSaved = "Content saved";
            autoAttack = "Auto attack";
            gobackTo = "Goback to map: {0}, zone: {1}, coordinates: ({2}, {3})";
            youAreNotNamekian = "You are not a Namekian character";
            completed = "Completed";
            modMenuPanelTabName = new string[][]
            {
                new string[]{ "Toggle", "" },
                new string[]{ "Adjust", "" },
                new string[]{ "Func-", "tions" },
            };
            someonePet = $" [{{0}}'s <color=cyan>{mResources.pet}</color>]";
            petLostMaster = $" [<color=cyan>{mResources.pet}</color> lost master]";
            zone = "zone";
            functionShouldBeDisabled = "You need to disable the \"{0}\" feature!";
            functionShouldBeEnabled = "You need to enable the \"{0}\" feature!";
            valueChanged = "Changed {0} to {1}";
            invalidValue = "Invalid value";
            inputNumberOutOfRange = "The number entered must be between {0} and {1}";
            inputNumberMustBeBiggerThanOrEqual = "The number entered must be bigger than or equal to {0}";
            empty = "Empty";

            inputFPS = "Input the number of frames per second";
            inputTimeChangeBg = "Input the background interval";
            inputTimeChangeBgHint = "Time (seconds)";
            customBgChatPopup = "Supported background types: static images (*.jpg, *.png), animated images (*.gif), videos (*.mp4).\nAnimated images and videos consume a lot of system resources, which you should consider before using them.";
            customBgOpenBgList = "Open background list";
            customBgAddNewBg = "Add new background";
            customBgRemoveAll = "Remove all backgrounds";
            customBgAllBgRemoved = "All backgrounds have been removed from the list";
            customBgAutoChangeBg = "Auto change background";
            customBgSetTimeChange = "Change background interval";
            customBgChangeGifSpeed = "Change animated background speed";
            imageVideoFile = "Image/video file";
            allFileTypes = "All file types";
            customBgSelectBgFiles = "Select background files";
            customBgInputGifSpeed = "Input animated background speed";
            speed = "Speed";
            customBgSwitchToThisBg = "Switch to this background";
            delete = "Delete";
            customBgRemovedBg = "Background \"{0}\" has been removed";
            fullPath = "Full path";
            customBgList = "Custom background list";
            customBgGifSpeed = "Animated background speed";
            pickMobMonsterAdded = "Monster with id {0} has been added to the list";
            pickMobMonsterRemoved = "Monster with id {0} has been removed from the list";
            pickMobMonsterTypeAdded = "\"{0}\" [{1}] has been added to the list";
            pickMobMonsterTypeRemoved = "\"{0}\" [{1}] has been removed from the list";
            pickMobAutoPickItemListRemoved = "\"{0}\" [{1}] has been removed from the auto pick item list";
            pickMobAutoPickItemListAdded = "\"{0}\" [{1}] has been added to the auto pick item list";
            pickMobAutoPickItemTypesListRemoved = "Item type {0} has been removed from the auto pick list";
            pickMobAutoPickItemTypesListAdded = "Item type {0} has been added to the auto pick list";
            pickMobPlsFocusOnMonsterOrItem = "Please focus on the monster or item to add to the corresponding list";
            pickMobMonsterListCleared = "Monster list has been cleared";
            pickMobItemListResetToDefault = "Item list has been reset to default";
            pickMobConfiguredPickGemsOnly = "Configured to pick gems only";
            pickMobSkillListRemoved = "\"{0}\" [{1}] has been removed from the skill list used to attack monsters";
            pickMobSkillListAdded = "\"{0}\" [{1}] has been added to the skill list used to attack monsters";
            pickMobSkillListResetToDefault = "Skill list used to attack monsters has been reset to default";
            pickMobDontPickItemListAdded = "\"{0}\" [{1}] has been added to the item list not to auto pick";
            pickMobDontPickItemListRemoved = "\"{0}\" [{1}] has been removed from the item list not to auto pick";
            pickMobDontPickItemTypeListAdded = "Item type {0} has been added to the list not to auto pick";
            pickMobDontPickItemTypeListRemoved = "Item type {0} has been removed from the list not to auto pick";
            pickMobPlsFocusOnItem = "Please focus on the item to add to the list";
            pickMobFocusedMob = "Focused monster: {0}, id: {1}, type: {2}";
            pickMobFocusedItem = "Focused item: {0}, id: {1}, type: {2}";
            pickMobRemoveMobIdFromList = "Remove monster with id ({0}) from the list";
            pickMobAddMobIdToList = "Add monster with id ({0}) to the list";
            pickMobRemoveFromList = "Remove {0} from the list";
            pickMobAddToList = "Add {0} to the list";
            pickMobAddItemTypeToList = "Add item type ({0}) to the list";
            pickMobRemoveItemTypeFromList = "Remove item type ({0}) from the list";
            pickMobRemoveFromDontPickList = "Remove {0} from the list not to auto pick";
            pickMobAddToDontPickList = "Add {0} to the list not to auto pick";
            pickMobRemoveItemTypeFromDontPickList = "Remove item type ({0}) from the list not to auto pick";
            pickMobAddItemTypeToDontPickList = "Add item type ({0}) to the list not to auto pick";
            pickMobClearMonsterList = "Clear monster list";
            pickMobAddToSkillList = "Add {0} to the skill list";
            pickMobRemoveFromSkillList = "Remove {0} from the skill list";
            pickMobResetSkillListToDefault = "Reset the skill list to default";
            pickMobResetItemListToDefault = "Reset the item list to default";
            pickMobViewMonsterList = "View monster list";
            pickMobMonsterIdList = "Monster id list";
            pickMobMonsterTypeList = "Monster type list";
            pickMobViewItemList = "View item list";
            pickMobAutoPickItemList = "Auto pick item list";
            pickMobAutoPickItemTypeList = "Auto pick item type list";
            pickMobDontPickItemList = "Don't auto pick item list";
            pickMobDontPickItemTypeList = "Don't auto pick item type list";
            pickMobViewSkillList = "View skill list";
            pickMobSkillList = "Skill list used to attack monsters";
            //---------------------------------------------------
            vSyncDescription = "Automatically limit the FPS according to the monitor's refresh rate";
            showTargetInfoTitle = "Target's effect information";
            showTargetInfoDescription = "Show the near-accurate duration of Black Star Dragonball, Energy Shield, Super Monkey, Whistle, etc. of the target character";
            autoSendAttackDescription = "Automatically send attack commands to the target character without moving your character";
            showCharListTitle = "Character list";
            showCharListDescription = "Show the character list in the current zone";
            showPetInCharListTitle = "Include disciples in the character list";
            showPetInCharListDescription = "Include disciples in the character list in the current zone";
            autoTrainForNewbieTitle = "Auto train new account";
            autoTrainForNewbieDescription = "Auto train new account until the clan mission";
            noLongerNewAccount = "You have completed the clan mission";
            customBackgroundTitle = "Custom background";
            customBackgroundDescription = "Replace the game default background with custom background";
            skipSpaceshipTitle = "Skip spaceship";
            skipSpaceshipDescription = "Skip spaceship animation";
            notifyBossTitle = "Boss notification";
            notifyBossDescription = "Show boss notification list";
            pickMobTitle = "Slaughter";
            pickMobDescription = "Auto attack monsters";
            avoidSuperMobTitle = "Avoid super monsters";
            avoidSuperMobDescription = "Don't attack super monsters when slaughtering";
            vdhTitle = "Cross terrain while slaughtering";
            vdhDescription = "Automatically cross terrain while slaughtering";
            autoPickItemTitle = "Auto pick items";
            autoPickItemDescription = "Automatically pick up nearby items";
            pickMyItemOnlyTitle = "Don't pick up other people's items";
            pickMyItemOnlyDescription = "Only pick up your own items, items dropped by your own disciples when fighting monsters, and items that do not belong to anyone";
            limitPickTimesTitle = "Limit the number of pickup attempts";
            limitPickTimesDescription = "Limit the number of attempts to pick up an item";
            autoAskForPeansTitle = "Auto ask for Senzu beans";
            autoAskForPeansDescription = "Automatically send messages asking for Senzu beans to the clan chat channel";
            youAreNotInAClan = "You are not in any clan";
            autoDonatePeansTitle = "Auto donate Senzu beans";
            autoDonatePeansDescription = "Automatically give Senzu beans when there is a request message from a clan member";
            autoHarvestPeansTitle = "Auto harvest Senzu beans";
            autoHarvestPeansDescription = "Automatically harvest Senzu beans from the Senzu tree at home";
            setFPSDescription = "Adjust the number of frames per second of the game";
            setReduceGraphicsTitle = "Reduce graphics quality";
            level = "Level";
            setReduceGraphicsChoices = new string[]
            {
                "Disabled",
                $"Enabled ({level.ToLower()} 1)",
                $"Enabled ({level.ToLower()} 2)",
                $"Enabled ({level.ToLower()} 3)",
            };
            setGoBackChoices = new string[]
            {
                "Disabled",
                "Enabled (return to the same place where you died)",
                "Enabled (go to the fixed map, zone, and position when you died)",
            };
            setAutoTrainPetTitle = "Auto train disciple";
            setAutoTrainPetChoices = new string[]
            {
                "Disabled",
                "Enabled (normal mode)",
                "Enabled (avoid super monsters mode)",
                "Enabled (kaioken mode)"
            };
            youDontHaveDisciple = "You don't have disciple";
            setAutoAttackWhenDiscipleNeededTitle = "Attack when the disciple needs";
            setAutoAttackWhenDiscipleNeededChoices = new string[]
            {
                "Nearest monster",
                "Attack the disciple (automatically change flag to gray)",
                "Attack self (automatically change flag to gray)"
            };
            setAutoRescueTitle = "Auto rescue";
            setAutoRescueChoices = new string[]
            {
                "Disabled",
                "Enabled (everyone)",
                "Enabled (clan members only)",
                "Enabled (disciple only)",
                "Enabled (own disciple only)"
            };
            setAutoRescueSkill3Null = "You don't have Rescue skill";
            setAutoRescueSkill3BuffInvalid = "Your third skill is not Rescue";
            setTimeChangeCustomBgTitle = "Background duration";
            setTimeChangeCustomBgDescription = "Adjust the time to switch background (seconds)";
            openXmapMenuTitle = "Xmap menu";
            openXmapMenuDescription = "Open the Xmap menu (\"xmp\" chat command or \'x\' key)";
            openPickMobMenuTitle = "PickMob menu";
            openPickMobMenuDescription = "Open the PickMob menu (\"pickmob\" chat command)";
            openTeleportMenuTitle = "Teleport menu";
            openTeleportMenuDescription = "Open the Teleport menu (\"tele\" chat command or \'z\' key)";
            openCustomBackgroundMenuTitle = "Custom Background menu";
            openCustomBackgroundMenuDescription = "Open the Custom Background menu";
            openSetsMenuTitle = "Sets menu";
            openSetsMenuDescription = "Open the Sets menu (\"set\" command or \'`\' key)";
        }

        //static void LoadLanguageID()
        //{
        //    communityMod = "Mod Komunitas DBO";
        //    gameVersion = "Versi game";
        //    registered = "Terdaftar";
        //    arbitration = "Penyelenggara";
        //    autoChatDisabled = "Auto Chat dinonaktifkan";
        //    inputContent = "Masukkan konten";
        //    delaySeconds = "Penundaan:\n{0} detik";
        //    viewContent = "Lihat konten";
        //    autoChatContent = "Konten Auto Chat";
        //    inputDelay = "Masukkan waktu penundaan";
        //    timeMilliseconds = "Waktu (ms)";
        //    errorOccurred = "Terjadi kesalahan";
        //    contentSaved = "Konten disimpan";
        //    delayChanged = "Mengubah penundaan menjadi {0} detik";
        //    autoAttack = "Serangan otomatis";
        //    gobackTo = "Kembali ke peta: {0}, zona: {1}, koordinat: ({2}, {3})";
        //    youAreNotNamekian = "Anda bukan karakter Namekian";
        //    completed = "Selesai";
        //    modMenuPanelTabName = new string[][]
        //    {
        //        new string[]{ "Beralih", "" },
        //        new string[]{ "Menyes-", "uaikan" },
        //        new string[]{ "fungsi", "" },
        //    };
        //    someonePet = $" [<color=cyan>{mResources.pet}</color> {{0}}]";
        //    petLostMaster = $" [<color=cyan>{mResources.pet}</color> itu kehilangan gurunya]";
        //    zone = "zona";
        //    functionShouldBeDisabled = "Anda perlu menonaktifkan fitur \"{0}\"!";
        //    functionShouldBeEnabled = "Anda harus mengaktifkan fitur \"{0}\"!";
        //    valueChanged = "Berubah {0} menjadi {1}";
        //    invalidValue = "Nilai tidak valid";
        //    inputFPS = "Masukkan jumlah frame per detik";
        //    inputTimeChangeBg = "Masukkan waktu untuk mengganti wallpaper latar belakang";
        //    inputTimeChangeBgHint = "Waktu (detik)";
        //    customBgChatPopup = "Jenis latar belakang yang didukung: gambar statis (*.jpg, *.png), gambar animasi (*.gif), video (*.mp4).\nGambar animasi dan video mengonsumsi banyak sumber daya, jadi pertimbangkan itu sebelum menggunakannya.";
        //    customBgOpenBgList = "Buka daftar latar belakang";
        //    customBgAddNewBg = "Tambahkan latar belakang baru";
        //    customBgRemoveAll = "Hapus semua latar belakang";
        //    customBgAllBgRemoved = "Semua latar belakang telah dihapus dari daftar";
        //    customBgAutoChangeBg = "Ganti latar belakang otomatis";
        //    //---------------------------------------------------
        //    vSyncDescription = "Secara otomatis membatasi FPS sesuai dengan kecepatan refresh layar";
        //    showTargetInfoTitle = "Informasi sasaran";
        //    showTargetInfoDescription = "Menampilkan waktu bola naga bintang hitam, perisai energi, transformasi, peluit... hampir seakurat mungkin untuk karakter sasaran";
        //    autoSendAttackDescription = "Secara otomatis mengirim perintah serangan ke sasaran tanpa memindahkan karakter";
        //    showCharListTitle = "Daftar karakter";
        //    showCharListDescription = "Menampilkan daftar karakter di zona saat ini";
        //    showPetInCharListTitle = "Sertakan murid dalam daftar karakter";
        //    showPetInCharListDescription = "Sertakan murid dalam daftar karakter di zona saat ini";
        //    autoTrainForNewbieTitle = "Latih akun baru secara otomatis";
        //    autoTrainForNewbieDescription = "Latih otomatis akun baru ke misi klan";
        //    noLongerNewAccount = "Anda telah melewati misi klan";
        //    customBackgroundTitle = "Wallpaper khusus";
        //    customBackgroundDescription = "Ganti latar belakang game dengan wallpaper khusus";
        //    notifyBossTitle = "Pemberitahuan Boss";
        //    //someone pls translate this
        //    notifyBossDescription = "";
        //}
    }
}
