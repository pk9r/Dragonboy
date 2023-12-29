$md5CryptoServiceProvider = New-Object -TypeName System.Security.Cryptography.MD5CryptoServiceProvider;
$hash = $hash = [System.BitConverter]::ToString($md5CryptoServiceProvider.ComputeHash([System.IO.File]::ReadAllBytes("Game\Game_Data\Managed\Assembly-CSharp.dll"))).Replace("-", "");
$hash += "`n" + [System.BitConverter]::ToInt32([System.IO.File]::ReadAllBytes("Game\Game_Data\Managed\Assembly-CSharp.dll"), 0x00000088) + "`n";
$hash += $hash = [System.BitConverter]::ToString($md5CryptoServiceProvider.ComputeHash([System.IO.File]::ReadAllBytes("Game\QLTK.dll"))).Replace("-", "");
$hash += "`n" + [System.BitConverter]::ToInt32([System.IO.File]::ReadAllBytes("Game\QLTK.exe"), 0x00000088);
Out-File -Encoding utf8 -FilePath "HashAndTimeStamp.txt" -InputObject $hash