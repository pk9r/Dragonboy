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

![image](https://user-images.githubusercontent.com/98677996/186596345-4eb768d0-57ff-4dd1-86fd-41ffe483735b.png)
#### Điều chỉnh
Chứa các chức năng cần điều chỉnh giá trị và các chức năng nhiều trạng thái

![image](https://user-images.githubusercontent.com/98677996/186596434-23f79704-8134-4b5e-b3a1-95748d8f728a.png)
#### Chức năng
Chứa các chức năng khác

![image](https://user-images.githubusercontent.com/98677996/186596496-0a3ce578-c4bd-40e0-a461-5eda3dd0760f.png)

