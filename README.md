<img src="https://raw.githubusercontent.com/tobychui/cyinput/master/image/header.png"></img>
# cyinput
<img src="https://raw.githubusercontent.com/tobychui/cyinput/master/image/banner.png" width="70%"></img>

速型輸入法 - 輕便又免費的開源繁體中文字型輸入法

此輸入法使用數字鍵進行輸入，兼容 Windows 7 及以上的作業系統（見最低要求），

而且亦不需要安裝，可直接在 USB 啟動，所謂居家旅行必備程式

## 系統要求
- Windows 7 或以上
- .NET framework 4.5.1 或以上
- 記憶體：2GB 或以上 （程式只使用約 64MB）
- 硬盤： 64GB 或以上 （程式只使用約 1.5MB）

## 特色
1. 開源又免費，有甚麼用不慣可以自己改改看
2. 免安裝，支援 USB 啟動
3. 高兼容性，在 Window Form 及 UWP 上都能用
4. 運作於 svchost 根命名空間內，系統管理員無法知道本程式跟其他 Window Service 的分別

## 啟動和使用方法
1. 雙擊 cyinput.exe
2. 使用數字鍵*點選想要的字型（先左上後右上）
3. 選擇及點取你想輸入的中文字
4. 重複第 2 步及第 3步直至你已完成輸入所有你需要輸入的文字

<sub> *數字鍵即為鍵盤最右面的九個數字按鍵。部分筆記本電腦上或會缺少這些按鈕，在此況下你需要一個外置的數字鍵盤。 </sub>

## 輸入模式
cyinput 具有三種輸入模式，分別為
- 經剪貼簿 ➜ 文字先經過剪貼簿再輸出到輸入欄，軟件例如舊版 Firefox 在 Win7 環境下需要用這種輸入模式
- 直接輸出 ➜ 文字直接經過 .NET 的 IO 輸出到系統，一般 Win10 用家建議選這個
- 倉頡轉碼 ➜ 文字輸出為倉頡碼，再經倉頡輸入法輸入到軟件，非中文 Windows 系統可先安裝倉頡再使用此功能以避免亂碼問題

## 界面預覽
<img src="https://raw.githubusercontent.com/tobychui/cyinput/master/image/preview.png"></img>


