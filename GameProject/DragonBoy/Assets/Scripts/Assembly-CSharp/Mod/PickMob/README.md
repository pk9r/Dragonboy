# PickMobNRO
 
Hỗ trợ tự động nhặt vật phẩm và tự động đánh quái. Nếu các bạn có sử dụng source của mình để làm bản mod khác mình hi vọng có đề cập tới mình trong chức năng này.

Hướng dẫn cài đặt: ~~https://youtu.be/CHnGEqVQE2I~~
## Lưu ý
### Sử dụng item tự động luyện tập (TDLT)
- Tự động đánh quái sẽ không né siêu quái khi bật TDLT
- Tự động vật phẩm nhặt mà có bật TDLT, không bật tàn sát sẽ dịch chuyển tới vật phẩm để nhặt
- Tự động vật phẩm nhặt mà có bật TDLT, bật tàn sát sẽ không hoạt động
- TDLT sẽ không đánh quái nếu không bật tàn sát

### Các id sử dụng trong lệnh chat
- Id quái phụ thuộc vào map, khác với id loại quái
- Thứ tự skill là thứ tự của skill trong hành trang bắt đầu từ 1 đến 8, khác với id skill. Ví dụ thứ tự 3 của namec là hồi sinh, trái đất là tdhs, xayda là ttnl
- Id loại vật phẩm khác với id vật phẩm. Vd: Ngọc Rồng 1 sao (14), Bí ngô 1 sao (702), Ngọc đen 1 sao (807) có chung một id loại vật phẩm là 12 (mình tạm gọi là loại vật phẩm ngọc rồng)
- ***Id vật phẩm (id), id loại vật phẩm (type), id loại quái, id skill xem tại: ~~https://youtu.be/If7VFSOJwh0~~***

### Khác
- Liên hoàn được ưu tiên sử dụng hơn Chiêu đấm Demon (namec) trong tự động đánh quái. Nếu Liên hoàn không thể sử dụng thì mới sử dụng Chiêu đấm Demon (thiếu KI, chưa học skill)
- Kaioken được ưu tiên sử dụng hơn Chiêu đấm Dragon (Trái đất) trong tự động đánh quái. Nếu Kaioken không thể sử dụng thì mới sử dụng Chiêu đấm Dragon (thiếu KI, thiếu HP, chưa học skill) (Chưa làm)
- Các skill Quả cầu kênh khi, Makankosappo, Tự phát nổ, Trói, Trị thương không thể sử dụng trong tự động đánh quái
- Tự động đánh quái chỉ có thể vượt địa hình đơn giản (mấy map phức tạp bật TDLT hoặc add quái nhé). Nếu vượt địa hình lỗi thì dùng lệnh "vdh" để tắt.

## Lệnh chat
- add (trỏ vào quái hay vật phẩm): Thêm/Xoá quái hoặc vật phẩm ở danh sách tương ứng
- addt (trỏ vào quái hay vật phẩm): Thêm/Xoá loại quái hoặc loại vật phẩm ở danh sách tương ứng

### Tự động đánh quái
- ts: Bật/Tắt tự động đánh quái
- nsq: Bật/Tắt né siêu quái khi tự động đánh quái (Mặc định bật)
- addm***X***: Thêm vào/Xoá khỏi danh sách đánh quái id ***X*** (***X*** là id quái)
- addtm: Thêm vào/Xoá khỏi danh sách đánh loại quái id ***X*** (***X*** là id loại quái)
- clrm: Xoá danh sách tàn sát
- skill (trỏ vào skill): Thêm vào/Xoá khỏi danh sách skill sử dụng tự động đánh quái
- skill***X***: Thêm vào/Xoá khỏi danh sách skill sử dụng tự động đánh quái skill thứ ***X*** (***X*** là thứ tự skill)
- skillid***X***: Thêm vào/Xoá khỏi danh sách skill sử dụng tự động đánh quái skill id ***X*** (***X*** là id skill)
- clrs: Đặt danh sách skill sử dụng tự động đánh quái về mặc định
- ~~abf: Bật/Tắt tự động sử dụng đậu thần (KI, HP dưới 20%)~~
- ~~abf***X***: Bật tự động sử dụng đậu thần khi HP dưới ***X***%~~
- ~~abf***X*** ***Y***: Bật tự động sử dụng đậu thần khi HP dưới ***X***%, KI dưới ***Y***%~~
- vdh: Bật/Tắt vượt địa hình (mặc định Bật)

### Lệnh tự động nhặt vật phẩm
- anhat: Bật/Tắt tự động nhặt vật phẩm (Mặc định bật)
- itm: Bật/Tắt lọc không nhặt vật phẩm của người khác (Mặc định bật)
- sln: Bật/Tắt giới hạn số lần tự động nhặt một vật phẩm (Mặc định bật)
- sln***X***: Thay đổi giới hạn số lần nhặt là ***X*** (Mặc định ***X*** = 7)
- addi***X***: Thêm vào/Xoá khỏi danh sách chỉ tự động nhặt vật phẩm id ***X*** (***X*** là id vật phẩm)
- blocki (trỏ vào vật phẩm): Thêm vào/Xoá khỏi danh sách không tự động nhặt vật phẩm đang trỏ
- blocki***X***: Thêm vào/Xoá khỏi danh sách không tự động nhặt vật phẩm id ***X***
- addti***X***: Thêm vào/Xoá khỏi danh sách chỉ tự động nhặt loại vật phẩm id ***X***(X là id loại vật phẩm)
- blockti***X***: Thêm vào/Xoá khỏi danh sách không tự động nhặt loại vật phẩm id ***X***
- clri: Cài đặt lại danh sách lọc vật phẩm vể mặc định (danh sách chỉ nhặt và danh sách chặn)
- cnn: Cài đặt nhanh chỉ nhặt ngọc (tương ứng lệnh chat "clri" và "addi77")

## Phím tắt
- ~~T: Bật/Tắt tự động đánh quái~~
- ~~N: Bật/Tắt tự động nhặt vật phẩm~~
- ~~A: Tương ứng lệnh chat "add"~~
- ~~B: Tương ứng lệnh chat "abf"~~

## Danh sách mặc định
### Danh sách tự động nhặt vật phẩm
- Danh sách không tự động nhặt vật phẩm: 225, 353, 354, 355, 356, 357, 358, 359, 360, 362
  - Tương ứng: Mảnh đá vụn, Ngọc Rồng Namek 1 Sao -> Ngọc Rồng Namek 7 Sao, Ngọc Rồng Namek, Hóa thạch Ngọc Rồng
- Danh sách chỉ tự động nhặt vật phẩm: Không có gì (Nhặt tất cả item không bị chặn)
- Danh sách chỉ tự động nhặt loại vật phẩm: Không có gì (Nhặt tất cả item không bị chặn)
- Danh sách không tự động nhặt loại vật phẩm: Không có gì

### Danh sách tự động đánh quái
- Danh sách skill sử dụng tự động đánh quái: 0, 2, 17, 4
  - Tương ứng: Chiêu đấm Dragon, Chiêu đấm Demon, Liên hoàn, Chiêu đấm Galick
- Danh sách đánh quái: Không có gì (Đánh tất cả quái trong map)
- Danh sách đánh loại quái: Không có gì (Đánh tất cả quái trong map)
