# Chinese

中文解析通用工具。包括拼音，数字读法，货币读法。

<br/>

## 拼音

拼音解析引用：**Microsoft Visual Studio International Pack 1.0 Service**，提供音标解析。

```c#
// "qu4 ba1，pi2 ka3 qiu1！"
Pinyin.Get("去吧，皮卡丘！", PinyinFormat.Default);
```

```c#
// "qu ba，pi ka qiu！"
Pinyin.Get("去吧，皮卡丘！", PinyinFormat.WithoutTone);
```

```c#
// "qù bā，pí kǎ qiū！"
Pinyin.Get("去吧，皮卡丘！", PinyinFormat.PhoneticSymbol);
```

<br/>

## 数字读法

### 计量读法

小写读法：

```c#
var options = new ChineseNumberOptions { Verbose = false, Upper = false };
ChineseNumber.GetString(10_0001, options);    // "十万零一"
ChineseNumber.GetString(10_0101, options);    // "十万零一百零一"
ChineseNumber.GetString(10_1001, options);    // "十万一千零一"
ChineseNumber.GetString(10_1010, options);    // "十万一千零一十"
```

```c#
var options = new ChineseNumberOptions { Verbose = true, Upper = false };
ChineseNumber.GetString(10_0001, options);    // "一十万零一"
ChineseNumber.GetString(10_0101, options);    // "一十万零一百零一"
ChineseNumber.GetString(10_1001, options);    // "一十万一千零一"
ChineseNumber.GetString(10_1010, options);    // "一十万一千零一十"
```
大写读法：

```c#
var options = new ChineseNumberOptions { Verbose = false, Upper = true };
ChineseNumber.GetString(10_0001, options);    // "拾万零壹"
ChineseNumber.GetString(10_0101, options);    // "拾万零壹佰零壹"
ChineseNumber.GetString(10_1001, options);    // "拾万壹仟零壹"
ChineseNumber.GetString(10_1010, options);    // "拾万壹仟零壹拾"
```

```c#
var options = new ChineseNumberOptions { Verbose = true, Upper = true };
ChineseNumber.GetString(10_0001, options);    // "壹拾万零壹"
ChineseNumber.GetString(10_0101, options);    // "壹拾万零壹佰零壹"
ChineseNumber.GetString(10_1001, options);    // "壹拾万壹仟零壹"
ChineseNumber.GetString(10_1010, options);    // "壹拾万壹仟零壹拾"
```

大数读法（万、亿、兆、京、垓、秭、穰）：

```c#
// "一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九"
ChineseNumber.GetString(1_2345_6789_0123_4567_8901_2345_6789m);
```

```c#
// "壹穰贰仟叁佰肆拾伍秭陆仟柒佰捌拾玖垓零壹佰贰拾叁京肆仟伍佰陆拾柒兆捌仟玖佰零壹亿贰仟叁佰肆拾伍万陆仟柒佰捌拾玖"
ChineseNumber.GetString(1_2345_6789_0123_4567_8901_2345_6789m, x => x.Upper = true);
```

<br/>

### 编号读法

```c#
ChineseNumber.GetPureString(10_0001, upper: false);    // "一〇〇〇〇一"
ChineseNumber.GetPureString(10_0101, upper: false);    // "一〇〇一〇一"
ChineseNumber.GetPureString(10_1001, upper: false);    // "一〇一〇〇一"
ChineseNumber.GetPureString(10_1010, upper: false);    // "一〇一〇一〇"
```

```c#
ChineseNumber.GetPureString(10_0001, upper: true);     // "壹零零零零壹"
ChineseNumber.GetPureString(10_0101, upper: true);     // "壹零零壹零壹"
ChineseNumber.GetPureString(10_1001, upper: true);     // "壹零壹零零壹"
ChineseNumber.GetPureString(10_1010, upper: true);     // "壹零壹零壹零"
```

<br/>

### 货币读法

货币小写读法：

```c#
var options = new ChineseNumberOptions { Verbose = false, Upper = false };
ChineseCurrency.GetString(10_0001, options);    // "十万零一元整"
ChineseCurrency.GetString(10_0101, options);    // "十万零一百零一元整"
ChineseCurrency.GetString(10_1001, options);    // "十万一千零一元整"
ChineseCurrency.GetString(10_1010, options);    // "十万一千零一十元整"
ChineseCurrency.GetString(10_0001.2m, options);    // "十万零一元二角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "十万零一元二角三分"
ChineseCurrency.GetString(10_0001.03m, options);   // "十万零一元零三分"
```

```c#
var options = new ChineseNumberOptions { Verbose = true, Upper = false };
ChineseCurrency.GetString(10_0001, options);    // "一十万零一元整"
ChineseCurrency.GetString(10_0101, options);    // "一十万零一百零一元整"
ChineseCurrency.GetString(10_1001, options);    // "一十万一千零一元整"
ChineseCurrency.GetString(10_1010, options);    // "一十万一千零一十元整"
ChineseCurrency.GetString(10_0001.2m, options);    // "一十万零一元二角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "一十万零一元二角三分"
ChineseCurrency.GetString(10_0001.03m, options);   // "一十万零一元零三分"
```

货币大写读法：

```c#
var options = new ChineseNumberOptions { Verbose = false, Upper = true };
ChineseCurrency.GetString(10_0001, options);    // "拾万零壹圆整"
ChineseCurrency.GetString(10_0101, options);    // "拾万零壹佰零壹圆整"
ChineseCurrency.GetString(10_1001, options);    // "拾万壹仟零壹圆整"
ChineseCurrency.GetString(10_1010, options);    // "拾万壹仟零壹拾圆整"
ChineseCurrency.GetString(10_0001.2m, options);    // "拾万零壹元贰角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "拾万零壹圆贰角叁分"
ChineseCurrency.GetString(10_0001.03m, options);   // "拾万零壹圆零叁分"
```

```c#
var options = new ChineseNumberOptions { Verbose = false, Upper = true };
ChineseCurrency.GetString(10_0001, options);    // "壹拾万零壹圆整"
ChineseCurrency.GetString(10_0101, options);    // "壹拾万零壹佰零壹圆整"
ChineseCurrency.GetString(10_1001, options);    // "壹拾万壹仟零壹圆整"
ChineseCurrency.GetString(10_1010, options);    // "壹拾万壹仟零壹拾圆整"
ChineseCurrency.GetString(10_0001.2m, options);    // "壹拾万零壹元贰角整"
ChineseCurrency.GetString(10_0001.23m, options);   // "壹拾万零壹圆贰角叁分"
ChineseCurrency.GetString(10_0001.03m, options);   // "壹拾万零壹圆零叁分"
```

<br/>