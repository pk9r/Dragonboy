# Dragonboy

Hướng dẫn xây dựng thêm tính năng cho bản mod: https://youtu.be/9sCof2eQH3Q

Discord hỏi đáp: https://discord.gg/zdvBdVxRh6
## Các tính năng trong game
### Lệnh chat
***Các lệnh chat phải bắt đầu bằng dấu gạch chéo (/)***
- ak: bật/tắt tự đánh
- bt: sử dụng bông tai
- buffme, hsme: sử dụng chiêu hồi sinh Namec vào bản thân
- csb: sử dụng capsule bay
- csdb: bật/tắt sử dụng capsule đặc biệt khi Xmap
- cspeed***X***, tdc***X***: chỉnh tốc độ chạy ***X***
- k***X***: chuyển khu ***X***
- speed***X***: chỉnh tốc độ game ***X*** (có thể nhập số thập phân)
- skey***X***: đồng bộ điều khiển phím với kênh ***X***
- tele: mở menu dịch chuyển tức thời
- xmp: mở menu Xmap
- xmp***X***: Xmap đến map có id ***X***
- ***Các lệnh chat của PickMob có thể xem tại [đây](AssemblyCSharp/Mod/PickMob/README.md).***
#### Invoke method bằng lệnh chat
- Gọi method trong assembly của game bằng tên đầy đủ (Namespace + tên class + tên method). Method được invoke phải là method static và không phải là constructor (.ctor).
### Phím tắt
- **Z**: mở menu dịch chuyển tức thời
- **B**: sử dụng chiêu hồi sinh Namec vào bản thân
- **N**: mở menu dịch chuyển đến NPC trong map
- **J**: sang map bên trái
- **K**: vào map giữa
- **L**: sang map bên phải
- **G**: gửi lệnh giao dịch đến nhân vật đang trỏ
- **W**: khinh công (dịch lên trên 50 đơn vị)
- **A**: dịch trái (dịch sang trái 50 đơn vị)
- **S**: độn thổ (dịch xuống 50 đơn vị) 
- **D**: dịch phải (dịch sang phải 50 đơn vị)
- **X**: mở menu Xmap
- **C**: bật/tắt sử dụng capsule thường khi Xmap
### Các chức năng khác
***Các chức năng khác của phiên bản mod ở trong phần "Menu Mod". Lưu ý các chức năng có màu xám không thể điều chỉnh.***
#### Bật/tắt
Chứa các chức năng có 2 trạng thái

![image](https://user-images.githubusercontent.com/98677996/196203308-60396427-5137-4597-bf2f-9013e4f2a237.png)
#### Điều chỉnh
Chứa các chức năng cần điều chỉnh giá trị và các chức năng nhiều trạng thái

![image](https://user-images.githubusercontent.com/98677996/196203694-5bf456aa-5661-428f-a260-4e0eeb251163.png)
#### Chức năng
Chứa các chức năng khác

![image](https://user-images.githubusercontent.com/98677996/196203774-9ffe6e15-c290-4d82-b4da-27013191a1b6.png)
#### Phần mở rộng
Chứa chức năng của các phần mở rộng, chỉ hiện khi có ít nhất 1 phần mở rộng
![image](https://user-images.githubusercontent.com/98677996/196202880-37927897-5137-4a94-9079-6a46460f98f7.png)


